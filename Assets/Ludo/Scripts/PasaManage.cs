using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;

public class PasaManage : MonoBehaviour
{
    public bool isMoving;
    public Vector3 firstPosition;
    public int pasaCurrentNo;
    public GameObject pasaParentObj;
    public GameObject pasaObj;
    public int playerNo;
    public int updatedPlayerNo;
    public int playerSubNo;
    public int orgParentNo;
    public float singlePasaScaleX;
    public float singlePasaScaleY;

    public int diceValue;



    public bool isStarted = false;

    public float scale2;
    public float scale3;
    public float scale4;
    public float scale5;
    public float scale6;
    public bool isClick;
    public bool isSafe;
    public bool isStopZoom;

    public bool isPlayer;
    public bool isReadyForClick;
    public int orgNo;


    public bool isOneTimeMove;
    public List<PasaMove> scaleList1 = new List<PasaMove>();
    public bool isPasaWin;
    // Start is called before the first frame update
    void Start()
    {
        firstPosition = transform.position;
        //playerNo = DataManager.Instance.playerNo;

        if (this.GetComponent<Image>().sprite == LudoManager.Instance.blueToken)
        {
            playerNo = 1;
            updatedPlayerNo = 1;
        }
        else if (this.GetComponent<Image>().sprite == LudoManager.Instance.redToken)
        {
            playerNo = 2;
            updatedPlayerNo = 2;
        }
        else if (this.GetComponent<Image>().sprite == LudoManager.Instance.greenToken)
        {
            playerNo = 3;
            updatedPlayerNo = 3;
        }
        else if (this.GetComponent<Image>().sprite == LudoManager.Instance.yellowToken)
        {
            playerNo = 4;
            updatedPlayerNo = 4;
        }



        LudoManager.Instance.pasaSocketList.Add(this);
        if (playerNo == DataManager.Instance.playerNo)
        {
            isPlayer = true;
            LudoManager.Instance.currentPlayerPasaList.Add(this);

            if (LudoManager.Instance.currentPlayerPasaList.Count == 4)
            {
                if (DataManager.Instance.modeType == 2 || DataManager.Instance.modeType == 3)
                {
                    LudoManager.Instance.TokenAllNumberFirst();
                }
            }
        }

        if (playerNo != DataManager.Instance.playerNo)
        {
            LudoManager.Instance.pasaBotPlayer.Add(this);
        }

    }


    int GetOrginialNumber(int cNo, int pNo)
    {
        //print(" Player Number : "+ pNo);
        if (pNo == 1)
        {
            return cNo;
        }
        else if (pNo == 2)
        {
            return LudoManager.Instance.orgListNo2[cNo - 1];
        }
        else if (pNo == 3)
        {
            return LudoManager.Instance.orgListNo3[cNo - 1];
        }
        else if (pNo == 4)
        {
            return LudoManager.Instance.orgListNo4[cNo - 1];
        }
        return 0;
    }

    #region pasa First

    public void PasaOnFirst()
    {
        pasaCurrentNo = 1;
        orgNo = GetOrginialNumber(pasaCurrentNo, orgParentNo);
        if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
        {
            LudoManager.Instance.pasaObjects.Add(this.gameObject);
        }

        List<GameObject> findObj = CheckAvaliableObjectSamePos(1, false);

        if (findObj.Count == 4)
        {
            ScaleManageMent(findObj, LudoManager.Instance.numberObj[0].transform.position);
        }
    }


    #endregion

    #region ScaleManage

    public void ScaleManageMent(List<GameObject> scaleObj, Vector3 pos)
    {

        float posX = pos.x;
        float posY = pos.y;
        float scale = 0;
        float increment = 0;
        //if(scaleObj.Count == 3)
        //{
        //    var duplicates = scaleObj.GroupBy(x => x.GetComponent<PasaManage>().orgNo).Where(g => g.Count() > 1).Select(y => y.Key).ToList();
        //    for (int i = scaleObj.Count - 1; i >= 0 ; i--)
        //    {
        //        var obj = scaleObj[i].GetComponent<PasaManage>();
        //        if (obj.orgNo != duplicates[0])
        //            scaleObj.RemoveAt(i);
        //    }
        //}
        if(scaleObj.Count > 4)
        {
            var list = scaleObj.Distinct().ToList();
            scaleObj.Clear();
            scaleObj = list;
        }

        if (scaleObj.Count == 1)
        {
            increment = 0f;
            posX = pos.x;
            scale = scale2;
            print("Scale object 1");
        }
        else if (scaleObj.Count == 2)
        {
            increment = 0.1f;
            posX = pos.x - (increment * 1.5f);
            scale = scale2;
            print("Scale object 2");
        }
        else if (scaleObj.Count == 3)
        {
            increment = 0.07f;
            posX = pos.x - (increment * 2.1f);
            scale = scale3;
            print("Scale object 3");
        }
        else if (scaleObj.Count == 4)
        {
            increment = 0.05f;
            posX = pos.x - (increment * 2.5f);
            scale = scale4;
            print("Scale object 4");
        }
        else if (scaleObj.Count == 5)
        {
            scale = scale5;
            increment = 0.04f;
            posX = pos.x - (increment * 3f);
            scale = scale4;
            print("Scale object 5");
        }
        else if (scaleObj.Count == 6)
        {
            scale = scale6;
            increment = 0.03f;
            posX = pos.x - (increment * 3.5f);
            scale = scale4;
            print("Scale object 6");
        }
        else if(scaleObj.Count > 6)
        {
            scale = scale5;
            increment = 0.02f;
            posX = pos.x - (increment * 3.5f);
            scale = scale4;
            print("Scale object > 6");
        }


        for (int i = 0; i < scaleObj.Count; i++)
        {
            posX += increment;
            scaleObj[i].transform.position = new Vector3(posX, posY, pos.z);
            scaleObj[i].transform.localScale = new Vector3(scale, scale, scale);
            print("PasaDetails" + scaleObj[i].name  + " x pos " + posX +  " y pos " + posY + " x orginal " + gameObject.transform.position.x + " y orginal " + gameObject.transform.position.y);
        }

    }

    #endregion


    #region Increament Pasa

    public void PasaButtonClick()
    {
        print("Clicked on pasa Button");
        if (isClick == false && playerNo == LudoManager.Instance.currentPlayerNo 
            && !LudoManager.Instance.pasaCollectList.Contains(this) && isReadyForClick 
            && (DataManager.Instance.isFourPlayer ? LudoManager.Instance.playerRoundChecker == DataManager.Instance.playerNo : true))
        {
            isClick = true;
            LudoManager.Instance.isPathClick = false;
            SoundManager.Instance.TickTimerStop();
            if (pasaCurrentNo == 0)
            {
                LudoManager.Instance.MovePlayer(playerSubNo, 1);
                LudoManager.Instance.PlayerStopDice();
                LudoManager.Instance.RestartTimer();

                pasaCurrentNo = 1;
                isSafe = true;
                List<GameObject> findObj = CheckAvaliableObjectSamePos(1, false);
                if (findObj.Count == 0)
                {
                    Vector3 pos = LudoManager.Instance.numberObj[pasaCurrentNo - 1].transform.position;
                    if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                    {
                        LudoManager.Instance.pasaObjects.Add(this.gameObject);
                    }
                    List<GameObject> createList = new List<GameObject>();
                    createList.Add(pasaObj);
                    pasaObj.transform.DOMove(LudoManager.Instance.numberObj[pasaCurrentNo - 1].transform.position, 0.25f);
                    pasaObj.transform.DOScale(new Vector3(singlePasaScaleX, singlePasaScaleY, singlePasaScaleX), 0.25f).OnComplete(() =>
                    ScaleManageMent(createList, pos));

                }
                else
                {
                    if (findObj.Count == 1)
                    {
                        Vector3 pos = LudoManager.Instance.numberObj[pasaCurrentNo - 1].transform.position;
                        pasaCurrentNo = 1;
                        if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                        {
                            LudoManager.Instance.pasaObjects.Add(this.gameObject);
                        }
                        List<GameObject> createList = new List<GameObject>();
                        createList.Add(findObj[0]);
                        createList.Add(pasaObj);
                        pasaObj.transform.DOMove(new Vector3(pos.x + 0.05f, pos.y, pos.z), 0.25f);
                        pasaObj.transform.DOScale(new Vector3(scale2, scale2, scale2), 0.25f).OnComplete(() =>
                        ScaleManageMent(createList, pos));
                    }
                    if (findObj.Count == 2)
                    {
                        Vector3 pos = LudoManager.Instance.numberObj[pasaCurrentNo - 1].transform.position;
                        pasaCurrentNo = 1;
                        if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                        {
                            LudoManager.Instance.pasaObjects.Add(this.gameObject);
                        }
                        List<GameObject> createList = new List<GameObject>();
                        createList.Add(findObj[0]);
                        createList.Add(findObj[1]);
                        createList.Add(pasaObj);
                        pasaObj.transform.DOMove(new Vector3(pos.x, pos.y, pos.z), 0.25f);
                        pasaObj.transform.DOScale(new Vector3(scale3, scale3, scale3), 0.25f).OnComplete(() =>
                        ScaleManageMent(createList, pos));
                    }
                    if (findObj.Count == 3)
                    {
                        Vector3 pos = LudoManager.Instance.numberObj[pasaCurrentNo - 1].transform.position;
                        pasaCurrentNo = 1;
                        if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                        {
                            LudoManager.Instance.pasaObjects.Add(this.gameObject);
                        }
                        List<GameObject> createList = new List<GameObject>();
                        createList.Add(findObj[0]);
                        createList.Add(findObj[1]);
                        createList.Add(findObj[2]);
                        createList.Add(pasaObj);
                        pasaObj.transform.DOMove(new Vector3(pos.x + 0.027f, pos.y, pos.z), 0.25f);
                        pasaObj.transform.DOScale(new Vector3(scale4, scale4, scale4), 0.25f).OnComplete(() =>
                        ScaleManageMent(createList, pos));

                    }
                    if (findObj.Count == 4)
                    {
                        Vector3 pos = LudoManager.Instance.numberObj[pasaCurrentNo - 1].transform.position;
                        pasaCurrentNo = 1;
                        if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                        {
                            LudoManager.Instance.pasaObjects.Add(this.gameObject);
                        }
                        List<GameObject> createList = new List<GameObject>();
                        createList.Add(findObj[0]);
                        createList.Add(findObj[1]);
                        createList.Add(findObj[2]);
                        createList.Add(findObj[3]);
                        createList.Add(pasaObj);
                        pasaObj.transform.DOMove(new Vector3(pos.x + 0.025f, pos.y - 0.05f, pos.z), 0.25f);
                        pasaObj.transform.DOScale(new Vector3(scale5, scale5, scale5), 0.25f).OnComplete(() =>
                        ScaleManageMent(createList, pos));

                    }
                    if (findObj.Count == 5)
                    {
                        Vector3 pos = LudoManager.Instance.numberObj[pasaCurrentNo - 1].transform.position;
                        pasaCurrentNo = 1;
                        if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                        {
                            LudoManager.Instance.pasaObjects.Add(this.gameObject);
                        }
                        List<GameObject> createList = new List<GameObject>();
                        createList.Add(findObj[0]);
                        createList.Add(findObj[1]);
                        createList.Add(findObj[2]);
                        createList.Add(findObj[3]);
                        createList.Add(findObj[4]);
                        createList.Add(pasaObj);
                        pasaObj.transform.DOMove(new Vector3(pos.x, pos.y - 0.05f, pos.z), 0.25f);
                        pasaObj.transform.DOScale(new Vector3(scale5, scale5, scale5), 0.25f).OnComplete(() =>
                        ScaleManageMent(createList, pos));

                    }
                    if (findObj.Count == 6)
                    {
                        Vector3 pos = LudoManager.Instance.numberObj[pasaCurrentNo - 1].transform.position;
                        pasaCurrentNo = 1;
                        if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                        {
                            LudoManager.Instance.pasaObjects.Add(this.gameObject);
                        }
                        List<GameObject> createList = new List<GameObject>();
                        createList.Add(findObj[0]);
                        createList.Add(findObj[1]);
                        createList.Add(findObj[2]);
                        createList.Add(findObj[3]);
                        createList.Add(findObj[4]);
                        createList.Add(findObj[5]);
                        createList.Add(pasaObj);
                        pasaObj.transform.DOMove(new Vector3(pos.x, pos.y - 0.05f, pos.z), 0.25f);
                        pasaObj.transform.DOScale(new Vector3(scale6, scale6, scale6), 0.25f).OnComplete(() =>
                        ScaleManageMent(createList, pos));

                    }
                    if (findObj.Count == 7)
                    {
                        Vector3 pos = LudoManager.Instance.numberObj[pasaCurrentNo - 1].transform.position;
                        pasaCurrentNo = 1;
                        if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                        {
                            LudoManager.Instance.pasaObjects.Add(this.gameObject);
                        }
                        List<GameObject> createList = new List<GameObject>();
                        createList.Add(findObj[0]);
                        createList.Add(findObj[1]);
                        createList.Add(findObj[2]);
                        createList.Add(findObj[3]);
                        createList.Add(findObj[4]);
                        createList.Add(findObj[5]);
                        createList.Add(findObj[6]);
                        createList.Add(pasaObj);
                        pasaObj.transform.DOMove(new Vector3(pos.x + 0.08f, pos.y - 0.05f, pos.z), 0.25f);
                        pasaObj.transform.DOScale(new Vector3(scale6, scale6, scale6), 0.25f).OnComplete(() =>
                        ScaleManageMent(createList, pos));
                    }
                }

                orgNo = GetOrginialNumber(pasaCurrentNo, orgParentNo);

            }
            for (int i = 0; i < LudoManager.Instance.currentPlayerPasaList.Count; i++)
            {
                LudoManager.Instance.currentPlayerPasaList[i].isReadyForClick = false;
            }

            if (LudoManager.Instance.pasaCurrentNo != 6)
            {
                LudoManager.Instance.PlayerDiceChange();
            }
            else if (LudoManager.Instance.pasaCurrentNo == 6)
            {
                LudoManager.Instance.isClickAvaliableDice = 0;
            }

        }
    }


