using System;
using System.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MultiplayerCore_Woyboy
{
    public class NetworkPlayer : NetworkBehaviour
    {
        // Assignables --------------------------------------
        [SerializeField] private NetworkPlayerMovement movement;
        [SerializeField] private NetworkFirstPersonCameraController cameraController;
        [SerializeField] private NetworkPlayerAudioController audioController;
        [SerializeField] private NetworkPlayerStats stats;
        [SerializeField] private NetworkRagdollController ragdollController;

        public NetworkPlayerMovement Movement => movement;
        public NetworkFirstPersonCameraController CameraController => cameraController;
        public NetworkPlayerAudioController AudioController => audioController;
        public NetworkPlayerStats Stats => stats;
        public NetworkRagdollController RagdollController => ragdollController;

        // ---------------------- Core ----------------------

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                StartCoroutine(SetSpawnDelay());
                StartCoroutine(AssignScripts());
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!IsOwner) return;
            StartCoroutine(SetSpawnDelay());
            Debug.Log("NetworkPlayer: Scene changed, teleporting player...");
        }

        private IEnumerator AssignScripts()
        {
            while (NetworkPlayerUIManager.instance == null)
                yield return null; 
            NetworkPlayerUIManager.instance.AssignScripts(movement, stats);
        }

        /// <summary>
        /// The host takes a while to build the scene and load into the scene,
        /// not giving enough time for teleporting to work so we wait until
        /// the scene is fully built and then teleport the host. The client is 
        /// different since the scene is already built and the server is running.
        /// </summary>
        /// <returns></returns>
        private IEnumerator SetSpawnDelay()
        {
            if (!IsOwner) yield break; // unnecssary but just in case.

            SceneSpawnPoint spawns = null;

            // Wait until a SceneSpawnPoint exists in the scene
            while (spawns == null || spawns.spawnPoints.Length == 0)
            {
                spawns = FindAnyObjectByType<SceneSpawnPoint>();
                yield return null; 
            }

            RandomSpawnTeleport(spawns);
        }

        private void RandomSpawnTeleport(SceneSpawnPoint spawns)
        {
            int randSpawnPoint = UnityEngine.Random.Range(0, spawns.spawnPoints.Length);
            Transform spawn = spawns.GetSpawn(randSpawnPoint);
            TeleportTo(spawn.position);
        }

        // ---------------------- Public Methods ----------------------


        public void DisablePlayerControl()
        {
            Movement.DisableMovement();
            CameraController.DisableLook();
        }

        public void EnablePlayerControl()
        {
            Movement.EnableMovement();
            CameraController.EnableLook();
        }

        public void SpectatorMode()
        {
            Movement.DisableMovement();
            CameraController.DisableLook();
            CameraController.LockCursor();
        }

        /// <summary>
        /// Make sure when you're teleporting the player, you disable the 
        /// CharacterController component. Otherwise you'd be freezing in place.
        /// </summary>
        /// <param name="targetPos"></param>
        public void TeleportTo(Vector3 targetPos)
        {
            Debug.Log("NetworkPlayer: TeleportTo() to: " + targetPos);
            movement.ToggleCharacterControllerComp(false);
            transform.position = targetPos;
            movement.ToggleCharacterControllerComp(true);
        }
        public void ReviveAndTeleport(Vector3 respawnPoint)
        {
            // NetworkPlayerStats.cs and NetworkPlayerUIManager.cs always references to 100
            // be mindful of changing health.
            int healAmount = 100;
            stats.HealPlayer(healAmount);
            TeleportTo(respawnPoint);
        }
    }
}