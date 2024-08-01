using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

[System.Serializable]
public class MatchWinLeaderData
{
    public string userId;
    public string userName;
    public string lobbyId;
    public string roomName;
    public float entryAmount;
    public float winAmount;
    public bool isBot;
    public string gameName;
}
[System.Serializable]
public class WinDataSend
{
    public List<MatchWinLeaderData> matchWinLeaderDatas = new List<MatchWinLeaderData>();
}

public class LudoWinManager : MonoBehaviour
{
    /*public Text rankTxtMain;
    public Image profileImgMain;
    public Text wonTitleMain;
    public Text scoreTxtMain;*/

    //public Text[] rankTxt;
    public Image[] profileImg;
    public Text[] profileNameTxt;
    //public Text[] scoreTxt;
    public Text[] winTxt;

    public GameObject[] rowObj;
    public Sprite[] profileSprite;

    public MatchWinLeaderData leaderData1;
    public MatchWinLeaderData leaderData2;
    public MatchWinLeaderData leaderData3;
    public MatchWinLeaderData leaderData4;

    public WinDataSend winDataSend;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.WinSound();
        
        LudoManager.Instance.isLudoOpenWin = true;
        winDataSend = new WinDataSend();
        leaderData1 = new MatchWinLeaderData();
        leaderData2 = new MatchWinLeaderData();
        //DataSetLudo();
        Invoke(nameof(DataSetLudo), 0.5f);
    }

    #region Ludo Win
    void DataSetLudo()
    {
        SoundManager.Instance.TickTimerStop();
        //LudoManager.Instance.isOpenWin = true;
        if (DataManager.Instance.isTwoPlayer)
        {
            for (int i = 0; i < rowObj.Length; i++)
            {
                if (i <= 1)
                {
                    rowObj[i].SetActive(true);
                }
                else
                {
                    rowObj[i].SetActive(false);
                }
            }
            int playerWin = 0;
            if (LudoManager.Instance.isOtherPlayLeft == false)
            {
                int no1 = LudoManager.Instance.playerScoreCnt1;
                int no2 = LudoManager.Instance.playerScoreCnt3;


                if (no1 == no2)
                {
                    playerWin = 1;
                }
                else if (no1 > no2)
                {
                    playerWin = 2;
                }
                else
                {
                    playerWin = 3;
                }

                if (LudoManager.Instance.isTimeFinish)
                    playerWin = 3;
            }
            else if (LudoManager.Instance.isOtherPlayLeft == true)
            {
                playerWin = 2;
            }




            if (playerWin == 1 || playerWin == 2)
            {
                //rankTxtMain.transform.parent.transform.GetChild(4).GetComponent<Text>().text = "Congratulations";


                if (playerWin == 1)
                {
                    //wonTitleMain.text = "YOU WON " + (DataManager.Instance.winAmount / 2).ToString("F2") + " Coin";
                    //rankTxtMain.text = "1";

                    float adminCommision = ((DataManager.Instance.tourEntryMoney ) * 2) - DataManager.Instance.winAmount;

                    DataManager.Instance.AddAmount(((float)DataManager.Instance.winAmount / 2), TestSocketIO.Instace.roomid, "Win Game " + TestSocketIO.Instace.roomid, "won", adminCommision, 0);


                    DataManager.Instance.SetWonMoneyGame(DataManager.Instance.GetWonMoneyGame() + DataManager.Instance.winAmount);
                }
                else
                {
                    //wonTitleMain.text = "YOU WON " + DataManager.Instance.winAmount + " Coin";
                    //rankTxtMain.text = "1";

                    float adminCommision = ((DataManager.Instance.tourEntryMoney) * 2) - DataManager.Instance.winAmount;

                    DataManager.Instance.AddAmount(DataManager.Instance.winAmount, TestSocketIO.Instace.roomid, "Win Game " + TestSocketIO.Instace.roomid, "won", adminCommision, 0);

                    DataManager.Instance.SetWonMoneyGame(DataManager.Instance.GetWonMoneyGame() + DataManager.Instance.winAmount);
                }
            }
            else
            {
                //rankTxtMain.text = "2";
                //wonTitleMain.text = "YOU WON ₹ " + 0;
                //rankTxtMain.transform.parent.transform.GetChild(4).GetComponent<Text>().text = "You Lost";
                //wonTitleMain.text = "Let's try again";
            }

            //scoreTxtMain.text = LudoManager.Instance.playerScoreCnt1.ToString();


            if (DataManager.Instance.isTwoPlayer)
            {

                //rankTxt[0].text = "1";

                if (LudoManager.Instance.isOtherPlayLeft)
                {
                    //rankTxt[1].text = "-";

                }
                else
                {
                    //rankTxt[1].text = "2";
                }

            }

            if (playerWin == 1 || playerWin == 2)
            {
                int getIndex = 0;
                if (DataManager.Instance.playerNo == 3)
                {
                    getIndex = 1;
                }
                //profileImgMain.sprite = profileSprite[DataManager.Instance.joinPlayerDatas[getIndex].avtar];

                //profileImg[0].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[getIndex].avtar];
                DataManager.Instance.GetImages(DataManager.Instance.joinPlayerDatas[getIndex].avtar, profileImg[0]);
                profileNameTxt[0].text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[getIndex].userName);
                //scoreTxt[0].text = LudoManager.Instance.playerScoreCnt1.ToString();
                winTxt[0].text = DataManager.Instance.winAmount.ToString();

                leaderData1.userId = DataManager.Instance.joinPlayerDatas[getIndex].userId;
                leaderData1.userName = DataManager.Instance.joinPlayerDatas[getIndex].userName;
                leaderData1.lobbyId = DataManager.Instance.joinPlayerDatas[getIndex].lobbyId;
                leaderData1.roomName = TestSocketIO.Instace.roomid;
                leaderData1.entryAmount = (DataManager.Instance.tourEntryMoney );
                leaderData1.winAmount = (DataManager.Instance.winAmount );
                if (BotManager.Instance.isConnectBot)
                {
                    if (!leaderData1.userId.Equals(DataManager.Instance.playerData._id))
                    {
                        leaderData1.isBot = true;
                    }
                }

                int secondIndex = 0;
                if (getIndex == 0)
                {
                    secondIndex = 1;
                }
                else if (getIndex == 1)
                {
                    secondIndex = 0;
                }

                if (LudoManager.Instance.isOtherPlayLeft)
                {
                    //scoreTxt[1].text = "";
                    winTxt[1].text = "Left";
                    leaderData2.userName = LudoManager.Instance.leftPlayerNames[0];
                    profileNameTxt[1].text = leaderData2.userName;
                }
                else
                {
                    //scoreTxt[1].text = LudoManager.Instance.playerScoreCnt3.ToString();
                    winTxt[1].text = "0";
                }
                if (LudoManager.Instance.isOtherPlayLeft == false)
                {
                    //profileImg[1].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[secondIndex].avtar];
                    DataManager.Instance.GetImages(DataManager.Instance.joinPlayerDatas[secondIndex].avtar, profileImg[1]);
                    profileNameTxt[1].text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[secondIndex].userName);
                    leaderData2.userId = DataManager.Instance.joinPlayerDatas[secondIndex].userId;
                    leaderData2.userName = DataManager.Instance.joinPlayerDatas[secondIndex].userName;
                    leaderData2.lobbyId = DataManager.Instance.joinPlayerDatas[secondIndex].lobbyId;
                    leaderData2.roomName = TestSocketIO.Instace.roomid;
                    leaderData2.entryAmount = (DataManager.Instance.tourEntryMoney);
                    leaderData2.winAmount = 0;
                }
                else
                {
                    profileNameTxt[1].text = UserNameStringManage(LudoManager.Instance.leftPlayerNames[0]);
                }
                if (BotManager.Instance.isConnectBot)
                {
                    if (!leaderData2.userId.Equals(DataManager.Instance.playerData._id))
                    {
                        leaderData2.isBot = true;
                    }
                }

                winDataSend.matchWinLeaderDatas.Add(leaderData1);
                winDataSend.matchWinLeaderDatas.Add(leaderData2);

                string sendWinJson = JsonUtility.ToJson(winDataSend);
                DataManager.Instance.SendLeaderBoardData(leaderData1.userId, leaderData1.winAmount, leaderData1.lobbyId, 1, leaderData1.roomName, "Win Game " + TestSocketIO.Instace.roomid, sendWinJson);


            }
            else if (playerWin == 3)
            {
                int getIndex = 0;
                if (DataManager.Instance.playerNo == 3)
                {
                    getIndex = 1;
                }

                //profileImgMain.sprite = profileSprite[DataManager.Instance.joinPlayerDatas[getIndex].avtar];
                //profileImg[1].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[getIndex].avtar];
                DataManager.Instance.GetImages(DataManager.Instance.joinPlayerDatas[getIndex].avtar, profileImg[1]);
                profileNameTxt[1].text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[getIndex].userName);

                //scoreTxt[1].text = LudoManager.Instance.playerScoreCnt1.ToString();
                winTxt[1].text = "0";



                leaderData1.userId = DataManager.Instance.joinPlayerDatas[getIndex].userId;
                leaderData1.userName = DataManager.Instance.joinPlayerDatas[getIndex].userName;
                leaderData1.lobbyId = DataManager.Instance.joinPlayerDatas[getIndex].lobbyId;
                leaderData1.roomName = TestSocketIO.Instace.roomid;
                leaderData1.entryAmount = (DataManager.Instance.tourEntryMoney);
                leaderData1.winAmount = 0;
                if (BotManager.Instance.isConnectBot)
                {
                    if (!leaderData1.userId.Equals(DataManager.Instance.playerData._id))
                    {
                        leaderData1.isBot = true;
                    }
                }


                int secondIndex = 0;
                if (getIndex == 0)
                {
                    secondIndex = 1;
                }
                else if (getIndex == 1)
                {
                    secondIndex = 0;
                }
                //profileImg[0].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[secondIndex].avtar];
                DataManager.Instance.GetImages(DataManager.Instance.joinPlayerDatas[secondIndex].avtar, profileImg[0]);
                profileNameTxt[0].text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[secondIndex].userName);

                //scoreTxt[0].text = LudoManager.Instance.playerScoreCnt3.ToString();
                winTxt[0].text = DataManager.Instance.winAmount.ToString();

                leaderData2.userId = DataManager.Instance.joinPlayerDatas[secondIndex].userId;
                leaderData2.userName = DataManager.Instance.joinPlayerDatas[secondIndex].userName;
                leaderData2.lobbyId = DataManager.Instance.joinPlayerDatas[secondIndex].lobbyId;
                leaderData2.roomName = TestSocketIO.Instace.roomid;
                leaderData2.entryAmount = (DataManager.Instance.tourEntryMoney);
                leaderData2.winAmount = (DataManager.Instance.winAmount);
                if (BotManager.Instance.isConnectBot)
                {
                    if (!leaderData2.userId.Equals(DataManager.Instance.playerData._id))
                    {
                        leaderData2.isBot = true;
                    }
                }
                winDataSend.matchWinLeaderDatas.Add(leaderData1);
                winDataSend.matchWinLeaderDatas.Add(leaderData2);

                string sendWinJson = JsonUtility.ToJson(winDataSend);
                DataManager.Instance.SendLeaderBoardData(leaderData2.userId, leaderData2.winAmount, leaderData2.lobbyId, 2, leaderData2.roomName, "Win Game " + TestSocketIO.Instace.roomid, sendWinJson);
            }

        }
        else if (DataManager.Instance.isFourPlayer)
        {
            for (int i = 0; i < rowObj.Length; i++)
            {
                rowObj[i].SetActive(true);
            }
            int playerWin = 0;
            int[] scores = null;
            int[] playersScore = null;
            int userScore = 0; // Score of the particular device user
           



                int no1 = LudoManager.Instance.playerScoreCnt1;
                int no2 = LudoManager.Instance.playerScoreCnt2;
                int no3 = LudoManager.Instance.playerScoreCnt3;
                int no4 = LudoManager.Instance.playerScoreCnt4;
                switch (DataManager.Instance.playerNo)
                {
                    case 1:
                        no1 = LudoManager.Instance.playerScoreCnt1;
                        no2 = LudoManager.Instance.playerScoreCnt2;
                        no3 = LudoManager.Instance.playerScoreCnt3;
                        no4 = LudoManager.Instance.playerScoreCnt4;
                        if(no2 == -1 && no3 == -1)
                        {
                            no2 = no4;
                        }
                        else if(no2 == -1 && no4 == -1)
                        {
                            no2 = no3;
                        }
                        else if(no2 == -1)
                        {
                            no2 = no3;
                            no3 = no4;
                        }
                        else if(no3 == -1)
                        {
                            no3 = no4;
                        }
                        break;
                    case 2:
                        no1 = LudoManager.Instance.playerScoreCnt4;
                        no2 = LudoManager.Instance.playerScoreCnt1;
                        no3 = LudoManager.Instance.playerScoreCnt2;
                        no4 = LudoManager.Instance.playerScoreCnt3;
                        if(no4 == -1 && no1 == -1)
                        {
                            no1 = no3;
                        }
                        else if(no3 == -1 && no1 == -1)
                        {
                            no1 = no4;
                        }
                        else if(no3 == -1)
                        {
                            no3 = no4;
                        }
                        else if(no1 == -1)
                        {
                            no1 = no4;
                        }
                        break;
                    case 3:
                        no1 = LudoManager.Instance.playerScoreCnt3;
                        no2 = LudoManager.Instance.playerScoreCnt4;
                        no3 = LudoManager.Instance.playerScoreCnt1;
                        no4 = LudoManager.Instance.playerScoreCnt2;
                        if(no1 == -1)
                        {
                            no1 = no4;
                        }
                        else if (no2 == -1)
                        {
                            no2 = no1;
                            no1 = no4;
                        }

                        break;
                    case 4:
                        no1 = LudoManager.Instance.playerScoreCnt2;
                        no2 = LudoManager.Instance.playerScoreCnt3;
                        no3 = LudoManager.Instance.playerScoreCnt4;
                        no4 = LudoManager.Instance.playerScoreCnt1;
                        break;
                }
                    


                if (no1 == no2 && no1 == no3 && no1 == no4)
                {
                    playerWin = 1; // All players have the same score
                }
                else if (no1 >= no2 && no1 >= no3 && no1 >= no4)
                {
                    playerWin = 1; // Player 1 has the highest score
                }
                else if (no2 >= no1 && no2 >= no3 && no2 >= no4)
                {
                    playerWin = 2; // Player 2 has the highest score
                }
                else if (no3 >= no1 && no3 >= no2 && no3 >= no4)
                {
                    playerWin = 3; // Player 3 has the highest score
                }
                else
                {
                    playerWin = 4; // Player 4 has the highest score
                }

                if (playerWin == DataManager.Instance.playerNo)
                    playerWin = 1;

                
                scores = new int[] { no1, no2, no3, no4 };
                playersScore = new[] { no1, no2, no3, no4 };

                switch (DataManager.Instance.joinPlayerDatas.Count)
                {
                    case 2:
                        scores = new int[] { no1, no2 };
                        playersScore = new[] { no1, no2 };
                        break;
                    case 3:
                        scores = new int[] { no1, no2, no3 };
                        playersScore = new[] { no1, no2, no3 };
                        break;
                    case 1:
                        scores = new int[] { no1};
                        playersScore = new[] { no1};
                        break;

                }

            
            if (DataManager.Instance.joinPlayerDatas.Count == 1)
            {
                //wonTitleMain.text = "YOU WON " + (DataManager.Instance.winAmount / 4).ToString("F2") + " Coin";
                //rankTxtMain.text = "1";

                float adminCommision = ((DataManager.Instance.tourEntryMoney) * 4) - DataManager.Instance.winAmount;
                DataManager.Instance.AddAmount(((float)DataManager.Instance.winAmount / 4), TestSocketIO.Instace.roomid, "Win Game " + TestSocketIO.Instace.roomid, "won", adminCommision, 0);
                DataManager.Instance.SetWonMoneyGame(DataManager.Instance.GetWonMoneyGame() + DataManager.Instance.winAmount);
                //profileImgMain.sprite = profileSprite[DataManager.Instance.joinPlayerDatas[0].avtar];
                //profileImg[0].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[0].avtar];
                DataManager.Instance.GetImages(DataManager.Instance.joinPlayerDatas[0].avtar, profileImg[0]);
                profileNameTxt[0].text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[0].userName);
                //scoreTxt[0].text = LudoManager.Instance.playerScoreCnt1.ToString(); // Display the highest score
                winTxt[0].text = DataManager.Instance.winAmount.ToString();

                leaderData1.userId = DataManager.Instance.joinPlayerDatas[0].userId;
                leaderData1.userName = DataManager.Instance.joinPlayerDatas[0].userName;
                leaderData1.lobbyId = DataManager.Instance.joinPlayerDatas[0].lobbyId;
                leaderData1.roomName = TestSocketIO.Instace.roomid;
                leaderData1.entryAmount = DataManager.Instance.tourEntryMoney;
                leaderData1.winAmount = DataManager.Instance.winAmount;

                if (LudoManager.Instance.leftPlayerNames.Count == 1)
                {
                    leaderData4.userName = LudoManager.Instance.leftPlayerNames[0];
                    profileNameTxt[3].text = leaderData4.userName;
                    winTxt[3].text = "Left";
                    //scoreTxt[3].text = "";
                }
                else if (LudoManager.Instance.leftPlayerNames.Count == 2)
                {
                    leaderData3.userName = LudoManager.Instance.leftPlayerNames[0];
                    leaderData4.userName = LudoManager.Instance.leftPlayerNames[1];
                    profileNameTxt[2].text = leaderData3.userName;
                    profileNameTxt[3].text = leaderData4.userName;
                    winTxt[2].text = "Left";
                    winTxt[3].text = "Left";
                    //scoreTxt[2].text = "";
                    //scoreTxt[3].text = "";
                }
                else if (LudoManager.Instance.leftPlayerNames.Count == 3)
                {
                    leaderData2.userName = LudoManager.Instance.leftPlayerNames[0];
                    leaderData3.userName = LudoManager.Instance.leftPlayerNames[1];
                    leaderData4.userName = LudoManager.Instance.leftPlayerNames[2];
                    profileNameTxt[1].text = leaderData2.userName;
                    profileNameTxt[2].text = leaderData3.userName;
                    profileNameTxt[3].text = leaderData4.userName;
                    winTxt[1].text = "Left";
                    winTxt[2].text = "Left";
                    winTxt[3].text = "Left";
                    //scoreTxt[1].text = "";
                    //scoreTxt[2].text = "";
                    //scoreTxt[3].text = "";
                }
                return;


            }
            int[] sortedIndices = new int[] { 0, 1, 2, 3 };
            Array.Sort(scores, sortedIndices); // Sorting indices based on scores
            /*else if (LudoManager.Instance.isOtherPlayLeft == true)
            {
                playerWin = 4;
            }*/


            // Assign index values based on the sorted indices
            int count = DataManager.Instance.joinPlayerDatas.Count;
            var firstIndex = count == 4 ? sortedIndices[3] : count == 3 ? sortedIndices[2] : count == 2 ? sortedIndices[1] : sortedIndices[0];
            var secondIndex =count == 4 ? sortedIndices[2] : count == 3 ? sortedIndices[1] : count == 2 ? sortedIndices[0] : -1 ;
            var thirdIndex =count == 4 ? sortedIndices[1] : count == 3 ? sortedIndices[0] : -1;
            var fourthIndex =count == 4 ? sortedIndices[0] : -1;

            if (playerWin >= 1 && playerWin <= 4)
            {
                //rankTxtMain.transform.parent.transform.GetChild(4).GetComponent<Text>().text = "Congratulations";
                if (playerWin == 1)
                {
                    int winnerIndex = firstIndex; // Change this to match the correct index for the winner
                    bool isWinner = DataManager.Instance.playerData._id == DataManager.Instance.joinPlayerDatas[winnerIndex].userId;

                    if (isWinner)
                    {
                        //wonTitleMain.text = "YOU WON " + (DataManager.Instance.winAmount / 4).ToString("F2") + " Coin";
                        //rankTxtMain.text = "1";

                        float adminCommision = ((DataManager.Instance.tourEntryMoney) * 4) - DataManager.Instance.winAmount;
                        DataManager.Instance.AddAmount(((float)DataManager.Instance.winAmount / 4), TestSocketIO.Instace.roomid, "Win Game " + TestSocketIO.Instace.roomid, "won", adminCommision, 0);
                        DataManager.Instance.SetWonMoneyGame(DataManager.Instance.GetWonMoneyGame() + DataManager.Instance.winAmount);
                    }
                    else
                    {
                        // Show "You Lost" for non-winning players
                        //rankTxtMain.text = "2";
                        //rankTxtMain.transform.parent.transform.GetChild(4).GetComponent<Text>().text = "You Lost";
                        //wonTitleMain.text = "Let's try again";
                    }
                }
                else if (playerWin == 2 || playerWin == 3 || playerWin == 4)
                {
                    // Show "You Lost" for non-winning players
                    //rankTxtMain.text = "-";
                    //rankTxtMain.transform.parent.transform.GetChild(4).GetComponent<Text>().text = "You Lost";
                    //wonTitleMain.text = "Let's try again";
                }
            }
            else
            {
                // Handle unexpected playerWin value
                //rankTxtMain.text = "-";
                //rankTxtMain.transform.parent.transform.GetChild(4).GetComponent<Text>().text = "Game Over";
                //wonTitleMain.text = "No Winner";
            }

            // Find the index of the winner player 
            int playerIndex = DataManager.Instance.joinPlayerDatas.FindIndex(player => player.userId == DataManager.Instance.playerData._id);
            if (playerIndex != -1)
            {
                userScore = playersScore[playerIndex]; // Get the score of the particular device user
                //scoreTxtMain.text = userScore.ToString();
            }



            //rankTxt[0].text = "1";

            if (LudoManager.Instance.isOtherPlayLeft)
            {
                //rankTxt[1].text = "-";

            }
            else
            {
                //rankTxt[1].text = "2";
                //rankTxt[2].text = "3";
                //rankTxt[3].text = "4";
            }

            if (playerWin is 1 or 2 or 3 or 4)
            {
                /*var firstIndex = 0;
                var secondIndex = 1;
                var thirdIndex = 2;
                var fourthIndex = 3;*/


                //profileImgMain.sprite = profileSprite[DataManager.Instance.joinPlayerDatas[firstIndex].avtar];
                //profileImg[0].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[firstIndex].avtar];
                DataManager.Instance.GetImages(DataManager.Instance.joinPlayerDatas[firstIndex].avtar, profileImg[0]);
                profileNameTxt[0].text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[firstIndex].userName);
                //scoreTxt[0].text = scores.Last().ToString(); // Display the highest score
                winTxt[0].text = DataManager.Instance.winAmount.ToString();

                leaderData1.userId = DataManager.Instance.joinPlayerDatas[firstIndex].userId;
                leaderData1.userName = DataManager.Instance.joinPlayerDatas[firstIndex].userName;
                leaderData1.lobbyId = DataManager.Instance.joinPlayerDatas[firstIndex].lobbyId;
                leaderData1.roomName = TestSocketIO.Instace.roomid;
                leaderData1.entryAmount = DataManager.Instance.tourEntryMoney;
                leaderData1.winAmount = DataManager.Instance.winAmount;
                if (BotManager.Instance.isConnectBot && !leaderData1.userId.Equals(DataManager.Instance.playerData._id))
                    leaderData1.isBot = true;

                if(count == 1)
                {
                    if (LudoManager.Instance.leftPlayerNames.Count == 1)
                    {
                        leaderData4.userName = LudoManager.Instance.leftPlayerNames[0];
                        profileNameTxt[3].text = leaderData4.userName;
                        winTxt[3].text = "Left";
                        //scoreTxt[3].text = "";
                    }
                    else if (LudoManager.Instance.leftPlayerNames.Count == 2)
                    {
                        leaderData3.userName = LudoManager.Instance.leftPlayerNames[0];
                        leaderData4.userName = LudoManager.Instance.leftPlayerNames[1];
                        profileNameTxt[2].text = leaderData3.userName;
                        profileNameTxt[3].text = leaderData4.userName;
                        winTxt[2].text = "Left";
                        winTxt[3].text = "Left";
                        //scoreTxt[2].text = "";
                        //scoreTxt[3].text = "";
                    }
                    else if (LudoManager.Instance.leftPlayerNames.Count == 3)
                    {
                        leaderData2.userName = LudoManager.Instance.leftPlayerNames[0];
                        leaderData3.userName = LudoManager.Instance.leftPlayerNames[1];
                        leaderData4.userName = LudoManager.Instance.leftPlayerNames[2];
                        profileNameTxt[1].text = leaderData2.userName;
                        profileNameTxt[2].text = leaderData3.userName;
                        profileNameTxt[3].text = leaderData4.userName;
                        winTxt[1].text = "Left";
                        winTxt[2].text = "Left";
                        winTxt[3].text = "Left";
                        //scoreTxt[1].text = "";
                        //scoreTxt[2].text = "";
                        //scoreTxt[3].text = "";
                    }
                    return;
                }

                if (count > 1)
                {
                    //profileImg[1].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[secondIndex].avtar];
                    DataManager.Instance.GetImages(DataManager.Instance.joinPlayerDatas[secondIndex].avtar, profileImg[1]);
                    profileNameTxt[1].text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[secondIndex].userName);
                    //scoreTxt[1].text = scores[DataManager.Instance.joinPlayerDatas.Count - 2].ToString(); // Display the second highest score
                    winTxt[1].text = "0";

                    leaderData2.userId = DataManager.Instance.joinPlayerDatas[secondIndex].userId;
                    leaderData2.userName = DataManager.Instance.joinPlayerDatas[secondIndex].userName;
                    leaderData2.lobbyId = DataManager.Instance.joinPlayerDatas[secondIndex].lobbyId;
                    leaderData2.roomName = TestSocketIO.Instace.roomid;
                    leaderData2.entryAmount = DataManager.Instance.tourEntryMoney;
                    leaderData2.winAmount = 0;
                    if (BotManager.Instance.isConnectBot && !leaderData2.userId.Equals(DataManager.Instance.playerData._id))
                        leaderData2.isBot = true;
                }
                else
                {
                    if (LudoManager.Instance.leftPlayerNames.Count == 1)
                    {
                        leaderData4.userName = LudoManager.Instance.leftPlayerNames[0];
                        profileNameTxt[3].text = leaderData4.userName;
                        winTxt[3].text = "Left";
                        //scoreTxt[3].text = "";
                    }
                    else if (LudoManager.Instance.leftPlayerNames.Count == 2)
                    {
                        leaderData3.userName = LudoManager.Instance.leftPlayerNames[0];
                        leaderData4.userName = LudoManager.Instance.leftPlayerNames[1];
                        profileNameTxt[2].text = leaderData3.userName;
                        profileNameTxt[3].text = leaderData4.userName;
                        winTxt[2].text = "Left";
                        winTxt[3].text = "Left";
                        //scoreTxt[3].text = "";
                    }
                    else if (LudoManager.Instance.leftPlayerNames.Count == 3)
                    {
                        leaderData2.userName = LudoManager.Instance.leftPlayerNames[0];
                        leaderData3.userName = LudoManager.Instance.leftPlayerNames[1];
                        leaderData4.userName = LudoManager.Instance.leftPlayerNames[2];
                        profileNameTxt[1].text = leaderData2.userName;
                        profileNameTxt[2].text = leaderData3.userName;
                        profileNameTxt[3].text = leaderData4.userName;
                        winTxt[1].text = "Left";
                        winTxt[2].text = "Left";
                        winTxt[3].text = "Left";
                        //scoreTxt[1].text = "";
                        //scoreTxt[2].text = "";
                        //scoreTxt[3].text = "";
                    }

                }
                if (count > 2)
                {
                    //profileImg[2].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[thirdIndex].avtar];
                    DataManager.Instance.GetImages(DataManager.Instance.joinPlayerDatas[thirdIndex].avtar, profileImg[2]);
                    profileNameTxt[2].text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[thirdIndex].userName);
                    //scoreTxt[2].text = scores[DataManager.Instance.joinPlayerDatas.Count - 3].ToString(); // Display the third highest score
                    winTxt[2].text = "0";

                    leaderData3.userId = DataManager.Instance.joinPlayerDatas[thirdIndex].userId;
                    leaderData3.userName = DataManager.Instance.joinPlayerDatas[thirdIndex].userName;
                    leaderData3.lobbyId = DataManager.Instance.joinPlayerDatas[thirdIndex].lobbyId;
                    leaderData3.roomName = TestSocketIO.Instace.roomid;
                    leaderData3.entryAmount = DataManager.Instance.tourEntryMoney;
                    leaderData3.winAmount = 0;
                    if (BotManager.Instance.isConnectBot && !leaderData3.userId.Equals(DataManager.Instance.playerData._id))
                        leaderData3.isBot = true;
                }
                else
                {

                    if (LudoManager.Instance.leftPlayerNames.Count == 1)
                    {
                        leaderData4.userName = LudoManager.Instance.leftPlayerNames[0];
                        profileNameTxt[3].text = leaderData4.userName;
                        winTxt[3].text = "Left";
                        //scoreTxt[3].text = "";
                    }
                    else if (LudoManager.Instance.leftPlayerNames.Count == 2)
                    {
                        leaderData3.userName = LudoManager.Instance.leftPlayerNames[0];
                        leaderData4.userName = LudoManager.Instance.leftPlayerNames[1];
                        profileNameTxt[2].text = leaderData3.userName;
                        profileNameTxt[3].text = leaderData4.userName;
                        winTxt[2].text = "Left";
                        winTxt[3].text = "Left";
                        //scoreTxt[2].text = "";
                        //scoreTxt[3].text = "";
                    }
                    else if (LudoManager.Instance.leftPlayerNames.Count == 3)
                    {
                        leaderData2.userName = LudoManager.Instance.leftPlayerNames[0];
                        leaderData3.userName = LudoManager.Instance.leftPlayerNames[1];
                        leaderData4.userName = LudoManager.Instance.leftPlayerNames[2];
                        profileNameTxt[1].text = leaderData2.userName;
                        profileNameTxt[2].text = leaderData3.userName;
                        profileNameTxt[3].text = leaderData4.userName;
                        winTxt[1].text = "Left";
                        winTxt[2].text = "Left";
                        winTxt[3].text = "Left";
                        //scoreTxt[1].text = "";
                        //scoreTxt[2].text = "";
                        //scoreTxt[3].text = "";
                    }

                }
                if (count > 3)
                {
                    //profileImg[3].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[fourthIndex].avtar];
                    DataManager.Instance.GetImages(DataManager.Instance.joinPlayerDatas[fourthIndex].avtar, profileImg[3]);
                    profileNameTxt[3].text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[fourthIndex].userName);
                    //scoreTxt[3].text = scores[0].ToString(); // Display the lowest score
                    winTxt[3].text = "0";

                    leaderData4.userId = DataManager.Instance.joinPlayerDatas[fourthIndex].userId;
                    leaderData4.userName = DataManager.Instance.joinPlayerDatas[fourthIndex].userName;
                    leaderData4.lobbyId = DataManager.Instance.joinPlayerDatas[fourthIndex].lobbyId;
                    leaderData4.roomName = TestSocketIO.Instace.roomid;
                    leaderData4.entryAmount = DataManager.Instance.tourEntryMoney;
                    leaderData4.winAmount = 0;
                    if (BotManager.Instance.isConnectBot && !leaderData4.userId.Equals(DataManager.Instance.playerData._id))
                        leaderData4.isBot = true;
                }
                else
                {
                    if (LudoManager.Instance.leftPlayerNames.Count == 1)
                    {
                        leaderData4.userName = LudoManager.Instance.leftPlayerNames[0];
                        profileNameTxt[3].text = leaderData4.userName;
                        winTxt[3].text = "Left";
                        //scoreTxt[3].text = "";
                    }
                    else if (LudoManager.Instance.leftPlayerNames.Count == 2)
                    {
                        leaderData3.userName = LudoManager.Instance.leftPlayerNames[0];
                        leaderData4.userName = LudoManager.Instance.leftPlayerNames[1];
                        profileNameTxt[2].text = leaderData3.userName;
                        profileNameTxt[3].text = leaderData4.userName;
                        winTxt[2].text = "Left";
                        winTxt[3].text = "Left";
                        //scoreTxt[2].text = "";
                        //scoreTxt[3].text = "";
                    }
                    else if (LudoManager.Instance.leftPlayerNames.Count == 3)
                    {
                        leaderData2.userName = LudoManager.Instance.leftPlayerNames[0];
                        leaderData3.userName = LudoManager.Instance.leftPlayerNames[1];
                        leaderData4.userName = LudoManager.Instance.leftPlayerNames[2];
                        profileNameTxt[1].text = leaderData2.userName;
                        profileNameTxt[2].text = leaderData3.userName;
                        profileNameTxt[3].text = leaderData4.userName;
                        winTxt[1].text = "Left";
                        winTxt[2].text = "Left";
                        winTxt[3].text = "Left";
                        //scoreTxt[1].text = "";
                        //scoreTxt[2].text = "";
                        //scoreTxt[3].text = "";
                    }
                    
                }
            }
            else if (playerWin == 4)
            {
                int getIndex = 0;
                if (DataManager.Instance.playerNo == 4)
                {
                    getIndex = 1;
                }

                //profileImgMain.sprite = profileSprite[DataManager.Instance.joinPlayerDatas[getIndex].avtar];
                //profileImg[1].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[getIndex].avtar];
                DataManager.Instance.GetImages(DataManager.Instance.joinPlayerDatas[getIndex].avtar, profileImg[1]);
                profileNameTxt[1].text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[getIndex].userName);

                //scoreTxt[1].text = LudoManager.Instance.playerScoreCnt1.ToString();
                winTxt[1].text = "0";



                leaderData1.userId = DataManager.Instance.joinPlayerDatas[getIndex].userId;
                leaderData1.userName = DataManager.Instance.joinPlayerDatas[getIndex].userName;
                leaderData1.lobbyId = DataManager.Instance.joinPlayerDatas[getIndex].lobbyId;
                leaderData1.roomName = TestSocketIO.Instace.roomid;
                leaderData1.entryAmount = (DataManager.Instance.tourEntryMoney);
                leaderData1.winAmount = 0;
                if (BotManager.Instance.isConnectBot)
                {
                    if (!leaderData1.userId.Equals(DataManager.Instance.playerData._id))
                    {
                        leaderData1.isBot = true;
                    }
                }


                //int secondIndex = 0;
                if (getIndex == 0)
                {
                    secondIndex = 1;
                }
                else if (getIndex == 1)
                {
                    secondIndex = 0;
                }
                //profileImg[0].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[secondIndex].avtar];
                DataManager.Instance.GetImages(DataManager.Instance.joinPlayerDatas[secondIndex].avtar, profileImg[0]);
                profileNameTxt[0].text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[secondIndex].userName);

                //scoreTxt[0].text = LudoManager.Instance.playerScoreCnt3.ToString();
                winTxt[0].text = DataManager.Instance.winAmount.ToString();

                leaderData2.userId = DataManager.Instance.joinPlayerDatas[secondIndex].userId;
                leaderData2.userName = DataManager.Instance.joinPlayerDatas[secondIndex].userName;
                leaderData2.lobbyId = DataManager.Instance.joinPlayerDatas[secondIndex].lobbyId;
                leaderData2.roomName = TestSocketIO.Instace.roomid;
                leaderData2.entryAmount = (DataManager.Instance.tourEntryMoney);
                leaderData2.winAmount = (DataManager.Instance.winAmount);
                if (BotManager.Instance.isConnectBot)
                {
                    if (!leaderData2.userId.Equals(DataManager.Instance.playerData._id))
                    {
                        leaderData2.isBot = true;
                    }
                }
                winDataSend.matchWinLeaderDatas.Add(leaderData1);
                winDataSend.matchWinLeaderDatas.Add(leaderData2);

                string sendWinJson = JsonUtility.ToJson(winDataSend);
                DataManager.Instance.SendLeaderBoardData(leaderData2.userId, leaderData2.winAmount, leaderData2.lobbyId, 2, leaderData2.roomName, "Win Game " + TestSocketIO.Instace.roomid, sendWinJson);
            }



        }



    }
    #endregion
    

    public string UserNameStringManage(string name)
    {
        if (name != null && name != "")
        {
            if (name.Length > 7)
            {
                name = name.Substring(0, 5) + "...";
            }
            else
            {
                name = name;
            }
        }
        return name;
    }
    public void HomeButtonClick()
    {
        TestSocketIO.Instace.MatchEnded();
        SoundManager.Instance.ButtonClick();
        DataReset();
        SceneManager.LoadScene("Main");
    }

    public void PayAgainButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        DataReset();
        SceneManager.LoadScene("Main");


    }

    void DataReset()
    {
        DataManager.Instance.tournamentID = "";
        DataManager.Instance.tourEntryMoney = 0;
        DataManager.Instance.tourCommision = 0;
        DataManager.Instance.commisionAmount = 0;
        DataManager.Instance.orgIndexPlayer = 0;
        DataManager.Instance.joinPlayerDatas.Clear();
        TestSocketIO.Instace.roomid = "";
        TestSocketIO.Instace.userdata = "";
        TestSocketIO.Instace.playTime = 0;
        BotManager.Instance.isBotAvalible = false;
        BotManager.Instance.isConnectBot = false;
        DataManager.Instance.hasCalledOpenTournamentLoadScreen = false;
    }
}
