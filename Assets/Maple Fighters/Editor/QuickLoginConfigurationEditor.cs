using Scripts.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Scripts.Editor
{
    using Editor = UnityEditor.Editor;

    [CustomEditor(typeof(QuickLoginConfiguration))]
    public class QuickLoginConfigurationEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            RenderEmailField();
            RenderPasswordField();
        }

        private void RenderEmailField()
        {
            var quickLoginConfiguration = (QuickLoginConfiguration)target;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Email:");
            quickLoginConfiguration.Email = GUILayout.TextField(quickLoginConfiguration.Email, GUILayout.MinWidth(EditorGUIUtility.fieldWidth));
            EditorGUILayout.EndHorizontal();
        }

        private void RenderPasswordField()
        {
            var quickLoginConfiguration = (QuickLoginConfiguration)target;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Password:");
            quickLoginConfiguration.Password = GUILayout.PasswordField(quickLoginConfiguration.Password, "•"[0], GUILayout.MinWidth(EditorGUIUtility.fieldWidth));
            EditorGUILayout.EndHorizontal();
        }
    }
}