using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class LineNode : BaseNode
{
    public string DialogueLineText { get; private set; }

    public LineNode(DialogueGraphView graphView) : base(graphView) { }

    protected override void GenerateView()
    {
        title = "Dialogue Line";

        //Add Input Port
        var inputPort = GetPortInstance(this, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        inputContainer.Add(inputPort);

        // Insert button to add choice
        var button = new Button(() => AddChoicePort())
        {
            text = "Add Choice"
        };
        titleButtonContainer.Add(button);

        // Add text field for dialogue line
        var textField = new TextField("Dialogue Line Text:");
        textField.RegisterValueChangedCallback(evt =>
        {
            DialogueLineText = evt.newValue;
        });
        extensionContainer.Add(textField);

        RefreshExpandedState();
        RefreshPorts();
    }

    public override BaseNodeData Save()
    {
        var nodeData = new LineNodeData
        {
            DialogueLineText = this.DialogueLineText,
            Guid = Guid,
            Position = GetPosition()
        };

        return nodeData;
    }

    public override void Load(BaseNodeData nodeData, List<NodeLinkData> links)
    {
        Guid = nodeData.Guid;
        SetPosition(nodeData.Position);
        DialogueLineText = ((LineNodeData)nodeData).DialogueLineText;

        extensionContainer.Q<TextField>().value = DialogueLineText;

        foreach (var link in links)
        {
            AddChoicePort(link.ChoiceText);
        }
    }

    private void AddChoicePort(string overriddenPortName = "")
    {
        var generatedPort = GetPortInstance(this, Direction.Output);
        var typeLabel = generatedPort.contentContainer.Q<Label>("type");
        generatedPort.contentContainer.Remove(typeLabel);

        var outputPortCount = outputContainer.Query("connector").ToList().Count;
        var outputPortName = string.IsNullOrEmpty(overriddenPortName) ? $"Option {outputPortCount + 1}" : overriddenPortName;

        var textField = new TextField()
        {
            name = string.Empty,
            value = outputPortName
        };

        textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);

        generatedPort.contentContainer.Add(new Label("  "));

        generatedPort.contentContainer.Add(textField);
        var deleteButton = new Button(() => RemovePort(this, generatedPort))
        {
            text = "X"
        };
        generatedPort.contentContainer.Add(deleteButton);
        generatedPort.portName = outputPortName;
        outputContainer.Add(generatedPort);
        RefreshPorts();
        RefreshExpandedState();
    }
}
