using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InternetPanel : MonoBehaviour
{
    public static InternetPanel Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (MainMenuManager.Instance != null)
        {
            MainMenuManager.Instance.screenObj.Add(this.gameObject);
        }

    }

    public void BackButtonClick()
    {
        SoundManager.Instance.ButtonClick();

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Error. Check internet connection!");
        }
        else
        {
            Time.timeScale = 1;
            if (MainMenuManager.Instance != null)
            {
                MainMenuManager.Instance.screenObj.Remove(this.gameObject);
            }

            Destroy(this.gameObject);
            TestSocketIO.Instace.LeaveRoom();
            /*var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.ToString());*/
            SceneManager.LoadScene("Main"); 
            // SoundManager.Instance.StartBackgroundMusic();
            // to load the current scene
        }

    }

    public void UpdateButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (MainMenuManager.Instance != null)
        {
            MainMenuManager.Instance.screenObj.Remove(this.gameObject);
        }

        Application.OpenURL(DataManager.Instance.appUrl);
        Application.Quit();
        
    }

    public void CloseButtonClick()
    {
        SoundManager.Instance.ButtonClick();

        if (MainMenuManager.Instance != null)
        {
            MainMenuManager.Instance.screenObj.Remove(this.gameObject);
        }

        Destroy(this.gameObject);
    }
}
