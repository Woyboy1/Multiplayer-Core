using Unity.Netcode;
using UnityEngine;

namespace MultiplayerCore_Woyboy
{
    /// <summary>
    /// A script to synchronize audio for each player. 
    /// 
    /// Known Bug:
    /// - When the player spawns, you will recieve AudioListener duplicated messages and it disappears
    /// This is due to the timing of the player prefab being created in the scene and the
    /// scripts take a while to load until it's ready. You can safely ignore the error.
    /// </summary>
    public class NetworkPlayerAudioController : NetworkBehaviour
    {
        [SerializeField] private AudioListener audioListener;

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                audioListener.enabled = true;
            }
        }

        public void ToggleListener(bool toggle)
        {
            audioListener.enabled = toggle;
        }
    }
}