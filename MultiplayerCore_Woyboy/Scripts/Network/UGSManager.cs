using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;

namespace MultiplayerCore_Woyboy
{
    /// <summary>
    /// A script for connecting the player into the Unity's Gaming Services. This
    /// should be the first thing to happen before any netcode related events
    /// are to happen.
    /// 
    /// In short, this checks if the player is online or not.
    /// </summary>
    public class UGSManager : MonoBehaviour
    {
        private static UGSManager Instance;

        private async void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Initialize Unity Gaming Services
            try
            {
                await UnityServices.InitializeAsync();
                Debug.Log("Unity Services Initialized");

                if (!AuthenticationService.Instance.IsSignedIn)
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to initialize Unity Services: " + e);
            }
        }
    }
}