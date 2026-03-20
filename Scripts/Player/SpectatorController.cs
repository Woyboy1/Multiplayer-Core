using System.Collections;
using UnityEngine;
using System.Linq;

namespace MultiplayerCore_Woyboy
{
    /// <summary>
    /// NOT a NetworkObject. A locally instantiated camera system designed to follow the 
    /// remaining players that are alive.
    /// 
    /// The reason for this to not be a network object is because
    /// we aren't synchronizing a spectator controller to everyone else, unnecessary.
    /// </summary>
    public class SpectatorController : MonoBehaviour
    {
        [Header("Orbit Settings")]
        [SerializeField] private float rotationSpeed = 200f;       // Mouse sensitivity
        [SerializeField] private float verticalMin = -30f;         // Min vertical angle
        [SerializeField] private float verticalMax = 60f;          // Max vertical angle
        [SerializeField] private float distance = 3.5f;            // Distance from target
        [SerializeField] private float heightOffset = 1.7f;        // Camera height offset

        private int currentIndex = -1;                             // Tracks which player is being spectated
        private Transform currentTarget;                            // Current player's follow point

        private float currentYaw = 0f;                              // Horizontal rotation
        private float currentPitch = 20f;                           // Vertical rotation

        // ---------------- Core Methods ----------------

        private void Start()
        {
            StartCoroutine(InitSpectator());
        }

        private IEnumerator InitSpectator()
        {
            while (NetworkPlayerStats.AllPlayers.Count == 0)
                yield return null;

            CyclePlayer(0);
        }

        private void Update()
        {
            HandleInput();
            if (currentTarget != null)
            {
                UpdateCameraPosition();
            }
        }

        // ---------------- Input Handling ----------------

        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.E))
                CyclePlayer(1); // Next player

            if (Input.GetKeyDown(KeyCode.Q))
                CyclePlayer(-1); // Previous player

            if (currentTarget != null)
            {
                float mouseX = Input.GetAxisRaw("Mouse X") * rotationSpeed * Time.deltaTime;
                float mouseY = Input.GetAxisRaw("Mouse Y") * rotationSpeed * Time.deltaTime;

                currentYaw += mouseX;
                currentPitch -= mouseY;
                currentPitch = Mathf.Clamp(currentPitch, verticalMin, verticalMax);
            }
        }

        // ---------------- Camera Movement ----------------

        private void UpdateCameraPosition()
        {
            // Spherical coordinates offset
            Vector3 offset = Quaternion.Euler(currentPitch, currentYaw, 0f) * new Vector3(0, 0, -distance);
            transform.position = currentTarget.position + Vector3.up * heightOffset + offset;
            transform.LookAt(currentTarget.position + Vector3.up * heightOffset);
        }

        // ---------------- Player Cycling ----------------

        private void CyclePlayer(int direction)
        {
            var alivePlayers = NetworkPlayerStats.AllPlayers
                .Where(p => !p.IsDead.Value)
                .ToList();

            // Fallback: include local player if alone
            if (alivePlayers.Count == 0)
            {
                var localPlayer = NetworkPlayerStats.AllPlayers.FirstOrDefault(p => p.IsOwner);
                if (localPlayer != null)
                    alivePlayers.Add(localPlayer);
            }

            if (alivePlayers.Count == 0) return;

            if (currentTarget == null)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex = alivePlayers.IndexOf(
                    alivePlayers.FirstOrDefault(p => p.SpectatorFollowPoint == currentTarget)
                );

                if (currentIndex == -1) currentIndex = 0;

                currentIndex += direction;

                if (currentIndex >= alivePlayers.Count) currentIndex = 0;
                if (currentIndex < 0) currentIndex = alivePlayers.Count - 1;
            }

            currentTarget = alivePlayers[currentIndex].SpectatorFollowPoint;

            // Reset camera rotation
            currentYaw = 0f;
            currentPitch = 20f;
        }
    }
}