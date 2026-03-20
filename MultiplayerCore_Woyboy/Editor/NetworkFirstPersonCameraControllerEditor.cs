using UnityEditor;
using UnityEngine;

namespace MultiplayerCore_Woyboy
{
    [CustomEditor(typeof(NetworkFirstPersonCameraController))]
    public class NetworkFirstPersonCameraControllerEditor : Editor
    {
        private SerializedProperty playerCamera;
        private SerializedProperty cinemachineCamera;
        private SerializedProperty playerBody;
        private SerializedProperty cameraPivot;
        private SerializedProperty graphicsRoot;
        private SerializedProperty hideFromLocalCamera;

        private SerializedProperty sensitivity;
        private SerializedProperty maxLookAngle;

        // GUIStyle fields
        private GUIStyle titleStyle;
        private GUIStyle subHeaderStyle;
        private GUIStyle sectionHeaderStyle;

        // Checkbox
        private bool showDefaultInspector = false;

        private void OnEnable()
        {
            playerCamera = serializedObject.FindProperty("playerCamera");
            cinemachineCamera = serializedObject.FindProperty("cinemachineCamera");
            playerBody = serializedObject.FindProperty("playerBody");
            cameraPivot = serializedObject.FindProperty("cameraPivot");
            graphicsRoot = serializedObject.FindProperty("graphicsRoot");
            hideFromLocalCamera = serializedObject.FindProperty("hideFromLocalCamera");
            sensitivity = serializedObject.FindProperty("sensitivity");
            maxLookAngle = serializedObject.FindProperty("maxLookAngle");

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
            GUILayout.Label("NETWORK CAMERA CONTROLLER", titleStyle);
            GUILayout.Label("By Woyboy", subHeaderStyle);
            GUILayout.Space(10);

            EditorGUILayout.EndVertical();
        }

        private void DrawReferenceBlock()
        {
            EditorGUILayout.BeginVertical("box");

            GUILayout.Label("Assignables", sectionHeaderStyle);

            // Fields
            EditorGUILayout.PropertyField(playerCamera, new GUIContent("Player Camera"));
            GUILayout.Space(3);
            EditorGUILayout.PropertyField(cinemachineCamera, new GUIContent("Cinemachine Camera"));
            GUILayout.Space(3);
            EditorGUILayout.PropertyField(playerBody, new GUIContent("Player Body"));
            GUILayout.Space(3);
            EditorGUILayout.PropertyField(cameraPivot, new GUIContent("Camera Pivot"));
            GUILayout.Space(3);
            EditorGUILayout.PropertyField(graphicsRoot, new GUIContent("Graphics Root"));
            GUILayout.Space(3);
            EditorGUILayout.PropertyField(hideFromLocalCamera, new GUIContent("Hide From Local Camera Layer"));
            GUILayout.Space(3);

            GUILayout.Space(10);
            GUILayout.Label("Settings", sectionHeaderStyle);

            // Settings
            EditorGUILayout.PropertyField(sensitivity, new GUIContent("Mouse Sensitivity"));
            GUILayout.Space(3);
            EditorGUILayout.PropertyField(maxLookAngle, new GUIContent("Max Look Angle"));

            EditorGUILayout.EndVertical();
        }
    }
}