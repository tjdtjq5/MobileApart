using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleep : MonoBehaviour
{
    [Header("위치이동")]
    public Transform sleepTrans; 
    [Header("캐릭터")]
    public Transform character; Vector3 originPos; //위치
    public float characterTransSize;  float originSize; // 사이즈
    string originAni;
    [Header("캐릭터 카메라")]
    public Transform theCam; Vector3 originCamPos;

    [Header("SetOff")]
    public GameObject[] setOff;

    [Header("SetOn")]
    public GameObject[] setOn;

    public void SleepOpen()
    {
        // 위치 잡기 
        originPos = character.position;
        character.position = new Vector3(character.position.x, sleepTrans.position.y, character.position.z);

        // 사이즈 잡기 
        originSize = character.localScale.x;
        character.localScale = new Vector3(characterTransSize, characterTransSize, characterTransSize);

        // 카메라 위치 잡기 
        originCamPos = theCam.position;
        theCam.position = new Vector3(theCam.position.x, (originCamPos.y - originPos.y) + sleepTrans.position.y, theCam.position.z);

        //캐릭터 애니 
        originAni = character.GetComponent<SkeletonAnimation>().AnimationName;
        character.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "sleep", true);
        for (int i = 0; i < setOff.Length; i++)
        {
            setOff[i].SetActive(false);
        }
        for (int i = 0; i < setOn.Length; i++)
        {
            setOn[i].SetActive(true);
        }
    }

    public void SleepClose()
    {
        character.position = originPos;
        character.localScale = new Vector3(originSize, originSize, originSize);
        theCam.position = originCamPos;
        character.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, originAni, true);

        for (int i = 0; i < setOff.Length; i++)
        {
            setOff[i].SetActive(true);
        }
        for (int i = 0; i < setOn.Length; i++)
        {
            setOn[i].SetActive(false);
        }
    }
}
