using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joying : MonoBehaviour
{
    [Header("SetOff")]
    public GameObject[] setOff;

    [Header("선택 패널")]
    public Transform selectPannel;

    [Header("블러카메라")]
    public MobileBlur mobileBlur;

    [Header("가위바위보")]
    public RockPaperScissors rockPaperScissors;

    [Header("캐릭터모션")]
    public CharacterMotion characterMotion;

    public void JoyingOpen()
    {
        for (int i = 0; i < setOff.Length; i++)
        {
            setOff[i].SetActive(false);
        }

        selectPannel.gameObject.SetActive(true);
        mobileBlur.enabled = true;

        characterMotion.SetFlag(true);
    }

    public void JoyingClose()
    {
        for (int i = 0; i < setOff.Length; i++)
        {
            setOff[i].SetActive(true);
        }
        selectPannel.gameObject.SetActive(false);
        mobileBlur.enabled = false;
        rockPaperScissors.gameObject.SetActive(false);

        characterMotion.SetFlag(false);
    }

    public void SelectBtn(int index)
    {
        switch (index)
        {
            case 0:
                rockPaperScissors.gameObject.SetActive(true);
                rockPaperScissors.RockPaperScissorsOpen();

                if (GameManager.instance.userInfoManager.GetUserNeed(NeedKind.즐거움) < 50)
                {
                    GameManager.instance.userInfoManager.SetUserNeed(
                           50,
                           GameManager.instance.userInfoManager.GetUserNeed(NeedKind.청결함),
                           GameManager.instance.userInfoManager.GetUserNeed(NeedKind.포만감),
                           GameManager.instance.userInfoManager.GetUserNeed(NeedKind.활력));
                }
                selectPannel.gameObject.SetActive(false);
                mobileBlur.enabled = false;
                break;
            case 1:
                if (GameManager.instance.userInfoManager.GetUserMoney(MoneyKind.Gold) < 1000)
                {
                    OverrideCanvas.instance.RedAlram("골드가 모자랍니다.");
                    return;
                }

                if (GameManager.instance.userInfoManager.GetUserNeed(NeedKind.즐거움) < 50)
                {
                    GameManager.instance.userInfoManager.SetUserNeed(
                           50,
                           GameManager.instance.userInfoManager.GetUserNeed(NeedKind.청결함),
                           GameManager.instance.userInfoManager.GetUserNeed(NeedKind.포만감),
                           GameManager.instance.userInfoManager.GetUserNeed(NeedKind.활력));
                }

                GameManager.instance.userInfoManager.SetUserMoney(MoneyKind.Gold, GameManager.instance.userInfoManager.GetUserMoney(MoneyKind.Gold) - 1000);
                GameManager.instance.userInfoManager.SetUserNeed(
                      GameManager.instance.userInfoManager.GetUserNeed(NeedKind.즐거움) + 20,
                      GameManager.instance.userInfoManager.GetUserNeed(NeedKind.청결함),
                      GameManager.instance.userInfoManager.GetUserNeed(NeedKind.포만감),
                      GameManager.instance.userInfoManager.GetUserNeed(NeedKind.활력));

                selectPannel.gameObject.SetActive(false);
                mobileBlur.enabled = false;
                break;
            case 2:
                if (GameManager.instance.userInfoManager.GetUserMoney(MoneyKind.Crystal) < 1000)
                {
                    OverrideCanvas.instance.RedAlram("크리스탈이 모자랍니다.");
                    return;
                }
                GameManager.instance.userInfoManager.SetUserMoney(MoneyKind.Crystal, GameManager.instance.userInfoManager.GetUserMoney(MoneyKind.Crystal) - 1000);
                GameManager.instance.userInfoManager.SetUserNeed(
                     100,
                     GameManager.instance.userInfoManager.GetUserNeed(NeedKind.청결함),
                     GameManager.instance.userInfoManager.GetUserNeed(NeedKind.포만감),
                     GameManager.instance.userInfoManager.GetUserNeed(NeedKind.활력));

                selectPannel.gameObject.SetActive(false);
                mobileBlur.enabled = false;
                break;
        }

        selectPannel.gameObject.SetActive(false);
        mobileBlur.enabled = false;

        GameManager.instance.userInfoManager.SaveUserNeed(GameManager.instance.userInfoManager.currentCharacter);
    }
}
