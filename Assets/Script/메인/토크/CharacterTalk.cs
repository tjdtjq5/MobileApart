using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CharacterTalk : MonoBehaviour
{
    public static CharacterTalk instance;


    public GameObject ThinkingBubble;
    public GameObject TalkBubble;

    float fadeSpeed = 0.3f;

    private void Awake()
    {
        instance = this;
        tempThinkTalkCoroutine = ThinkingTalkCoroutine("", 0);
    }

    public void ThinkingTalk(string text, float time)
    {
        StopCoroutine(tempThinkTalkCoroutine);
        tempThinkTalkCoroutine = ThinkingTalkCoroutine(text, time);
        StartCoroutine(tempThinkTalkCoroutine);
    
    }
    IEnumerator tempThinkTalkCoroutine;
    IEnumerator ThinkingTalkCoroutine(string text, float time)
    {
        ThinkingBubble.SetActive(true);
        ThinkingBubble.transform.Find("Text").GetComponent<Text>().text = text;

        Vector3 tempLocalSalce = ThinkingBubble.transform.localScale;
        ThinkingBubble.transform.localScale = new Vector2(tempLocalSalce.x * .7F, tempLocalSalce.y * .7F);
        ThinkingBubble.transform.DOScale(tempLocalSalce, fadeSpeed);

        ThinkingBubble.GetComponent<Image>().DOFade(0, 0);
        ThinkingBubble.GetComponent<Image>().DOFade(1, fadeSpeed);
        for (int j = 0; j < ThinkingBubble.transform.childCount; j++)
        {
            if (ThinkingBubble.transform.GetChild(j).GetComponent<Image>() != null)
            {
                ThinkingBubble.transform.GetChild(j).GetComponent<Image>().DOFade(0, 0);
                ThinkingBubble.transform.GetChild(j).GetComponent<Image>().DOFade(1, fadeSpeed);
            }
            if (ThinkingBubble.transform.GetChild(j).GetComponent<Text>() != null)
            {
                ThinkingBubble.transform.GetChild(j).GetComponent<Text>().DOFade(0, 0);
                ThinkingBubble.transform.GetChild(j).GetComponent<Text>().DOFade(1, fadeSpeed);
            }
        }
        yield return new WaitForSeconds(time);

        ThinkingBubble.SetActive(false);
    }

    public void Talk(string text, float time)
    {
        StopCoroutine(tempTalkCoroutine);
        tempTalkCoroutine = TalkCoroutine(text, time);
        StartCoroutine(tempTalkCoroutine);
    }
    IEnumerator tempTalkCoroutine;
    IEnumerator TalkCoroutine(string text, float time)
    {
        TalkBubble.SetActive(true);
        TalkBubble.transform.Find("Text").GetComponent<Text>().text = text;

        Vector3 tempLocalSalce = TalkBubble.transform.localScale;
        TalkBubble.transform.localScale = new Vector2(tempLocalSalce.x * .7F, tempLocalSalce.y * .7F);
        TalkBubble.transform.DOScale(tempLocalSalce, fadeSpeed);

        TalkBubble.GetComponent<Image>().DOFade(0, 0);
        TalkBubble.GetComponent<Image>().DOFade(1, fadeSpeed);
        for (int j = 0; j < TalkBubble.transform.childCount; j++)
        {
            if (TalkBubble.transform.GetChild(j).GetComponent<Image>() != null)
            {
                TalkBubble.transform.GetChild(j).GetComponent<Image>().DOFade(0, 0);
                TalkBubble.transform.GetChild(j).GetComponent<Image>().DOFade(1, fadeSpeed);
            }
            if (TalkBubble.transform.GetChild(j).GetComponent<Text>() != null)
            {
                TalkBubble.transform.GetChild(j).GetComponent<Text>().DOFade(0, 0);
                TalkBubble.transform.GetChild(j).GetComponent<Text>().DOFade(1, fadeSpeed);
            }
        }
        yield return new WaitForSeconds(time);

        TalkBubble.SetActive(false);
    }


}
