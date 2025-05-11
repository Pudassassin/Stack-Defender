using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerProxy : MonoBehaviour
{
    public static GameObject instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this.gameObject;
        }
        else
        {
            Destroy(instance);
            instance = this.gameObject;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
