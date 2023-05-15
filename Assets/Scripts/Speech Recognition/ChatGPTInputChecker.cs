using ChatGPTWrapper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatGPTInputChecker : MonoBehaviour
{
    public static ChatGPTInputChecker instance = null;
    public ChatGPTConversation conversation = null;
    public UnityStringEvent inputEvent;
    private GameObject requester = null;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            conversation = gameObject.GetComponent<ChatGPTConversation>();
            inputEvent.AddListener(conversation.SendToChatGPT);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void GradeInput(string input, GameObject requester)
    {
        this.requester = requester;
        inputEvent.Invoke(input);
    }

    public void ReturnGrade(string grade)
    {
        requester.SendMessage("SpeechChecked", grade);
        requester = null;
    }
}
