using Unity.Netcode;
using UnityEngine;

namespace MultiplayerCore_Woyboy
{
    /// <summary>
    /// Simple Interactable interface.
    /// </summary>
    public interface INetworkInteractable
    {
        void Interact(NetworkObject interactor);
    }
}
