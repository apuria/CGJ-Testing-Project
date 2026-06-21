using System;
using System.Collections.Generic;

namespace UniFramework.Machine
{
    /// <summary>
    /// 状态机管理器：编排 IStateNode 的注册、切换、挂起、销毁
    /// </summary>
    public class StateMachine
    {
        /// <summary>
        /// 所有注册的节点，tag → IStateNode
        /// </summary>
        private readonly Dictionary<string, IStateNode> _nodes = new();
        private readonly Dictionary<string, IStateNode> _suspendedNodes = new();
        private readonly Dictionary<string, object> _blackboard = new();

        /// <summary>
        /// 当前活跃的节点
        /// </summary>
        private IStateNode _curNode;

        /// <summary>
        /// 当前活跃节点的 Tag
        /// </summary>
        private string _curNodeTag;

        /// <summary>
        /// 上一个节点的 Tag（用于追踪）
        /// </summary>
        private string _preNodeTag;

        /// <summary>
        /// 当前运行的节点 Tag
        /// </summary>
        public string CurrentNodeTag => _curNodeTag ?? string.Empty;

        /// <summary>
        /// 之前运行的节点 Tag
        /// </summary>
        public string PreviousNodeTag => _preNodeTag ?? string.Empty;

        public StateMachine() { }

        // =========================================================
        // Update
        // =========================================================

        /// <summary>
        /// 更新当前活跃的节点
        /// </summary>
        public void Update()
        {
            _curNode?.OnUpdate();
        }

        // =========================================================
        // 注册节点（"创建状态机"）
        // =========================================================

        /// <summary>
        /// 注册一个节点（创建"状态机"），泛型版本
        /// </summary>
        /// <typeparam name="TNode">实现 IStateNode 且有 new() 的节点类型</typeparam>
        /// <param name="tag">字符串标记，用于切换和恢复</param>
        /// <param name="data">绑定到此节点的数据</param>
        public void AddNode<TNode>(string tag, IStateData data) where TNode : IStateNode, new()
        {
            AddNode(tag, new TNode(), data);
        }

        /// <summary>
        /// 注册一个节点（创建"状态机"），实例版本
        /// </summary>
        /// <param name="tag">字符串标记，用于切换和恢复</param>
        /// <param name="node">实现了 IStateNode 的节点实例</param>
        /// <param name="data">绑定到此节点的数据</param>
        public void AddNode(string tag, IStateNode node, IStateData data)
        {
            if (string.IsNullOrEmpty(tag))
                throw new ArgumentNullException(nameof(tag));
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (_nodes.ContainsKey(tag))
            {
                UniLogger.Error($"State node tag already existed: {tag}");
                return;
            }
            node.OnCreate(this, data);
            _nodes.Add(tag, node);
        }

        // =========================================================
        // 启动
        // =========================================================

        /// <summary>
        /// 启动状态机，进入入口节点
        /// </summary>
        /// <param name="tag">入口节点的 Tag</param>
        public void Run(string tag)
        {
            if (_curNode != null)
            {
                UniLogger.Warning($"State machine already running at '{_curNodeTag}', fallback to ChangeState.");
                ChangeState(tag);
                return;
            }

            if (!_nodes.TryGetValue(tag, out var node))
                throw new Exception($"Not found entry node: {tag}");

            _curNode = node;
            _curNodeTag = tag;
            _preNodeTag = null;

            UniLogger.Log($"Start state machine: {tag}");
            _curNode.OnEnter();
        }

        // =========================================================
        // 销毁式切换
        // =========================================================

