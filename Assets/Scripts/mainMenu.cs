using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public GameObject Settings;
    public void play()
    {
        SceneManager.LoadScene(1);
    }

    public void settings()
    {
        Settings.SetActive(true);
        gameObject.SetActive(false);
    }
}
