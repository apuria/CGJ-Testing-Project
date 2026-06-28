using UnityEditor;
using UnityEngine;

/// <summary>
/// Dialogue 的自定义 PropertyDrawer：
/// 根据 onEnd 的选择，只显示对应的 Setting 字段（Battle、Branch 或 NextLog）。
/// </summary>
[CustomPropertyDrawer(typeof(Dialogue))]
public class DialogueDrawer : PropertyDrawer
{
    private const float LineHeight = 18f;
    private const float Padding = 2f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var leftIdxProp = property.FindPropertyRelative("leftSpeakerIndex");
        var rightIdxProp = property.FindPropertyRelative("rightSpeakerIndex");
        var talkProp = property.FindPropertyRelative("talkingSpeaker");
        var textProp = property.FindPropertyRelative("text");
        var onEndProp = property.FindPropertyRelative("onEnd");
        var battleProp = property.FindPropertyRelative("Battle");
        var branchProp = property.FindPropertyRelative("Branch");
        var nextLogProp = property.FindPropertyRelative("NextLog");

        float y = position.y;
        float x = position.x;
        float w = position.width;

        // ── 第 1 行：左角色索引 ──
        Rect r1 = new Rect(x, y, w, LineHeight);
        EditorGUI.PropertyField(r1, leftIdxProp, new GUIContent("Left Speaker Index"));
        y += LineHeight + Padding;

        // ── 第 2 行：右角色索引 ──
        Rect r2 = new Rect(x, y, w, LineHeight);
        EditorGUI.PropertyField(r2, rightIdxProp, new GUIContent("Right Speaker Index"));
        y += LineHeight + Padding;

        // ── 第 3 行：说话者方向 ──
        Rect r3 = new Rect(x, y, w, LineHeight);
        EditorGUI.PropertyField(r3, talkProp, new GUIContent("Talking Speaker"));
        y += LineHeight + Padding;

        // ── 第 4 行：对话文本（使用 PropertyField 自动适配 TextArea） ──
        float textH = EditorGUI.GetPropertyHeight(textProp, new GUIContent("Text"));
        Rect r4 = new Rect(x, y, w, textH);
        EditorGUI.PropertyField(r4, textProp, new GUIContent("Text"));
        y += textH + Padding;

        // ── 第 5 行：对话结束行为 ──
        Rect r5 = new Rect(x, y, w, LineHeight);
        EditorGUI.BeginChangeCheck();
        EditorGUI.PropertyField(r5, onEndProp, new GUIContent("On End"));
        if (EditorGUI.EndChangeCheck())
        {
            // 切换时清空不相关的 Setting
            var newOnEnd = (EOnEnd)onEndProp.enumValueIndex;
            if (newOnEnd != EOnEnd.StartBattle) battleProp.objectReferenceValue = null;
            if (newOnEnd != EOnEnd.StartBranch) branchProp.objectReferenceValue = null;
            if (newOnEnd != EOnEnd.NextLog) nextLogProp.objectReferenceValue = null;
        }
        y += LineHeight + Padding;

        // ── 第 6 行（条件）：根据 onEnd 显示对应 Setting ──
        var onEnd = (EOnEnd)onEndProp.enumValueIndex;
        if (onEnd == EOnEnd.StartBattle)
        {
            Rect r6 = new Rect(x, y, w, LineHeight);
            EditorGUI.PropertyField(r6, battleProp, new GUIContent("Battle"));
        }
        else if (onEnd == EOnEnd.StartBranch)
        {
            Rect r6 = new Rect(x, y, w, LineHeight);
            EditorGUI.PropertyField(r6, branchProp, new GUIContent("Branch Dialogue"));
        }
        else if (onEnd == EOnEnd.NextLog)
        {
            Rect r6 = new Rect(x, y, w, LineHeight);
            EditorGUI.PropertyField(r6, nextLogProp, new GUIContent("Next Log"));
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var onEndProp = property.FindPropertyRelative("onEnd");
        var textProp = property.FindPropertyRelative("text");
        var onEnd = (EOnEnd)onEndProp.enumValueIndex;

        float textH = EditorGUI.GetPropertyHeight(textProp, new GUIContent("Text"));

        // 5 行固定 + TextArea 自适应 + 行间距
        float baseHeight = LineHeight * 4 + textH + Padding * 5;

        // Battle、Branch 或 NextLog 时多一行
        if (onEnd == EOnEnd.StartBattle || onEnd == EOnEnd.StartBranch || onEnd == EOnEnd.NextLog)
        {
            baseHeight += LineHeight + Padding;
        }

        return baseHeight;
    }
}
