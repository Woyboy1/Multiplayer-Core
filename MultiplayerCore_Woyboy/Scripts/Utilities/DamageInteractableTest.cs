using Unity.Netcode;

namespace MultiplayerCore_Woyboy
{
    /// <summary>
    /// Damage player interacting test.
    /// </summary>
    public class DamageInteractableTest : NetworkInteractable
    {
        protected override void OnInteract(NetworkObject interactor)
        {
            if (!IsServer) return;

            NetworkPlayer networkPlayer = interactor.GetComponent<NetworkPlayer>();

            if (networkPlayer != null)
            {
                networkPlayer.Stats.TakeDamage(20);
            }
        }
    }
}