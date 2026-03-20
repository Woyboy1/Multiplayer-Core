using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

namespace MultiplayerCore_Woyboy
{
    /// <summary>
    /// A simple camera controller for the firstperson controller. It works with
    /// the Cinemachine to accessing its other features like impulse sources 
    /// and noise. does NOT use the POV aim (freelook) component for the input.
    /// 
    /// FAQ    
    /// - What is the point of the Layers? 
    /// Because this is a first-person project. We want to make the body
    /// invisible to the player but visible to other players. At the same time
    /// this project contains a ragdoll feature so the player should be able
    /// to see their own body.
    /// 
    /// </summary>
    public class NetworkFirstPersonCameraController : NetworkBehaviour
    {
        // Assignables --------------------------------------
        [SerializeField] private Camera playerCamera;
        [SerializeField] private CinemachineCamera cinemachineCamera;
        [SerializeField] private Transform playerBody;
        [SerializeField] private Transform cameraPivot;
        [SerializeField] private GameObject graphicsRoot;
        [SerializeField] private LayerMask hideFromLocalCamera; // for the player model.


        // Settings --------------------------------------
        [SerializeField] private float sensitivity = 2.5f;
        [SerializeField] private float maxLookAngle = 80f;

        // Internal
        private float verticalRotation = 0f;
        private bool lookEnabled = true;

        // Layers
        private int defaultLayer;
        private int localLayer;
        private int deadLayer;

        public Camera PlayerCamera => playerCamera;
        public GameObject GraphicsRoot => graphicsRoot;
        public int DefaultLayer => defaultLayer;
        public int LocalLayer => localLayer;
        public int DeadLayer => deadLayer;

        // -------------------- Core --------------------

        #region Core

        public override void OnNetworkSpawn()
        {
            defaultLayer = LayerMask.NameToLayer("Default");
            localLayer = LayerMask.NameToLayer("LocalPlayerBody");
            deadLayer = LayerMask.NameToLayer("DeadPlayer");

            if (!IsOwner)
            {
                playerCamera.gameObject.SetActive(false);
                return;
            }

            cinemachineCamera.Priority = 10;

            SetLayerRecursively(graphicsRoot, localLayer);

            playerCamera.cullingMask &= ~hideFromLocalCamera;

            LockCursor();
        }

        private void Update()
        {
            if (!IsOwner) return;
            if (lookEnabled)
                Look();
        }

        public void SetLayerRecursively(GameObject obj, int layer)
        {
            obj.layer = layer;

            foreach (Transform t in obj.transform)
                SetLayerRecursively(t.gameObject, layer);
        }

        private void Look()
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity * 100f * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * 100f * Time.deltaTime;

            // Horizontal rotation (player body)
            playerBody.Rotate(Vector3.up * mouseX);

            // Vertical rotation (camera)
            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);

            cameraPivot.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        }

        #endregion

        // -------------------- Camera Controls --------------------

        #region Camera Controls

        public void EnableLook()
        {
            lookEnabled = true;
            LockCursor();
        }

        public void DisableLook()
        {
            lookEnabled = false;
            UnlockCursor();
        }

        public void ToggleLook(bool state)
        {
            lookEnabled = state;

            if (state)
                LockCursor();
            else
                UnlockCursor();
        }

        public bool IsLookEnabled()
        {
            return lookEnabled;
        }

        public void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        #endregion
    }
}