using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueGraphData : ScriptableObject
{
    public LineNodeData StartNode;

    [SerializeReference]
    public List<BaseNodeData> Nodes = new();

    public List<NodeLinkData> NodeLinks = new();
}
