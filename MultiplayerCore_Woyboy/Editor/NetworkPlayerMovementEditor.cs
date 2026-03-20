using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;

namespace MultiplayerCore_Woyboy
{
    [CustomEditor(typeof(NetworkPlayerMovement))]
    public class NetworkPlayerMovementEditor : Editor
    {
        private SerializedProperty playerAnimator;

        private SerializedProperty acceleration;
        private SerializedProperty deceleration;
        private SerializedProperty moveSpeed;
        private SerializedProperty gravity;

        private SerializedProperty enableSprinting;
        private SerializedProperty sprintingSpeed;
        private SerializedProperty currentStamina;
        private SerializedProperty maxStamina;
        private SerializedProperty staminaRegenRate;
        private SerializedProperty staminaDrainRate;

        // GUIStyle fields
        private GUIStyle titleStyle;
        private GUIStyle subHeaderStyle;
        private GUIStyle sectionHeaderStyle;

        // Checkbox
        private bool showDefaultInspector = false;

        private void OnEnable()
        {
            playerAnimator = serializedObject.FindProperty("playerAnimator");
            acceleration = serializedObject.FindProperty("acceleration");
            deceleration = serializedObject.FindProperty("deceleration");
            moveSpeed = serializedObject.FindProperty("moveSpeed");
            gravity = serializedObject.FindProperty("gravity");
            enableSprinting = serializedObject.FindProperty("enableSprinting");
            sprintingSpeed = serializedObject.FindProperty("sprintingSpeed");
            currentStamina = serializedObject.FindProperty("currentStamina");
            maxStamina = serializedObject.FindProperty("maxStamina");
            staminaRegenRate = serializedObject.FindProperty("staminaRegenRate");
            staminaDrainRate = serializedObject.FindProperty("staminaDrainRate");

            // Initialize GUIStyles
            titleStyle = new GUIStyle()
            {
                fontSize = 25,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.white }
            };
            titleStyle.hover = titleStyle.normal;
            titleStyle.active = titleStyle.normal;
            titleStyle.focused = titleStyle.normal;

            sectionHeaderStyle = new GUIStyle()
            {
                fontSize = 16,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft,
                normal = { textColor = Color.white }
            };
            sectionHeaderStyle.hover = sectionHeaderStyle.normal;
            sectionHeaderStyle.active = sectionHeaderStyle.normal;
            sectionHeaderStyle.focused = sectionHeaderStyle.normal;

            subHeaderStyle = new GUIStyle()
            {
                fontSize = 16,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.white }
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (showDefaultInspector)
            {
                // Just draw the normal inspector
                DrawCheckBox();
                DrawDefaultInspector();
            }
            else
            {
                // Draw custom inspector
                DrawTitle();
                DrawCheckBox();
                DrawReferenceBlock();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawCheckBox()
        {
            EditorGUILayout.BeginVertical("box");
            // Checkbox to toggle
            showDefaultInspector = EditorGUILayout.ToggleLeft("Show Default Inspector", showDefaultInspector);
            EditorGUILayout.Space(5);
            EditorGUILayout.EndVertical();
        }

        private void DrawTitle()
        {
            EditorGUILayout.BeginVertical("box");

            GUILayout.Space(5);
            GUILayout.Label("NETWORK PLAYER MOVEMENT", titleStyle);
            GUILayout.Label("By Woyboy", subHeaderStyle);
            GUILayout.Space(10);

            EditorGUILayout.EndVertical();
        }

        private void DrawReferenceBlock()
        {
            EditorGUILayout.BeginVertical("box");

            GUILayout.Label("Assignables", sectionHeaderStyle);

            // Fields
            EditorGUILayout.PropertyField(playerAnimator, new GUIContent("Player Animator"));
            GUILayout.Space(3);

            GUILayout.Label("Settings", sectionHeaderStyle);
            EditorGUILayout.PropertyField(acceleration, new GUIContent("Acceleration"));
            GUILayout.Space(3);
            EditorGUILayout.PropertyField(deceleration, new GUIContent("Deceleration"));
            GUILayout.Space(3);
            EditorGUILayout.PropertyField(moveSpeed, new GUIContent("Walk Speed"));
            GUILayout.Space(3);
            EditorGUILayout.PropertyField(gravity, new GUIContent("Gravity Speed"));
            GUILayout.Space(3);

            GUILayout.Space(10);
            GUILayout.Label("Stamina System / Sprinting", sectionHeaderStyle);

            // Settings
            EditorGUILayout.PropertyField(enableSprinting, new GUIContent("Enable Sprinting"));
            GUILayout.Space(3);
            EditorGUILayout.PropertyField(sprintingSpeed, new GUIContent("Sprinting Speed"));
            GUILayout.Space(3);
            EditorGUILayout.PropertyField(currentStamina, new GUIContent("Current Stamina"));
            GUILayout.Space(3);
            EditorGUILayout.PropertyField(maxStamina, new GUIContent("Max Stamina"));
            GUILayout.Space(3);
            EditorGUILayout.PropertyField(staminaRegenRate, new GUIContent("Stamina Regen Rate"));
            GUILayout.Space(3);
            EditorGUILayout.PropertyField(staminaDrainRate, new GUIContent("Stamina Drain Rate"));
            GUILayout.Space(3);
            EditorGUILayout.EndVertical();
        }
    }
}