    public int counter;
    public void Move_Increment_Steps(int no)
    {
        if (isPasaWin)
        {
            return;
        }
        counter++;
        isMoving = true;//self player start
        LudoManager.Instance.ScoreManage(orgParentNo, 1);
        LudoManager.Instance.TimerStop();
        float moveScale = 0.15f;
        float currentScale = this.gameObject.transform.localScale.x;
        currentScale += moveScale;


        pasaCurrentNo += 1;

        orgNo = GetOrginialNumber(pasaCurrentNo, orgParentNo);
        isOneTimeMove = true;
        SoundManager.Instance.TokenMoveSound();


        if (pasaCurrentNo == 57)
        {
            

            this.gameObject.transform.DOScale(new Vector3(currentScale, currentScale, currentScale), 0.16f).OnComplete(() =>
            this.gameObject.transform.DOScale(new Vector3(currentScale - moveScale, currentScale - moveScale, currentScale - moveScale), 0.1f));
            this.gameObject.transform.DOMoveY(this.gameObject.transform.position.y + 0.2f, 0.27f).OnComplete(() =>
              Check_Move_Increment_Next(no));
        }
        else
        {
            if (isPasaWin == false)
            {
                this.gameObject.transform.DOScale(new Vector3(currentScale, currentScale, currentScale), 0.16f).OnComplete(() =>
                this.gameObject.transform.DOScale(new Vector3(currentScale - moveScale, currentScale - moveScale, currentScale - moveScale), 0.1f));
                this.gameObject.transform.DOMove(LudoManager.Instance.numberObj[pasaCurrentNo - 1].transform.position, 0.25f).OnComplete(() =>
                Check_Move_Increment_Next(no));
            }
        }

    }



    bool isEnterMove = false;

    void Check_Move_Increment_Next(int no)
    {
        diceValue = no;
        bool isLast = false;
        if (no == counter)
        {
            isMoving = false;//self player stop
            counter = 0;
            isEnterMove = false;
            isLast = true;
            if (pasaCurrentNo == 1 || pasaCurrentNo == 9 || pasaCurrentNo == 22 || pasaCurrentNo == 35 || pasaCurrentNo == 48 || pasaCurrentNo == 1 || pasaCurrentNo == 14 || pasaCurrentNo == 27 || pasaCurrentNo == 40)
            {
                isSafe = true;
            }
            else if (pasaCurrentNo > 51)
                isSafe = true;
            else
            {
                isSafe = false;
            }
            if (DataManager.Instance.modeType == 3)
            {
                if (LudoManager.Instance.isDicelessKillTimeFree == false)
                {
                    //GameUIManager.Instance.FirstNumberRemove();
                }
                else
                {
                    LudoManager.Instance.isDicelessKillTimeFree = false;
                }
            }
            //if (DataManager.Instance.modeType == 3 && DataManager.Instance.playerNo == 3)//might need change for 4 player
            //{
            //    if (GameUIManager.Instance.moveCnt == 24)
            //    {
            //        LudoManager.Instance.WinUserShow();
            //    }
            //}
            if (pasaCurrentNo == 57)
            {
                if (this.GetComponent<Image>().sprite == LudoManager.Instance.blueToken)
                {
                    LudoManager.Instance.bluePasaWinList.Add(this.gameObject);
                    ScaleManageMent(LudoManager.Instance.bluePasaWinList, this.transform.position);
                }
                if (this.GetComponent<Image>().sprite == LudoManager.Instance.redToken)
                {
                    LudoManager.Instance.redPasaWinList.Add(this.gameObject);
                    ScaleManageMent(LudoManager.Instance.redPasaWinList, this.transform.position);
                }
                if (this.GetComponent<Image>().sprite == LudoManager.Instance.greenToken)
                {
                    LudoManager.Instance.greenPasaWinList.Add(this.gameObject);
                    ScaleManageMent(LudoManager.Instance.greenPasaWinList, this.transform.position);
                }
                if (this.GetComponent<Image>().sprite == LudoManager.Instance.yellowToken)
                {
                    LudoManager.Instance.yellowPasaWinList.Add(this.gameObject);
                    ScaleManageMent(LudoManager.Instance.yellowPasaWinList, this.transform.position);
                }
                SoundManager.Instance.TokenHomeSound();
                LudoManager.Instance.GeneratePasaFire();
                isClick = false;
                isPasaWin = true;
                isEnterMove = true;
                LudoManager.Instance.ScoreManage(orgParentNo, 56);
                
                //this.gameObject.transform.DOScale(new Vector3(0, 0, 0), 0.16f).OnComplete(() =>
                LudoManager.Instance.pasaObjects.Remove(this.gameObject)/*)*/;
                LudoManager.Instance.currentPlayerPasaList.Remove(this);
                if (LudoManager.Instance.currentPlayerPasaList.Count == 0)
                    LudoManager.Instance.WinUserShow();

                else if(LudoManager.Instance.bluePasaWinList.Count == 4 || LudoManager.Instance.redPasaWinList.Count == 4 || LudoManager.Instance.greenPasaWinList.Count == 4 || LudoManager.Instance.yellowPasaWinList.Count == 4)
                    LudoManager.Instance.WinUserShow();

                
            }

            bool isTurnChange = false;
            if (DataManager.Instance.modeType == 3)
            {
                isTurnChange = true;
            }

            if (LudoManager.Instance.pasaCurrentNo != 6)
            {
                if (isEnterMove == false)
                {
                    isTurnChange = true;
                }
                else
                {
                    LudoManager.Instance.isClickAvaliableDice = 0;
                    LudoManager.Instance.RestartTimer();
                }

            }
            else if (LudoManager.Instance.pasaCurrentNo == 6)
            {
                LudoManager.Instance.isClickAvaliableDice = 0;
                LudoManager.Instance.RestartTimer();
                //if(DataManager.Instance.modeType == 3)
                //{
                //    LudoManager.Instance.DiceLessPasaButton();
                //}
            }
            if (pasaCurrentNo >= 0)
            {
                ScaleManageToNext(false, pasaCurrentNo, true, isTurnChange);
            }
            if (pasaCurrentNo - 1 >= 0)
            {
                ScaleManageToNext(false, pasaCurrentNo - 1, false, false);
            }
        }
        else
        {
            if (pasaCurrentNo >= 0)
            {
                ScaleManageToNext(false, pasaCurrentNo, false, false);
            }
            if (pasaCurrentNo - 1 >= 0)
            {
                ScaleManageToNext(false, pasaCurrentNo - 1, false, false);
            }
            Move_Increment_Steps(no);
        }


    }
    #endregion



    #region Move Decerement

