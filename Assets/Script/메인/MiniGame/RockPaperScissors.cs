using DG.Tweening;
using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RockPaperScissors : MonoBehaviour
{
    public static RockPaperScissors instance;
    [Header("말풍선")] public Talk talk;

    private void Start()
    {
        instance = this;
    }

    [Header("off해 놓을 것들")]
    public GameObject[] setOff;
    [Header("on해 놓을 것들")]
    public GameObject[] setOn;

    public void RockPaperScissorsOpen()
    {
        for (int i = 0; i < setOff.Length; i++)
        {
            setOff[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < setOn.Length; i++)
        {
            setOn[i].gameObject.SetActive(true);
        }
    }

    public void RockPaperScissorsClose()
    {
        for (int i = 0; i < setOff.Length; i++)
        {
            setOff[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < setOn.Length; i++)
        {
            setOn[i].gameObject.SetActive(false);
        }
    }
}
