using UnityEditor;
using UnityEngine;

/// <summary>
/// GameNode 的自定义 PropertyDrawer：
/// 根据 stateType 的选择，只显示对应的 Setting 字段，并支持拖入 Icon。
/// </summary>
[CustomPropertyDrawer(typeof(GameNode))]
public class GameNodeDrawer : PropertyDrawer
{
    private const float LineHeight = 18f;
    private const float Padding = 2f;
    private const float IconPreviewSize = 48f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var iconProp = property.FindPropertyRelative("icon");
        var nameProp = property.FindPropertyRelative("nodeName");
        var stateTypeProp = property.FindPropertyRelative("stateType");
        var battleProp = property.FindPropertyRelative("battleSetting");
        var dialogueProp = property.FindPropertyRelative("dialogueSetting");
        var branchProp = property.FindPropertyRelative("branchSetting");

        // 背景框
        Rect bgRect = new Rect(position.x, position.y, position.width, GetPropertyHeight(property, label));
        EditorGUI.HelpBox(bgRect, string.Empty, MessageType.None);

        float y = position.y + Padding;
        float x = position.x + Padding;
        float w = position.width - Padding * 2;

        // ── 第一行：Icon（带预览） ──
        float row1H = IconPreviewSize;

        // Icon 字段（左） + Icon 预览（右）
        float iconFieldW = w - IconPreviewSize - 8f;
        Rect iconFieldRect = new Rect(x, y, iconFieldW, LineHeight);
        Rect iconPreviewRect = new Rect(x + iconFieldW + 4f, y, IconPreviewSize, IconPreviewSize);

        EditorGUI.PropertyField(iconFieldRect, iconProp, new GUIContent("Icon"));
        DrawIconPreview(iconPreviewRect, iconProp.objectReferenceValue as Sprite);

        y += row1H + Padding;

        // ── 第二行：节点名 ──
        Rect nameRect = new Rect(x, y, w, LineHeight);
        EditorGUI.PropertyField(nameRect, nameProp, new GUIContent("Node Name"));
        y += LineHeight + Padding;

        // ── 第三行：类型枚举（独立一行） ──
        Rect typeRect = new Rect(x, y, w, LineHeight);
        EditorGUI.BeginChangeCheck();
        EditorGUI.PropertyField(typeRect, stateTypeProp, new GUIContent("State Type"));
        if (EditorGUI.EndChangeCheck())
        {
            // 切换类型时清空不相关的 setting
            var newType = (GameState)stateTypeProp.enumValueIndex;
            if (newType != GameState.Battle) battleProp.objectReferenceValue = null;
            if (newType != GameState.DiaLogue) dialogueProp.objectReferenceValue = null;
            if (newType != GameState.Branch) branchProp.objectReferenceValue = null;
        }
        y += LineHeight + Padding;

        // ── 第四行：根据类型显示对应 Setting ──
        var currentType = (GameState)stateTypeProp.enumValueIndex;
        switch (currentType)
        {
            case GameState.Battle:
                var r4_b = new Rect(x, y, w, LineHeight);
                EditorGUI.PropertyField(r4_b, battleProp, new GUIContent("Battle Setting"));
                break;

            case GameState.DiaLogue:
                DrawDialogueSettingInline(ref y, x, w, dialogueProp);
                break;

            case GameState.Branch:
                var r4_br = new Rect(x, y, w, LineHeight);
                EditorGUI.PropertyField(r4_br, branchProp, new GUIContent("Branch Setting"));
                break;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var dialogueProp = property.FindPropertyRelative("dialogueSetting");
        var stateTypeProp = property.FindPropertyRelative("stateType");
        var currentType = (GameState)stateTypeProp.enumValueIndex;

        float extraHeight = 0f;
        if (currentType == GameState.DiaLogue && dialogueProp.objectReferenceValue != null)
        {
            // DialogueSettings 内联：对象引用行 + hasBackground 行 + 可能的 BackGround 行
            extraHeight = LineHeight + Padding; // 对象字段额外一行
            var dialogueObj = dialogueProp.objectReferenceValue;
            var so = new SerializedObject(dialogueObj);
            var hasBg = so.FindProperty("hasBackground");
            if (hasBg != null && hasBg.boolValue)
            {
                extraHeight += LineHeight + Padding; // BackGround 字段
            }
        }

        return IconPreviewSize + LineHeight * 3 + Padding * 5 + extraHeight;
    }

    /// <summary>
    /// 绘制 DialogueSettings 内联引用，支持 hasBackground 勾选后显示 BackGround。
    /// </summary>
    private static void DrawDialogueSettingInline(ref float y, float x, float w, SerializedProperty dialogueProp)
    {
        // 对象引用字段
        Rect objRect = new Rect(x, y, w, LineHeight);
        var oldValue = dialogueProp.objectReferenceValue;
        EditorGUI.PropertyField(objRect, dialogueProp, new GUIContent("Dialogue Setting"));
        y += LineHeight + Padding;

        // 检查对象是否发生变化
        var dialogueObj = dialogueProp.objectReferenceValue;
        if (dialogueObj == null)
            return;

        // 检查引用是否刚被改变，如果是则清空多余的 serializedObject 缓存
        if (oldValue != dialogueObj)
        {
            GUI.changed = true;
        }

        // 绘制 hasBackground 和 BackGround
        var so = new SerializedObject(dialogueObj);
        var hasBgProp = so.FindProperty("hasBackground");
        if (hasBgProp == null)
            return;

        // 缩进以示层级
        float indentX = x + 14f;
        float indentW = w - 14f;

        so.Update();

        // hasBackground 勾选框
        Rect hasBgRect = new Rect(indentX, y, indentW, LineHeight);
        EditorGUI.PropertyField(hasBgRect, hasBgProp, new GUIContent("Has Background"));
        y += LineHeight + Padding;

        // 勾选后才显示 BackGround
        if (hasBgProp.boolValue)
        {
            var bgProp = so.FindProperty("BackGround");
            if (bgProp != null)
            {
                Rect bgRect = new Rect(indentX, y, indentW, LineHeight);
                EditorGUI.PropertyField(bgRect, bgProp, new GUIContent("Background"));
                y += LineHeight + Padding;
            }
        }

        so.ApplyModifiedProperties();
    }

    /// <summary>
    /// 在指定区域绘制 Icon 预览（灰底 + Sprite 纹理）
    /// </summary>
    private static void DrawIconPreview(Rect rect, Sprite sprite)
    {
        Color bgColor = new Color(0.2f, 0.2f, 0.2f);
        EditorGUI.DrawRect(rect, bgColor);

        if (sprite != null && sprite.texture != null)
        {
            Rect texRect = sprite.textureRect;
            float texAspect = texRect.width / texRect.height;
            float rectAspect = rect.width / rect.height;

            Rect drawRect;
            if (texAspect > rectAspect)
            {
                float h = rect.width / texAspect;
                drawRect = new Rect(rect.x, rect.y + (rect.height - h) / 2f, rect.width, h);
            }
            else
            {
                float w = rect.height * texAspect;
                drawRect = new Rect(rect.x + (rect.width - w) / 2f, rect.y, w, rect.height);
            }

            GUI.DrawTexture(drawRect, sprite.texture, ScaleMode.ScaleToFit);
        }
        else
        {
            var labelStyle = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                wordWrap = true,
            };
            GUI.Label(rect, "No Icon", labelStyle);
        }
    }
}
