using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Interaction Event Channel")]
public class InteractionChannel : ScriptableObject
{
    public delegate void InteractionCallback(GameObject interactedWithObject);
    public InteractionCallback OnInteract;
    public InteractionCallback OnEndInteraction;
    public InteractionCallback OnLookedAt;
    public InteractionCallback OnLookAway;

    public void RaiseOnInteract(GameObject interactedWithObject)
    {
        OnInteract?.Invoke(interactedWithObject);
    }

    public void RaiseOnEndInteraction(GameObject interactedWithObject)
    {
        OnEndInteraction?.Invoke(interactedWithObject);
    }

    public void RaiseOnLookedAt(GameObject interactedWithObject) 
    {  
        OnLookedAt?.Invoke(interactedWithObject); 
    }

    public void RaiseOnLookAway(GameObject interactedWithObject)
    {
        OnLookAway?.Invoke(interactedWithObject);
    }
}
