using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniFramework.Event;
using UniFramework.Tween;
using UniFramework.Log;
using UnityEngine;

public class Boot : MonoBehaviour
{

//TODO:
/*
1. 将游戏管理器的状态机改为开始游戏
*/
    void Awake()
    {
        Application.targetFrameRate = 60;
        Application.runInBackground = true;
        DontDestroyOnLoad(this.gameObject);

        UniEvent.Initalize(); // 初始化事件系统（确保在 GameManager.Start 之前就绪）
    }

    async UniTaskVoid Start()
    {
        UniTween.Initalize(); // 初始化 Tween 动画系统
        UniLog.Initalize();   // 初始化文件日志系统
        await UniTask.Yield();
        GameManager.Instance.Init(); // 初始化游戏管理器
    }
}
