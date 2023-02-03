using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class namescript : MonoBehaviour
{
    public GameObject nameTag;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        nameTag.transform.GetChild(0).GetComponent<TMP_Text>().text = gameObject.name;
    }
    void Update()
    {
        if(Vector3.Distance(gameObject.transform.position, player.transform.position) < 3)
        {
            nameTag.SetActive(true);
        }
        else
        {
            nameTag.SetActive(false);
        }
    }

}