    public void Move_Decrement_Steps(int pNo, bool isSocket)
    {
        List<GameObject> nObj = new List<GameObject>();
        if (pNo == 1)
        {
            nObj = LudoManager.Instance.numberObj;
        }
        else if (pNo == 2)
        {
            nObj = LudoManager.Instance.numberObj3;//change back to 2 if there are conflicts in 2 player
        }
        else if (pNo == 3)
        {
            if (DataManager.Instance.isTwoPlayer)
            {
                nObj = LudoManager.Instance.numberObj2;
            }
            else
            {
                nObj = LudoManager.Instance.numberObj2;
            }
        }
        else if (pNo == 4)
        {
            nObj = LudoManager.Instance.numberObj4;
        }

        pasaCurrentNo -= 1;

        this.gameObject.transform.DOMove(nObj[pasaCurrentNo].transform.position, 0.08f).OnComplete(() =>
        Check_Move_Decrement_Next(pNo, isSocket, nObj));

    }
    void Check_Move_Decrement_Next(int pNo, bool isSocket, List<GameObject> nObj)
    {
        if (DataManager.Instance.modeType == 1)
        {
            if (pasaCurrentNo == 0)
            {
                this.gameObject.transform.DOMove(firstPosition, 0.08f);
                this.gameObject.transform.DOScale(Vector3.one, 0.08f);
                isClick = false;
                isStarted = false;
                isOneTimeMove = false;
                orgNo = 0;
                LudoManager.Instance.isClickAvaliableDice = 0;
                LudoManager.Instance.RestartTimer();
                DataManager.Instance.isTimeAuto = false;
                LudoManager.Instance.pasaObjects.Remove(this.gameObject);
                if (DataManager.Instance.isTwoPlayer ? BotManager.Instance.isConnectBot && pNo == 1 && LudoManager.Instance.isSix == false :
                    (DataManager.Instance.joinPlayerDatas[LudoManager.Instance.playerRoundChecker - 1].userId.Contains("Ludo")
                    && LudoManager.Instance.isSix == false))
                {
                    DataManager.Instance.isDiceClick = false;
                    LudoManager.Instance.isClickAvaliableDice = 1;
                    DataManager.Instance.isTimeAuto = false;

                    if (DataManager.Instance.isDiceClick == false)
                    {
                        print("Bot killing turn");
                        //LudoManager.Instance.OnceTimeTurnBot();
                        Invoke(nameof(WaitAfterSecondTurn), 0.5f);
                    }
                }
                else if (LudoManager.Instance.isSix == true)
                {
                    LudoManager.Instance.isSix = false;
                }
                LudoManager.Instance.RestartTimer();
            }
            else
            {
                Move_Decrement_Steps(pNo, isSocket);
            }
        }
        else if (DataManager.Instance.modeType == 2 || DataManager.Instance.modeType == 3)
        {
            if (pasaCurrentNo == 0)
            {
                //this.gameObject.transform.DOMove(nObj[pasaCurrentNo-1].transform.position, 0.08f); //.OnComplete(() =>
                isOneTimeMove = false;
                isSafe = true;
                pasaCurrentNo = 1;
                orgNo = GetOrginialNumber(pasaCurrentNo, orgParentNo);

                //LudoManager.Instance.isClickAvaliableDice = 0;
                //LudoManager.Instance.RestartTimer();

                LudoManager.Instance.isClickAvaliableDice = 0;
                DataManager.Instance.isTimeAuto = false;

                LudoManager.Instance.RestartTimer();
                /*if (DataManager.Instance.isTwoPlayer ? BotManager.Instance.isConnectBot :
                    (DataManager.Instance.joinPlayerDatas[LudoManager.Instance.playerRoundChecker - 1].userId.Contains("Ludo")
                    && LudoManager.Instance.isSix == false))*/
                if (DataManager.Instance.isTwoPlayer ? BotManager.Instance.isConnectBot && pNo == 1 && LudoManager.Instance.isSix == false
                        : (DataManager.Instance.joinPlayerDatas[LudoManager.Instance.playerRoundChecker - 1].userId
                               .Contains("Ludo")
                           && LudoManager.Instance.isSix == false))

                {
                    LudoManager.Instance.isClickAvaliableDice = 1;
                    DataManager.Instance.isDiceClick = false;
                    DataManager.Instance.isTimeAuto = false;
                    if (DataManager.Instance.isDiceClick == false)
                    {
                        print("Bot killing turn");
                        //LudoManager.Instance.OnceTimeTurnBot();
                        Invoke(nameof(WaitAfterSecondTurn), 0.5f);
                    }
                }
                else if (LudoManager.Instance.isSix == true)
                    LudoManager.Instance.isSix = false;
                if (DataManager.Instance.isDiceClick)
                {
                    GenerateDieAfterChanceDiceless();
                }
                else
                {
                    LudoManager.Instance.RestartTimer();
                }

                if (DataManager.Instance.isDiceClick)
                {
                    print("pass No : " + orgNo);
                    ScaleManageToNext_Kill(orgNo, true);
                }
                else
                {
                    print("pass No : " + pasaCurrentNo);
                    ScaleManageToNext_Kill(pasaCurrentNo, false);
                }

                //if (orgNo >= 0)
                //{

                //    if (!isSocket)
                //    {
                //        print("Org Pass No :" + orgNo);
                //        Second_Three_Death(orgNo, isSocket);
                //    }
                //    else
                //    {
                //        print("Pass No :" + pasaCurrentNo);

                //        Second_Three_Death(pasaCurrentNo, isSocket);

                //        //ScaleManageToNext(false, pasaCurrentNo+1, false, false);
                //    }

                //}


                // LudoManager.Instance.pasaObjects.Remove(this.gameObject);
            }
            else
            {
                Move_Decrement_Steps(pNo, isSocket);
            }
        }
    }





    #endregion

    //#region Zoom

    //float getStartScale = 0;
    //public bool isFirstZoom;
    //public void PlayerPasaZoom()
    //{
    //    if (isFirstZoom == false)
    //    {
    //        isFirstZoom = true;
    //        getStartScale = pasaObj.transform.localScale.x;
    //    }
    //    pasaObj.transform.DOScale(new Vector3(getStartScale - 0.05f, getStartScale - 0.05f, getStartScale - 0.05f), 0.25f).OnComplete(() =>
    //       pasaObj.transform.DOScale(new Vector3(getStartScale + 0.05f, getStartScale + 0.05f, getStartScale + 0.05f), 0.25f).OnComplete(() =>
    //        CheckZoom()
    //    ));
    //}
    //void CheckZoom()
    //{
    //    if(isStopZoom==false)
    //    {
    //        pasaObj.transform.localScale = new Vector3(getStartScale, getStartScale , getStartScale);
    //        PlayerPasaZoom();
    //    }
    //    else
    //    {
    //        pasaObj.transform.localScale = new Vector3(getStartScale, getStartScale, getStartScale);
    //    }
    //}


    //#endregion

    #region Common Method


    void GenerateDieAfterChanceDiceless()
    {
        DataManager.Instance.isDiceClick = true;

        LudoManager.Instance.isClickAvaliableDice = 0;
        LudoManager.Instance.OurShadowMaintain();
        DataManager.Instance.isTimeAuto = false;
        LudoManager.Instance.RestartTimer();
        if (DataManager.Instance.modeType == 3)
        {
            LudoManager.Instance.isDicelessKillTimeFree = true;
            LudoManager.Instance.DiceLessPasaButton_Kill();
        }
    }

    List<GameObject> CheckAvaliableObjectSamePos(int no, bool isSocket)
    {

        List<GameObject> checkList = new List<GameObject>();
        for (int i = 0; i < LudoManager.Instance.pasaObjects.Count; i++)
        {
            GameObject pObj = LudoManager.Instance.pasaObjects[i];
            if (pObj.GetComponent<PasaManage>().pasaCurrentNo == no && pObj.GetComponent<PasaManage>().playerNo == DataManager.Instance.playerNo)
            {
                checkList.Add(pObj);
            }
            else if (pObj.GetComponent<PasaManage>().orgNo == no && pObj.GetComponent<PasaManage>().playerNo != DataManager.Instance.playerNo)
            {
                checkList.Add(pObj);
            }
        }
        return checkList;
    }
    public int IsEntryZone(PasaManage p)
    {
        if (p.pasaCurrentNo == 52 || p.pasaCurrentNo == 53 || p.pasaCurrentNo == 54 || p.pasaCurrentNo == 55 || p.pasaCurrentNo == 56)
        {
            //Ek Manage
            return 1;
        }
        else
        {
            return 0;
        }
        return 0;
    }




    public void ScaleManageToNext_Kill(int no, bool isPassOrgNo)
    {
        List<GameObject> checkList = new List<GameObject>();

        for (int i = 0; i < LudoManager.Instance.pasaObjects.Count; i++)
        {
            GameObject pObj = LudoManager.Instance.pasaObjects[i];
            PasaManage manage = pObj.GetComponent<PasaManage>();
            if (manage.orgNo == no && (IsEntryZone(manage) != 1 || manage.orgParentNo == DataManager.Instance.playerNo))
            {
                if (manage.orgNo > 51)
                {

                    checkList.Add(pObj);

                }
                else
                {
                    checkList.Add(pObj);
                }
            }

        }

        if (checkList.Count > 0)
        {
            Vector3 changePos = Vector3.zero;
            for (int i = 0; i < checkList.Count; i++)
            {
                PasaManage checkPasaManage = checkList[i].GetComponent<PasaManage>();

                if (isPassOrgNo == true)
                {
                    if (checkPasaManage.orgNo == no)
                    {
                        int getNo = 0;

                        getNo = checkPasaManage.orgNo - 1;

                        if (getNo >= 0)
                        {
                            changePos = LudoManager.Instance.numberObj[getNo].transform.position;
                            ScaleManageMent(checkList, changePos);
                            print("Kill is called");
                        }
                        break;
                    }
                }
                else if (isPassOrgNo == false)
                {
                    if (checkPasaManage.pasaCurrentNo == no)
                    {
                        int getNo = 0;

                        getNo = checkPasaManage.pasaCurrentNo - 1;
                        //print("Simple get No : " + getNo + "  Check list Count : " + checkList.Count);
                        for (int j = 0; j < checkList.Count; j++)
                        {
                            print(j + " : " + checkList[j]);
                        }


                        if (getNo >= 0)
                        {
                            changePos = LudoManager.Instance.numberObj[getNo].transform.position;
                            ScaleManageMent(checkList, changePos);
                            //print("Socket Check List No in else if : " + checkList[0].name);
                            print("Kill is called");
                        }
                        break;
                    }
                }
            }

        }
    }

