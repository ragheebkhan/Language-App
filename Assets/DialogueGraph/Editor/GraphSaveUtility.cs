using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Linq;
using UnityEngine;
using System;
using UnityEditor;
using System.Collections.Generic;

public class GraphSaveUtility
{
    private DialogueGraphView m_graphView;

    public GraphSaveUtility(DialogueGraphView graphView)
    {
        m_graphView = graphView;
    }

    public void Save(string fileName)
    {
        if(!PerformValidityChecks(fileName))
        {
            return;
        }

        var graphData = ScriptableObject.CreateInstance<DialogueGraphData>();
        SaveNodes(graphData);
        SaveLinks(graphData);

        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            AssetDatabase.CreateFolder("Assets", "Resources");

        AssetDatabase.CreateAsset(graphData, $"Assets/Resources/{fileName}.asset");
    }

    private bool PerformValidityChecks(string fileName)
    {
        if(string.IsNullOrEmpty(fileName))
        {
            EditorUtility.DisplayDialog("Error!", "Graph name cannot be blank!", "OK");
            return false;
        }

        if(m_graphView.edges.Count() == 0)
        {
            EditorUtility.DisplayDialog("Error!", "Cannot save graph with no edges!", "OK");
            return false;
        }

        if((m_graphView.StartNode.outputContainer.Children().First() as Port).connections.Count() == 0)
        {
            EditorUtility.DisplayDialog("Error!", "Start node must have a connection!!", "OK");
            return false;
        }

        return true;
    }

    private void SaveNodes(DialogueGraphData graphData)
    {
        var startNode = new LineNodeData
        {
            Guid = m_graphView.StartNode.Guid,
            DialogueLineText = m_graphView.StartNode.StartText
        };

        graphData.StartNode = startNode;

        foreach(BaseNode node in m_graphView.nodes.Where(x => x is BaseNode))
        {
            graphData.Nodes.Add(node.Save());
        }
    }

    private void SaveLinks(DialogueGraphData graphData)
    {
        var startLink = new NodeLinkData
        {
            BaseNodeGuid = m_graphView.StartNode.Guid,
            TargetNodeGuid = (m_graphView.StartNode.outputContainer.Q<Port>().connections.ToList()[0].input.node as BaseNode).Guid,
            ChoiceText = m_graphView.StartNode.StartText
        };

        graphData.NodeLinks.Add(startLink);

        foreach(BaseNode node in m_graphView.nodes.Where(x => x is BaseNode))
        {
            foreach(Port port in node.outputContainer.Children())
            {
                if(!port.connected)
                {
                    var emptyLinkData = new NodeLinkData
                    {
                        BaseNodeGuid = node.Guid,
                        TargetNodeGuid = null,
                        ChoiceText = port.portName
                    };
                    graphData.NodeLinks.Add(emptyLinkData);
                    continue;
                }
                var linkData = new NodeLinkData
                {
                    BaseNodeGuid = node.Guid,
                    TargetNodeGuid = (port.connections.ToList()[0].input.node as BaseNode).Guid,
                    ChoiceText = port.portName
                };
                graphData.NodeLinks.Add(linkData);
            }
        }
    }

    public void Load(String fileName)
    {
        var graphData = AssetDatabase.LoadAssetAtPath($"Assets/Resources/{fileName}.asset", typeof(DialogueGraphData)) as DialogueGraphData;
        if (graphData == null)
        {
            EditorUtility.DisplayDialog("Error!", "No file with that name", "OK");
            return;
        }

        m_graphView.graphElements.ToList().ForEach(x => m_graphView.RemoveElement(x));
        AddNodes(graphData);
        LinkNodes(graphData);
    }

    public void AddNodes(DialogueGraphData graphData)
    {
        m_graphView.AddStartNode(graphData.StartNode.DialogueLineText, graphData.StartNode.Guid);
        foreach(BaseNodeData node in graphData.Nodes)
        {
            Type t = Type.GetType((node.GetType().ToString()).Substring(0, node.GetType().ToString().Length - 4));
            dynamic newNode = Activator.CreateInstance(t, new object[] { m_graphView });

            m_graphView.AddElement(newNode);
            newNode.Load(node, graphData.NodeLinks.Where(x => x.BaseNodeGuid == node.Guid).ToList());
        }
    }

    public void LinkNodes(DialogueGraphData graphData)
    {
        string startTargetGuid = graphData.NodeLinks.Find(x => x.BaseNodeGuid == m_graphView.StartNode.Guid).TargetNodeGuid;
        Port startConnectionPort = m_graphView.ports.ToList().Find(x => (x.direction == Direction.Input)
        && (x.node as BaseNode).Guid == startTargetGuid);

        m_graphView.AddElement(m_graphView.StartNode.outputContainer.Q<Port>().ConnectTo(startConnectionPort));

        foreach (BaseNode node in m_graphView.nodes.Where(x => x is BaseNode))
        {
            foreach (Port port in node.outputContainer.Children())
            {
                string targetGuid = graphData.NodeLinks.Find(x => (x.BaseNodeGuid == (port.node as BaseNode).Guid) && 
                (x.ChoiceText == port.portName)).TargetNodeGuid;

                if (string.IsNullOrEmpty(targetGuid))
                    continue;

                Port targetPort = m_graphView.ports.ToList().Find(x => (x.direction == Direction.Input)
                && (x.node as BaseNode).Guid == targetGuid);

                m_graphView.AddElement(port.ConnectTo(targetPort));
            }
        }
    }
}
