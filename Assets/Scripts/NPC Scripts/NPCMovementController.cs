using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovementController : MonoBehaviour, IInteractable
{
    public NPCBaseState currentState;
    public NPCIdleState idleState = new();
    public NPCInteractState interactState = new();
    public NPCMoveState moveState = new();

    private List<Vector3> navPoints;
    private Animator animator;
    private NavMeshAgent agent;

    public InteractionChannel m_interactionChannel;
    private bool m_interactedWith;

    public float idleTime;
    private GameObject player;

    private void Awake()
    {
        RegisterFunctions();

        navPoints = new List<Vector3>();
        foreach (Transform child in GameObject.Find("/Environment/Points").transform)
        {
            navPoints.Add(child.position);
        }
        animator = gameObject.GetComponent<Animator>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");

        m_interactedWith = false;
        currentState = idleState;
        currentState.EnterState(this);
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    private void OnDestroy()
    {
        UnregisterFunctions();
    }

    public void SwitchState(NPCBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public abstract class NPCBaseState
    {   
        public abstract void EnterState(NPCMovementController NPC);
        public abstract void UpdateState(NPCMovementController NPC);
    }

    public class NPCMoveState : NPCBaseState
    {
        public override void EnterState(NPCMovementController NPC)
        {
            NPC.agent.SetDestination(NPC.navPoints[Random.Range(0, NPC.navPoints.Count)]);
            NPC.animator.SetBool("Idle",false);
        }

        public override void UpdateState(NPCMovementController NPC)
        {
            if ((NPC.agent.remainingDistance < 0.02f) && !(NPC.agent.pathPending))
            {
                NPC.SwitchState(NPC.idleState);
            }
            else if(NPC.m_interactedWith)
            {
                NPC.SwitchState(NPC.interactState);
            }
        }
    }

    public class NPCIdleState : NPCBaseState
    {
        float timer;
        public override void EnterState(NPCMovementController NPC)
        {
            timer = 0;
            NPC.animator.SetBool("Idle", true);
        }

        public override void UpdateState(NPCMovementController NPC)
        {
            timer += Time.deltaTime;
            if(NPC.m_interactedWith)
            {
                NPC.SwitchState(NPC.interactState);
            }
            else if(timer >= NPC.idleTime)
            {
                NPC.SwitchState(NPC.moveState);
            }
        }
    }

    public class NPCInteractState : NPCBaseState
    {
        public override void EnterState(NPCMovementController NPC)
        {
            NPC.animator.SetBool("Idle", true);
            NPC.agent.isStopped = true;
            NPC.StartCoroutine(Rotate(NPC));
        }

        public override void UpdateState(NPCMovementController NPC)
        {
            if(!NPC.m_interactedWith && NPC.agent.hasPath)
            {
                NPC.agent.isStopped = false;
                NPC.SwitchState(NPC.moveState);
            }
            else if(!NPC.m_interactedWith)
            {
                NPC.agent.isStopped = false;
                NPC.SwitchState(NPC.idleState);
            }
        }

        IEnumerator Rotate(NPCMovementController NPC)
        {
            float elapsedTime = 0;
            float time = 1;
            Quaternion startingRotation = NPC.gameObject.transform.rotation; 
            Quaternion targetRotation = Quaternion.LookRotation(-NPC.player.transform.forward, NPC.player.transform.up);
            while(elapsedTime < time) {
                elapsedTime += Time.deltaTime; 
                NPC.gameObject.transform.rotation = Quaternion.Lerp(startingRotation, targetRotation, (elapsedTime / time));
                yield return new WaitForEndOfFrame();
            }
        }
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

    public void OnLookedAt(GameObject gameObject)
    {
        return;
    }

    public void OnLookAway(GameObject gameObject)
    {
        return;
    }

    public void OnInteract(GameObject gameObject)
    {
        if(gameObject== this.gameObject)
        {
            m_interactedWith = true;
        }
    }

    public void OnEndInteraction(GameObject gameObject)
    {
        m_interactedWith = false;
    }
}
