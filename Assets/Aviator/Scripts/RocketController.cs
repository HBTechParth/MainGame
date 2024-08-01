using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    public GameObject flameObject;
    public GameObject rocketObject;
    public GameObject blastObject;

    private Animator flameAnimator;
    private Animator blastAnimator;
    
  
    /*private void Start()
    {
        flameAnimator = flameObject.GetComponent<Animator>();
        blastAnimator = blastObject.GetComponent<Animator>();

        flameObject.SetActive(false);
        blastObject.SetActive(false);

        AviatorGameManager.Instance.OnGameStart += HandleGameStart;
        AviatorGameManager.Instance.OnGameCrash += HandleGameCrash;
        AviatorGameManager.Instance.OnGameRestart += HandleGameRestart;
    }*/
    
    public void InitializeRocketController()
    {
        flameAnimator = flameObject.GetComponent<Animator>();
        blastAnimator = blastObject.GetComponent<Animator>();

        flameObject.SetActive(false);
        blastObject.SetActive(false);

        AviatorGameManager.Instance.OnGameStart += HandleGameStart;
        AviatorGameManager.Instance.OnGameCrash += HandleGameCrash;
        AviatorGameManager.Instance.OnGameRestart += HandleGameRestart;
        
        print("RocketController is called");
    }

    private void OnDestroy()
    {
        AviatorGameManager.Instance.OnGameStart -= HandleGameStart;
        AviatorGameManager.Instance.OnGameCrash -= HandleGameCrash;
        AviatorGameManager.Instance.OnGameRestart -= HandleGameRestart;
    }

    private void HandleGameStart()
    {
        flameObject.SetActive(true);
        rocketObject.SetActive(true);
        blastObject.SetActive(false);
        flameAnimator.Play("Flame");
        SoundManager.Instance.PlayRocketThrustSound();
    }

    private void HandleGameCrash()
    {
        flameObject.SetActive(false);
        rocketObject.SetActive(false);
        blastObject.SetActive(true);
        blastAnimator.Play("RocketBlast");
        
        SoundManager.Instance.StopRocketThrustSound();
        SoundManager.Instance.RocketBlastSound();
    }
    
    public void HandleGameRestart()
    {
        flameObject.SetActive(false);
        rocketObject.SetActive(true);
        blastObject.SetActive(false);
        transform.localPosition = Vector3.zero;
        SoundManager.Instance.StopRocketThrustSound();
    }
}