    public void ScaleManageToNext(bool isSocket, int no, bool isLast, bool isTurn)
    {
        List<GameObject> checkList = new List<GameObject>();
        bool isDiePlayer = false;
        for (int i = 0; i < LudoManager.Instance.pasaObjects.Count; i++)
        {
            GameObject pObj = LudoManager.Instance.pasaObjects[i];
            PasaManage manage = pObj.GetComponent<PasaManage>();
            if (manage.orgNo == no && (IsEntryZone(manage) != 1 || manage.updatedPlayerNo/*change to orgParentNo if problem arises*/ == DataManager.Instance.playerNo))
            {
                if (manage.orgNo > 51)
                {
                    if (manage.playerNo == DataManager.Instance.playerNo)
                    {
                        checkList.Add(pObj);
                    }
                    else //remove this condition if there is problem in token positioning
                        checkList.Add(pObj);
                }
                else
                {
                    checkList.Add(pObj);
                }
            }
            
        }
        if (isLast)
        {
            int cntOnePos = 0;

            for (int i = 0; i < checkList.Count; i++)
            {
                PasaManage cPasaManage = checkList[i].GetComponent<PasaManage>();
                if (cPasaManage.isSafe == false)
                {
                    cntOnePos++;
                }
            }
            List<GameObject> checkForFriendlyPasa = new List<GameObject>();
            if (DataManager.Instance.isFourPlayer)
                checkForFriendlyPasa = checkList.FindAll(x => x.GetComponent<PasaManage>().updatedPlayerNo == LudoManager.Instance.playerRoundChecker);
            else if (DataManager.Instance.isTwoPlayer)
                checkForFriendlyPasa = checkList.FindAll(x => x.GetComponent<PasaManage>().playerNo == this.playerNo);
            if (checkForFriendlyPasa.Count > 1)
                cntOnePos = 0;
            if (cntOnePos == 2)
            {
                for (int i = 0; i < checkList.Count; i++)
                {
                    PasaManage cPasaManage = new PasaManage();
                    PasaManage kPasaManage = new PasaManage();
                    if (DataManager.Instance.isTwoPlayer)
                    {
                        if(this.playerNo == LudoManager.Instance.currentPlayerNo && checkList.Count == 2)
                            cPasaManage = checkList.Find(x => x.GetComponent<PasaManage>().playerNo != this.playerNo).GetComponent<PasaManage>();
                        else if(checkList.Count == 2 && this.playerNo != LudoManager.Instance.currentPlayerNo)
                            cPasaManage = checkList.Find(x => x.GetComponent<PasaManage>().playerNo == LudoManager.Instance.currentPlayerNo).GetComponent<PasaManage>();
                    }
                    else if(DataManager.Instance.isFourPlayer)
                    {
                        cPasaManage = checkList.Find(x => x.GetComponent<PasaManage>().updatedPlayerNo == LudoManager.Instance.playerRoundChecker).GetComponent<PasaManage>();//checkList[i].GetComponent<PasaManage>();//this pasa is killing kPasaManage
                        kPasaManage = checkList.Find(x => x.GetComponent<PasaManage>().updatedPlayerNo != LudoManager.Instance.playerRoundChecker).GetComponent<PasaManage>();//checkList[i + 1].GetComponent<PasaManage>();//this pasa is getting killed by cPasaManage
                    }
                    if (isSocket)// true means other player is moving and false means that we are moving for admin
                    {
                        if (cPasaManage.playerNo == DataManager.Instance.playerNo && DataManager.Instance.isTwoPlayer)
                        {
                            LudoManager.Instance.ScoreManageDecrese(cPasaManage.orgParentNo, cPasaManage.pasaCurrentNo - 1);
                            int sNo = 0;
                            if (cPasaManage.orgParentNo == 1)
                            {
                                sNo = 3;
                            }
                            if (cPasaManage.orgParentNo == 3)
                            {
                                sNo = 1;
                            }
                            //LudoManager.Instance.ScoreManage(sNo, cPasaManage.pasaCurrentNo);//uncomment if there are issues in scoring
                            isDiePlayer = true;
                            if (isDiePlayer == true)
                            {
                                print("dice no for kill = " + LudoManager.Instance.pasaCurrentNo);
                                if (BotManager.Instance.isConnectBot ? diceValue == 6 : LudoManager.Instance.pasaCurrentNo == 6 && DataManager.Instance.modeType != 3/*Player is getting another chance if getting killed in diceless mode*/)
                                    LudoManager.Instance.isSix = true;
                                isTurn = true;
                            }
                            //isTurn = true;
                            cPasaManage.Move_Decrement_Steps(cPasaManage.orgParentNo, isSocket);
                        }
                        else if (DataManager.Instance.isFourPlayer)
                        {
                            LudoManager.Instance.ScoreManageDecrese(kPasaManage.orgParentNo, kPasaManage.pasaCurrentNo - 1);
                            int sNo = 0;
                            if (cPasaManage.orgParentNo == 1 && DataManager.Instance.isTwoPlayer)
                                sNo = 3;
                            else if (cPasaManage.orgParentNo == 3 && DataManager.Instance.isTwoPlayer)
                                sNo = 1;
                            else
                                sNo = cPasaManage.orgParentNo;
                            //LudoManager.Instance.ScoreManage(sNo, cPasaManage.pasaCurrentNo);
                            isDiePlayer = true;
                            if (isDiePlayer == true)
                            {
                                print("dice no = " + LudoManager.Instance.pasaCurrentNo);
                                if (BotManager.Instance.isConnectBot ? diceValue == 6 : LudoManager.Instance.pasaCurrentNo == 6)
                                    LudoManager.Instance.isSix = true;
                                isTurn = true;
                            }
                            //LudoManager.Instance.ScoreManage(sNo, cPasaManage.pasaCurrentNo);//uncomment if there are issues in scoring
                            print("Player No. " + cPasaManage.playerNo + " killing Player No. " + kPasaManage.playerNo);
                            if (DataManager.Instance.isFourPlayer && LudoManager.Instance.isAdmin == true)
                                kPasaManage.Move_Decrement_Steps(kPasaManage.orgParentNo, isSocket);
                            else if (LudoManager.Instance.isAdmin == false && DataManager.Instance.isFourPlayer)
                                kPasaManage.Move_Decrement_Steps(kPasaManage.orgParentNo, isSocket);
                            else
                                cPasaManage.Move_Decrement_Steps(cPasaManage.orgParentNo, isSocket);
                        }
                        break;
                        //}
                    }
                    else
                    {
                        if (cPasaManage.playerNo != DataManager.Instance.playerNo && DataManager.Instance.isTwoPlayer)
                        {
                            LudoManager.Instance.ScoreManageDecrese(cPasaManage.orgParentNo, cPasaManage.pasaCurrentNo - 1);
                            int sNo = 0;
                            if (cPasaManage.orgParentNo == 1)
                            {
                                sNo = 3;
                            }
                            if (cPasaManage.orgParentNo == 3)
                            {
                                sNo = 1;
                            }
                            //LudoManager.Instance.ScoreManage(sNo, cPasaManage.pasaCurrentNo);
                            isDiePlayer = true;
                            isTurn = true;
                            cPasaManage.Move_Decrement_Steps(cPasaManage.orgParentNo, isSocket);
                            break;
                        }
                        else if (DataManager.Instance.isFourPlayer ? cPasaManage.updatedPlayerNo == DataManager.Instance.playerNo : cPasaManage.playerNo != DataManager.Instance.playerNo)
                        {
                            LudoManager.Instance.ScoreManageDecrese(kPasaManage.orgParentNo, kPasaManage.pasaCurrentNo - 1);
                            int sNo = 0;
                            if (DataManager.Instance.isTwoPlayer)
                            {
                                if (cPasaManage.orgParentNo == 1)
                                    sNo = 3;
                                if (cPasaManage.orgParentNo == 3)
                                    sNo = 1;
                            }
                            else
                                sNo = cPasaManage.orgParentNo;
                            //LudoManager.Instance.ScoreManage(sNo, cPasaManage.pasaCurrentNo);
                            isDiePlayer = true;
                            if (isDiePlayer == true)
                                isTurn = true;
                            print("Player No. " + cPasaManage.playerNo + " killing Player No. " + kPasaManage.playerNo);
                            if (DataManager.Instance.isFourPlayer && LudoManager.Instance.isAdmin == true /*? true : LudoManager.Instance.playerRoundChecker == 1*/)
                                kPasaManage.Move_Decrement_Steps(kPasaManage.orgParentNo, isSocket);
                            else if (DataManager.Instance.isFourPlayer && LudoManager.Instance.isAdmin == false /*&& DataManager.Instance.playerNo == LudoManager.Instance.playerRoundChecker*/)
                                kPasaManage.Move_Decrement_Steps(kPasaManage.orgParentNo, isSocket);
                            else if (DataManager.Instance.isTwoPlayer)
                                cPasaManage.Move_Decrement_Steps(cPasaManage.orgParentNo, isSocket);

                            //--
                            break;
                        }

                    }
                }
            }
        }

        if (checkList.Count > 0 && isDiePlayer == false)
        {
            Vector3 changePos = Vector3.zero;
            for (int i = 0; i < checkList.Count; i++)
            {
                PasaManage checkPasaManage = checkList[i].GetComponent<PasaManage>();


                if (isSocket)
                {
                    if (checkPasaManage.playerNo != DataManager.Instance.playerNo)
                    {
                        int getNo = 0;

                        if (isSocket)
                        {
                            getNo = checkPasaManage.orgNo - 1;

                        }
                        else
                        {
                            getNo = checkPasaManage.pasaCurrentNo - 1;

                        }
                        if (getNo >= 0)
                        {
                            if (isSocket)
                            {
                                if (getNo >= 51)
                                {

                                    // print("Enter The Socket Condition 1-1: " + getNo);
                                    changePos = LudoManager.Instance.numberObj2[25].transform.position;
                                }
                                else
                                {
                                    //print("Enter The Socket Condition 1-2: " + getNo);

                                    changePos = LudoManager.Instance.numberObj[getNo].transform.position;
                                }
                            }
                            else
                            {
                                if (getNo >= 51)
                                {

                                    //print("Enter The Socket Condition 2 - 1: " + getNo);
                                    changePos = LudoManager.Instance.numberObj2[25].transform.position;
                                }
                                else
                                {
                                    //print("Enter The Socket Condition 2 - 2: " + getNo);

                                    changePos = LudoManager.Instance.numberObj[getNo].transform.position;
                                }
                                // changePos = LudoManager.Instance.numberObj[getNo].transform.position;
                            }


                            if (no - 1 >= 0 && getNo - 1 >= 0)
                            {
                                List<GameObject> checkList1 = new List<GameObject>();
                                for (int j = 0; j < LudoManager.Instance.pasaObjects.Count; j++)
                                {
                                    GameObject pObj = LudoManager.Instance.pasaObjects[j];

                                    //if (pObj.GetComponent<PasaManage>().orgNo == no-1)
                                    //{
                                    //    checkList1.Add(pObj);
                                    //}
                                    PasaManage manage1 = pObj.GetComponent<PasaManage>();
                                    if (manage1.orgNo == no/*uncomment this if causing problems - 1*/ && (IsEntryZone(manage1) != 1 || manage1.updatedPlayerNo == DataManager.Instance.playerNo))
                                    {
                                        if (manage1.updatedPlayerNo != DataManager.Instance.playerNo)
                                        {
                                            checkList.Add(pObj);
                                        }
                                    }


                                }
                                Vector3 changePos1 = Vector3.zero;
                                if (isSocket)
                                {
                                    if (getNo - 1 >= 51)
                                    {

                                        //print("Minus Condition Else 1");
                                        changePos1 = LudoManager.Instance.numberObj2[25 - 1].transform.position;
                                    }
                                    else
                                    {

                                        changePos1 = LudoManager.Instance.numberObj[getNo - 1].transform.position;
                                    }
                                }
                                else
                                {
                                    changePos1 = LudoManager.Instance.numberObj[getNo - 1].transform.position;
                                }
                                ScaleManageMent(checkList1, changePos1);
                                print("Scale is called isTurn = " + isTurn);
                                //print("Socket Check List No inside : " + checkList[0].name);
                            }
                            //print("Socket Check List No : " + checkList[0].name);
                            //if (checkList.Count == 3)
                            //{
                            //    int max = -1;
                            //    for (int j = checkList.Count - 1; j >= 0; j--)
                            //    {
                            //        var obj = checkList[j].GetComponent<PasaManage>();
                            //        print("position of pasa = " + obj.orgNo);
                            //        //if (obj.orgNo > max && max != -1)
                            //        //{
                            //        //    max = obj.orgNo;
                            //        //    checkList.RemoveAt(j);
                            //        //}
                            //        //if(max == -1 && obj.orgNo > max)
                            //        //{
                            //        //    max = obj.orgNo;
                            //        //}

                            //        //print("isMoving pawns  = " + isMoving);
                            //        //if (checkList[j].GetComponent<PasaManage>().isMoving == false)
                            //        //    checkList.RemoveAt(j);
                            //    }
                            //}

                            ScaleManageMent(checkList, changePos);////////////////////////////////////////////////
                            //print("Scale is called");
                        }
                        break;
                    }
                }
                else
                {
                    if (checkPasaManage.updatedPlayerNo == DataManager.Instance.playerNo)
                    {
                        int getNo = 0;

                        if (isSocket)
                        {
                            getNo = checkPasaManage.orgNo - 1;

                        }
                        else
                        {
                            getNo = checkPasaManage.pasaCurrentNo - 1;

                        }
                        if (getNo >= 0)
                        {
                            if (isSocket)
                            {

                                if (getNo >= 51)
                                {
                                    // print("Enter The Simple Condition 1-1: " + getNo);
                                    changePos = LudoManager.Instance.numberObj2[25].transform.position;
                                }
                                else
                                {
                                    // print("Enter The Simple Condition 1-2: " + getNo);

                                    changePos = LudoManager.Instance.numberObj[getNo].transform.position;
                                }
                            }
                            else
                            {
                                //print("Enter The Simple Condition 2-1: " + getNo);

                                changePos = LudoManager.Instance.numberObj[getNo].transform.position;
                            }
                            if (no - 1 >= 0 && getNo - 1 >= 0)
                            {
                                List<GameObject> checkList1 = new List<GameObject>();
                                for (int j = 0; j < LudoManager.Instance.pasaObjects.Count; j++)
                                {
                                    GameObject pObj = LudoManager.Instance.pasaObjects[j];

                                    //if (pObj.GetComponent<PasaManage>().orgNo == no - 1)
                                    //{
                                    //    checkList1.Add(pObj);
                                    //}

                                    PasaManage manage1 = pObj.GetComponent<PasaManage>();
                                    if (manage1.orgNo == no - 1 && (IsEntryZone(manage1) != 1 || manage1.orgParentNo == DataManager.Instance.playerNo))
                                    {
                                        if (manage1.playerNo == DataManager.Instance.playerNo && isTurn == false)
                                        {
                                            checkList1.Add(pObj);
                                        }
                                    }
                                }

                                Vector3 changePos1 = Vector3.zero;
                                if (isSocket)
                                {
                                    if (getNo - 1 >= 51)
                                    {
                                        //print("Minus Condition Else 1");
                                        changePos1 = LudoManager.Instance.numberObj2[25 - 1].transform.position;
                                    }
                                    else
                                    {
                                        changePos1 = LudoManager.Instance.numberObj[getNo - 1].transform.position;
                                    }
                                }
                                else
                                {
                                    //                                    print("Minus Condition Else 2");
                                    if (checkList1.Count > 0)
                                    {
                                        //  print("Simple Check List No : " + checkList1[0].name);
                                    }
                                    changePos1 = LudoManager.Instance.numberObj[getNo - 1].transform.position;
                                }
                                ScaleManageMent(checkList1, changePos1);
                                print("Scale is called");
                                //print("Socket Check List No in else comdiction: " + checkList[0].name);
                            }
                            //print("Simple Check List No : " + checkList[0].name);
                            ScaleManageMent(checkList, changePos);
                            print("Scale is called");
                        }
                        break;
                    }
                    /* else
                     {
                         int getNo = 0;

                         if (isSocket)
                         {
                             getNo = checkPasaManage.orgNo - 1;

                         }
                         else
                         {
                             getNo = checkPasaManage.pasaCurrentNo - 1;

                         }

                         print("Check No : " + no + " --- Get No : " + getNo);
                         if (no - 1 >= 0 && getNo - 1 >= 0)
                         {
                             List<GameObject> checkList1 = new List<GameObject>();
                             for (int j = 0; j < LudoManager.Instance.pasaObjects.Count; j++)
                             {
                                 GameObject pObj = LudoManager.Instance.pasaObjects[j];

                                 //if (pObj.GetComponent<PasaManage>().orgNo == no - 1)
                                 //{
                                 //    checkList1.Add(pObj);
                                 //}

                                 PasaManage manage1 = pObj.GetComponent<PasaManage>();
                                 if (manage1.orgNo == no - 1 && (IsEntryZone(manage1) != 1 || manage1.orgParentNo == DataManager.Instance.playerNo))
                                 {
                                     if (manage1.playerNo != DataManager.Instance.playerNo)
                                     {
                                         checkList1.Add(pObj);
                                     }
                                 }
                             }

                             Vector3 changePos1 = Vector3.zero;
                             if (isSocket)
                             {
                                 if (getNo - 1 >= 51)
                                 {
                                     //print("Minus Condition Else 1");
                                     changePos1 = LudoManager.Instance.numberObj2[getNo - 1].transform.position;
                                 }
                                 else
                                 {
                                     changePos1 = LudoManager.Instance.numberObj[getNo - 1].transform.position;
                                 }
                             }
                             else
                             {
                                 print("Minus Condition Else 2");
                                 if (checkList1.Count > 0)
                                 {
                                     print("Simple Check List No : " + checkList1[0].name);
                                 }
                                 changePos1 = LudoManager.Instance.numberObj[getNo - 1].transform.position;
                             }
                             ScaleManageMent(checkList1, changePos1);
                         }
                     }*/
                    // }
                }
            }


        }


        //if (isTurn)
        //{
        //    if (isDiePlayer)
        //    {
        //        LudoManager.Instance.isClickAvaliableDice = 0;
        //        LudoManager.Instance.RestartTimer();
        //    }
        //    else
        //    {
        //        LudoManager.Instance.isClickAvaliableDice = 0;
        //        LudoManager.Instance.PlayerChangeTurn();
        //    }
        //}
        if (isTurn)
        {
            if (isDiePlayer)
            {
                LudoManager.Instance.isClickAvaliableDice = 0;
                LudoManager.Instance.RestartTimer();
            }
            else
            {
                LudoManager.Instance.isClickAvaliableDice = 0;

                //Greejesh Ludo

                if (DataManager.Instance.isFourPlayer ? (LudoManager.Instance.isPlayerNextTurn() == false
                    && LudoManager.Instance.isAdmin) :
                    BotManager.Instance.isConnectBot)//if it is 2 player mode then change to previous condition
                {
                    bool isSendBot = false;
                    bool isSendPlayer = false;
                    print("Dice Click Value : " + DataManager.Instance.isDiceClick);
                    //if (DataManager.Instance.isDiceClick)
                    //{
                    //    isSendBot = false;
                    //    isSendPlayer = true;
                    //}
                    //else
                    //{
                    //    isSendBot = true;
                    //    isSendPlayer = false;
                    //}
                    if (DataManager.Instance.isTwoPlayer)
                    {
                        if (DataManager.Instance.isDiceClick)
                        {
                            isSendBot = false;
                            isSendPlayer = true;
                        }
                        else
                        {
                            isSendBot = true;
                            isSendPlayer = false;
                        }
                    }
                    else
                    {
                        // isSendBot = true;
                        // isSendPlayer = false;

                        print("$$$$$$$$$$$$$$$$$$$$$$ TRIGEREED &&&&&&&&&&&&&&&&&&&&&&&&&&&&&");
                        // isSendBot = false;
                        // isSendPlayer = true;
                        if (LudoManager.Instance.playerRoundChecker == 4)// DataManager.Instance.isDiceClick && 
                        {
                            isSendBot = true;
                            isSendPlayer = false;
                            print("$$$$$$$$$$$$$ Inside the if &&&&&&&&&&&& roundChecker = " + LudoManager.Instance.playerRoundChecker);
                        }
                        else
                        {
                            isSendBot = false;
                            isSendPlayer = true;
                            print("$$$$$$$$$$$$$ Outside the if &&&&&&&&&&&& roundChecker = " + LudoManager.Instance.playerRoundChecker);
                        }


                    }

                    LudoManager.Instance.BotChangeTurn(isSendBot, isSendPlayer);
                }
                else if (DataManager.Instance.isFourPlayer ? (LudoManager.Instance.isPlayerNextTurn() == false && LudoManager.Instance.isAdmin == false) : false)
                {
                    //LudoManager.Instance.playerRoundChecker = LudoManager.Instance.playerRoundChecker switch
                    //{
                    //    1 => 2,
                    //    2 => 3,
                    //    3 => 4,
                    //    4 => 1,
                    //    _ => LudoManager.Instance.playerRoundChecker
                    //};
                    LudoManager.Instance.ChangeTurn();
                    Debug.Log("roundChecker = " + LudoManager.Instance.playerRoundChecker);
                    DataManager.Instance.isDiceClick = false;
                    LudoManager.Instance.isClickAvaliableDice = 1;
                    DataManager.Instance.isTimeAuto = false;
                    LudoManager.Instance.BotDiceChange(DataManager.Instance.joinPlayerDatas[LudoManager.Instance.playerRoundChecker - 1].userId, LudoManager.Instance.playerRoundChecker, true);
                }
                else
                {
                    LudoManager.Instance.PlayerChangeTurn();
                }
            }
        }
        else
        {
            if (LudoManager.Instance.playerRoundChecker == DataManager.Instance.playerNo && DataManager.Instance.modeType != 3)
                DataManager.Instance.isTimeAuto = false;
        }
    }




