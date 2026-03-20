using UnityEngine;
using System.Collections;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace MultiplayerCore_Woyboy
{
    /// <summary>
    /// Welcome to ConnectionsManager.cs a complicated system for being the backbone
    /// of all the online connections. This script uses a singleton pattern to allow
    /// the developer to easily access. The script handles all connections, disconnections
    /// and approval checks. You can edit most of the things you want specifically.
    /// 
    /// The default MaxPlayer is set to 4 but you can increase it if you ever want to.
    /// Some methods that you should be aware of:
    /// 
    /// To join a Host's game:
    /// - Use JoinGame(string)
    /// 
    /// To host a game:
    /// - Use HostGame(int)
    /// 
    /// To set a game's state
    /// - Use SetGameState(GameState)
    /// 
    /// To disconnecting a player:
    /// - Use LeaveGameClient()
    /// 
    /// To properly shutdown a game:
    /// - Use ShutdownHost()
    /// </summary>
    public class ConnectionsManager : MonoBehaviour
    {
        public static ConnectionsManager Instance { get; private set; }

        [SerializeField] private string connectionSceneName = "ConnectionScene";
        [SerializeField] private string lobbySceneName = "LobbyRoom";
        [SerializeField] private string gameSceneName = "MultiplayerDemo";
        [SerializeField] private string currentJoinCode = "Null"; // leave empty.

        private GameState currentGameState = GameState.Lobby;

        public GameState CurrentGameState => currentGameState;
        public string LobbySceneName => lobbySceneName;
        public string GameSceneName => gameSceneName;
        public string CurrentJoinCode => currentJoinCode;

        // -------------------- INIT --------------------

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            StartCoroutine(InitializeNetcode());
        }

        // temp join code display
        private void Update()
        {
            // temp. You can remove this when you're ready.
            NetworkPlayerUIManager.instance?.updatetext(currentJoinCode);
        }

        private IEnumerator InitializeNetcode()
        {
            while (NetworkManager.Singleton == null)
                yield return null;

            while (NetworkManager.Singleton.SceneManager == null)
                yield return null;

            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }

        // -------------------- APPROVAL --------------------

        private void ApprovalCheck(
            NetworkManager.ConnectionApprovalRequest request,
            NetworkManager.ConnectionApprovalResponse response)
        {
            if (CurrentGameState != GameState.Lobby)
            {
                response.Approved = false;
                response.Reason = "ConnectionsManager: Match in progress.";
                return;
            }

            if (NetworkManager.Singleton.ConnectedClients.Count >= 4)
            {
                response.Approved = false;
                response.Reason = "ConnectionsManager: Match is full.";
                return;
            }

            response.Approved = true;
            response.CreatePlayerObject = true;
        }

        // -------------------- HOST --------------------

        public async Task<string> HostGame(int maxPlayers = 4)
        {
            if (!AuthenticationService.Instance.IsSignedIn)
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

            Allocation allocation =
                await RelayService.Instance.CreateAllocationAsync(maxPlayers);

            string joinCode =
                await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            var transport =
                NetworkManager.Singleton.GetComponent<UnityTransport>();

            transport.SetHostRelayData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData
            );

            NetworkManager.Singleton.StartHost();

            NetworkManager.Singleton.SceneManager.LoadScene(
                lobbySceneName,
                LoadSceneMode.Single
            );

            Debug.Log("ConnectionsManager: Your joinCode = " + joinCode);
            currentJoinCode = joinCode;
            return joinCode;
        }

        // -------------------- CLIENT --------------------

        public async Task JoinGame(string joinCode)
        {
            if (!AuthenticationService.Instance.IsSignedIn)
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

            JoinAllocation joinAllocation =
                await RelayService.Instance.JoinAllocationAsync(joinCode);

            var transport =
                NetworkManager.Singleton.GetComponent<UnityTransport>();

            transport.SetClientRelayData(
                joinAllocation.RelayServer.IpV4,
                (ushort)joinAllocation.RelayServer.Port,
                joinAllocation.AllocationIdBytes,
                joinAllocation.Key,
                joinAllocation.ConnectionData,
                joinAllocation.HostConnectionData
            );

            NetworkManager.Singleton.StartClient();
        }

        // -------------------- SCENE CHANGE --------------------

        public void LoadNetworkScene(string sceneName)
        {
            if (!NetworkManager.Singleton.IsServer)
            {
                Debug.LogWarning("ConnectionsManager: Only server can change scenes");
                return;
            }

            NetworkManager.Singleton.SceneManager.LoadScene(
                sceneName,
                LoadSceneMode.Single);
        }

        // -------------------- STATE --------------------

        public void SetGameState(GameState newState)
        {
            if (!NetworkManager.Singleton.IsServer)
                return;

            currentGameState = newState;
        }

        // -------------------- DISCONNECT --------------------

        private void OnClientDisconnected(ulong clientId)
        {
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                SceneManager.LoadScene(connectionSceneName);
            }
        }

        public void LeaveGameCient()
        {
            NetworkManager.Singleton.Shutdown();
            SceneManager.LoadScene(connectionSceneName);
        }

        public void ShutdownHost()
        {
            NetworkManager.Singleton.Shutdown();
            SceneManager.LoadScene(connectionSceneName);
        }
    }

    public enum GameState
    {
        Lobby,
        Starting,
        InGame
    }
}