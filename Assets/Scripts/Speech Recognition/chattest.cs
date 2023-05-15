using ChatGPTWrapper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class chattest : MonoBehaviour
{
    public ChatGPTConversation conversation;
    public UnityStringEvent tester;
    public string test = "hi are you real?";
    public void display(string text)
    {
        Debug.Log(text);
    }

    public void Start()
    {
        tester.AddListener(conversation.SendToChatGPT);
        tester.Invoke(test);
    }
}