    #endregion

    #region Socket Method

    public void MoveStart(int listNo, int move)
    {
        List<GameObject> numberObj = new List<GameObject>();
        //playerstart
        isMoving = true;
        if (listNo == 1)
        {
            numberObj = LudoManager.Instance.numberObj;
        }
        else if (listNo == 2)
        {
            numberObj = LudoManager.Instance.numberObj3;//change to 2 if problem arises
        }
        else if (listNo == 3)
        {
            if (DataManager.Instance.isTwoPlayer)
            {
                numberObj = LudoManager.Instance.numberObj2;
            }
            else
            {
                numberObj = LudoManager.Instance.numberObj2;
            }
        }
        else if (listNo == 4)
        {
            numberObj = LudoManager.Instance.numberObj4;
        }

        if (pasaCurrentNo == 0 && !isStarted)
        {
            pasaCurrentNo = 1;

            isSafe = true;
            isStarted = true;
            orgNo = GetOrginialNumber(pasaCurrentNo, orgParentNo);
            LudoManager.Instance.RestartTimer();

            List<GameObject> findObj = CheckAvaliableObjectSamePos(orgNo, true);
            if (findObj.Count == 0)
            {
                Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                {
                    LudoManager.Instance.pasaObjects.Add(this.gameObject);
                }
                List<GameObject> createList = new List<GameObject>();
                createList.Add(pasaObj);
                pasaObj.transform.DOMove(numberObj[pasaCurrentNo - 1].transform.position, 0.25f);
                pasaObj.transform.DOScale(new Vector3(singlePasaScaleX, singlePasaScaleY, singlePasaScaleX), 0.25f).OnComplete(() =>
                ScaleManageMent(createList, pos));
            }
            else
            {
                if (findObj.Count == 1)
                {
                    Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                    pasaCurrentNo = 1;
                    if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                    {
                        LudoManager.Instance.pasaObjects.Add(this.gameObject);
                    }
                    List<GameObject> createList = new List<GameObject>();
                    createList.Add(findObj[0]);
                    createList.Add(pasaObj);
                    pasaObj.transform.DOMove(new Vector3(pos.x + 0.05f, pos.y, pos.z), 0.25f);
                    pasaObj.transform.DOScale(new Vector3(scale2, scale2, scale2), 0.25f).OnComplete(() =>
                    ScaleManageMent(createList, pos));
                }
                if (findObj.Count == 2)
                {
                    Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                    pasaCurrentNo = 1;
                    if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                    {
                        LudoManager.Instance.pasaObjects.Add(this.gameObject);
                    }
                    List<GameObject> createList = new List<GameObject>();
                    createList.Add(findObj[0]);
                    createList.Add(findObj[1]);
                    createList.Add(pasaObj);
                    pasaObj.transform.DOMove(new Vector3(pos.x, pos.y, pos.z), 0.25f);
                    pasaObj.transform.DOScale(new Vector3(scale3, scale3, scale3), 0.25f).OnComplete(() =>
                    ScaleManageMent(createList, pos));
                }
                if (findObj.Count == 3)
                {
                    Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                    pasaCurrentNo = 1;
                    if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                    {
                        LudoManager.Instance.pasaObjects.Add(this.gameObject);
                    }
                    List<GameObject> createList = new List<GameObject>();
                    createList.Add(findObj[0]);
                    createList.Add(findObj[1]);
                    createList.Add(findObj[2]);
                    createList.Add(pasaObj);
                    pasaObj.transform.DOMove(new Vector3(pos.x + 0.027f, pos.y, pos.z), 0.25f);
                    pasaObj.transform.DOScale(new Vector3(scale4, scale4, scale4), 0.25f).OnComplete(() =>
                    ScaleManageMent(createList, pos));
                }
                if (findObj.Count == 4)
                {
                    Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                    pasaCurrentNo = 1;
                    if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                    {
                        LudoManager.Instance.pasaObjects.Add(this.gameObject);
                    }
                    List<GameObject> createList = new List<GameObject>();
                    createList.Add(findObj[0]);
                    createList.Add(findObj[1]);
                    createList.Add(findObj[2]);
                    createList.Add(findObj[3]);
                    createList.Add(pasaObj);
                    pasaObj.transform.DOMove(new Vector3(pos.x + 0.025f, pos.y - 0.05f, pos.z), 0.25f);
                    pasaObj.transform.DOScale(new Vector3(scale5, scale5, scale5), 0.25f).OnComplete(() =>
                    ScaleManageMent(createList, pos));
                }
                if (findObj.Count == 5)
                {
                    Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                    pasaCurrentNo = 1;
                    if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                    {
                        LudoManager.Instance.pasaObjects.Add(this.gameObject);
                    }
                    List<GameObject> createList = new List<GameObject>();
                    createList.Add(findObj[0]);
                    createList.Add(findObj[1]);
                    createList.Add(findObj[2]);
                    createList.Add(findObj[3]);
                    createList.Add(findObj[4]);
                    createList.Add(pasaObj);
                    pasaObj.transform.DOMove(new Vector3(pos.x, pos.y - 0.05f, pos.z), 0.25f);
                    pasaObj.transform.DOScale(new Vector3(scale5, scale5, scale5), 0.25f).OnComplete(() =>
                    ScaleManageMent(createList, pos));
                }
                if (findObj.Count == 6)
                {
                    Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                    pasaCurrentNo = 1;
                    if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                    {
                        LudoManager.Instance.pasaObjects.Add(this.gameObject);
                    }
                    List<GameObject> createList = new List<GameObject>();
                    createList.Add(findObj[0]);
                    createList.Add(findObj[1]);
                    createList.Add(findObj[2]);
                    createList.Add(findObj[3]);
                    createList.Add(findObj[4]);
                    createList.Add(findObj[5]);
                    createList.Add(pasaObj);
                    pasaObj.transform.DOMove(new Vector3(pos.x, pos.y - 0.05f, pos.z), 0.25f);
                    pasaObj.transform.DOScale(new Vector3(scale6, scale6, scale6), 0.25f).OnComplete(() =>
                    ScaleManageMent(createList, pos));
                }
                if (findObj.Count == 7)
                {
                    Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                    pasaCurrentNo = 1;
                    if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                    {
                        LudoManager.Instance.pasaObjects.Add(this.gameObject);
                    }
                    List<GameObject> createList = new List<GameObject>();
                    createList.Add(findObj[0]);
                    createList.Add(findObj[1]);
                    createList.Add(findObj[2]);
                    createList.Add(findObj[3]);
                    createList.Add(findObj[4]);
                    createList.Add(findObj[5]);
                    createList.Add(findObj[6]);
                    createList.Add(pasaObj);
                    pasaObj.transform.DOMove(new Vector3(pos.x + 0.08f, pos.y - 0.05f, pos.z), 0.25f);
                    pasaObj.transform.DOScale(new Vector3(scale6, scale6, scale6), 0.25f).OnComplete(() =>
                    ScaleManageMent(createList, pos));
                }
            }
        }
        else
        {
            Move_Increment_Steps1(move, listNo);
        }
    }


