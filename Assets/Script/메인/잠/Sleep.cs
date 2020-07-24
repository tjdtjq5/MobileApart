using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleep : MonoBehaviour
{
    [Header("위치이동")]
    public Transform sleepTrans; Vector3 originPos;
    [Header("캐릭터")]
    public Transform character;
    public float characterTransSize;  float originSize;
    [Header("캐릭터 카메라")]
    public Transform theCam;

    public void SleepOpen()
    {
        // 위치 잡기 
        originPos = character.position;
        character.position = new Vector3(character.position.x, sleepTrans.position.y, character.position.z);

        // 사이즈 잡기 
        originSize = character.localScale.x;
        character.localScale = new Vector3(characterTransSize, characterTransSize, characterTransSize);

        // 카메라 위치 잡기 
        //theCam.position = new Vector3()
    }

    public void SleepClose()
    {

    }
}
