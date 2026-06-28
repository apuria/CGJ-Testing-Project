using System.Collections;
using System.Collections.Generic;
using UniFramework.Event;
using UnityEngine;
using UnityEngine.UI;

public class MapPanel : BasePanel
{
//TODO:
/*
1. 6个节点的图标
2. 退出按钮
3. 继续按钮
*/

#region 属性

    public Button exitButton;
    public Button continueButton;

    public Image node1;
    public Image node2;
    public Image node3;
    public Image node4;
    public Image node5;
    public Image node6;

    private List<Image> nodeImages;

#endregion
#region 生命周期

    protected void Awake()
    {
        eventGroup = new();
        nodeImages = new List<Image> { node1, node2, node3, node4, node5, node6 };
    }

    protected void Start()
    {
        eventGroup.AddListener<SceneEventDefine.UpdateMapUI>(OnHandleEventMessage);

        exitButton.onClick.AddListener(() =>
        {
            TipPanelEventDefine.ShowTip.SendEventMessage("确定要退出到主菜单吗？", "确认", () =>
            {
                GameEventDefine.ReturnToMainMenu.SendEventMessage();
            }, "取消", null);
        });

        continueButton.onClick.AddListener(() =>
        {
            SceneEventDefine.NodeGame.SendEventMessage();
        });
    }

    protected void OnDestroy()
    {
        eventGroup.RemoveAllListener();
    }
#endregion

#region 逻辑控制

    public override void HideMe()
    {

    }

    public override void ShowMe()
    {
        UpdateNodes();
    }

    /// <summary>
    /// 更新六个节点的图标状态
    /// </summary>
    private void UpdateNodes()
    {
        var gameNodes = GameManager.Instance.GameNodes;
        var nowFlow = GameManager.Instance.PlayerData.NowFlow;

        for (int i = 0; i < nodeImages.Count && i < gameNodes.Count; i++)
        {
            int nodeIndex = i + 1; // Node1 对应索引 1

            // 设置节点图标
            if (gameNodes[i].icon != null)
            {
                nodeImages[i].sprite = gameNodes[i].icon;
                nodeImages[i].enabled = true;
            }
            else
            {
                nodeImages[i].enabled = false;
            }

            //TODO: 根据当前流程设置节点外观
            /*
            if (nodeIndex < (int)nowFlow)
                已完成的节点（变灰/打勾）
            else if (nodeIndex == (int)nowFlow)
                当前节点（高亮/发光）
            else
                未解锁节点（锁定/暗色）
            */
        }
    }

#endregion

#region 事件监听

    public void OnHandleEventMessage(IEventMessage message)
    {
        if (message is SceneEventDefine.UpdateMapUI)
        {
            UpdateNodes();
        }
    }

#endregion
}
