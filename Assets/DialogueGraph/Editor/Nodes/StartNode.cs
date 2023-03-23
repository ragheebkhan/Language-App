using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class StartNode : Node
{
    public string Guid { get; private set; }
    public string StartText { get; private set; } = "Start Dialogue";
    public StartNode()
    {
        Guid = System.Guid.NewGuid().ToString();

        title = "Start";
        Port output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
        output.RemoveAt(1);
        var startText = new TextField("Start Text");
        startText.value = "Start Dialogue";
        startText.RegisterValueChangedCallback(evt => { StartText = evt.newValue; });
        output.Insert(1, startText);
        output.Insert(1,new Label("   "));
        outputContainer.Add(output);
    }

    public StartNode(string overrideStartText, string overrideGuid)
    {
        Guid = overrideGuid;

        title = "Start";
        Port output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
        output.RemoveAt(1);
        var startText = new TextField("Start Text");
        startText.value = overrideStartText;
        StartText = overrideStartText;
        startText.RegisterValueChangedCallback(evt => { StartText = evt.newValue; });
        output.Insert(1, startText);
        output.Insert(1, new Label("   "));
        outputContainer.Add(output);
    }
}
