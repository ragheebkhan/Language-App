using System.Collections;
using System;
using UnityEngine;
using UnityEngine.AI;
public class CF_NPC_Movement : MonoBehaviour
{
    NavMeshAgent agent;
    Vector3 endingPos;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void moveDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public void InitialmoveDestination(Vector3 position, Action onComplete = null)
    {
        agent.destination = position; //Alternatively we can use agent.SetSestination(position);
        StartCoroutine(afterMoveDestination(onComplete));
    }

    private IEnumerator afterMoveDestination(Action onComplete)
    {
        while(agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        moveDestination(endingPos);

        onComplete?.Invoke();
    }
}
