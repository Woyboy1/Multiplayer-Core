using Unity.Netcode;
using UnityEngine;

namespace MultiplayerCore_Woyboy
{
    /// <summary>
    /// Small script for testing out the ConnectionsManager and
    /// NetworkManager.Singleton methods
    /// </summary>
    public class NetworkDebugButtons : MonoBehaviour
    {
        public void StartHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        public void StartClient()
        {
            NetworkManager.Singleton.StartClient();
        }

        public async void ConnectionStartHost()
        {
            await ConnectionsManager.Instance.HostGame();
        }

        public async void ConnectionJoin(string activeCode)
        {
            await ConnectionsManager.Instance.JoinGame(activeCode);
        }
    }
}