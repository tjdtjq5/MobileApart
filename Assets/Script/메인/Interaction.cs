using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public static Interaction instance;

    public bool time7Flag = false;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        time7Flag = true;
        CharacterTalk.instance.Talk("일찍 일어 났네?", 3);
        Talk.instance.Init();
    }
}
