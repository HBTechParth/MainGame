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

    private void Start()
    {
        _numberOfTurns = PlayerPrefs.GetInt("RemainingTurns", 3);
       
        uiSpinButton.onClick.AddListener(() =>
        {
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
                uiSpinButton.interactable = true;
                UserEarnManage(wheelPiece.Index);
                uiSpinButtonText.text = "SPIN";
            });

            pickerWheel.Spin(1);

        });
     
    }

    void UserEarnManage(int index)
    {
       
           //     DataManager.Instance.BonusDebitAmount_Credit((winMoney / 1).ToString(), "Spin Reward", "won");
           
           

           
        
    }

   

   

    

   
}
