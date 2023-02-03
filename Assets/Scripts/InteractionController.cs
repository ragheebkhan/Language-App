using System.Collections;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Windows.Speech;

public class InteractionController : MonoBehaviour
{
    // This script is attched to the main camera and controls the player's interactions with NPC's, as well as the UI.

    [SerializeField] float maxDistance; // Furthest distance that Interactable objects can be detected

    public bool interactionState = false;

    public GameObject completeMsg;
    public GameObject player;
    public GameObject interactNotification;
    public GameObject dialogueBox;
    public GameObject questDesc;
    public GameObject speechInt;
    public GameObject objBox;
    public GameObject initialThankYou;
    public GameObject finalThankYou;
    public GameObject genericMessage;
    public GameObject questManager;
    public GameObject inProgressMessage;
    GameObject CurrentQuestGiver;
    GameObject npc;
    int questActive;
    int totalQuests;
    int completedQuests;
    bool answer;
    Rigidbody rb;

    // Start is called before the first frame update
    void Awake()
    {
        totalQuests = GameObject.FindGameObjectsWithTag("QuestGiver").Length;
        completedQuests = 0;
        questActive = 0; // no quest active, quest set to 0
        objBox.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = "Speak to People";
        Debug.Log(totalQuests);
    }

    // Update is called once per frame
    void Update()
    {
        if((totalQuests == completedQuests) && !(completeMsg.active))
        {
            //SceneManager.LoadScene("Endgame");
        }
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance) && (hit.collider.gameObject.CompareTag("QuestGiver") || hit.collider.gameObject.CompareTag("QuestReciever")))
        {
            if (player.GetComponent<PlayerMovement>().enabled == true)
            {
                interactNotification.SetActive(true);
            }
        }
        else
        {
            interactNotification.SetActive(false);
        }
    }

    public void setNPC()
    {
        RaycastHit npchit;
        Physics.Raycast(transform.position, transform.forward, out npchit, maxDistance);
        npc = npchit.collider.gameObject;
    }

    public void interaction()
    {
        interactionState = true;
        interactNotification.SetActive(false);
        player.GetComponent<PlayerMovement>().enabled = false;
        if (((initialThankYou.active == true || finalThankYou.active == true) || (genericMessage.active == true)) || inProgressMessage.active == true) // if a final message is displayed, disable it.
        {
            player.GetComponent<PlayerMovement>().enabled = true;
            initialThankYou.SetActive(false);
            finalThankYou.SetActive(false);
            genericMessage.SetActive(false);
            inProgressMessage.SetActive(false);
            interactionState = false;
        }
        else if(completeMsg.active == true)
        {
            Debug.Log("test2");
            completeMsg.SetActive(false);
            CurrentQuestGiver.GetComponent<QuestGiver>().questCompleted = true;
            player.GetComponent<PlayerMovement>().enabled = true;
            interactionState = false;
        }
        else if (questActive == 0 && !(completeMsg.active)) // first check: is there an active quest? NO
        {
            if(npc.GetComponent<QuestGiver>()) // Second Check: is the npc quest giver or reciever? GIVER
            {
                if(npc.GetComponent<QuestGiver>().questAssigned == true) // third check: has this npc assigned a quest before? YES
                {
                    // display final thank you message
                    finalThankYou.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = npc.name;
                    finalThankYou.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = npc.GetComponent<QuestGiver>().finalThanksMessage;
                    finalThankYou.SetActive(true);
                }
                else // third check: has this npc assigned a quest before? NO
                {
                    if (!(dialogueBox.active) && !(questDesc.active))
                    {
                        dialogueBox.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = npc.name;
                        dialogueBox.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = npc.GetComponent<QuestGiver>().questInfo;
                        dialogueBox.SetActive(true);
                    }
                    else
                    {
                        if (!(questDesc.active))
                        {
                            dialogueBox.SetActive(false);
                            questDesc.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = npc.GetComponent<QuestGiver>().questObjective;
                            questDesc.SetActive(true);
                        }
                        else
                        {
                            questDesc.SetActive(false);
                            CurrentQuestGiver = npc;
                            questActive = npc.GetComponent<QuestGiver>().questID;
                            npc.GetComponent<QuestGiver>().questAssigned = true;
                            objBox.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = npc.GetComponent<QuestGiver>().questObjective;
                            initialThankYou.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = npc.name;
                            initialThankYou.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = npc.GetComponent<QuestGiver>().initialThanksMessage;
                            initialThankYou.SetActive(true);
                        }
                    }
                }
            }
            else // Second Check: is the npc quest giver or reciever? RECIEVER
            {
                genericMessage.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = npc.name;
                genericMessage.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = npc.GetComponent<QuestReciever>().genericMessage;
                genericMessage.SetActive(true);
            }
        }
        else // first check: is there an active quest? YES
        {
            if (npc.GetComponent<QuestGiver>() && !(completeMsg.active)) // Second Check: is the npc quest giver or reciever? GIVER
            {
                if(npc.GetComponent<QuestGiver>().questAssigned == true) // third check: has this npc assigned a quest before? YES
                {
                    if(questActive == npc.GetComponent<QuestGiver>().questID) // fourth check: is the quest the NPC assigned the currecnt active quest? YES
                    {
                        genericMessage.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = npc.name;
                        inProgressMessage.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = npc.GetComponent<QuestGiver>().inProgressMessage;
                        inProgressMessage.SetActive(true);
                    }
                    else // fourth check: is the quest the NPC assigned the currecnt active quest? NO
                    {
                        finalThankYou.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = npc.name;
                        finalThankYou.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = npc.GetComponent<QuestGiver>().finalThanksMessage;
                        finalThankYou.SetActive(true);
                    }
                }
                else // third check: has this npc assigned a quest before? NO
                {
                    genericMessage.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = npc.name;
                    genericMessage.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = npc.GetComponent<QuestGiver>().genericMessage;
                    genericMessage.SetActive(true);
                }
            }
            else // Second Check: is the npc quest giver or reciever? RECIEVER
            {
                if(npc.GetComponent<QuestReciever>().questID != questActive) // third check: is this NPC the reciever of the current assigned quest? NO
                {
                    genericMessage.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = npc.name;
                    genericMessage.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = npc.GetComponent<QuestReciever>().genericMessage;
                    genericMessage.SetActive(true);
                }
                else // third check: is this NPC the reciever of the current assigned quest? YES
                {
                    if((!(dialogueBox.active) && (CurrentQuestGiver.GetComponent<QuestGiver>().questCompleted == false)) && !(speechInt.active))
                    {
                        dialogueBox.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = npc.name;
                        dialogueBox.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = npc.GetComponent<QuestReciever>().firstInteractionMessage;
                        dialogueBox.SetActive(true);
                        
                    }
                    else if((dialogueBox.active == true) && (CurrentQuestGiver.GetComponent<QuestGiver>().questCompleted == false))
                    {
                        dialogueBox.SetActive(false);
                        speechInt.SetActive(true);
                        speechInt.transform.GetChild(1).gameObject.SetActive(true);
                        speechInt.transform.GetChild(2).gameObject.SetActive(false);
                        speechInt.transform.GetChild(4).gameObject.SetActive(false);
                    }
                    else if((speechInt.active == true) && (CurrentQuestGiver.GetComponent<QuestGiver>().questCompleted == false))
                    {
                        speechInt.SetActive(false);
                        speechInt.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = "Waiting for Speech";
                        if(answer == true)
                        {
                            completeMsg.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = npc.name;
                            completeMsg.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = npc.GetComponent<QuestReciever>().questCompleteMessage;
                            completeMsg.SetActive(true);
                        }
                        else
                        {
                            dialogueBox.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = npc.name;
                            dialogueBox.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = npc.GetComponent<QuestReciever>().wrongAnswerMessage;
                            dialogueBox.SetActive(true);
                        }
                    }
                }
            }
        }
    }
    
    public void speech(string speech)
    {
        speechInt.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = "You Said: \"" + speech + "\"";
        for(int i = 0; i < npc.GetComponent<QuestReciever>().answers.Length; i++)
        {
            if(speech.ToLower() == npc.GetComponent<QuestReciever>().answers[i])
            {
                answer = true;
                break;
            }
            else
            {
                answer = false;
            }
        }
    }

    public void quit()
    {
        dialogueBox.SetActive(false);
        questDesc.SetActive(false);
        player.GetComponent<PlayerMovement>().enabled = true;
        interactionState = false;
        speechInt.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = "Waiting for Speech";
    }
    
}   

