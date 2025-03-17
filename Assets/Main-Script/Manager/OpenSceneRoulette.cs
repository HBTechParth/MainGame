using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenSceneRoulette : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenRoullete()
    {

        Debug.Log("OpenRoullete");
        SceneManager.LoadScene("ROGame");
        Screen.orientation = ScreenOrientation.Portrait;

    }
}
