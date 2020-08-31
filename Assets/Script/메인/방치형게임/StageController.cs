using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageController : MonoBehaviour
{
    public Text stageName;
    public GameObject selectPannel;
    public Transform context;

    [Header("몬스터")]
    public MonsterController monsterController;

    private void Start()
    {
        StartFunction();
    }

    //시작함수  
    void StartFunction()
    {
        string firstStageName = GameManager.instance.stageManager.stageInfoList[0].stageName;
        StageSet(firstStageName);
    }

    void StageSet(string stageName)
    {
        monsterController.Initialize(GameManager.instance.stageManager.GetStageInfo(stageName).monsterName, GameManager.instance.stageManager.GetStageInfo(stageName).hp, GameManager.instance.stageManager.GetStageInfo(stageName).defence);
    }

    public void StageSelectOpen()
    {
        selectPannel.SetActive(true);
        for (int i = 0; i < context.childCount; i++)
        {
            context.GetChild(i).gameObject.SetActive(false);
            context.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
        }

        int stageLength = GameManager.instance.stageManager.stageInfoList.Length;
        for (int i = 0; i < stageLength; i++)
        {
            context.GetChild(i).gameObject.SetActive(true);
            string stageName = GameManager.instance.stageManager.stageInfoList[i].stageName;
            context.GetChild(i).GetComponent<Button>().onClick.AddListener(() => { StageSet(stageName); selectPannel.SetActive(false); });
            context.GetChild(i).Find("스테이지 이름").GetComponent<Text>().text = "[" + stageName + "]";
            context.GetChild(i).Find("적정 공격력").GetComponent<Text>().text = "적정 공격력 " + GameManager.instance.stageManager.GetStageInfo(stageName).properAtkPower;
        }
    }
}
