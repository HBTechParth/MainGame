using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JhandiMundaResult : MonoBehaviour
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

    public void ChipDetails(List<int> winNo)
    {
        for (int i = 0; i < JhandiMundaManager.Instance.resultArea.Length; i++)
        {
            if(this.transform.parent == JhandiMundaManager.Instance.resultArea[i] && winNo[i] > 1)
            {
                this.GetComponent<Image>().sprite = ResList[0];
                this.transform.GetChild(0).GetComponent<Text>().text = winNo[i].ToString();
            }
            else if (this.transform.parent == JhandiMundaManager.Instance.resultArea[i] && winNo[i] < 2)
            {
                this.GetComponent<Image>().sprite = ResList[1];
                this.transform.GetChild(0).GetComponent<Text>().text = "X";
            }
        }
    }
}
