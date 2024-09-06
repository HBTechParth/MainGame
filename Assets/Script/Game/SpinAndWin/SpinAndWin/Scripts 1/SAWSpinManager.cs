using System.Collections;
using UnityEngine;
using EasyUI.PickerWheelUISAW;
using UnityEngine.UI;

public class SAWSpinManager : MonoBehaviour
{
    [SerializeField] private Button uiSpinButton;
    [SerializeField] private Text uiSpinButtonText;

    [SerializeField] private SAWPickerWheel pickerWheel;
    private int _numberOfTurns;
    public Text turnsText;
    public GameObject popupObject;

    public static SAWSpinManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }

    private void Start()
    {
        _numberOfTurns = PlayerPrefs.GetInt("RemainingTurns", 3);
       
      
     
    }

    public void CallSpinStart(int num)
    {
      //  uiSpinButton.onClick.AddListener(() =>
       // {
            Debug.Log("Click");
            SoundManager.Instance.ButtonClick();
            uiSpinButton.interactable = false;
            uiSpinButtonText.text = "";

            pickerWheel.OnSpinEnd(wheelPiece =>
            {
                Debug.Log(
                   @" <b>Index:</b> " + wheelPiece.Index + "           <b>Label:</b> " + wheelPiece.Label
                   + "\n <b>Amount:</b> " + wheelPiece.Amount + "      <b>Chance:</b> " + wheelPiece.Chance + "%"
                );
                UserEarnManage(wheelPiece.Index);
                uiSpinButton.interactable = true;
                uiSpinButtonText.text = "SPIN";
            });

            pickerWheel.Spin(num);

       // });
    }

    void UserEarnManage(int index)
    {
       
           //     DataManager.Instance.BonusDebitAmount_Credit((winMoney / 1).ToString(), "Spin Reward", "won");
           
           

           
        
    }

   

   

    

   
}
