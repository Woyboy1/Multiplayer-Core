using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace MultiplayerCore_Woyboy
{
    /// <summary>
    /// NetworkPlayerStats handles the state of the player in terms of Health and being
    /// dead. If you're looking for how stamina is handled, look in NetworkPlayerMovement.cs
    /// since movement is the only thing manipulating stamina.
    /// 
    /// You'll find all the the other functions here that works together with the SpectatorCamera
    /// prefab.
    /// </summary>
    public class NetworkPlayerStats : NetworkBehaviour
    {
        // Static list to add itself to the playerlist to keep track of how many players are present
        public static List<NetworkPlayerStats> AllPlayers = new List<NetworkPlayerStats>();

        // Network Variables --------------------------------------
        [SerializeField] private NetworkVariable<int> health = new NetworkVariable<int>(100);
        [SerializeField] private NetworkVariable<bool> isDead = new NetworkVariable<bool>(false);

        // Assignables --------------------------------------
        [SerializeField] private GameObject spectatorCameraPrefab;
        [SerializeField] private Transform spectatorFollowPoint;
        [SerializeField] private NetworkPlayer networkPlayerReference;
        [SerializeField] private Collider playerCollider;

        // The present spectatorCamera
        private GameObject spectatorInstance;

        public Transform SpectatorFollowPoint => spectatorFollowPoint;
        public NetworkVariable<int> Health => health;
        public NetworkVariable<bool> IsDead => isDead;

        // -------------------- Core --------------------

        #region Core

        public override void OnNetworkSpawn()
        {
            AllPlayers.Add(this);
            Health.OnValueChanged += OnHealthChanged;
        }

        public override void OnNetworkDespawn()
        {
            AllPlayers.Remove(this);
            Health.OnValueChanged -= OnHealthChanged;
        }

        private void Update()
        {
            if (!IsOwner) return;
        }

        private void OnHealthChanged(int oldValue, int newValue)
        {
            if (newValue <= 0 && IsServer && !IsDead.Value)
            {
                Die();
            }
            else if (newValue > 0 && IsServer && IsDead.Value)
            {
                Revive();
            }
        }

        private void Die()
        {
            IsDead.Value = true;

            if (playerCollider != null)
                playerCollider.enabled = false;

            EnableRagdollClientRpc();

            // Assigning Layers:
            GameObject root = networkPlayerReference.CameraController.GraphicsRoot;
            int deadLayer = networkPlayerReference.CameraController.DeadLayer;
            networkPlayerReference.CameraController.SetLayerRecursively(root, deadLayer);

            DisablePlayerControlClientRpc(new ClientRpcParams
            {
                Send = new ClientRpcSendParams { TargetClientIds = new ulong[] { OwnerClientId } }
            });

            EnterSpectatorClientRpc(new ClientRpcParams
            {
                Send = new ClientRpcSendParams { TargetClientIds = new ulong[] { OwnerClientId } }
            });
        }

        private void Revive()
        {
            IsDead.Value = false;

            if (playerCollider != null)
                playerCollider.enabled = true;

            DisableRagdollClientRpc();

            ReviveClientRpc(new ClientRpcParams
            {
                Send = new ClientRpcSendParams { TargetClientIds = new ulong[] { OwnerClientId } }
            });
        }

        #endregion

        // -------------------- Client RPCs --------------------

        #region ClientRPCs

        [ClientRpc]
        private void DisablePlayerControlClientRpc(ClientRpcParams rpcParams = default)
        {
            if (!IsOwner) return;

            networkPlayerReference.DisablePlayerControl();

            networkPlayerReference.AudioController.ToggleListener(false);
            networkPlayerReference.CameraController.LockCursor();
        }

        [ClientRpc]
        private void EnterSpectatorClientRpc(ClientRpcParams rpcParams = default)
        {
            if (!IsOwner) return;

            if (spectatorInstance != null) return;

            spectatorInstance = Instantiate(spectatorCameraPrefab);
        }

        [ClientRpc]
        private void EnableRagdollClientRpc()
        {
            if (networkPlayerReference == null)
            {
                Debug.LogError("NetworkPlayerStats.cs: NetworkPlayerReference missing");
                return;
            }

            networkPlayerReference.RagdollController.EnableRagdoll();
        }

        [ClientRpc]
        private void DisableRagdollClientRpc()
        {
            networkPlayerReference.RagdollController.DisableRagdoll();
        }

        [ClientRpc]
        private void ReviveClientRpc(ClientRpcParams rpcParams = default)
        {
            if (!IsOwner) return;

            networkPlayerReference.EnablePlayerControl();
            networkPlayerReference.AudioController.ToggleListener(true);

            if (spectatorInstance != null)
            {
                Destroy(spectatorInstance);
                spectatorInstance = null;
            }
        }

        #endregion

        // -------------------- Server RPCs --------------------

        #region ServerRPCs

        [ServerRpc]
        private void DebugDamageServerRpc(int damage)
        {
            TakeDamage(damage);
        }

        [ServerRpc]
        private void DebugHealServerRpc(int amount)
        {
            HealPlayer(amount);
        }

        #endregion

        // -------------------- Health --------------------

        #region Heath
        public void TakeDamage(int damage)
        {
            if (!IsServer) return;

            int minHealth = 0;
            int maxHealth = 100;

            if (IsDead.Value) return;

            Health.Value -= damage;
            Health.Value = Mathf.Clamp(Health.Value, minHealth, maxHealth);
        }

        public void HealPlayer(int amount)
        {
            if (!IsServer) return;

            int minHealth = 0;
            int maxHealth = 100;

            Health.Value += amount;
            Health.Value = Mathf.Clamp(Health.Value, minHealth, maxHealth);
        }

        #endregion
    }
}