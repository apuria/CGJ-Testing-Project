using UnityEditor;
using UnityEngine;

/// <summary>
/// BattleSetting 的自定义 Editor：
/// 根据 startEvent / endEvent / 每个 midEvent 的 triggerType 和 action，只显示对应的 Setting 字段。
/// </summary>
[CustomEditor(typeof(BattleSetting))]
public class BattleSettingEditor : Editor
{
    private SerializedProperty battleSceneNameProp;
    private SerializedProperty enemiesProp;

    // 开始事件
    private SerializedProperty startEventProp;
    private SerializedProperty startDialogueProp;

    // 结束事件
    private SerializedProperty endEventProp;
    private SerializedProperty endDialogueProp;
    private SerializedProperty endBranchProp;

    // 中途事件列表
    private SerializedProperty midEventsProp;

    private void OnEnable()
    {
        battleSceneNameProp = serializedObject.FindProperty("battleSceneName");
        enemiesProp = serializedObject.FindProperty("enemies");

        startEventProp = serializedObject.FindProperty("startEvent");
        startDialogueProp = serializedObject.FindProperty("startDialogue");

        endEventProp = serializedObject.FindProperty("endEvent");
        endDialogueProp = serializedObject.FindProperty("endDialogue");
        endBranchProp = serializedObject.FindProperty("endBranch");

        midEventsProp = serializedObject.FindProperty("midEvents");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // ── 战斗场景 ──
        EditorGUILayout.PropertyField(battleSceneNameProp, new GUIContent("Battle Scene Name"));

        EditorGUILayout.Space();

        // ── 敌人列表 ──
        EditorGUILayout.PropertyField(enemiesProp, new GUIContent("Enemies"), true);

        EditorGUILayout.Space();

        // ── 开始事件 ──
        DrawStartEvent();

        EditorGUILayout.Space();

        // ── 结束事件 ──
        DrawEndEvent();

        EditorGUILayout.Space();

        // ── 中途事件列表 ──
        DrawMidEvents();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawStartEvent()
    {
        EditorGUILayout.LabelField("Start Event", EditorStyles.boldLabel);
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(startEventProp, new GUIContent("Start Event"));
        if (EditorGUI.EndChangeCheck())
        {
            var newStartEvent = (EBattleStartEvent)startEventProp.enumValueIndex;
            if (newStartEvent != EBattleStartEvent.PlayDialogue) startDialogueProp.objectReferenceValue = null;
        }

        var startEvent = (EBattleStartEvent)startEventProp.enumValueIndex;
        switch (startEvent)
        {
            case EBattleStartEvent.PlayDialogue:
                EditorGUILayout.PropertyField(startDialogueProp, new GUIContent("Start Dialogue"));
                break;

            case EBattleStartEvent.None:
                EditorGUILayout.HelpBox("不需要额外配置，战斗将直接开始。", MessageType.Info);
                break;
        }
    }

    private void DrawEndEvent()
    {
        EditorGUILayout.LabelField("End Event", EditorStyles.boldLabel);
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(endEventProp, new GUIContent("End Event"));
        if (EditorGUI.EndChangeCheck())
        {
            var newEndEvent = (EBattleEndEvent)endEventProp.enumValueIndex;
            if (newEndEvent != EBattleEndEvent.NextDialogue) endDialogueProp.objectReferenceValue = null;
            if (newEndEvent != EBattleEndEvent.StartBranch) endBranchProp.objectReferenceValue = null;
        }

        var endEvent = (EBattleEndEvent)endEventProp.enumValueIndex;
        switch (endEvent)
        {
            case EBattleEndEvent.NextDialogue:
                EditorGUILayout.PropertyField(endDialogueProp, new GUIContent("End Dialogue"));
                break;

            case EBattleEndEvent.StartBranch:
                EditorGUILayout.PropertyField(endBranchProp, new GUIContent("End Branch"));
                break;

            case EBattleEndEvent.None:
                EditorGUILayout.HelpBox("不需要额外配置。", MessageType.Info);
                break;

            case EBattleEndEvent.GoBackToLastState:
                EditorGUILayout.HelpBox("不需要额外配置，将回到上一个状态。", MessageType.Info);
                break;

            case EBattleEndEvent.GoBackToMap:
                EditorGUILayout.HelpBox("不需要额外配置，将回到地图。", MessageType.Info);
                break;
        }
    }

    private void DrawMidEvents()
    {
        EditorGUILayout.LabelField("Mid Events", EditorStyles.boldLabel);

        if (midEventsProp == null) return;

        for (int i = 0; i < midEventsProp.arraySize; i++)
        {
            var elementProp = midEventsProp.GetArrayElementAtIndex(i);
            DrawMidEventElement(elementProp, i);
            EditorGUILayout.Space();
        }

        // ── 添加 / 清空按钮 ──
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("+ Add Mid Event"))
        {
            midEventsProp.arraySize++;
            // 新元素的默认值由 MidBattleEvent 类中的字段默认值处理
        }

        if (midEventsProp.arraySize > 0)
        {
            if (GUILayout.Button("Remove Last"))
            {
                midEventsProp.arraySize--;
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawMidEventElement(SerializedProperty elementProp, int index)
    {
        var triggerTypeProp = elementProp.FindPropertyRelative("triggerType");
        var targetIndexProp = elementProp.FindPropertyRelative("targetIndex");
        var hpPercentageProp = elementProp.FindPropertyRelative("hpPercentage");
        var turnCountProp = elementProp.FindPropertyRelative("turnCount");
        var actionProp = elementProp.FindPropertyRelative("action");
        var dialogueProp = elementProp.FindPropertyRelative("dialogue");
        var branchProp = elementProp.FindPropertyRelative("branch");

        // ── 标题与删除 ──
        var triggerType = (EMidEventTriggerType)triggerTypeProp.enumValueIndex;
        var action = (EMidEventAction)actionProp.enumValueIndex;
        string summary = GetMidEventSummary(triggerType, action,
            triggerType == EMidEventTriggerType.TurnCount ? turnCountProp.intValue : hpPercentageProp.intValue,
            targetIndexProp.intValue, index);

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField(summary, EditorStyles.boldLabel);

        EditorGUI.indentLevel++;

        // ── 触发条件类型 ──
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(triggerTypeProp, new GUIContent("Trigger Type"));
        if (EditorGUI.EndChangeCheck())
        {
            // 切换触发类型时，重置对应字段为默认值
            targetIndexProp.intValue = 0;
            hpPercentageProp.intValue = 50;
            turnCountProp.intValue = 1;
        }

        var currentTriggerType = (EMidEventTriggerType)triggerTypeProp.enumValueIndex;
        switch (currentTriggerType)
        {
            case EMidEventTriggerType.CharacterHpThreshold:
                EditorGUILayout.PropertyField(targetIndexProp, new GUIContent("角色索引"));
                EditorGUILayout.PropertyField(hpPercentageProp, new GUIContent("血量百分比"));
                EditorGUILayout.HelpBox($"当角色[{targetIndexProp.intValue}]的血量 ≤ {hpPercentageProp.intValue}% 时触发", MessageType.None);
                break;

            case EMidEventTriggerType.EnemyHpThreshold:
                EditorGUILayout.PropertyField(targetIndexProp, new GUIContent("敌人索引"));
                EditorGUILayout.PropertyField(hpPercentageProp, new GUIContent("血量百分比"));
                EditorGUILayout.HelpBox($"当敌人[{targetIndexProp.intValue}]的血量 ≤ {hpPercentageProp.intValue}% 时触发", MessageType.None);
                break;

            case EMidEventTriggerType.TurnCount:
                EditorGUILayout.PropertyField(turnCountProp, new GUIContent("回合数"));
                EditorGUILayout.HelpBox($"到达第 {turnCountProp.intValue} 回合时触发", MessageType.None);
                break;
        }

        EditorGUILayout.Space();

        // ── 触发后做什么 ──
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(actionProp, new GUIContent("Action"));
        if (EditorGUI.EndChangeCheck())
        {
            var newAction = (EMidEventAction)actionProp.enumValueIndex;
            if (newAction != EMidEventAction.PlayDialogue) dialogueProp.objectReferenceValue = null;
            if (newAction != EMidEventAction.StartBranch) branchProp.objectReferenceValue = null;
        }

        var currentAction = (EMidEventAction)actionProp.enumValueIndex;
        switch (currentAction)
        {
            case EMidEventAction.PlayDialogue:
                EditorGUILayout.PropertyField(dialogueProp, new GUIContent("Dialogue"));
                break;

            case EMidEventAction.StartBranch:
                EditorGUILayout.PropertyField(branchProp, new GUIContent("Branch"));
                break;
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }

    private static string GetMidEventSummary(EMidEventTriggerType triggerType, EMidEventAction action,
        int value, int targetIndex, int index)
    {
        string triggerDesc = triggerType switch
        {
            EMidEventTriggerType.CharacterHpThreshold => $"角色[{targetIndex}] HP ≤ {value}%",
            EMidEventTriggerType.EnemyHpThreshold => $"敌人[{targetIndex}] HP ≤ {value}%",
            EMidEventTriggerType.TurnCount => $"第 {value} 回合",
            _ => "未知",
        };

        string actionDesc = action switch
        {
            EMidEventAction.PlayDialogue => "播放对话",
            EMidEventAction.StartBranch => "开始分支",
            _ => "未知",
        };

        return $"Mid Event [{index}] | {triggerDesc} → {actionDesc}";
    }
}
