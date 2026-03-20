using UnityEditor;
using UnityEngine;

namespace MultiplayerCore_Woyboy
{
    [CustomEditor(typeof(NetworkPlayerInteractionController))]
    public class NetworkPlayerInteractionControllerEditor : Editor
    {
        private SerializedProperty networkPlayer;

        private SerializedProperty interactDistance;
        private SerializedProperty interactLayer;

        // GUIStyle fields
        private GUIStyle titleStyle;
        private GUIStyle subHeaderStyle;
        private GUIStyle sectionHeaderStyle;

        // Checkbox
        private bool showDefaultInspector = false;

        private void OnEnable()
        {
            networkPlayer = serializedObject.FindProperty("networkPlayer");
            interactDistance = serializedObject.FindProperty("interactDistance");
            interactLayer = serializedObject.FindProperty("interactLayer");

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
            GUILayout.Label("Network Player Interaction Controller", titleStyle);
            GUILayout.Label("By Woyboy", subHeaderStyle);
            GUILayout.Space(10);

            EditorGUILayout.EndVertical();
        }

        private void DrawReferenceBlock()
        {
            EditorGUILayout.BeginVertical("box");

            GUILayout.Label("References", sectionHeaderStyle);

            // Fields
            EditorGUILayout.PropertyField(networkPlayer, new GUIContent("NetworkPlayer Script"));
            GUILayout.Space(3);

            GUILayout.Label("Interaction Settings", sectionHeaderStyle);

            EditorGUILayout.PropertyField(interactDistance, new GUIContent("Interact Distance"));
            GUILayout.Space(3);
            EditorGUILayout.PropertyField(interactLayer, new GUIContent("Interact Layer"));
            GUILayout.Space(3);

            EditorGUILayout.EndVertical();
        }
    }
}