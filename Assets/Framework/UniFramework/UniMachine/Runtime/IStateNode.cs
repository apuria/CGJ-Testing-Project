namespace UniFramework.Machine
{
    public interface IStateNode
    {
        /// <summary>
        /// 创建时绑定的数据，由 StateMachine.AddNode 注入
        /// </summary>
         void OnCreate(StateMachine machine, IStateData stateData);
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }
}
