using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CodeMonkey.Utils;

public class CF_NPCController : MonoBehaviour
{

    public List<Vector3> destinations; // List of destinations for the NPC to visit
    //public List<GameObject> destinationObjects;
    private int currentDestinationIndex = 0; // Index of the current destination in the list
    [SerializeField] GameObject firstPos;
    [SerializeField] GameObject itemToDeliver; // Item to deliver to the NPC
    [SerializeField] GameObject npcItem; //Item to give to the NPC
    [SerializeField] GameObject deliverySpot;//Spot where the player delivers the item
    private NavMeshAgent agent; // Reference to the NavMeshAgent component
    private Animator animator; // Reference to the Animator component
    float positionSize = 2f;
    bool hasArrivedAtFirstDestination = false;
    public float stoppingDistance = 3f;
    public float speed = 3.5f;
    private bool isWalking = false;
    private bool isDeliveringItem = false;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = speed;
        agent.autoBraking = true;

        for (int i = 0; i <= 5; i++)
        {
            destinations.Add(firstPos.transform.position + new Vector3(0, 0, -1) * positionSize * i);
            World_Sprite.Create(destinations[i], new Vector3(.3f, .3f,.3f), Color.green);
        }

        // Set the "isWalking" parameter to false to start with the idle animation
        animator.SetBool("idle", false);


        // Start moving to the first destination
        MoveToNextDestination();
    }



    void MoveToNextDestination()
    {
        
        if (currentDestinationIndex >= destinations.Count)
        {
            Debug.Log("HERE1");
            // End of the list, stop moving
            agent.isStopped = true;
            return;
        }

        // Move to the next destination in the list
        agent.SetDestination(destinations[currentDestinationIndex]);

        // Set the "isWalking" parameter to true to start the walking animation
        if (!isWalking)
        {
            Debug.Log("Here2");
            isWalking = true;
            animator.SetBool("idle", false);
        }

        // Check if the NPC has reached the current destination
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            Debug.Log("Stop HERE");
            // Stop the agent and set the "isWalking" parameter to false to stop the walking animation
            agent.isStopped = true;
            isWalking = false;
            animator.SetBool("idle", true);

            Debug.Log("Stopping distance:"+agent.stoppingDistance);
            Debug.Log("remaining dist:" + agent.remainingDistance);
            // Increment the current destination index
            currentDestinationIndex++;

            // Check if the current destination is the last destination in the list
            if (currentDestinationIndex >= destinations.Count)
            {
                // Move to the first destination in the list
                currentDestinationIndex = 0;
            }
        }

        
        
    }


    void Update()
     {

        
        // Check if the NPC has reached the current destination
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            Debug.Log("Hre");
            // Move to the next destination in the list
            MoveToNextDestination();
            
            // Check if the current destination is the first in line
            if (currentDestinationIndex == 1)
            {
                Debug.Log("here&");
                // Face the player
                FacePlayer();
            }
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
}