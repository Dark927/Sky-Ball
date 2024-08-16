#if UNITY_EDITOR
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SceneLoader))]
public class SceneLoaderEditor : Editor
{
    private bool showSceneSettings = true;
    private bool showLoadSettings = true;

    public override void OnInspectorGUI()
    {
        SceneLoader script = (SceneLoader)target;

        DrawScriptComponentField(script);

        EditorGUILayout.Space();

        DrawSceneSettingsFields(script);

        EditorGUILayout.Space();

        DrawLoadScreenSettingsFields();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(script);
        }
    }

    private void DrawLoadScreenSettingsFields()
    {
        showLoadSettings = EditorGUILayout.Foldout(showLoadSettings, "Load Screens Settings");

        if (showLoadSettings)
        {
            DrawPropertiesExcluding(serializedObject, "m_Script");
        }
    }

    private void DrawSceneSettingsFields(SceneLoader script)
    {
        showSceneSettings = EditorGUILayout.Foldout(showSceneSettings, "Scene Settings");

        if (showSceneSettings)
        {
            string gameplaySceneLabel = GetNameWithSpaces(nameof(script.GameplayScene));
            script.GameplayScene = (SceneAsset)EditorGUILayout.ObjectField(gameplaySceneLabel, script.GameplayScene, typeof(SceneAsset), false);
        }
    }

    private void DrawScriptComponentField(SceneLoader script)
    {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(script), typeof(SceneLoader), false);
        EditorGUI.EndDisabledGroup();
    }

    private string GetNameWithSpaces(string name)
    {
        return Regex.Replace(name, "(?<!^)([A-Z])", " $1");
    }
}
#endif
