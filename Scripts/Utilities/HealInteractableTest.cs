using Unity.Netcode;
using UnityEngine;

namespace MultiplayerCore_Woyboy
{
    /// <summary>
    /// Heal player interacting test.
    /// </summary>
    public class HealInteractableTest : NetworkInteractable
    {
        protected override void OnInteract(NetworkObject interactor)
        {
            if (!IsServer) return;

            NetworkPlayer networkPlayer = interactor.GetComponent<NetworkPlayer>();

            if (networkPlayer != null)
            {
                networkPlayer.Stats.HealPlayer(20);
            }
        }
    }
}