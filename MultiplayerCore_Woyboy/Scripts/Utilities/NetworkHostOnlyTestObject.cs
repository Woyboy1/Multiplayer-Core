using MultiplayerCore_Woyboy;
using Unity.Netcode;
using UnityEngine;

namespace MultiplayerCore_Woyboy
{
    public class NetworkHostOnlyTestObject : NetworkInteractable
    {
        protected override void OnInteract(NetworkObject interactor)
        {
            // If client interacts with this object:
            if (interactor.OwnerClientId != NetworkManager.Singleton.LocalClientId)
            {
                Debug.Log("This player is not the host, cannot interact.");
                return;
            }

            InteractWithObjectClientRPC(interactor.OwnerClientId);
        }

        [ClientRpc]
        void InteractWithObjectClientRPC(ulong clientId)
        {
            Debug.Log("Interacted by host with ID: " + clientId);
            Debug.Log("Host has started the game. Loading scene...");

            ConnectionsManager.Instance.LoadNetworkScene(ConnectionsManager.Instance.GameSceneName);
        }
    }
}