using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Questmanager : MonoBehaviour
{
    public struct quest
    {
        public string questTitle;
        public string questGiverName;
        public string questGiverAsk;
        public string questObjective;
        public string acceptMessage;
        public string inProgressMessage;
        public string completionMessage;
        public string giverGenericMessage;
        public string questRecieverName;
        public string recieverGenericMessage;
        public string greeting;
        public string correctSpeechMessage;
        public string incorrectSpeechMessage;
        public string answer1;
        public string answer2;
        public string answer3;
        public string answer4;
        public string answer5;
    }

    quest[] quests;
    int saveSlot;

    void Start()
    {

        quests = new quest[8];
        /*
        for (int i = 0; i < 8; i++)
        {
            if (PlayerPrefs.GetString(quests[i].questTitle) != "")
            {
                quests[i].questTitle = PlayerPrefs.GetString(quests[i].questTitle);
                quests[i].questGiverName = PlayerPrefs.GetString(quests[i].questGiverName);
                quests[i].questGiverAsk = PlayerPrefs.GetString(quests[i].questGiverAsk);
                quests[i].questObjective = PlayerPrefs.GetString(quests[i].acceptMessage);
                quests[i].acceptMessage = PlayerPrefs.GetString(quests[i].acceptMessage);
                quests[i].inProgressMessage = PlayerPrefs.GetString(quests[i].inProgressMessage);
                quests[i].completionMessage = PlayerPrefs.GetString(quests[i].completionMessage);
                quests[i].giverGenericMessage = PlayerPrefs.GetString(quests[i].giverGenericMessage);
                quests[i].questRecieverName = PlayerPrefs.GetString(quests[i].questRecieverName);
                quests[i].recieverGenericMessage = PlayerPrefs.GetString(quests[i].recieverGenericMessage);
                quests[i].greeting = PlayerPrefs.GetString(quests[i].greeting);
                quests[i].correctSpeechMessage = PlayerPrefs.GetString(quests[i].correctSpeechMessage);
                quests[i].incorrectSpeechMessage = PlayerPrefs.GetString(quests[i].incorrectSpeechMessage);
                quests[i].answer1 = PlayerPrefs.GetString(quests[i].answer1);
                quests[i].answer2 = PlayerPrefs.GetString(quests[i].answer2);
                quests[i].answer3 = PlayerPrefs.GetString(quests[i].answer3);
                quests[i].answer4 = PlayerPrefs.GetString(quests[i].answer4);
                quests[i].answer5 = PlayerPrefs.GetString(quests[i].answer5);
            }
            else
            {
                quests[i].questTitle = null;
                quests[i].questGiverName = null;
                quests[i].questGiverAsk = null;
                quests[i].questObjective = null;
                quests[i].acceptMessage = null;
                quests[i].inProgressMessage = null;
                quests[i].completionMessage = null;
                quests[i].giverGenericMessage = null;
                quests[i].questRecieverName = null;
                quests[i].recieverGenericMessage = null;
                quests[i].greeting = null;
                quests[i].correctSpeechMessage = null;
                quests[i].incorrectSpeechMessage = null;
                quests[i].answer1 = null;
                quests[i].answer2 = null;
                quests[i].answer3 = null;
                quests[i].answer4 = null;
                quests[i].answer5 = null;
            }
        }
        */

        //GameObject.Find("Quest Creator/Current Quests Panel").transform.GetChild(0).GetComponent<Button>().onClick.Invoke();
    }

    public void changeSaveSlot(int slot)
    {
        saveSlot = slot;
    }

    public void save()
    {
        bool edited = true;
        for (int i = 3; i < 10; i++)
        {
            if ((GameObject.Find("Quest Creator/Editing Panel").transform.GetChild(i).GetChild(1).gameObject.GetComponent<TMP_InputField>().text == null) ||
                (GameObject.Find("Quest Creator/Editing Panel").transform.GetChild(i).GetChild(1).gameObject.GetComponent<TMP_InputField>().text == ""))
            {
                edited = false;
                break;
            }
        }

        for (int i = 11; i < 16; i++)
        {
            if ((GameObject.Find("Quest Creator/Editing Panel").transform.GetChild(i).GetChild(1).gameObject.GetComponent<TMP_InputField>().text == null) ||
                (GameObject.Find("Quest Creator/Editing Panel").transform.GetChild(i).GetChild(1).gameObject.GetComponent<TMP_InputField>().text == ""))
            {
                edited = false;
                break;
            }
        }

        for (int i = 1; i < 6; i++)
        {
            if ((GameObject.Find("Quest Creator/Editing Panel").transform.GetChild(16).GetChild(i).gameObject.GetComponent<TMP_InputField>().text == null) ||
                (GameObject.Find("Quest Creator/Editing Panel").transform.GetChild(16).GetChild(i).gameObject.GetComponent<TMP_InputField>().text == ""))
            {
                edited = false;
                break;
            }
        }

        if (edited == true)
        {
            PlayerPrefs.SetString(quests[saveSlot].questGiverName, GameObject.Find("Quest Creator/Editing Panel").transform.GetChild(3).GetChild(1).gameObject.GetComponent<TMP_InputField>().text);
            PlayerPrefs.SetString(quests[saveSlot].questGiverAsk, GameObject.Find("Quest Creator/Editing Panel").transform.GetChild(4).GetChild(1).gameObject.GetComponent<TMP_InputField>().text);
            PlayerPrefs.SetString(quests[saveSlot].questObjective, GameObject.Find("Quest Creator/Editing Panel").transform.GetChild(5).GetChild(1).gameObject.GetComponent<TMP_InputField>().text);
            PlayerPrefs.SetString(quests[saveSlot].acceptMessage, GameObject.Find("Quest Creator/Editing Panel").transform.GetChild(6).GetChild(1).gameObject.GetComponent<TMP_InputField>().text);
            PlayerPrefs.SetString(quests[saveSlot].inProgressMessage, GameObject.Find("Quest Creator/Editing Panel").transform.GetChild(7).GetChild(1).gameObject.GetComponent<TMP_InputField>().text);
            PlayerPrefs.SetString(quests[saveSlot].completionMessage, GameObject.Find("Quest Creator/Editing Panel").transform.GetChild(8).GetChild(1).gameObject.GetComponent<TMP_InputField>().text);
            PlayerPrefs.SetString(quests[saveSlot].giverGenericMessage, GameObject.Find("Quest Creator/Editing Panel").transform.GetChild(9).GetChild(1).gameObject.GetComponent<TMP_InputField>().text);

            PlayerPrefs.SetString(quests[saveSlot].questRecieverName, GameObject.Find("Quest Creator/Editing Panel").transform.GetChild(11).GetChild(1).gameObject.GetComponent<TMP_InputField>().text);
            PlayerPrefs.SetString(quests[saveSlot].recieverGenericMessage, GameObject.Find("Quest Creator/Editing Panel").transform.GetChild(12).GetChild(1).gameObject.GetComponent<TMP_InputField>().text);
            PlayerPrefs.SetString(quests[saveSlot].greeting, GameObject.Find("Quest Creator/Editing Panel").transform.GetChild(13).GetChild(1).gameObject.GetComponent<TMP_InputField>().text);
            PlayerPrefs.SetString(quests[saveSlot].correctSpeechMessage, GameObject.Find("Quest Creator/Editing Panel").transform.GetChild(14).GetChild(1).gameObject.GetComponent<TMP_InputField>().text);
            PlayerPrefs.SetString(quests[saveSlot].incorrectSpeechMessage, GameObject.Find("Quest Creator/Editing Panel").transform.GetChild(15).GetChild(1).gameObject.GetComponent<TMP_InputField>().text);

            PlayerPrefs.SetString(quests[saveSlot].answer1, GameObject.Find("Quest Creator/Editing Panel").transform.GetChild(16).GetChild(1).gameObject.GetComponent<TMP_InputField>().text);
            PlayerPrefs.SetString(quests[saveSlot].answer2, GameObject.Find("Quest Creator/Editing Panel").transform.GetChild(16).GetChild(2).gameObject.GetComponent<TMP_InputField>().text);
            PlayerPrefs.SetString(quests[saveSlot].answer3, GameObject.Find("Quest Creator/Editing Panel").transform.GetChild(16).GetChild(3).gameObject.GetComponent<TMP_InputField>().text);
            PlayerPrefs.SetString(quests[saveSlot].answer4, GameObject.Find("Quest Creator/Editing Panel").transform.GetChild(16).GetChild(4).gameObject.GetComponent<TMP_InputField>().text);
            PlayerPrefs.SetString(quests[saveSlot].answer5, GameObject.Find("Quest Creator/Editing Panel").transform.GetChild(16).GetChild(5).gameObject.GetComponent<TMP_InputField>().text);
            quests[saveSlot].questTitle = PlayerPrefs.GetString(quests[saveSlot].questTitle);
            quests[saveSlot].questGiverName = PlayerPrefs.GetString(quests[saveSlot].questGiverName);
            quests[saveSlot].questGiverAsk = PlayerPrefs.GetString(quests[saveSlot].questGiverAsk);
            quests[saveSlot].questObjective = PlayerPrefs.GetString(quests[saveSlot].acceptMessage);
            quests[saveSlot].acceptMessage = PlayerPrefs.GetString(quests[saveSlot].acceptMessage);
            quests[saveSlot].inProgressMessage = PlayerPrefs.GetString(quests[saveSlot].inProgressMessage);
            quests[saveSlot].completionMessage = PlayerPrefs.GetString(quests[saveSlot].completionMessage);
            quests[saveSlot].giverGenericMessage = PlayerPrefs.GetString(quests[saveSlot].giverGenericMessage);
            quests[saveSlot].questRecieverName = PlayerPrefs.GetString(quests[saveSlot].questRecieverName);
            quests[saveSlot].recieverGenericMessage = PlayerPrefs.GetString(quests[saveSlot].recieverGenericMessage);
            quests[saveSlot].greeting = PlayerPrefs.GetString(quests[saveSlot].greeting);
            quests[saveSlot].correctSpeechMessage = PlayerPrefs.GetString(quests[saveSlot].correctSpeechMessage);
            quests[saveSlot].incorrectSpeechMessage = PlayerPrefs.GetString(quests[saveSlot].incorrectSpeechMessage);
            quests[saveSlot].answer1 = PlayerPrefs.GetString(quests[saveSlot].answer1);
            quests[saveSlot].answer2 = PlayerPrefs.GetString(quests[saveSlot].answer2);
            quests[saveSlot].answer3 = PlayerPrefs.GetString(quests[saveSlot].answer3);
            quests[saveSlot].answer4 = PlayerPrefs.GetString(quests[saveSlot].answer4);
            quests[saveSlot].answer5 = PlayerPrefs.GetString(quests[saveSlot].answer5);
            changeSaveSlot(saveSlot);

        }
    }

    public void delete()
    {
        PlayerPrefs.DeleteKey(quests[saveSlot].questGiverName);
        PlayerPrefs.DeleteKey(quests[saveSlot].questGiverAsk);
        PlayerPrefs.DeleteKey(quests[saveSlot].questObjective);
        PlayerPrefs.DeleteKey(quests[saveSlot].acceptMessage);
        PlayerPrefs.DeleteKey(quests[saveSlot].inProgressMessage);
        PlayerPrefs.DeleteKey(quests[saveSlot].completionMessage);
        PlayerPrefs.DeleteKey(quests[saveSlot].giverGenericMessage);

        PlayerPrefs.DeleteKey(quests[saveSlot].questRecieverName);
        PlayerPrefs.DeleteKey(quests[saveSlot].recieverGenericMessage);
        PlayerPrefs.DeleteKey(quests[saveSlot].greeting);
        PlayerPrefs.DeleteKey(quests[saveSlot].correctSpeechMessage);
        PlayerPrefs.DeleteKey(quests[saveSlot].incorrectSpeechMessage);

        PlayerPrefs.DeleteKey(quests[saveSlot].answer1);
        PlayerPrefs.DeleteKey(quests[saveSlot].answer2);
        PlayerPrefs.DeleteKey(quests[saveSlot].answer3);
        PlayerPrefs.DeleteKey(quests[saveSlot].answer4);
        PlayerPrefs.DeleteKey(quests[saveSlot].answer5);

        quests[saveSlot].questTitle = null;
        quests[saveSlot].questGiverName = null;
        quests[saveSlot].questGiverAsk = null;
        quests[saveSlot].questObjective = null;
        quests[saveSlot].acceptMessage = null;
        quests[saveSlot].inProgressMessage = null;
        quests[saveSlot].completionMessage = null;
        quests[saveSlot].giverGenericMessage = null;
        quests[saveSlot].questRecieverName = null;
        quests[saveSlot].recieverGenericMessage = null;
        quests[saveSlot].greeting = null;
        quests[saveSlot].correctSpeechMessage = null;
        quests[saveSlot].incorrectSpeechMessage = null;
        quests[saveSlot].answer1 = null;
        quests[saveSlot].answer2 = null;
        quests[saveSlot].answer3 = null;
        quests[saveSlot].answer4 = null;
        quests[saveSlot].answer5 = null;
    }
}
