using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;

namespace MultiplayerCore_Woyboy
{
    [CustomEditor(typeof(ConnectionsManager))]
    public class ConnectionsManagerEditor : Editor
    {
        private SerializedProperty connectionSceneName;
        private SerializedProperty lobbySceneName;
        private SerializedProperty gameSceneName;
        private SerializedProperty currentJoinCode;

        // GUIStyle fields
        private GUIStyle titleStyle;
        private GUIStyle subHeaderStyle;
        private GUIStyle sectionHeaderStyle;

        // Checkbox
        private bool showDefaultInspector = false;

        private void OnEnable()
        {
            connectionSceneName = serializedObject.FindProperty("connectionSceneName");
            lobbySceneName = serializedObject.FindProperty("lobbySceneName");
            gameSceneName = serializedObject.FindProperty("gameSceneName");
            currentJoinCode = serializedObject.FindProperty("currentJoinCode");

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
            GUILayout.Label("Connections Manager", titleStyle);
            GUILayout.Label("By Woyboy", subHeaderStyle);
            GUILayout.Space(10);

            EditorGUILayout.EndVertical();
        }

        private void DrawReferenceBlock()
        {
            EditorGUILayout.BeginVertical("box");

            GUILayout.Label("Fields", sectionHeaderStyle);

            // Fields
            EditorGUILayout.PropertyField(connectionSceneName, new GUIContent("Connection Scene Name"));
            GUILayout.Space(3);
            EditorGUILayout.PropertyField(lobbySceneName, new GUIContent("Lobby Scene Name"));
            GUILayout.Space(3);
            EditorGUILayout.PropertyField(gameSceneName, new GUIContent("Game Scene Name"));
            GUILayout.Space(3);

            EditorGUILayout.EndVertical();
        }
    }
}