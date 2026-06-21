using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景切换管理器 主要用于切换场景
/// </summary>
public class SceneMgr : SingletonAutoMono<SceneMgr>
{
    private SceneMgr() { }

    //同步切换场景的方法
    public void LoadScene(string name, UnityAction callBack = null)
    {
        //切换场景
        SceneManager.LoadScene(name);
        //调用回调
        callBack?.Invoke();
    }

    //异步切换场景的方法（UniTask 版本）
    public async UniTaskVoid LoadSceneAsyn(string name, UnityAction callBack = null)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
        //每帧检测是否加载结束 如果加载结束就不会进这个循环了
        while (!ao.isDone)
        {
            //利用事件中心 每一帧将进度发送给订阅者（如 Loading 界面）
            // EventCenter.Instance.EventTrigger<float>(E_EventType.E_SceneLoadChange, ao.progress);
            await UniTask.Yield();
        }
        //避免最后一帧直接结束了 没有把完整进度同步出去
        // EventCenter.Instance.EventTrigger<float>(E_EventType.E_SceneLoadChange, 1);

        callBack?.Invoke();
    }
}
