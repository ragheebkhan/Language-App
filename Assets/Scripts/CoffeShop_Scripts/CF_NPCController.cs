using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CodeMonkey.Utils;


 public class CF_NPCController : MonoBehaviour
{
    public static List<Vector3> destinations = new List<Vector3>(); // List of destinations for the NPCs to visit
    public static List<CF_NPCController> allNPCs = new List<CF_NPCController>(); // List of all NPCs

    public int destinationIndex = 0; // Index of the destination this NPC is currently occupying

    [SerializeField] GameObject firstPos;
    [SerializeField] GameObject deliverySpot; // Spot where the player delivers the item
    [SerializeField] GameObject despawnLocation; // Spot where the NPC despawns

    private NavMeshAgent agent; // Reference to the NavMeshAgent component
    private Animator animator; // Reference to the Animator component

    bool isWalking = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Add this NPC to the allNPCs list
        allNPCs.Add(this);
    }

    private void OnDestroy()
    {
        // Remove this NPC from the allNPCs list when destroyed
        allNPCs.Remove(this);
    }

    void Start()
    {
        agent.autoBraking = true;
        agent.speed = 3.5f;
        animator.SetBool("idle", false);

        // Generate the destinations list based on the positions of the firstPos GameObject and subsequent positions
        for (int i = 0; i < 5; i++)
        {
            destinations.Add(firstPos.transform.position + new Vector3(0, 0, -1) * 2f * (i + 1));
            World_Sprite.Create(destinations[i], new Vector3(.3f, .3f, .3f), Color.green);
        }

        // Assign the destination index for this NPC based on its position in the allNPCs list
        destinationIndex = allNPCs.IndexOf(this);

        // Start moving to the assigned destination
        MoveToDestination();
    }

    void MoveToDestination()
    {
        // Check if the assigned destination index is within the range of the destinations list
        if (destinationIndex >= 0 && destinationIndex < destinations.Count)
        {
            // Move to the assigned destination
            agent.SetDestination(destinations[destinationIndex]);

            // Set the "isWalking" parameter to true to start the walking animation
            if (!isWalking)
            {
                isWalking = true;
                animator.SetBool("idle", false);
            }
        }
        else
        {
            // Set the "isWalking" parameter to false if the destination index is out of range
            if (isWalking)
            {
                isWalking = false;
                animator.SetBool("idle", true);
            }
        }
    }


    void Update()
    {
        // Check if the NPC has reached the current destination
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            
            // Stop the agent and set the "isWalking" parameter to false to stop the walking animation
            agent.isStopped = true;
            isWalking = false;
            animator.SetBool("idle", true);

            if (destinationIndex == 0)
            {
                // Face the player
                FacePlayer();
            }
        }
        /*
        // Check if the current destination is the first in line
        if (destinationIndex == 0 && deliverySpot.GetComponent<CF_DeliverySpot>().isItem)
        {
            // Call a method to despawn the NPC
            DespawnNPC();
        }
        // Check if the NPC is stuck
        if (isWalking && !agent.hasPath && agent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            Debug.Log("Character stuck");
            agent.enabled = false;
            agent.enabled = true;
            Debug.Log("NavMeshAgent re-enabled");
            // NavMeshAgent will start moving again
        }*/
    }

    public void DespawnNPC()
    {
        // Check if the NPC is at the first position in the destinations list
        if (destinationIndex == 0)
        {
            StartCoroutine(DespawnNPCCoroutine(1f));
        }
        else
        {
            Debug.Log("DespawnNPC - NPC is not at the first position");
        }
    }

    private IEnumerator DespawnNPCCoroutine(float waitTime)
    {
        // Despawn the NPC
        gameObject.SetActive(false);
        Debug.Log("In IEnumerator");
        // Wait until the next frame to ensure the NPC has been deactivated
        yield return new WaitForSeconds(waitTime);

        // Remove the first destination from the destinations list
        destinations.RemoveAt(0);
        Debug.Log("HERE@");
        // Shift the remaining NPCs in the list by decrementing their destinationIndex
        for (int i = 1; i < allNPCs.Count; i++)
        {
            allNPCs[i].destinationIndex--;
            Debug.Log("DesIndex: " + allNPCs[i].destinationIndex);
            Debug.Log("Moving NPCs");
            allNPCs[i].MoveToDestination();
        }
        Debug.Log("HERE--TOO");
        // Remove the NPC from the allNPCs list
        allNPCs.Remove(this);

        
        // Continue executing the remaining code
        ShiftNPCIndexes();
    }

    void ShiftNPCIndexes()
    {
        Debug.Log("InShiftIndexes");
        // Shift the remaining NPCs in the list by decrementing their destinationIndex
        for (int i = 0; i < allNPCs.Count; i++)
        {
            allNPCs[i].destinationIndex = i;
            Debug.Log("DesIndex: " + allNPCs[i].destinationIndex);
            Debug.Log("Moving NPCs");
            allNPCs[i].MoveToDestination();
        }
    }






    public void FacePlayer()
    {
        // Find the player object
        GameObject player = GameObject.FindGameObjectWithTag("Player");


        // Calculate the direction to the player
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0f;

        // Rotate the NPC to face the player
        transform.rotation = Quaternion.LookRotation(direction);

        agent.isStopped = true;
        isWalking = false;
        animator.SetBool("idle", true);
    }

    public void PrintPath()
    {
        // Create a new NavMeshPath object
        NavMeshPath navMeshPath = new NavMeshPath();

        // Calculate the path from the NPC's current position to the despawn location
        if (agent.CalculatePath(despawnLocation.transform.position, navMeshPath))
        {
            Debug.Log("Printing Path...");
            // Get the positions of all waypoints in the path
            Vector3[] waypoints = navMeshPath.corners;

            // Print out the positions of all waypoints in the path
            for (int i = 0; i < waypoints.Length; i++)
            {
                Debug.Log("Waypoint " + i + ": " + waypoints[i]);
            }
        }
        else
        {
            Debug.Log("Could not calculate path to despawn location");
        }
    }


}