        /// <summary>
        /// 销毁式切换：退出当前节点并丢弃（不可恢复），进入目标节点
        /// </summary>
        /// <param name="tag">目标节点的 Tag</param>
        public void ChangeState(string tag)
        {
            if (string.IsNullOrEmpty(tag))
                throw new ArgumentNullException(nameof(tag));

            if (_curNode == null)
            {
                Run(tag);
                return;
            }

            if (!_nodes.TryGetValue(tag, out var targetNode))
            {
                UniLogger.Error($"Can not found state node: {tag}");
                return;
            }

            UniLogger.Log($"{_curNodeTag} --> {tag} (destroy mode)");

            // 退出当前节点并丢弃
            _curNode.OnExit();

            _preNodeTag = _curNodeTag;
            _curNode = targetNode;
            _curNodeTag = tag;

            // 如果目标在挂起池中，移除（已恢复）
            _suspendedNodes.Remove(tag);

            _curNode.OnEnter();
        }

        // =========================================================
        // 挂起式切换
        // =========================================================

        /// <summary>
        /// 挂起式切换：退出当前节点并存入字典（可恢复），进入目标节点
        /// </summary>
        /// <param name="tag">目标节点的 Tag</param>
        public void SuspendAndChange(string tag)
        {
            if (string.IsNullOrEmpty(tag))
                throw new ArgumentNullException(nameof(tag));

            if (_curNode == null)
            {
                Run(tag);
                return;
            }

            if (!_nodes.TryGetValue(tag, out var targetNode))
            {
                UniLogger.Error($"Can not found state node: {tag}");
                return;
            }

            UniLogger.Log($"{_curNodeTag} --> {tag} (suspend mode, saved '{_curNodeTag}')");

            // 挂起当前节点
            _curNode.OnExit();
            _suspendedNodes[_curNodeTag] = _curNode;

            _preNodeTag = _curNodeTag;
            _curNode = targetNode;
            _curNodeTag = tag;

            // 如果目标在挂起池中，移除
            _suspendedNodes.Remove(tag);

            _curNode.OnEnter();
        }

        // =========================================================
        // 恢复
        // =========================================================

        /// <summary>
        /// 恢复被挂起的节点：挂起当前，从字典恢复目标
        /// </summary>
        /// <param name="tag">要恢复的挂起节点 Tag</param>
        public void Resume(string tag)
        {
            if (string.IsNullOrEmpty(tag))
                throw new ArgumentNullException(nameof(tag));

            if (!_suspendedNodes.TryGetValue(tag, out var targetNode))
            {
                UniLogger.Error($"Can not found suspended state node: {tag}");
                return;
            }

            // 挂起当前节点
            if (_curNode != null)
            {
                UniLogger.Log($"{_curNodeTag} --> {tag} (resume, saved '{_curNodeTag}')");
                _curNode.OnExit();
                _suspendedNodes[_curNodeTag] = _curNode;
            }

            _suspendedNodes.Remove(tag);
            _preNodeTag = _curNodeTag;
            _curNode = targetNode;
            _curNodeTag = tag;

            _curNode.OnEnter();
        }

        // =========================================================
        // 查询
        // =========================================================

        /// <summary>
        /// 查询指定 Tag 的节点是否被挂起
        /// </summary>
        public bool IsSuspended(string tag)
        {
            return _suspendedNodes.ContainsKey(tag);
        }

        /// <summary>
        /// 获取挂起池中指定 Tag 的节点（不会恢复该节点）
        /// </summary>
        public IStateNode GetSuspendedNode(string tag)
        {
            _suspendedNodes.TryGetValue(tag, out var node);
            return node;
        }

        // =========================================================
        // 黑板
        // =========================================================

        /// <summary>
        /// 设置黑板数据（跨节点共享）
        /// </summary>
        public void SetBlackboardValue(string key, object value)
        {
            if (_blackboard.ContainsKey(key) == false)
                _blackboard.Add(key, value);
            else
                _blackboard[key] = value;
        }

        /// <summary>
        /// 获取黑板数据
        /// </summary>
        public object GetBlackboardValue(string key)
        {
            if (_blackboard.TryGetValue(key, out object value))
                return value;

            UniLogger.Warning($"Not found blackboard value: {key}");
            return null;
        }
    }
}
