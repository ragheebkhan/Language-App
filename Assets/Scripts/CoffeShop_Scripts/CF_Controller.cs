using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CF_Controller : MonoBehaviour
{
    [SerializeField] GameObject firstPosG;
    // Start is called before the first frame update
    void Start()
    {
        
        List<Vector3> waitingQueuePos = new List<Vector3>();
        Vector3 firstPos = firstPosG.transform.position; //posisiton of the first in line
        float positionSize = 2f;
        for(int i=0; i < 5; i++)
        {
            waitingQueuePos.Add(firstPos + new Vector3(0, 0, -1) * positionSize * i);
        }

        CF_WaitingQ waitingQ = new CF_WaitingQ(waitingQueuePos);

       

    }

    
}
