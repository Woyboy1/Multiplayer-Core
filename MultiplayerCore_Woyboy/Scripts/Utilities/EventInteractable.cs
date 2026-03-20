using MultiplayerCore_Woyboy;
using Unity.Netcode;
using UnityEngine.Events;
using UnityEngine;

namespace MultiplayerCore_Woyboy
{
    /// <summary>
    /// EventInteractable.cs inherits NetworkInteractable and the purpose of this script
    /// is just an excuse to add UnityEvents onto the interaction, incase
    /// you want to make a small event happen but don't need a script for it.
    /// </summary>
    public class EventInteractable : NetworkInteractable
    {
        [SerializeField] private UnityEvent onInteract;

        protected override void OnInteract(NetworkObject interactor)
        {
            if (IsServer)
            {
                onInteract?.Invoke();
                InteractClientRpc();
            }
        }

        [ClientRpc]
        private void InteractClientRpc(ClientRpcParams rpcParams = default)
        {
            if (IsServer) return;
            onInteract?.Invoke();
        }
    }
}