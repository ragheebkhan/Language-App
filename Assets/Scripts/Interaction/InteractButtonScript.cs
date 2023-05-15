using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractButtonScript : MonoBehaviour, IInteractable
{
    
    public InteractionChannel m_interactionChannel;

    private void Awake()
    {
        RegisterFunctions();
    }

    private void OnDestroy()
    {
        UnregisterFunctions();
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
        this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "(E) " + gameObject.name;
        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        
    }

    public void OnLookAway(GameObject gameObject)
    {
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void OnInteract(GameObject gameObject)
    {
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void OnEndInteraction(GameObject gameObject)
    {

    }
}
