using Unity.Netcode;
using UnityEngine;

namespace MultiplayerCore_Woyboy
{
    /// <summary>
    /// Simple interaction script example, only debugs a message.
    /// </summary>
    public class InteractableTestObject : NetworkInteractable
    {
        protected override void OnInteract(NetworkObject interactor)
        {
            InteractWithObjectClientRPC(interactor.OwnerClientId);
        }

        [ClientRpc]
        void InteractWithObjectClientRPC(ulong clientId)
        {
            Debug.Log("This Sphere was interacted by ID: " + clientId);
        }
    }
}