using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using FrostweepGames.Plugins.GoogleCloud.SpeechRecognition;
using System;
using UnityEditor.PackageManager;

public class DialogueAgent : MonoBehaviour, IInteractable
{
    public InteractionChannel m_interactionChannel;

    public List<DialogueGraphData> DialogueData;
    private BaseNodeData m_currentNode;
    private DialogueGraphData m_currentGraph;

    public GameObject DialoguePanel;
    public TMP_Text NPCName;
    public TMP_Text DialogueText;
    public GameObject ButtonPanel;
    public GameObject ButtonPrefab;

    public GameObject SpeechPanel;
    public TMP_Text SpeechInstructions;
    public SpeechRecognizer SpeechRecognizer;

    public string InitialText;

    private void Awake()
    {
        RegisterFunctions();
    }

    private void OnDestroy()
    {
        UnregisterFunctions();
    }

    void Proceed(string guid, DialogueGraphData dialogue)
    {
        m_currentNode = dialogue.Nodes.Find(x => x.Guid == guid);

        if (m_currentNode == null)
        {
            m_interactionChannel.RaiseOnEndInteraction(this.gameObject);
            return;
        }

        Display(m_currentNode, dialogue);
    }

    void EnterDialogue(DialogueGraphData dialogue) 
    {
        Proceed((dialogue.NodeLinks.Find(x => x.BaseNodeGuid == dialogue.StartNode.Guid)).TargetNodeGuid, dialogue);
        m_currentGraph = dialogue;
    }

    void Display(BaseNodeData node, DialogueGraphData dialogue)
    {
        foreach(Transform child in ButtonPanel.transform)
        {
            Destroy(child.gameObject);
        }

        if(dialogue.NodeLinks.Find(x => x.BaseNodeGuid == m_currentNode.Guid) == null)
        {
            var button = Instantiate(ButtonPrefab);
            button.GetComponentInChildren<TMP_Text>().text = "Exit";
            button.GetComponent<Button>().onClick.AddListener(() => m_interactionChannel.RaiseOnEndInteraction(this.gameObject));
            button.transform.SetParent(ButtonPanel.transform);
            return;
        }

        switch (node)
        {
            case (LineNodeData line):
                DialogueText.text = (node as LineNodeData).DialogueLineText;
                foreach (var link in dialogue.NodeLinks.Where(x => x.BaseNodeGuid == node.Guid))
                {
                    var button = Instantiate(ButtonPrefab);
                    button.GetComponentInChildren<TMP_Text>().text = link.ChoiceText;
                    button.GetComponent<Button>().onClick.AddListener(() => Proceed(link.TargetNodeGuid, dialogue));
                    button.transform.SetParent(ButtonPanel.transform);
                }
                break;

            case (SpeechNodeData speech):
                SpeechInstructions.text = speech.Instructions;
                SpeechPanel.SetActive(true);
                SpeechRecognizer.ObjectToNotify = this.gameObject;
                break;
        }
    }

    void DisplaySpeech(string speech)
    {
        SpeechPanel.SetActive(false);
        DialogueText.text = "You Said: " + speech;
        var button = Instantiate(ButtonPrefab);
        button.GetComponentInChildren<TMP_Text>().text = "Continue";
        button.GetComponent<Button>().onClick.AddListener(() => CheckSpeech(speech));
        button.transform.SetParent(ButtonPanel.transform);
    }

    void CheckSpeech(string speech)
    {
        var analyzer = new TextSimilarityAnalyzer();
        if(analyzer.IsSimilar((m_currentNode as SpeechNodeData).TargetSpeech, speech))
        {
            foreach (Transform child in ButtonPanel.transform)
            {
                Destroy(child.gameObject);
            }

            DialogueText.text = "Correct!";
            var button = Instantiate(ButtonPrefab);
            button.GetComponentInChildren<TMP_Text>().text = "Continue";
            button.GetComponent<Button>().onClick.AddListener(() => Proceed((m_currentGraph.NodeLinks.Find(x=>x.BaseNodeGuid == m_currentNode.Guid)).TargetNodeGuid, 
                m_currentGraph));
            button.transform.SetParent(ButtonPanel.transform);
        }
        else
        {
            foreach (Transform child in ButtonPanel.transform)
            {
                Destroy(child.gameObject);
            }

            DialogueText.text = "Incorrect!";
            var button = Instantiate(ButtonPrefab);
            button.GetComponentInChildren<TMP_Text>().text = "Try Again";
            button.GetComponent<Button>().onClick.AddListener(() => Proceed(m_currentNode.Guid, m_currentGraph));
            button.transform.SetParent(ButtonPanel.transform);
        }
    }

    public void OnEndInteraction(GameObject gameObject)
    {
        foreach (Transform child in ButtonPanel.transform)
        {
            Destroy(child.gameObject);
        }

        DialoguePanel.SetActive(false);
        SpeechPanel.SetActive(false );
        m_currentNode = null;
        m_currentGraph = null;
    }

    public void OnInteract(GameObject gameObject)
    {
        m_currentNode = null;
        m_currentGraph = null;

        NPCName.text = gameObject.name;
        DialogueText.text = InitialText;
        foreach(var dialogue in DialogueData)
        {
            var button = Instantiate(ButtonPrefab);
            button.GetComponentInChildren<TMP_Text>().text = dialogue.StartNode.DialogueLineText;
            button.GetComponent<Button>().onClick.AddListener(() => EnterDialogue(dialogue));
            button.transform.SetParent(ButtonPanel.transform);
        }

        DialoguePanel.SetActive(true);
    }

    public void OnLookAway(GameObject gameObject)
    {
        return;
    }

    public void OnLookedAt(GameObject gameObject)
    {
        return;
    }

    public void RegisterFunctions()
    {

        m_interactionChannel.OnInteract += OnInteract;
        m_interactionChannel.OnEndInteraction += OnEndInteraction;
        m_interactionChannel.OnLookedAt += OnLookedAt;
        m_interactionChannel.OnLookAway += OnLookAway;
    }

    public void UnregisterFunctions()
    {
        m_interactionChannel.OnInteract -= OnInteract;
        m_interactionChannel.OnEndInteraction -= OnEndInteraction;
        m_interactionChannel.OnLookedAt -= OnLookedAt;
        m_interactionChannel.OnLookAway -= OnLookAway;
    }
}
