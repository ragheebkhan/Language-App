using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CF_NPC : MonoBehaviour
{
    public CF_NPC_Movement move;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveTo(Vector3 destination)
    {
        move.moveDestination(destination);
    }


}
