using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionInstigator : MonoBehaviour
{
    private RaycastHit m_hit;
    private GameObject m_interactedWithObject;

    [SerializeField]
    public InteractionChannel m_InteractionChannel;

    [SerializeField]
    private float m_maxDistance;

    private bool m_interactingWithObject = false;

    private void Start()
    {
        m_InteractionChannel.OnEndInteraction += OnEndInteraction;
        m_InteractionChannel.OnInteract += OnInteract;
    }

    private void OnDestroy()
    {
        m_InteractionChannel.OnEndInteraction -= OnEndInteraction;
        m_InteractionChannel.OnInteract -= OnInteract;
    }
    // Update is called once per frame
    void Update()
    {
        if(m_hit.collider != null && m_hit.collider.gameObject.TryGetComponent(out IInteractable interactable))
        {
            m_interactedWithObject = m_hit.collider.gameObject;
        }
        else
        {
            m_interactedWithObject = null;
        }

        if(m_interactingWithObject)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                m_InteractionChannel.RaiseOnEndInteraction(m_interactedWithObject);
            }
        }
        else if(m_interactedWithObject != null)
        {
            m_InteractionChannel.RaiseOnLookedAt(m_interactedWithObject);

            if (Input.GetKeyDown(KeyCode.E))
            {
                m_InteractionChannel.RaiseOnInteract(m_interactedWithObject);
            }
        }
        else
        {
            m_InteractionChannel.RaiseOnLookAway(m_interactedWithObject);
        }
    }

    private void FixedUpdate()
    {
        Physics.Raycast(transform.position, transform.forward, out m_hit, m_maxDistance);
    }

    public void OnEndInteraction(GameObject gameObject)
    {
        m_interactingWithObject = false;
    }

    public void OnInteract(GameObject gameObject)
    {
        m_interactingWithObject = true;
    }
}
