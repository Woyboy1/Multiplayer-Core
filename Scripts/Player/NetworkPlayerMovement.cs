using Unity.Netcode;
using UnityEngine;

namespace MultiplayerCore_Woyboy
{
    /// <summary>
    /// Welcome to the NetworkPlayerMovement script. There is a lot happening here,
    /// but in a short summary we are using a CharacterController script with
    /// a custom gravity. If you want to add a jumping mechanic you can do so without
    /// the need of a RigidBody component.
    /// 
    /// Additionally, stamina usage is also handled here. If you ever see yourself
    /// changing stamina values in another script, it's recommended to change it 
    /// through NetworkPlayerMovement.cs to make it easier on yourself. If it gets too
    /// complicated, I encourage making a separate script dedicated to handling stamina.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class NetworkPlayerMovement : NetworkBehaviour
    {
        // Assignables --------------------------------------
        [SerializeField] private Animator playerAnimator;

        // Settings --------------------------------------
        [SerializeField] private float acceleration = 10f;
        [SerializeField] private float deceleration = 15f;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float gravity = -9.81f;

        // Stamina System / Sprinting --------------------------------------
        [SerializeField] private bool enableSprinting;
        [SerializeField] private float sprintingSpeed = 8.0f;
        [SerializeField] private float currentStamina = 100;
        [SerializeField] private float maxStamina = 100;
        [SerializeField] private float staminaRegenRate = 4.0f;
        [SerializeField] private float staminaDrainRate = 4.0f;

        private CharacterController controller;
        private Vector3 currentMove;
        private Vector3 velocity;
        private bool movementEnabled = true;
        private bool canSprint = true;

        public Animator PlayerAnimator => playerAnimator;
        public CharacterController Controller => controller;

        public float GetStaminaPercent()
        {
            return currentStamina / maxStamina;
        }

        // -------------------- Core --------------------

        #region Core

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            currentStamina = maxStamina;
        }

        private void Update()
        {
            if (!IsOwner) return;
            if (!movementEnabled) return;

            Gravity();
            HandleMovement();
        }

        private void HandleMovement()
        {
            Vector2 input = GetInput();

            bool sprintInput = Input.GetKey(KeyCode.LeftShift);

            bool isSprinting = false;
            float speed = moveSpeed;

            if (enableSprinting)
            {
                isSprinting = sprintInput && canSprint && input.y > 0.1f;

                HandleStamina(isSprinting);

                speed = isSprinting ? sprintingSpeed : moveSpeed;
            }

            MoveCharacter(input, speed);
            UpdateAnimator(input.x, input.y, isSprinting);
        }

        private Vector2 GetInput()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            return new Vector2(x, z);
        }

        private void MoveCharacter(Vector2 input, float speed)
        {
            Vector3 targetMove =
                transform.right * input.x +
                transform.forward * input.y;

            if (targetMove.magnitude > 1f)
                targetMove.Normalize();

            targetMove *= speed;

            // Smooth acceleration / deceleration
            float accel = input.magnitude > 0.1f ? acceleration : deceleration;

            currentMove = Vector3.Lerp(
                currentMove,
                targetMove,
                accel * Time.deltaTime
            );

            Vector3 finalMove = currentMove + velocity;

            controller.Move(finalMove * Time.deltaTime);
        }

        private void HandleStamina(bool isSprinting)
        {
            if (isSprinting)
            {
                currentStamina -= staminaDrainRate * Time.deltaTime;

                if (currentStamina <= 0)
                {
                    currentStamina = 0;
                    canSprint = false;
                }
            }
            else
            {
                if (currentStamina < maxStamina)
                {
                    currentStamina += staminaRegenRate * Time.deltaTime;

                    if (currentStamina >= maxStamina)
                    {
                        currentStamina = maxStamina;
                        canSprint = true;
                    }
                }
            }

            // Allow sprinting again once some stamina is restored
            if (!canSprint && currentStamina > maxStamina * 0.2f)
            {
                canSprint = true;
            }
        }

        private void UpdateAnimator(float horizontalInput, float verticalInput, bool isSprinting)
        {
            float threshold = 0.1f;
            float velocityValue = 0f;

            if (verticalInput > threshold) // forward
                velocityValue = isSprinting ? 3f : 2f;
            else if (verticalInput < -threshold) // backward
                velocityValue = -2f;
            else if (horizontalInput > threshold) // right
                velocityValue = 1f;
            else if (horizontalInput < -threshold) // left
                velocityValue = -1f;
            else
                velocityValue = 0f; // idle

            playerAnimator?.SetFloat("Velocity", velocityValue, 0.1f, Time.deltaTime);
        }


        private void Gravity()
        {
            if (controller.isGrounded && velocity.y < 0)
                velocity.y = -2f;

            velocity.y += gravity * Time.deltaTime;
        }

        #endregion

        // -------------------- Movement Controls --------------------

        #region Movement Controls

        public void DisableMovement()
        {
            movementEnabled = false;
            velocity = Vector3.zero;
            controller.enabled = false;
        }

        public void EnableMovement()
        {
            movementEnabled = true;
            controller.enabled = true;
        }

        public void ToggleMovement(bool state)
        {
            movementEnabled = state;

            if (!state)
                velocity = Vector3.zero;
        }

        public bool IsMovementEnabled()
        {
            return movementEnabled;
        }

        public void ToggleCharacterControllerComp(bool toggle)
        {
            controller.enabled = toggle;
        }

        #endregion
    }
}