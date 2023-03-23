using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class SpeechNode : BaseNode
{
    public string TargetSpeech { get; private set; } = null;
    public string Instructions { get; private set; } = null;

    public SpeechNode(DialogueGraphView graphView) : base(graphView) { }

    protected override void GenerateView()
    {
        title = "User Speech Input";

        //Add Input Port
        var inputPort = GetPortInstance(this, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        inputContainer.Add(inputPort);

        //Add Output Port
        var outputPort = GetPortInstance(this, Direction.Output, Port.Capacity.Single);
        outputPort.portName = "Output";
        outputContainer.Add(outputPort);

        // Add text field for dialogue line
        var textField = new TextField("Target Speech:");
        textField.name = "targetspeech";
        textField.RegisterValueChangedCallback(evt =>
        {
            TargetSpeech = evt.newValue;
        });
        extensionContainer.Add(textField);

        // Add text field for instructions
        var instructionField = new TextField("Instructions:");
        instructionField.name = "instructions";
        instructionField.RegisterValueChangedCallback(evt =>
        { 
            Instructions = evt.newValue;
        });
        extensionContainer.Add(instructionField);

        RefreshExpandedState();
        RefreshPorts();
    }

    public override void Load(BaseNodeData nodeData, List<NodeLinkData> links)
    {
        Guid = nodeData.Guid;
        SetPosition(nodeData.Position);
        TargetSpeech = ((SpeechNodeData)nodeData).TargetSpeech;
        Instructions = ((SpeechNodeData)nodeData).Instructions;

        extensionContainer.Q<TextField>("targetspeech").value = TargetSpeech;
        extensionContainer.Q<TextField>("instructions").value = Instructions;
    }

    public override BaseNodeData Save()
    {
        var nodeData = new SpeechNodeData
        {
            TargetSpeech = this.TargetSpeech,
            Instructions = this.Instructions,
            Guid = this.Guid,
            Position = this.GetPosition()
        };

        return nodeData;
    }
}
