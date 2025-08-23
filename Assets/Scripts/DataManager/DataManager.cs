using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public GameObject object1;
    public GameObject object2;
    public string customString;

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GoToScene2()
    {
        customString = "combat";

        
        DontDestroyOnLoad(object1);
        DontDestroyOnLoad(object2);

        
        SceneManager.LoadScene("Scene2");
    }
}
