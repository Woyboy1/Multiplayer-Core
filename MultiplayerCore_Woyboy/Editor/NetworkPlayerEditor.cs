using UnityEditor;
using UnityEngine;

namespace MultiplayerCore_Woyboy
{
    [CustomEditor(typeof(NetworkPlayer))]
    public class NetworkPlayerEditor : Editor
    {
        private SerializedProperty movement;
        private SerializedProperty cameraController;
        private SerializedProperty audioController;
        private SerializedProperty stats;
        private SerializedProperty ragdollController;

        // GUIStyle fields
        private GUIStyle titleStyle;
        private GUIStyle subHeaderStyle;
        private GUIStyle sectionHeaderStyle;

        private bool showDefaultInspector = false;

        private void OnEnable()
        {
            // Serialized Properties
            movement = serializedObject.FindProperty("movement");
            cameraController = serializedObject.FindProperty("cameraController");
            audioController = serializedObject.FindProperty("audioController");
            stats = serializedObject.FindProperty("stats");
            ragdollController = serializedObject.FindProperty("ragdollController");

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
            GUILayout.Label("NETWORK PLAYER", titleStyle);
            GUILayout.Label("By Woyboy", subHeaderStyle);
            GUILayout.Space(10);

            EditorGUILayout.EndVertical();
        }

        private void DrawReferenceBlock()
        {
            EditorGUILayout.BeginVertical("box");

            GUILayout.Label("Assignables", sectionHeaderStyle);

            // Fields
            EditorGUILayout.PropertyField(movement, new GUIContent("Movement"));
            GUILayout.Space(3);
            EditorGUILayout.PropertyField(cameraController, new GUIContent("Camera Controller"));
            GUILayout.Space(3);
            EditorGUILayout.PropertyField(audioController, new GUIContent("Audio Controller"));
            GUILayout.Space(3);
            EditorGUILayout.PropertyField(stats, new GUIContent("Stats"));
            GUILayout.Space(3);
            EditorGUILayout.PropertyField(ragdollController, new GUIContent("Ragdoll Controller"));

            EditorGUILayout.EndVertical();
        }
    }
}