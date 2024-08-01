using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DealCardScript : MonoBehaviour
{
    public Button button;
    public DealRummyManager.CardSuffle card;
    public GameObject wildJoker;
    // Start is called before the first frame update
    void Start()
    {
        if (button != null)
            button.onClick.AddListener(() => DealRummyManager.Instance.CardClick(this.gameObject));
    }
}