    public void Move_Increment_Steps1(int no, int listNo)
    {
        if (isPasaWin == true)
        {
            return;
        }
        List<GameObject> numberObj = new List<GameObject>();
        if (listNo == 1)
        {
            numberObj = LudoManager.Instance.numberObj;
        }
        else if (listNo == 2)
        {
            numberObj = LudoManager.Instance.numberObj3;//change to 2 if problem arises
        }
        else if (listNo == 3)
        {
            if (DataManager.Instance.isTwoPlayer)
            {
                numberObj = LudoManager.Instance.numberObj2;
            }
            else
            {
                numberObj = LudoManager.Instance.numberObj2;
            }
        }
        else if (listNo == 4)
        {
            numberObj = LudoManager.Instance.numberObj4;
        }

        counter++;
        LudoManager.Instance.TimerStop();
        float moveScale = 0.15f;
        float currentScale = this.gameObject.transform.localScale.x;
        currentScale += moveScale;

        pasaCurrentNo += 1;

        isOneTimeMove = true;
        print("-------------move step 1 is called---------------------- for pasaNo = " + this.gameObject.name);
        //print("org Parent No : " + orgParentNo);
        LudoManager.Instance.ScoreManage(orgParentNo, 1);
        orgNo = GetOrginialNumber(pasaCurrentNo, orgParentNo);

        //print("Second OrgNo : " + orgNo);
        SoundManager.Instance.TokenMoveSound();


        //if (pasaCurrentNo == 57)
        //{
        //    this.gameObject.transform.DOScale(new Vector3(currentScale, currentScale, currentScale), 0.16f).OnComplete(() =>
        //    this.gameObject.transform.DOScale(new Vector3(currentScale - moveScale, currentScale - moveScale, currentScale - moveScale), 0.1f));
        //    this.gameObject.transform.DOMoveY(this.gameObject.transform.position.y - 0.2f, 0.3f).OnComplete(() =>
        //      Check_Move_Increment_Next1(no, listNo));
        //}
        //else
        //{
            if (isPasaWin == false)
            {
                this.gameObject.transform.DOScale(new Vector3(currentScale, currentScale, currentScale), 0.16f).OnComplete(() =>
                this.gameObject.transform.DOScale(new Vector3(currentScale - moveScale, currentScale - moveScale, currentScale - moveScale), 0.1f));
                this.gameObject.transform.DOMove(numberObj[pasaCurrentNo - 1].transform.position, 0.25f).OnComplete(() =>
                Check_Move_Increment_Next1(no, listNo));
            }

        //}

    }
    void Check_Move_Increment_Next1(int no, int lno)
    {
        diceValue = no;
        bool isLast = false;
        bool isEnterWin = false;
        print("no = " + no + " counter = " + counter);
        if (no == counter || counter >= no)
        {
            
            counter = 0;
            if (orgNo == 1 || orgNo == 9 || orgNo == 22 || orgNo == 35 || orgNo == 48 || orgNo == 1 || orgNo == 14 || orgNo == 27 || orgNo == 40)
            {
                isSafe = true;
            }
            else if (orgNo > 51)
                isSafe = true;
            else
            {
                isSafe = false;
            }
            if (orgNo == 57 && orgParentNo != 1)
                isSafe = false;
            isLast = true;


            if (pasaCurrentNo == 57)
            {
                if (this.GetComponent<Image>().sprite == LudoManager.Instance.blueToken)
                {
                    LudoManager.Instance.bluePasaWinList.Add(this.gameObject);
                    ScaleManageMent(LudoManager.Instance.bluePasaWinList, this.transform.position);
                }
                if (this.GetComponent<Image>().sprite == LudoManager.Instance.redToken)
                {
                    LudoManager.Instance.redPasaWinList.Add(this.gameObject);
                    ScaleManageMent(LudoManager.Instance.redPasaWinList, this.transform.position);
                }
                if (this.GetComponent<Image>().sprite == LudoManager.Instance.greenToken)
                {
                    LudoManager.Instance.greenPasaWinList.Add(this.gameObject);
                    ScaleManageMent(LudoManager.Instance.greenPasaWinList, this.transform.position);
                }
                if (this.GetComponent<Image>().sprite == LudoManager.Instance.yellowToken)
                {
                    LudoManager.Instance.yellowPasaWinList.Add(this.gameObject);
                    ScaleManageMent(LudoManager.Instance.yellowPasaWinList, this.transform.position);
                }

                SoundManager.Instance.TokenHomeSound();
                LudoManager.Instance.GeneratePasaFire();
                isClick = false;
                isPasaWin = true;
                isEnterWin = true;
                LudoManager.Instance.ScoreManage(orgParentNo, 56);
                //this.gameObject.transform.DOScale(new Vector3(0, 0, 0), 0.16f).OnComplete(() =>
                LudoManager.Instance.pasaObjects.Remove(this.gameObject)/*)*/;
                LudoManager.Instance.pasaBotPlayer.Remove(this);
                if (LudoManager.Instance.pasaBotPlayer.Count == 0)
                    LudoManager.Instance.WinUserShow();
                
                else if (LudoManager.Instance.bluePasaWinList.Count == 4 || LudoManager.Instance.redPasaWinList.Count == 4 || LudoManager.Instance.greenPasaWinList.Count == 4 || LudoManager.Instance.yellowPasaWinList.Count == 4)
                    LudoManager.Instance.WinUserShow();
            }
            //if (DataManager.Instance.modeType == 3 && DataManager.Instance.playerNo == 1)
            //{
            //    if (GameUIManager.Instance.moveCnt == 24)
            //    {
            //        LudoManager.Instance.WinUserShow();
            //    }
            //}
            if (no == 6)//&& DataManager.Instance.modeType == 1)
            {
                LudoManager.Instance.isClickAvaliableDice = 0;
                LudoManager.Instance.RestartTimer();
                //if (DataManager.Instance.modeType == 3)
                //    LudoManager.Instance.DiceLessPasaButton();
            }

            if (isEnterWin == true)
            {
                LudoManager.Instance.isClickAvaliableDice = 0;
                LudoManager.Instance.RestartTimer();

            }
            if (orgNo >= 0)
            {
                ScaleManageToNext(true, orgNo, true, false);
            }
            if (orgNo - 1 >= 0)
            {
                ScaleManageToNext(true, orgNo - 1, false, false);
            }
            //playerstop
            isMoving = false;
        }
        else
        {
            if (orgNo >= 0)
            {
                ScaleManageToNext(true, orgNo, false, false);
            }
            if (orgNo - 1 >= 0)
            {
                ScaleManageToNext(true, orgNo - 1, false, false);
            }
            Move_Increment_Steps1(no, lno);
        }
        if (isPasaWin == false)
        {
            for (int i = 0; i < LudoManager.Instance.pasaObjects.Count; i++)
            {
                //   LudoManager.Instance.pasaObjects[i].GetComponent<PasaManage>().RescaleEvery(true, isLast);
            }
        }
    }

