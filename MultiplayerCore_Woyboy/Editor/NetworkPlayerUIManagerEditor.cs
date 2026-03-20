using UnityEditor;
using UnityEngine;

namespace MultiplayerCore_Woyboy
{
    [CustomEditor(typeof(NetworkPlayerUIManager))]
    public class NetworkPlayerUIManagerEditor : Editor
    {
        private SerializedProperty staminaFillImage;
        private SerializedProperty healthFillImage;

        private SerializedProperty playerMovement;
        private SerializedProperty playerStats;

        // GUIStyle fields
        private GUIStyle titleStyle;
        private GUIStyle subHeaderStyle;
        private GUIStyle sectionHeaderStyle;

        // Checkbox
        private bool showDefaultInspector = false;

        private void OnEnable()
        {
            staminaFillImage = serializedObject.FindProperty("staminaFillImage");
            healthFillImage = serializedObject.FindProperty("healthFillImage");
            playerMovement = serializedObject.FindProperty("playerMovement");
            playerStats = serializedObject.FindProperty("playerStats");

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
            GUILayout.Label("Network Player UI Manager", titleStyle);
            GUILayout.Label("By Woyboy", subHeaderStyle);
            GUILayout.Space(10);

            EditorGUILayout.EndVertical();
        }

        private void DrawReferenceBlock()
        {
            EditorGUILayout.BeginVertical("box");

            GUILayout.Label("Assignables", sectionHeaderStyle);

            // Fields
            EditorGUILayout.PropertyField(staminaFillImage, new GUIContent("Stamina Fill Image"));
            GUILayout.Space(3);
            EditorGUILayout.PropertyField(healthFillImage, new GUIContent("Health Fill Image"));
            GUILayout.Space(3);

            GUILayout.Label("References", sectionHeaderStyle);

            // References
            EditorGUILayout.PropertyField(playerMovement, new GUIContent("Player Movement Script"));
            GUILayout.Space(3);
            EditorGUILayout.PropertyField(playerStats, new GUIContent("Player Stats Script"));
            GUILayout.Space(3);

            EditorGUILayout.EndVertical();
        }
    }
}