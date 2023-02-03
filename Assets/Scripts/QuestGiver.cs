using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    // This Script is attached to NPCs that give quests
    // The Script only holds information to be passed to the Interaction Manager and Quest Manager scripts

    public int questID; // Unique Quest ID number
    public string questObjective; // Quest objective to be displayed in objective box and accept/deny box
    public string questInfo; // The dialogue displayed when you first interact with this NPC
    public string initialThanksMessage; // Dialogue displayed after accepting quest
    public string denyMessage; // Dialogue displayed if quest is denied
    public string inProgressMessage; // Message that will be displayed if the player interacts with this NPC while its quest is active
    public string finalThanksMessage; // Message that will be displayed if the player interacts with this NPC after its quest is completed
    public string genericMessage; // Message that will be displayed if the player interacts with this NPC while another quest is active;
    public GameObject questReciever; // The NPC that must be interacted with to complete this quest;

    public bool questAssigned; // Indicates if this NPC has assigned its quest to the player
    public bool questCompleted; // Indicates if this NPC's quest is completed

    // Start is called before the first frame update
    void Start()
    {
        // Sets quest assignment and completion status to false on start, as player has not yet completed any quests or interacted with any NPCs.

        questAssigned = false;
        questCompleted = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
