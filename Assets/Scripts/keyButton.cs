using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class keyButton : MonoBehaviour
{
    public string inputKey;
    Button button;
    void Awake()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e") && inputKey == "e")
        {
            if (button.interactable == true)
            {
                button.onClick.Invoke();
            }
        }
        else if(Input.GetKeyDown("escape") && inputKey == "esc")
        {
            button.onClick.Invoke();
        }
    }
}
