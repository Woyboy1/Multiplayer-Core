using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace MultiplayerCore_Woyboy
{
    /// <summary>
    /// A simple NetworkPlayerUIManager to handling all the local UI the player
    /// ever needs. This does not need to be a NetworkBehavior since we only need
    /// one canvas to handling all the local UI for a player. 
    /// </summary>
    public class NetworkPlayerUIManager : MonoBehaviour
    {
        public static NetworkPlayerUIManager instance;

        // Assignables --------------------------------------
        [SerializeField] private Image staminaFillImage;
        [SerializeField] private Image healthFillImage;

        // References --------------------------------------
        [SerializeField] private NetworkPlayerMovement playerMovement;
        [SerializeField] private NetworkPlayerStats playerStats;

        // temp
        public TextMeshProUGUI joincodeText;

        private float maxHealth = 100f;

        // ---------------- a simple method for updating the join code text.
        // you can remove when you no longer need it.
        public void updatetext(string text)
        {
            joincodeText.text = text;
        }

        // -------------------- Core --------------------

        private void Awake()
        {
            instance = this;
        }

        private void OnDestroy()
        {
            if (playerStats != null && playerStats.IsOwner)
                playerStats.Health.OnValueChanged -= OnHealthChanged;
        }

        private void Update()
        {
            if (playerMovement == null) return;
            staminaFillImage.fillAmount = playerMovement.GetStaminaPercent();
        }

        public void AssignScripts(NetworkPlayerMovement movement, NetworkPlayerStats stats)
        {
            Debug.Log("Added scripts");
            playerMovement = movement;
            playerStats = stats;

            if (playerStats != null && playerStats.IsOwner)
            {
                UpdateHealthUI(playerStats.Health.Value);
                playerStats.Health.OnValueChanged += OnHealthChanged;
            }
        }

        // -------------------- State --------------------

        private void OnHealthChanged(int oldValue, int newValue)
        {
            UpdateHealthUI(newValue);
        }

        private void UpdateHealthUI(int health)
        {
            float healthPercent = health / maxHealth;
            healthFillImage.fillAmount = healthPercent;
        }
    }
}