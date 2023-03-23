using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEditor;

public class DialogueGraphView : GraphView
{
    public new class UxmlFactory : UxmlFactory<DialogueGraphView, GraphView.UxmlTraits> { }

    public StartNode StartNode { get; private set; }

    public DialogueGraphView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(new FreehandSelector());
        this.AddManipulator(new ContentZoomer());

        AddStartNode();

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/DialogueGraph/Editor/DialogueGraphEditorWindow.uss");
        styleSheets.Add(styleSheet);
    }

    public void AddStartNode(string startText = null, string startGuid = null)
    {
        if(string.IsNullOrEmpty(startText))
        {
            StartNode = new();
        }
        else
        {
            StartNode = new(startText, startGuid);
        }
        AddElement(StartNode);
        StartNode.capabilities -= Capabilities.Movable;
        StartNode.capabilities -= Capabilities.Deletable;
        StartNode.capabilities -= Capabilities.Copiable;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        var position = evt.localMousePosition; // Mouse position upon opening context menu

        var dialogueNodeTypes = TypeCache.GetTypesDerivedFrom<BaseNode>();

        base.BuildContextualMenu(evt);

        foreach (var type in dialogueNodeTypes)
        {
            evt.menu.InsertAction(0, $"Create Node/{type.Name}", (a) => AddNode(type, position));
        }
    }

    private void AddNode(Type nodeType, Vector2 position)
    {
        AddElement(CreateNode(nodeType, position));
    }

    public Node CreateNode(Type nodeType, Vector2 position)
    {
        var tempNode = (Node)Activator.CreateInstance(nodeType, new object[] { this });
        tempNode.SetPosition(new Rect(position, new Vector2(200,200)));
        return tempNode;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();
        var startPortView = startPort;

        ports.ForEach((port) =>
        {
            var portView = port;
            if (startPortView != portView && startPortView.node != portView.node && startPortView.direction != portView.direction)
                compatiblePorts.Add(port);
        });

        return compatiblePorts;
    }
}
