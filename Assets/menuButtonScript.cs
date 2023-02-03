using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class menuButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    Button button;
    TMP_Text buttonText;
    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        buttonText = gameObject.transform.GetChild(0).GetComponent<TMP_Text>();
        this.gameObject.GetComponent<Image>().color = new Color32(168, 218, 220, 255);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if(gameObject.transform.Find("Toggle/Label"))
        {
            gameObject.transform.Find("Toggle/Label").GetComponent<Text>().color = new Color32(168, 218, 220, 255);
        }
        if (gameObject.GetComponent<Image>().color != new Color32(29, 53, 87, 254))
        {
            buttonText.color = new Color32(168, 218, 220, 255);
            this.gameObject.GetComponent<Image>().color = new Color32(29, 53, 87, 255);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (transform.Find("Dropdown/Dropdown List"))
        {
            buttonText.color = new Color32(168, 218, 220, 255);
            this.gameObject.GetComponent<Image>().color = new Color32(29, 53, 87, 255);
        }
        else if(gameObject.GetComponent<Image>().color != new Color32(29, 53, 87, 254))
        {
            buttonText.color = new Color32(29, 53, 87, 255);
            this.gameObject.GetComponent<Image>().color = new Color32(168, 218, 220, 255);
            if (gameObject.transform.Find("Toggle/Label"))
            {
                gameObject.transform.Find("Toggle/Label").GetComponent<Text>().color = new Color32(29, 53, 87, 255);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (transform.Find("Dropdown/Dropdown List"))
        {
            buttonText.color = new Color32(168, 218, 220, 255);
            this.gameObject.GetComponent<Image>().color = new Color32(29, 53, 87, 255);
        }
        else if (gameObject.name != "Slot")
        {
            buttonText.color = new Color32(29, 53, 87, 255);
            this.gameObject.GetComponent<Image>().color = new Color32(168, 218, 220, 255);
            if (gameObject.transform.Find("Toggle/Label"))
            {
                gameObject.transform.Find("Toggle/Label").GetComponent<Text>().color = new Color32(29, 53, 87, 255);
            }
        }
        else if(gameObject.name == "Slot")
        {
            foreach(Transform child in gameObject.transform.parent)
            {
                if(child.transform != gameObject.transform)
                {
                    child.transform.gameObject.GetComponent<Image>().color = new Color32(168, 218, 220, 255);
                    child.transform.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color32(29, 53, 87, 255);
                }
            }
            buttonText.color = new Color32(168, 218, 220, 255);
            this.gameObject.GetComponent<Image>().color = new Color32(29, 53, 87, 254);
        }
    }

    public void simulatedClick()
    {
        if (transform.Find("Dropdown/Dropdown List"))
        {
            buttonText.color = new Color32(168, 218, 220, 255);
            this.gameObject.GetComponent<Image>().color = new Color32(29, 53, 87, 255);
        }
        else if (gameObject.name != "Slot")
        {
            buttonText.color = new Color32(29, 53, 87, 255);
            this.gameObject.GetComponent<Image>().color = new Color32(168, 218, 220, 255);
            if (gameObject.transform.Find("Toggle/Label"))
            {
                gameObject.transform.Find("Toggle/Label").GetComponent<Text>().color = new Color32(29, 53, 87, 255);
            }
        }
        else if (gameObject.name == "Slot")
        {
            foreach (Transform child in gameObject.transform.parent)
            {
                if (child.transform != gameObject.transform)
                {
                    child.transform.gameObject.GetComponent<Image>().color = new Color32(168, 218, 220, 255);
                    child.transform.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color32(29, 53, 87, 255);
                }
            }
            buttonText.color = new Color32(168, 218, 220, 255);
            this.gameObject.GetComponent<Image>().color = new Color32(29, 53, 87, 254);
        }
    }
    public void play()
    {
        SceneManager.LoadScene(1);
    }

    public void settings()
    {
        gameObject.transform.parent.parent.GetChild(2).gameObject.SetActive(true);
        gameObject.transform.parent.gameObject.SetActive(false);
    }

    public void creator()
    {
        gameObject.transform.parent.parent.GetChild(3).gameObject.SetActive(true);
        gameObject.transform.parent.gameObject.SetActive(false);
    }

    public void credits()
    {
        gameObject.transform.parent.parent.GetChild(4).gameObject.SetActive(true);
        gameObject.transform.parent.gameObject.SetActive(false);
    }

    public void mainMenu()
    {
        gameObject.transform.parent.parent.GetChild(1).gameObject.SetActive(true);
        gameObject.transform.parent.gameObject.SetActive(false);
    }

}
