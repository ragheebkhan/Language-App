using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AImovement : MonoBehaviour
{
    private Animator animator;
    float idleTime = 10f;
    private Vector3 positiontracker;
    private Vector3 postrack2;
    private float timer = 0;
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Idle = Animator.StringToHash("Idle");
    List<Vector3> points;

    private Transform target;
    private UnityEngine.AI.NavMeshAgent agent;

    // Use this for initialization
    void OnEnable()
    {

        points = new List<Vector3>();
        foreach (Transform child in GameObject.Find("Environment/Points").transform)
        {
            points.Add(child.position);
        }
        animator = gameObject.GetComponent<Animator>();
        animator.SetTrigger("Walk");
        if (GetComponent<UnityEngine.AI.NavMeshAgent>())
        {
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            agent.SetDestination(points[Random.Range(0, points.Count)]);
        }
        else
        {
            Debug.Log("Fail");
        }
        agent.stoppingDistance = 1f;
        agent.speed = 1f;
        agent.baseOffset = 0;
        positiontracker = agent.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        if ((agent.isStopped == true) && (GameObject.FindWithTag("Player").GetComponent<InteractionController>().interactionState == false))
        {
            agent.isStopped = false;
            animator.SetTrigger("Walk");
            agent.SetDestination(points[Random.Range(0, points.Count)]);
        }
        if ((agent.remainingDistance <= 1f) && GameObject.FindWithTag("Player").GetComponent<InteractionController>().interactionState == false)
        {
            animator.SetTrigger("Idle");
            agent.isStopped = false;
            timer += Time.deltaTime;
            if (timer >= idleTime)
            {
                agent.SetDestination(points[Random.Range(0, points.Count)]);
                timer = 0;
            }
        }
        else if(GameObject.FindWithTag("Player").GetComponent<InteractionController>().interactionState == true)
        {
            animator.SetTrigger("Idle");
            agent.isStopped = true;
        }
        else
        {
            animator.SetTrigger("Walk");
        }


        /*
        timer2 += Time.deltaTime;

        if((timer2 >= 0f) && (timer2 <= 0.1f))
        {
            positiontracker = agent.transform.position;
        }
        else if((timer2 >= 2f) && (timer2 <= 2.1f))
        {
            postrack2 = agent.transform.position;
            if((postrack2 == positiontracker) && (agent.isStopped == false))
            {
                agent.SetDestination(points[Random.Range(0, points.Count)]);
                Debug.Log(gameObject.name);
                timer2 = 0;
            }
        }
        else if((timer2 > 0.1f) && (timer2 < 2f))
        {

        }
        else
        {
            timer2 = 0;
        }
        */


    }

}
