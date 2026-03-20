using UnityEditor;
using UnityEngine;

namespace MultiplayerCore_Woyboy
{
    [CustomEditor(typeof(NetworkPlayerStats))]
    public class NetworkPlayerStatsEditor : Editor
    {
        private SerializedProperty health;
        private SerializedProperty isDead;
        private SerializedProperty spectatorCameraPrefab;
        private SerializedProperty spectatorFollowPoint;
        private SerializedProperty networkPlayerReference;
        private SerializedProperty playerCollider;

        // GUIStyle fields
        private GUIStyle titleStyle;
        private GUIStyle subHeaderStyle;
        private GUIStyle sectionHeaderStyle;

        // Checkox
        private bool showDefaultInspector = false;

        private void OnEnable()
        {
            // Serialized Properties
            health = serializedObject.FindProperty("health");
            isDead = serializedObject.FindProperty("isDead");
            spectatorCameraPrefab = serializedObject.FindProperty("spectatorCameraPrefab");
            spectatorFollowPoint = serializedObject.FindProperty("spectatorFollowPoint");
            networkPlayerReference = serializedObject.FindProperty("networkPlayerReference");
            playerCollider = serializedObject.FindProperty("playerCollider");

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
            GUILayout.Label("NETWORK PLAYER STATS", titleStyle);
            GUILayout.Label("By Woyboy", subHeaderStyle);
            GUILayout.Space(10);

            EditorGUILayout.EndVertical();
        }

        private void DrawReferenceBlock()
        {
            EditorGUILayout.BeginVertical("box");

            GUILayout.Label("Network Variables", sectionHeaderStyle);

            // Fields
            EditorGUILayout.PropertyField(health, new GUIContent("Health"));
            GUILayout.Space(3);
            EditorGUILayout.PropertyField(isDead, new GUIContent("Is Dead?"));
            GUILayout.Space(3);

            GUILayout.Space(10);
            GUILayout.Label("Assignables", sectionHeaderStyle);
            
            // Assignables
            EditorGUILayout.PropertyField(spectatorCameraPrefab, new GUIContent("Spectator Camera Prefab"));
            GUILayout.Space(3);
            EditorGUILayout.PropertyField(spectatorFollowPoint, new GUIContent("Spectator Follow Point Transform"));
            GUILayout.Space(3);
            EditorGUILayout.PropertyField(networkPlayerReference, new GUIContent("NetworkPlayer Script Reference"));
            GUILayout.Space(3);
            EditorGUILayout.PropertyField(playerCollider, new GUIContent("Player Collider"));

            EditorGUILayout.EndVertical();
        }
    }
}
