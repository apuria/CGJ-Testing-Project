using UnityEditor;
using UnityEngine;

/// <summary>
/// DialogueSettings 的自定义 Editor：
/// hasBackground 勾选后才显示 BackGround 字段。
/// 其余字段保持默认显示效果不变。
/// </summary>
[CustomEditor(typeof(DialogueSettings))]
public class DialogueSettingsEditor : Editor
{
    private SerializedProperty hasBackgroundProp;
    private SerializedProperty backGroundProp;
    private SerializedProperty speakersProp;
    private SerializedProperty dialoguesProp;

    private void OnEnable()
    {
        hasBackgroundProp = serializedObject.FindProperty("hasBackground");
        backGroundProp = serializedObject.FindProperty("BackGround");
        speakersProp = serializedObject.FindProperty("speakers");
        dialoguesProp = serializedObject.FindProperty("dialogues");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // ── hasBackground ──
        EditorGUILayout.PropertyField(hasBackgroundProp);

        // ── BackGround（hasBackground 勾选时才显示） ──
        if (hasBackgroundProp.boolValue)
        {
            EditorGUILayout.PropertyField(backGroundProp);
        }

        // ── 其余字段（保持默认显示） ──
        EditorGUILayout.PropertyField(speakersProp, true);
        EditorGUILayout.PropertyField(dialoguesProp, true);

        serializedObject.ApplyModifiedProperties();
    }
}
