using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniFramework.Event;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialoguePanel : BasePanel
{
/*
1. 显示对话内容:
    1. 背景图片设置
2. 更新对话内容:
    1. 对话内容设置
    2. 对话者设置
3. 点击发送事件

1. 角色图片
2. 对话内容
3. 背景图片
4. 角色名字
5. 退出按钮
*/

#region 属性

    public Image background; // 背景图片
    public Image leftRole; // 左侧角色图片
    public Image rightRole; // 右侧角色图片
    public Image leftRoleBg; // 左侧角色背景图片
    public Image rightRoleBg; // 右侧角色背景图片
    public TextMeshProUGUI content;
    public TextMeshProUGUI leftRoleName;
    public TextMeshProUGUI rightRoleName;
    public Button quitButton;
    public Button finishButton;
    public Button continueButton;

    public List<Speaker> speakers = new List<Speaker>();

    private CancellationTokenSource typewriterCts;
    private string currentFullText;

#endregion

#region 生命周期

    protected void Awake()
    {
        eventGroup = new();
        eventGroup.AddListener<DiaLogueEventDefine.ShowUI>(OnHandleEventMessage);
        eventGroup.AddListener<DiaLogueEventDefine.UpdateUI>(OnHandleEventMessage);
    }

    protected void Start()
    {
        quitButton.onClick.AddListener(() =>
        {
            TipPanelEventDefine.ShowTip.SendEventMessage("确定要退出对话吗？", "确认", () =>
            {
                // 关闭对话面板
                UIMgr.Instance.HidePanel<DialoguePanel>();
                // 返回地图
                StateEventDefine.ChangeState.SendEventMessage<MapState>("MapState");
            }, "取消", null);
        });

        finishButton.onClick.AddListener(() =>
        {
            // 直接完成文字显示
            if(typewriterCts != null)
            {
                typewriterCts.Cancel();
                typewriterCts.Dispose();
                typewriterCts = null;
            }
            content.text = currentFullText;
            finishButton.gameObject.SetActive(false);
            continueButton.gameObject.SetActive(true);
        });

        continueButton.onClick.AddListener(() =>
        {
            // 发送事件, 下一句对话
            DiaLogueEventDefine.Next.SendEventMessage();
        });
    }

    protected void OnDestroy()
    {
        typewriterCts?.Cancel();
        typewriterCts?.Dispose();
        typewriterCts = null;
        eventGroup.RemoveAllListener();
    }
#endregion

#region 逻辑控制

    public override void HideMe()
    {

    }

    public override void ShowMe()
    {

    }

    private async UniTaskVoid TypewriterEffect(string text, CancellationToken ct)
    {
        content.text = "";
        for(int i = 0; i < text.Length; i++)
        {
            content.text += text[i];
            try
            {
                await UniTask.Delay(50, cancellationToken: ct);
            }
            catch(System.OperationCanceledException)
            {
                return;
            }
        }
        // 打字完成
        finishButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(true);
        typewriterCts?.Dispose();
        typewriterCts = null;
    }

#endregion

#region 事件监听

    public void OnHandleEventMessage(IEventMessage message)
    {
        if(message is DiaLogueEventDefine.ShowUI showUI)
        {
            speakers = showUI.speakers;
            background.gameObject.SetActive(showUI.hasBackground);
            if (showUI.hasBackground)
            {
                background.sprite = showUI.BackGround;
            }
        }
        else if(message is DiaLogueEventDefine.UpdateUI updateUI)
        {
            // 取消上一次的打字效果
            if(typewriterCts != null)
            {
                typewriterCts.Cancel();
                typewriterCts.Dispose();
                typewriterCts = null;
            }

            currentFullText = updateUI.content;

            // 设置左侧角色
            if(updateUI.leftRoleIndex >= 0 && updateUI.leftRoleIndex < speakers.Count)
            {
                var leftSpeaker = speakers[updateUI.leftRoleIndex];
                bool hasArt = leftSpeaker?.CharaArtwork != null;
                leftRoleBg.gameObject.SetActive(hasArt);
                leftRole.gameObject.SetActive(hasArt);
                leftRoleName.gameObject.SetActive(leftSpeaker != null);
                if (hasArt)
                    leftRole.sprite = leftSpeaker.CharaArtwork;
                if (leftSpeaker != null)
                    leftRoleName.text = leftSpeaker.name;
                leftRole.color = updateUI.speaker == TalkingSpeaker.Left ? Color.white : Color.gray;
                leftRoleName.color = updateUI.speaker == TalkingSpeaker.Left ? Color.white : Color.gray;
            }
            else
            {
                leftRoleBg.gameObject.SetActive(false);
                leftRole.gameObject.SetActive(false);
                leftRoleName.gameObject.SetActive(false);
            }

            // 设置右侧角色
            if(updateUI.rightRoleIndex >= 0 && updateUI.rightRoleIndex < speakers.Count)
            {
                var rightSpeaker = speakers[updateUI.rightRoleIndex];
                bool hasArt = rightSpeaker?.CharaArtwork != null;
                rightRoleBg.gameObject.SetActive(hasArt);
                rightRole.gameObject.SetActive(hasArt);
                rightRoleName.gameObject.SetActive(rightSpeaker != null);
                if (hasArt)
                    rightRole.sprite = rightSpeaker.CharaArtwork;
                if (rightSpeaker != null)
                    rightRoleName.text = rightSpeaker.name;
                rightRole.color = updateUI.speaker == TalkingSpeaker.Right ? Color.white : Color.gray;
                rightRoleName.color = updateUI.speaker == TalkingSpeaker.Right ? Color.white : Color.gray;
            }
            else
            {
                rightRoleBg.gameObject.SetActive(false);
                rightRole.gameObject.SetActive(false);
                rightRoleName.gameObject.SetActive(false);
            }

            // 开始打字效果
            continueButton.gameObject.SetActive(false);
            finishButton.gameObject.SetActive(true);
            typewriterCts = new CancellationTokenSource();
            TypewriterEffect(currentFullText, typewriterCts.Token).Forget();
        }
    }

#endregion  
}
