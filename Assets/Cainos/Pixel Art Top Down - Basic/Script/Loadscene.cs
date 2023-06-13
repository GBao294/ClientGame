using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneButton : MonoBehaviour
{
    public void LoadNextScene()
    {
        SceneManager.LoadScene("Client");
    }
}