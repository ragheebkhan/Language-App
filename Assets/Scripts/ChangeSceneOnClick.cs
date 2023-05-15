using UnityEngine.SceneManagement;
using UnityEngine;

public class ChangeSceneOnClick : MonoBehaviour
{
    [SerializeField] string sceneName;

    private void OnMouseDown()
    {
        SceneManager.LoadScene(sceneName);
    }
}