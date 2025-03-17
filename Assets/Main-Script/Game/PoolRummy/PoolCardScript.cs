using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoolCardScript : MonoBehaviour
{
    public Button button;
    public PoolRummyManager.CardSuffle card;
    public GameObject wildJoker;
    // Start is called before the first frame update
    void Start()
    {
        if (button != null)
            button.onClick.AddListener(() => PoolRummyManager.Instance.CardClick(this.gameObject));
    }

    
}
