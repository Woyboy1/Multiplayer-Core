using UnityEngine;

namespace MultiplayerCore_Woyboy
{
    /// <summary>
    /// A simple ragdoll controller with a sole purpose of
    /// disabline and enabling the rigidbodies, colliders,
    /// and the animator component.
    /// </summary>
    public class NetworkRagdollController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Transform ragdollRoot;

        private Rigidbody[] ragdollBodies;
        private Collider[] ragdollColliders;

        // -------------------- Core --------------------

        void Awake()
        {
            ragdollBodies = ragdollRoot.GetComponentsInChildren<Rigidbody>();
            ragdollColliders = ragdollRoot.GetComponentsInChildren<Collider>();

            SetRagdollState(false);
        }

        private void SetRagdollState(bool state)
        {
            foreach (var rb in ragdollBodies)
            {
                rb.isKinematic = !state;
            }
        }

        // -------------------- Public Methods --------------------

        public void EnableRagdoll()
        {
            if (ragdollBodies == null || ragdollColliders == null)
            {
                Debug.Log("Missing");
                ragdollBodies = ragdollRoot.GetComponentsInChildren<Rigidbody>();
                ragdollColliders = ragdollRoot.GetComponentsInChildren<Collider>();
            }

            animator.enabled = false;

            foreach (var rb in ragdollBodies)
            {
                rb.isKinematic = false;
            }
        }

        public void DisableRagdoll()
        {
            SetRagdollState(false);
            animator.enabled = true;
        }
    }
}