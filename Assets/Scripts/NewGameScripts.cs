using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameScripts : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void NewGame()
    {
        SceneManager.LoadScene("StartGameScene");
    }
}
