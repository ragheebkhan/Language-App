using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CF_DeliverySpot : MonoBehaviour
{
    public enum Tag
    {
        Cookies,
        Croissant,
        Cookie_Cupcake,
        //Redvelvet_Cupcake,
        //Chocolate_Donut,
        //Strawberry_Donut,
        //Vanilla_Donut,
        //Choc_Chip_Cupcake,
        //Cherry_Cupcake
    }

    [SerializeField] Tag [] itemsToDeliver; // Array of items to deliver
    public bool isItem = false;
    public int correctItemIndex = 0;
    public GameObject correctPanel;
    public GameObject incorrectPanel;
    public GameObject completionPanel;
    public Tag currentTag;
    private void Start()
    {
        // Assign all the tags to the itemsToDeliver array
        itemsToDeliver = new Tag[System.Enum.GetNames(typeof(Tag)).Length];
        for (int i = 0; i < itemsToDeliver.Length; i++)
        {
            itemsToDeliver[i] = (Tag)i;
        }
        currentTag = itemsToDeliver[correctItemIndex];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isItem) return; // Prevents multiple collisions that may activate both panels at the same time

        if (other.CompareTag(itemsToDeliver[correctItemIndex].ToString()))
        {
            Debug.Log("Delivered correct item: " + itemsToDeliver[correctItemIndex]);
            correctItemIndex++;

            if (correctItemIndex >= itemsToDeliver.Length)
            {
                Debug.Log("Completed the activity!");
                ActivateCompletionPanel();
            }
            else
            {
                currentTag = itemsToDeliver[correctItemIndex];
                isItem = true;
                Destroy(other.gameObject);
                correctPanel.SetActive(true);
                StartCoroutine(DeactivatePanel(correctPanel, 3f));
            }
        }
        else
        {
            Debug.Log("Delivered incorrect item: " + other.tag);
            Destroy(other.gameObject);
            incorrectPanel.SetActive(true);
            StartCoroutine(DeactivatePanel(incorrectPanel, 3f));
        }
    }

    private void ActivateCompletionPanel()
    {
        completionPanel.SetActive(true);
    }

    IEnumerator DeactivatePanel(GameObject panel, float waitTime)
    {
        Debug.Log("Deactivating");
        yield return new WaitForSeconds(waitTime);
        panel.SetActive(false);
        isItem = false;
    }
}
