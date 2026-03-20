using Unity.Netcode;
using UnityEngine;

namespace MultiplayerCore_Woyboy
{
    /// <summary>
    /// The base NetworkInteractable component. Uses an interface to include a
    /// Interact() method. All interactable scripts are suggested to inherit this parent
    /// class when making a custom interactable class.
    /// </summary>

    [DisallowMultipleComponent]
    public abstract class NetworkInteractable : NetworkBehaviour, INetworkInteractable
    {
        [Header("Interaction Settings")]
        [SerializeField] private float interactCooldown = 0.5f;

        private float lastInteractTime;

        public void Interact(NetworkObject interactor)
        {
            if (!IsServer) return;

            if (Time.time - lastInteractTime < interactCooldown)
                return;

            lastInteractTime = Time.time;

            OnInteract(interactor);
        }

        protected abstract void OnInteract(NetworkObject interactor);
    }
}