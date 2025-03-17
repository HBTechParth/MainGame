using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SnakePasa : MonoBehaviour
{
    public int pasaCurrentNo;
    public int playerNo;
    public GameObject pasaObj;
    public bool isStopZoom;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Zoom

    float getStartScale = 0;
    public bool isFirstZoom;
    public void PlayerPasaZoom()
    {

        if (isFirstZoom == false)
        {
            getStartScale = pasaObj.transform.localScale.x;
        }
        pasaObj.transform.DOScale(new Vector3(getStartScale - 0.05f, getStartScale - 0.05f, getStartScale - 0.05f), 0.25f).OnComplete(() =>
           pasaObj.transform.DOScale(new Vector3(getStartScale + 0.05f, getStartScale + 0.05f, getStartScale + 0.05f), 0.25f).OnComplete(() =>
            CheckZoom()
        ));
    }
    void CheckZoom()
    {
        if (isStopZoom == false)
        {
            pasaObj.transform.localScale = new Vector3(getStartScale, getStartScale, getStartScale);
            PlayerPasaZoom();
        }
        else
        {
            pasaObj.transform.localScale = new Vector3(getStartScale, getStartScale, getStartScale);
        }
    }


    #endregion

    #region Move Increment
    int counter;
    public void IncrementPasa(int lNo, bool isSocket, bool isBot)
    {
        pasaCurrentNo++;
        SoundManager.Instance.TokenMoveSound();
        //CheckZoom();
        if (BotManager.Instance.isConnectBot)
        {
            if (isBot)
            {
                SnakeManager.Instance.playerScoreCnt2 = pasaCurrentNo;
            }
            else
            {
                SnakeManager.Instance.playerScoreCnt1 = pasaCurrentNo;
            }
        }
        else
        {
            if (isSocket)
            {
                SnakeManager.Instance.playerScoreCnt2 = pasaCurrentNo;
            }
            else
            {
                SnakeManager.Instance.playerScoreCnt1 = pasaCurrentNo;
            }
        }

        counter++;
        float moveScale = 0.15f;
        float currentScale = this.gameObject.transform.localScale.x;
        currentScale += moveScale;

        this.gameObject.transform.DOScale(new Vector3(currentScale, currentScale, currentScale), 0.16f).OnComplete(() =>
                this.gameObject.transform.DOScale(new Vector3(currentScale - moveScale, currentScale - moveScale, currentScale - moveScale), 0.1f));
        this.gameObject.transform.DOMove(SnakeManager.Instance.allObj[pasaCurrentNo].transform.position, 0.3f).OnComplete(() =>
        Check_Move_Increment_Next(lNo, isSocket));
    }


    void Check_Move_Increment_Next(int no, bool isSocket)
    {

        if (no == counter)
        {
            counter = 0;

            bool isUp = CheckUp(isSocket);
            bool isDown = CheckDown(isSocket);

            if (isUp == false && isDown == false && isSocket == false)
            {
                CheckSamePos();
                if (BotManager.Instance.isConnectBot)
                {
                    bool isSendPlayer = false;
                    bool isSendBot = false;
                    if (DataManager.Instance.isDiceClick == true)
                    {
                        isSendPlayer = true;
                        isSendBot = false;
                    }
                    else if (DataManager.Instance.isDiceClick == false)
                    {
                        isSendPlayer = false;
                        isSendBot = true;
                    }
                    SnakeManager.Instance.Change_Turn_Bot(isSendPlayer, isSendBot);
                }
                else
                {
                    SnakeManager.Instance.isClickAvaliableDice = 0;
                    SnakeManager.Instance.PlayerChangeTurn();
                }
            }

            if (isSocket == true)
            {
                CheckSamePos();
            }

        }
        else
        {
            CheckSamePos();
            if (pasaCurrentNo >= 99)
            {
                SnakeManager.Instance.WinUserShow();

            }
            if (pasaCurrentNo < 99)
            {
                IncrementPasa(no, isSocket, false);
            }

        }
    }

    void CheckSamePos()
    {
        int pos1 = SnakeManager.Instance.yellowPasa.pasaCurrentNo;
        int pos2 = SnakeManager.Instance.redPasa.pasaCurrentNo;
        if (pos1 == pos2)
        {
            SnakeManager.Instance.yellowPasa.YellowPasaMovePos();
            SnakeManager.Instance.redPasa.RedPasaMovePos();
        }
        else
        {
            SnakeManager.Instance.yellowPasa.SinglePasaMove();
            SnakeManager.Instance.redPasa.SinglePasaMove();
        }
    }

    public void SinglePasaMove()
    {
        this.gameObject.transform.DOMove(SnakeManager.Instance.allObj[pasaCurrentNo].transform.position, 0.05f);
    }

    public void YellowPasaMovePos()
    {
        this.gameObject.transform.DOMoveX(SnakeManager.Instance.allObj[pasaCurrentNo].transform.position.x - 0.1f, 0.05f);

    }
    public void RedPasaMovePos()
    {
        this.gameObject.transform.DOMoveX(SnakeManager.Instance.allObj[pasaCurrentNo].transform.position.x + 0.1f, 0.05f);

    }

    bool CheckUp(bool isSocket)
    {
        bool isSetUp = false;
        int lastNo = 0;
        for (int i = 0; i < SnakeManager.Instance.upSnkaeBoard.Count; i++)
        {
            if (SnakeManager.Instance.upSnkaeBoard[i].fisrtNo == pasaCurrentNo)
            {
                lastNo = SnakeManager.Instance.upSnkaeBoard[i].lastNo;
                isSetUp = true;
                break;

            }
        }
        if (isSetUp == true)
        {
            pasaCurrentNo = lastNo;
            if (isSocket)
            {
                SnakeManager.Instance.playerScoreCnt2 = pasaCurrentNo;
            }
            else
            {
                SnakeManager.Instance.playerScoreCnt1 = pasaCurrentNo;
            }
            float moveScale = 0.15f;
            float currentScale = this.gameObject.transform.localScale.x;
            currentScale += moveScale;
            this.gameObject.transform.DOScale(new Vector3(currentScale, currentScale, currentScale), 0.16f).OnComplete(() =>
                this.gameObject.transform.DOScale(new Vector3(currentScale - moveScale, currentScale - moveScale, currentScale - moveScale), 0.1f));
            this.gameObject.transform.DOMove(SnakeManager.Instance.allObj[pasaCurrentNo].transform.position, 0.3f).OnComplete(() => CompleteUp(isSocket));
        }
        return isSetUp;

    }

    void CompleteUp(bool isSocket)
    {
        CheckSamePos();
        if (isSocket == false)
        {
            if (BotManager.Instance.isConnectBot)
            {
                bool isSendPlayer = false;
                bool isSendBot = false;
                if (DataManager.Instance.isDiceClick == true)
                {
                    DataManager.Instance.isDiceClick = false;
                    isSendPlayer = true;
                    isSendBot = false;
                }
                else if (DataManager.Instance.isDiceClick == false)
                {
                    isSendPlayer = false;
                    isSendBot = true;
                }
                SnakeManager.Instance.Change_Turn_Bot(isSendPlayer, isSendBot);
            }
            else
            {
                SnakeManager.Instance.isClickAvaliableDice = 0;
                SnakeManager.Instance.PlayerChangeTurn();
            }
        }
    }
    bool CheckDown(bool isSocket)
    {
        bool isSetUp = false;
        int lastNo = 0;
        DOTweenPath path = null;
        for (int i = 0; i < SnakeManager.Instance.downSnkaeBoard.Count; i++)
        {
            if (SnakeManager.Instance.downSnkaeBoard[i].fisrtNo == pasaCurrentNo)
            {
                lastNo = SnakeManager.Instance.downSnkaeBoard[i].lastNo;
                path = SnakeManager.Instance.downSnkaeBoard[i].path;
                isSetUp = true;
                break;

            }
        }
        if (isSetUp == true)
        {
            //if(isSocket==false)
            //{
            //    if (pasaCurrentNo == 26)
            //    {

            //        this.gameObject.transform.DOPath(SnakeManager.Instance.path26.path.wps, 2f).OnComplete(() => CompleteDown(isSocket));
            //    }
            //    else if (pasaCurrentNo == 39)
            //    {

            //        this.gameObject.transform.DOPath(SnakeManager.Instance.path39.path.wps, 2f).OnComplete(() => CompleteDown(isSocket));
            //    }
            //    else if (pasaCurrentNo == 51)
            //    {

            //        this.gameObject.transform.DOPath(SnakeManager.Instance.path51.path.wps, 2f).OnComplete(() => CompleteDown(isSocket));
            //    }
            //}
            pasaCurrentNo = lastNo;
            if (isSocket)
            {
                SnakeManager.Instance.playerScoreCnt2 = pasaCurrentNo;
            }
            else
            {
                SoundManager.Instance.TokenKillSound();
                SnakeManager.Instance.playerScoreCnt1 = pasaCurrentNo;
            }
            //float moveScale = 0.15f;
            //float currentScale = this.gameObject.transform.localScale.x;
            //currentScale += moveScale;
            print("Pasa Current No : " + pasaCurrentNo);
            //if (isSocket == true)
            //{
            //    if (pasaCurrentNo == 26)
            //    {

            //        this.gameObject.transform.DOPath(SnakeManager.Instance.path26.path.wps, 2f).OnComplete(() => CompleteDown(isSocket));
            //    }
            //    else if (pasaCurrentNo == 39)
            //    {

            //        this.gameObject.transform.DOPath(SnakeManager.Instance.path39.path.wps, 2f).OnComplete(() => CompleteDown(isSocket));
            //    }
            //    else if (pasaCurrentNo == 51)
            //    {

            //        this.gameObject.transform.DOPath(SnakeManager.Instance.path51.path.wps, 2f).OnComplete(() => CompleteDown(isSocket));
            //    }
            //}
            //this.gameObject.transform.DOScale(new Vector3(currentScale, currentScale, currentScale), 0.16f).OnComplete(() =>
            //    this.gameObject.transform.DOScale(new Vector3(currentScale - moveScale, currentScale - moveScale, currentScale - moveScale), 0.1f));
            this.gameObject.transform.DOMove(SnakeManager.Instance.allObj[pasaCurrentNo].transform.position, 1f).OnComplete(() => CompleteDown(isSocket));
        }
        return isSetUp;

    }

    void CompleteDown(bool isSocket)
    {
        CheckSamePos();
        print("Is Socket : " + isSocket);
        if (isSocket == false)
        {
            if (BotManager.Instance.isConnectBot)
            {
                bool isSendPlayer = false;
                bool isSendBot = false;

                if (DataManager.Instance.isDiceClick == true)
                {

                    DataManager.Instance.isDiceClick = false;
                    isSendPlayer = true;
                    isSendBot = false;

                }
                else if (DataManager.Instance.isDiceClick == false)
                {
                    isSendPlayer = false;
                    isSendBot = true;
                }
                SnakeManager.Instance.Change_Turn_Bot(isSendPlayer, isSendBot);
            }
            else
            {
                SnakeManager.Instance.isClickAvaliableDice = 0;
                SnakeManager.Instance.PlayerChangeTurn();
            }
        }
    }


    #endregion
}


