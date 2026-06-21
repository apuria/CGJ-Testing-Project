using UniFramework.Machine;

/// <summary>
/// 状态节点基类，实现 IStateNode 接口。
/// 子类可通过 this.StateData 获取创建时绑定的数据，通过 Machine 访问状态机管理器。
/// </summary>
public class BaseState : IStateNode
{
    /// <summary>
    /// 持有此节点的状态机管理器引用
    /// </summary>
    protected StateMachine Machine { get; private set; }

    public virtual void OnCreate(StateMachine machine, IStateData data)
    {
        Machine = machine;
    }

    public virtual void OnEnter()
    {

    }

    public virtual void OnExit()
    {

    }

    public virtual void OnUpdate()
    {

    }
}
