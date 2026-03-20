using UnityEngine;
using Unity.Netcode;

namespace MultiplayerCore_Woyboy
{
    /// <summary>
    /// A simple Interaction controller with the only purpose of casting a raycast
    /// searching for interactable objects that inherit NetworkInteractable.cs
    /// </summary>
    public class NetworkPlayerInteractionController : NetworkBehaviour
    {
        // References --------------------------------------
        [SerializeField] private NetworkPlayer networkPlayer;

        // Interaction Settings --------------------------------------
        [SerializeField] private float interactDistance = 3f;
        [SerializeField] private LayerMask interactLayer; // default, interactable

        private INetworkInteractable currentInteractable;
        private NetworkObject currentInteractableObject;

        // -------------------- Core --------------------

        private void Update()
        {
            if (!IsOwner) return;

            CheckForInteractable();
            CheckInput();
        }

        private void CheckForInteractable()
        {
            Ray ray = new Ray(networkPlayer.CameraController.PlayerCamera.transform.position, networkPlayer.CameraController.PlayerCamera.transform.forward);

            // Debug Ray
            Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red);

            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayer))
            {
                INetworkInteractable interactable = hit.collider.GetComponent<INetworkInteractable>();

                if (interactable != null)
                {
                    currentInteractable = interactable;
                    currentInteractableObject = hit.collider.GetComponent<NetworkObject>();
                    return;
                }
            }

            currentInteractable = null;
            currentInteractableObject = null;
        }

        private void CheckInput()
        {
            if (currentInteractable == null)
                return;

            if (Input.GetKeyDown(KeyCode.E))
            {
                TryInteractServerRpc(currentInteractableObject.NetworkObjectId);
            }
        }

        [ServerRpc]
        private void TryInteractServerRpc(ulong interactableId)
        {
            if (!NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(interactableId, out NetworkObject interactableObj))
                return;

            INetworkInteractable interactable = interactableObj.GetComponent<INetworkInteractable>();

            if (interactable != null)
            {
                interactable.Interact(NetworkObject);
            }
        }
    }
}