    #endregion

    #region Bot Manager

    public void MoveStart_Bot(int listNo, int move)
    {
        List<GameObject> numberObj = new List<GameObject>();
        //botstart
        isMoving = true;
        if (listNo == 1)
        {
            numberObj = LudoManager.Instance.numberObj;
        }
        else if (listNo == 2)
        {
            //numberObj = LudoManager.Instance.numberObj2;
            numberObj = DataManager.Instance.isTwoPlayer ? LudoManager.Instance.numberObj2 : LudoManager.Instance.numberObj3;
        }
        else if (listNo == 3)
        {
            //numberObj = DataManager.Instance.isTwoPlayer ? LudoManager.Instance.numberObj3 : LudoManager.Instance.numberObj2;
            if (DataManager.Instance.isTwoPlayer)
            {
                numberObj = LudoManager.Instance.numberObj2;
            }
            else
            {
                numberObj = LudoManager.Instance.numberObj2;
            }
        }
        else if (listNo == 4)
        {
            numberObj = LudoManager.Instance.numberObj4;
        }

        if (pasaCurrentNo == 0 && !isStarted)
        {
            Debug.LogWarning("Inside the if conduction");
            pasaCurrentNo = 1;
            LudoManager.Instance.BotStopDice(DataManager.Instance.joinPlayerDatas[LudoManager.Instance.playerRoundChecker - 1].userId);
            LudoManager.Instance.MoveBot(playerSubNo, 1, DataManager.Instance.joinPlayerDatas[LudoManager.Instance.playerRoundChecker - 1].userId, updatedPlayerNo);
            isSafe = true;
            isStarted = true;
            orgNo = GetOrginialNumber(pasaCurrentNo, orgParentNo);
            LudoManager.Instance.RestartTimer();

            List<GameObject> findObj = CheckAvaliableObjectSamePos(orgNo, true);
            if (findObj.Count == 0)
            {
                Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                {
                    LudoManager.Instance.pasaObjects.Add(this.gameObject);
                }
                List<GameObject> createList = new List<GameObject>();
                createList.Add(pasaObj);
                pasaObj.transform.DOMove(numberObj[pasaCurrentNo - 1].transform.position, 0.25f);
                pasaObj.transform.DOScale(new Vector3(singlePasaScaleX, singlePasaScaleY, singlePasaScaleX), 0.25f).OnComplete(() =>
                ScaleManageMent(createList, pos));
            }
            else
            {
                if (findObj.Count == 1)
                {
                    Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                    pasaCurrentNo = 1;
                    if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                    {
                        LudoManager.Instance.pasaObjects.Add(this.gameObject);
                    }
                    List<GameObject> createList = new List<GameObject>();
                    createList.Add(findObj[0]);
                    createList.Add(pasaObj);
                    pasaObj.transform.DOMove(new Vector3(pos.x + 0.05f, pos.y, pos.z), 0.25f);
                    pasaObj.transform.DOScale(new Vector3(scale2, scale2, scale2), 0.25f).OnComplete(() =>
                    ScaleManageMent(createList, pos));
                }
                if (findObj.Count == 2)
                {
                    Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                    pasaCurrentNo = 1;
                    if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                    {
                        LudoManager.Instance.pasaObjects.Add(this.gameObject);
                    }
                    List<GameObject> createList = new List<GameObject>();
                    createList.Add(findObj[0]);
                    createList.Add(findObj[1]);
                    createList.Add(pasaObj);
                    pasaObj.transform.DOMove(new Vector3(pos.x, pos.y, pos.z), 0.25f);
                    pasaObj.transform.DOScale(new Vector3(scale3, scale3, scale3), 0.25f).OnComplete(() =>
                    ScaleManageMent(createList, pos));
                }
                if (findObj.Count == 3)
                {
                    Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                    pasaCurrentNo = 1;
                    if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                    {
                        LudoManager.Instance.pasaObjects.Add(this.gameObject);
                    }
                    List<GameObject> createList = new List<GameObject>();
                    createList.Add(findObj[0]);
                    createList.Add(findObj[1]);
                    createList.Add(findObj[2]);
                    createList.Add(pasaObj);
                    pasaObj.transform.DOMove(new Vector3(pos.x + 0.027f, pos.y, pos.z), 0.25f);
                    pasaObj.transform.DOScale(new Vector3(scale4, scale4, scale4), 0.25f).OnComplete(() =>
                    ScaleManageMent(createList, pos));
                }
                if (findObj.Count == 4)
                {
                    Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                    pasaCurrentNo = 1;
                    if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                    {
                        LudoManager.Instance.pasaObjects.Add(this.gameObject);
                    }
                    List<GameObject> createList = new List<GameObject>();
                    createList.Add(findObj[0]);
                    createList.Add(findObj[1]);
                    createList.Add(findObj[2]);
                    createList.Add(findObj[3]);
                    createList.Add(pasaObj);
                    pasaObj.transform.DOMove(new Vector3(pos.x + 0.025f, pos.y - 0.05f, pos.z), 0.25f);
                    pasaObj.transform.DOScale(new Vector3(scale5, scale5, scale5), 0.25f).OnComplete(() =>
                    ScaleManageMent(createList, pos));
                }
                if (findObj.Count == 5)
                {
                    Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                    pasaCurrentNo = 1;
                    if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                    {
                        LudoManager.Instance.pasaObjects.Add(this.gameObject);
                    }
                    List<GameObject> createList = new List<GameObject>();
                    createList.Add(findObj[0]);
                    createList.Add(findObj[1]);
                    createList.Add(findObj[2]);
                    createList.Add(findObj[3]);
                    createList.Add(findObj[4]);
                    createList.Add(pasaObj);
                    pasaObj.transform.DOMove(new Vector3(pos.x, pos.y - 0.05f, pos.z), 0.25f);
                    pasaObj.transform.DOScale(new Vector3(scale5, scale5, scale5), 0.25f).OnComplete(() =>
                    ScaleManageMent(createList, pos));
                }
                if (findObj.Count == 6)
                {
                    Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                    pasaCurrentNo = 1;
                    if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                    {
                        LudoManager.Instance.pasaObjects.Add(this.gameObject);
                    }
                    List<GameObject> createList = new List<GameObject>();
                    createList.Add(findObj[0]);
                    createList.Add(findObj[1]);
                    createList.Add(findObj[2]);
                    createList.Add(findObj[3]);
                    createList.Add(findObj[4]);
                    createList.Add(findObj[5]);
                    createList.Add(pasaObj);
                    pasaObj.transform.DOMove(new Vector3(pos.x, pos.y - 0.05f, pos.z), 0.25f);
                    pasaObj.transform.DOScale(new Vector3(scale6, scale6, scale6), 0.25f).OnComplete(() =>
                    ScaleManageMent(createList, pos));
                }
                if (findObj.Count == 7)
                {
                    Vector3 pos = numberObj[pasaCurrentNo - 1].transform.position;
                    pasaCurrentNo = 1;
                    if (!LudoManager.Instance.pasaObjects.Contains(this.gameObject))
                    {
                        LudoManager.Instance.pasaObjects.Add(this.gameObject);
                    }
                    List<GameObject> createList = new List<GameObject>();
                    createList.Add(findObj[0]);
                    createList.Add(findObj[1]);
                    createList.Add(findObj[2]);
                    createList.Add(findObj[3]);
                    createList.Add(findObj[4]);
                    createList.Add(findObj[5]);
                    createList.Add(findObj[6]);
                    createList.Add(pasaObj);
                    pasaObj.transform.DOMove(new Vector3(pos.x + 0.08f, pos.y - 0.05f, pos.z), 0.25f);
                    pasaObj.transform.DOScale(new Vector3(scale6, scale6, scale6), 0.25f).OnComplete(() =>
                    ScaleManageMent(createList, pos));
                }
            }
            Invoke(nameof(WaitAfterSecondTurn), 0.3f);
        }
        else
        {
            LudoManager.Instance.BotStopDice(DataManager.Instance.joinPlayerDatas[LudoManager.Instance.playerRoundChecker - 1].userId);
            print("Enter The Move Bot Player");
            Move_Increment_Steps_Bot(move, listNo);
            LudoManager.Instance.MoveBot(playerSubNo, move, DataManager.Instance.joinPlayerDatas[LudoManager.Instance.playerRoundChecker - 1].userId, LudoManager.Instance.playerRoundChecker);
        }
    }

