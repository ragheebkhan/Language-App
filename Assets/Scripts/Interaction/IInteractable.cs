using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable 
{
    public void RegisterFunctions();
    public void UnregisterFunctions();
    public void OnLookedAt(GameObject gameObject);
    public void OnLookAway(GameObject gameObject);
    public void OnInteract(GameObject gameObject);
    public void OnEndInteraction(GameObject gameObject);
}
