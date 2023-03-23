using System.Collections;
using UnityEditor.Experimental.GraphView;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseNode : Node
{
    public string Guid { get; protected set; }
    private readonly DialogueGraphView m_graphView;

    public BaseNode(DialogueGraphView graphView)
    {
        m_graphView = graphView;
        Guid = System.Guid.NewGuid().ToString();
        GenerateView();
    }
    protected abstract void GenerateView();

    public abstract BaseNodeData Save();

    public abstract void Load(BaseNodeData nodeData, List<NodeLinkData> links);

    protected Port GetPortInstance(Node node, Direction nodeDirection,
            Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, nodeDirection, capacity, typeof(float));
    }

    protected void RemovePort(Node node, Port socket) 
    {
        foreach(Edge edge in socket.connections)
        {
            edge.input.Disconnect(edge);
            m_graphView.RemoveElement(edge);
        }
        node.outputContainer.Remove(socket);
        node.RefreshPorts();
        node.RefreshExpandedState();
    }
}
