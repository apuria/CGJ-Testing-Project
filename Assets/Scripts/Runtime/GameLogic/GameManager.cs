using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Machine;
using UniFramework.Event;

public enum GameState
{
    Battle,
    DiaLogue,
    Branch
}

public class GameManager : SingletonMono<GameManager>
{
//TODO:
/*

*/
    private StateMachine stateMachine;
    private EventGroup eventGroup;
    private PlayerData playerData; 

    public PlayerData PlayerData => playerData;

#region 生命周期
    void Awake()
    {
        stateMachine = new StateMachine();
        eventGroup = new EventGroup();
    }

    void Start()
    {
        
    }

    void Update()
    {
        stateMachine.Update();
    }

    void OnDistroy()
    {
        eventGroup.RemoveAllListener();
    }

#endregion

#region 事件监听
    private void OnHandleEventMessage(IEventMessage message)
    {
        if (message is StateEventDefine.ChangeState changeState)
        {
            stateMachine.SwitchTo(changeState.stateType, changeState.tag, changeState.data, changeState.destroy);
        }
        else if (message is StateEventDefine.BackToPrevState backMsg)
        {
            string prevTag = stateMachine.PreviousNodeTag;
            if (string.IsNullOrEmpty(prevTag))
            {
                Debug.LogError("No previous state to go back to.");
                return;
            }

            if (!stateMachine.IsSuspended(prevTag))
            {
                Debug.LogError($"Previous state '{prevTag}' was destroyed, cannot go back.");
                return;
            }

            stateMachine.SwitchTo(prevTag, backMsg.destroy);
        }
    }
#endregion

#region 玩家数据
    public void SavePlayerData()
    {
        //TODO: 用Json管理器保存玩家数据
        JsonMgr.Instance.SaveData(playerData, "PlayerData");
    }

    public void LoadPlayerData()
    {
        //TODO: 用Json管理器保存玩家数据
        playerData = JsonMgr.Instance.LoadData<PlayerData>("PlayerData");
    }

    public void StartANewGame()
    {
        //TODO: 直接让playerData重置
        playerData = new PlayerData();
    }
#endregion

#region 游戏流程配置

    [SerializeField]
    public GameNode StartNode;
    [SerializeField]
    public GameNode EndNode;

    [SerializeField]
    [Tooltip("游戏流程的 6 个节点配置，每个节点对应一个状态类型及其 Setting")]
    private List<GameNode> gameNodes = new()
    {
        new GameNode { nodeName = "Node 1", stateType = GameState.Battle },
        new GameNode { nodeName = "Node 2", stateType = GameState.Battle },
        new GameNode { nodeName = "Node 3", stateType = GameState.DiaLogue },
        new GameNode { nodeName = "Node 4", stateType = GameState.Battle },
        new GameNode { nodeName = "Node 5", stateType = GameState.DiaLogue },
        new GameNode { nodeName = "Node 6", stateType = GameState.Battle },
    };

    /// <summary>
    /// 获取节点配置列表（只读）
    /// </summary>
    public IReadOnlyList<GameNode> GameNodes => gameNodes;

#endregion

    public void NextNode()
    {
        //TODO:
        /*
        1. 切换到这个节点的状态
        2. 让PlayerData.currentNodeIndex+1
        */
    }

}
