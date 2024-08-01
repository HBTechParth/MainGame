using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPrefab7 : MonoBehaviour
{
    public Text WinnedText;
    public int Winned = 2;
    public List<Sprite> ResList;
    public bool Hidden = true;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().preserveAspect = true;
    }

    public void ChipDetails(int winNo, int result)
    {
        switch (winNo)
        {
            case 1:// 1 = 7 Down
                this.GetComponent<Image>().sprite = ResList[0];
                transform.GetChild(0).GetComponent<Text>().text = result.ToString();
                break;
            case 2:// 2 = 7 Up
                this.GetComponent<Image>().sprite = ResList[1];
                transform.GetChild(0).GetComponent<Text>().text = result.ToString();
                break;
            case 3:// 3 = 7
                this.GetComponent<Image>().sprite = ResList[2];
                transform.GetChild(0).GetComponent<Text>().text = result.ToString();
                break;
            default:
                print("No data to track");
                break;
        }
    }
    
}
