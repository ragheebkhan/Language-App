using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

public class DialogueGraphEditorWindow : EditorWindow
{
    private DialogueGraphView m_dialogueGraphView;
    private string m_fileName = "New Dialogue Graph";

    private GraphSaveUtility m_saveUtil;

    [MenuItem("Graph Editors/Dialogue Graph Editor Window")]
    public static void OpenWindow()
    {
        DialogueGraphEditorWindow wnd = GetWindow<DialogueGraphEditorWindow>();
        wnd.titleContent = new GUIContent("Dialogue Graph Editor");
        wnd.minSize = new Vector2(450, 200);
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/DialogueGraph/Editor/DialogueGraphEditorWindow.uxml");
        visualTree.CloneTree(root);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/DialogueGraph/Editor/DialogueGraphEditorWindow.uss");
        root.styleSheets.Add(styleSheet);

        m_dialogueGraphView = root.Q<DialogueGraphView>();
        m_saveUtil = new GraphSaveUtility(m_dialogueGraphView);

        root.Q<Button>("Save_Button").clicked += SaveGraph;
        root.Q<Button>("Load_Button").clicked += LoadGraph;

        root.Q<TextField>("Graph_Name").SetValueWithoutNotify(m_fileName);
        root.Q<TextField>("Graph_Name").MarkDirtyRepaint();
        root.Q<TextField>("Graph_Name").RegisterValueChangedCallback(evt =>
        {
            m_fileName = evt.newValue;
        });
    }

    private void LoadGraph()
    {
        m_saveUtil.Load(m_fileName);
    }

    private void SaveGraph()
    {
        m_saveUtil.Save(m_fileName);
    }
}