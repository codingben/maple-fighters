using ScriptableObjects.Configurations;
using UnityEditor;
using UnityEngine;

namespace Scripts.Editor
{
    [CustomEditor(typeof(QuickLoginConfiguration))]
    public class QuickLoginConfigurationEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var quickLoginConfiguration = (QuickLoginConfiguration)target;

            RenderEmailField(quickLoginConfiguration);
            RenderPasswordField(quickLoginConfiguration);

            EditorUtility.SetDirty(quickLoginConfiguration);
        }

        private void RenderEmailField(QuickLoginConfiguration quickLoginConfiguration)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Email:");
            quickLoginConfiguration.Email = GUILayout.TextField(quickLoginConfiguration.Email, GUILayout.MinWidth(EditorGUIUtility.fieldWidth));
            EditorGUILayout.EndHorizontal();
        }

        private void RenderPasswordField(QuickLoginConfiguration quickLoginConfiguration)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Password:");
            quickLoginConfiguration.Password = GUILayout.PasswordField(quickLoginConfiguration.Password, "*"[0], GUILayout.MinWidth(EditorGUIUtility.fieldWidth));
            EditorGUILayout.EndHorizontal();
        }
    }
}