    void WaitAfterSecondTurn()
    {
        print("WaitAfterSecondTurn");
        LudoManager.Instance.OnceTimeTurnBot();

    }

    public void Move_Increment_Steps_Bot(int no, int listNo)
    {
        if (isPasaWin == true)
        {
            return;
        }
        List<GameObject> numberObj = new List<GameObject>();
        if (listNo == 1)
        {
            numberObj = LudoManager.Instance.numberObj;
        }
        else if (listNo == 2)
        {
            //numberObj = LudoManager.Instance.numberObj2;
            numberObj = DataManager.Instance.isTwoPlayer ? LudoManager.Instance.numberObj2 : LudoManager.Instance.numberObj3;
        }
        else if (listNo == 3)
        {
            if (DataManager.Instance.isTwoPlayer)
            {
                numberObj = LudoManager.Instance.numberObj2;
            }
            else
            {
                numberObj = LudoManager.Instance.numberObj2;
            }
            //numberObj = DataManager.Instance.isTwoPlayer ? LudoManager.Instance.numberObj3 : LudoManager.Instance.numberObj2;
        }
        else if (listNo == 4)
        {
            numberObj = LudoManager.Instance.numberObj4;
        }

        counter++;
        LudoManager.Instance.TimerStop();
        float moveScale = 0.15f;
        float currentScale = this.gameObject.transform.localScale.x;
        currentScale += moveScale;

        pasaCurrentNo += 1;

        isOneTimeMove = true;
        LudoManager.Instance.ScoreManage(orgParentNo, 1);
        orgNo = GetOrginialNumber(pasaCurrentNo, orgParentNo);

        SoundManager.Instance.TokenMoveSound();


        //if (pasaCurrentNo == 57)
        //{
        //    pasaObj.gameObject.transform.DOScale(new Vector3(currentScale, currentScale, currentScale), 0.16f).OnComplete(() =>
        //    pasaObj.gameObject.transform.DOScale(new Vector3(currentScale - moveScale, currentScale - moveScale, currentScale - moveScale), 0.1f));
        //    //pasaObj.gameObject.transform.DOMoveY(this.gameObject.transform.position.y - 0.2f, 0.3f).OnComplete(() => 
        //        Check_Move_Increment_Next_Bot(no, listNo)/*)*/;
        //}
        //else
        //{
            if (isPasaWin == false)
            {
                pasaObj.gameObject.transform.DOScale(new Vector3(currentScale, currentScale, currentScale), 0.16f).OnComplete(() =>
                pasaObj.gameObject.transform.DOScale(new Vector3(currentScale - moveScale, currentScale - moveScale, currentScale - moveScale), 0.1f));
                pasaObj.gameObject.transform.DOMove(numberObj[pasaCurrentNo - 1].transform.position, 0.25f).OnComplete(() =>
                Check_Move_Increment_Next_Bot(no, listNo));
                print("-------- BotPlayerNo--------------- " + LudoManager.Instance.botPlayerNo);
                print("-------- BotPlayerMoveNO--------------- " + numberObj[pasaCurrentNo - 1].name);
                print("-------- BotPasaCurrentNo" + playerNo + " --------------- " + pasaCurrentNo);
                print("-------- BotPlayerOrg No" + playerNo + "--------------- " + orgNo);
            }

        //}

    }
    void Check_Move_Increment_Next_Bot(int no, int lno)
    {
        diceValue = no;
        bool isLast = false;
        bool isEnterWin = false;
        print("no = " + no + " counter = " + counter);
        if(no == counter || counter >= no) //(no == counter)//actual condition : if (no == counter ? true : counter >= no)
        {
            //botstop
            isMoving = false;
            counter = 0;
            if (orgNo == 1 || orgNo == 9 || orgNo == 22 || orgNo == 35 || orgNo == 48 || orgNo == 1 || orgNo == 14 || orgNo == 27 || orgNo == 40)
            {
                isSafe = true;
            }
            else if(orgNo > 51)
                isSafe = true;
            else
            {
                isSafe = false;
            }
            if (orgNo == 57 && orgParentNo != 1)
                isSafe = false;
            isLast = true;


            if (pasaCurrentNo == 57)
            {
                if (this.GetComponent<Image>().sprite == LudoManager.Instance.blueToken)
                {
                    LudoManager.Instance.bluePasaWinList.Add(this.gameObject);
                    ScaleManageMent(LudoManager.Instance.bluePasaWinList, this.transform.position);
                }
                if (this.GetComponent<Image>().sprite == LudoManager.Instance.redToken)
                {
                    LudoManager.Instance.redPasaWinList.Add(this.gameObject);
                    ScaleManageMent(LudoManager.Instance.redPasaWinList, this.transform.position);
                }
                if (this.GetComponent<Image>().sprite == LudoManager.Instance.greenToken)
                {
                    LudoManager.Instance.greenPasaWinList.Add(this.gameObject);
                    ScaleManageMent(LudoManager.Instance.greenPasaWinList, this.transform.position);
                }
                if (this.GetComponent<Image>().sprite == LudoManager.Instance.yellowToken)
                {
                    LudoManager.Instance.yellowPasaWinList.Add(this.gameObject);
                    ScaleManageMent(LudoManager.Instance.yellowPasaWinList, this.transform.position);
                }

                SoundManager.Instance.TokenHomeSound();
                LudoManager.Instance.GeneratePasaFire();
                isClick = false;
                isPasaWin = true;
                isEnterWin = true;
                LudoManager.Instance.ScoreManage(orgParentNo, 56);
                //this.gameObject.transform.DOScale(new Vector3(0, 0, 0), 0.16f).OnComplete(() =>
                LudoManager.Instance.pasaObjects.Remove(this.gameObject)/*)*/;
                LudoManager.Instance.pasaBotPlayer.Remove(this);
                if (LudoManager.Instance.pasaBotPlayer.Count == 0)
                {
                    LudoManager.Instance.WinUserShow();
                }
                else if (LudoManager.Instance.bluePasaWinList.Count == 4 || LudoManager.Instance.redPasaWinList.Count == 4 || LudoManager.Instance.greenPasaWinList.Count == 4 || LudoManager.Instance.yellowPasaWinList.Count == 4)
                    LudoManager.Instance.WinUserShow();
            }
            if (DataManager.Instance.modeType == 3 && DataManager.Instance.playerNo == 1)
            {
                if (LudoUIManager.Instance.moveCnt == 24)
                {
                    LudoManager.Instance.WinUserShow();
                }
            }
            bool isTurnChange = true;
            if (no == 6 && DataManager.Instance.modeType != 3)//&& DataManager.Instance.modeType == 1)
            {
                print("Enter the bot 6 COndition");

                LudoManager.Instance.isClickAvaliableDice = 0;
                LudoManager.Instance.RestartTimer();
                LudoManager.Instance.OnceTimeTurnBot();
                isTurnChange = false;
            }

            if (isEnterWin == true)
            {
                LudoManager.Instance.isClickAvaliableDice = 0;
                LudoManager.Instance.RestartTimer();
                LudoManager.Instance.OnceTimeTurnBot();
                isTurnChange = false;

            }
            if (orgNo >= 0)
            {
                //print("isTurnChange Bot: " + isTurnChange);
                print("Org No 1 : " + orgNo);
                ScaleManageToNext(true, orgNo, true, isTurnChange);
            }
            if (orgNo - 1 >= 0)
            {
                print("Org No 2 : " + (orgNo - 1));
                ScaleManageToNext(true, orgNo - 1, false, false);
            }
            if (isTurnChange)
            {
                LudoManager.Instance.isCheckEnter = false;
            }
        }
        else
        {
            if (orgNo >= 0)
            {
                ScaleManageToNext(true, orgNo, false, false);
            }
            if (orgNo - 1 >= 0)
            {
                ScaleManageToNext(true, orgNo - 1, false, false);
            }
            Move_Increment_Steps_Bot(no, lno);
        }
        if (isPasaWin == false)
        {
            for (int i = 0; i < LudoManager.Instance.pasaObjects.Count; i++)
            {
                //LudoManager.Instance.pasaObjects[i].GetComponent<PasaManage>().RescaleEvery(true, isLast);
            }
        }
    }


    #endregion

}

[System.Serializable]
public class PasaMove
{
    public GameObject scaleObj;
    public List<GameObject> numberObj;
}