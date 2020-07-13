using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class RockPaperScissors : MonoBehaviour
{
    public static RockPaperScissors instance;

    [Header("캐릭터")] public GameObject character;
    [Header("말풍선")] public Talk talk;
    [Header("카운트")]
    public Text countText;
    public Slider countSlider;
    [Header("묵찌빠")]
    public GameObject RockObj;
    public GameObject PaperObj;
    public GameObject ScissorObj;
    [Header("이긴횟수 텍스트")]
    public Text winCountText;
    int winCount;

    [Header("묵찌빠토크말풍선")]
    public GameObject talkBubbleObj;
    public GameObject talkBubble;

    private void Start()
    {
        instance = this;
    }

    [Header("off해 놓을 것들")]
    public GameObject[] setOff;
    [Header("on해 놓을 것들")]
    public GameObject[] setOn;

    string originAniState;
    public void RockPaperScissorsOpen()
    {
        originAniState = character.GetComponent<SkeletonAnimation>().AnimationName;

        for (int i = 0; i < setOff.Length; i++)
        {
            setOff[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < setOn.Length; i++)
        {
            setOn[i].gameObject.SetActive(true);
        }

        winCount = 0;

        Init();

        Play();
    }

    public void RockPaperScissorsClose()
    {
        character.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, originAniState, true);

        for (int i = 0; i < setOff.Length; i++)
        {
            setOff[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < setOn.Length; i++)
        {
            setOn[i].gameObject.SetActive(false);
        }

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
    }

    void Init()
    {
        whatPlayer = "";
        whatCharacter = "";
        script = "";

        countText.text = "";
        countSlider.value = 0;

        RockObj.transform.Find("circle").GetComponent<Image>().color = new Color(25 / 255f, 140 / 255f, 1, 1);
        PaperObj.transform.Find("circle").GetComponent<Image>().color = new Color(25 / 255f, 140 / 255f, 1, 1);
        ScissorObj.transform.Find("circle").GetComponent<Image>().color = new Color(25 / 255f, 140 / 255f, 1, 1);
    }

    string whatPlayer;

    public void Rock()
    {
        whatPlayer = "Rock";
        RockObj.transform.Find("circle").GetComponent<Image>().color = new Color(0, 213 / 255f, 165 / 255f, 1);
        PaperObj.transform.Find("circle").GetComponent<Image>().color = new Color(25 / 255f, 140 / 255f, 1, 1);
        ScissorObj.transform.Find("circle").GetComponent<Image>().color = new Color(25 / 255f, 140 / 255f, 1, 1);
    }

    public void Paper()
    {
        whatPlayer = "Paper";
        RockObj.transform.Find("circle").GetComponent<Image>().color = new Color(25 / 255f, 140 / 255f, 1, 1);
        PaperObj.transform.Find("circle").GetComponent<Image>().color = new Color(0, 213 / 255f, 165 / 255f, 1);
        ScissorObj.transform.Find("circle").GetComponent<Image>().color = new Color(25 / 255f, 140 / 255f, 1, 1);
    }

    public void Scissor()
    {
        whatPlayer = "Scissor";
        RockObj.transform.Find("circle").GetComponent<Image>().color = new Color(25 / 255f, 140 / 255f, 1, 1);
        PaperObj.transform.Find("circle").GetComponent<Image>().color = new Color(25 / 255f, 140 / 255f, 1, 1);
        ScissorObj.transform.Find("circle").GetComponent<Image>().color = new Color(0, 213 / 255f, 165 / 255f, 1);
    }

    IEnumerator currentCoroutine;
    void Play()
    {
        currentCoroutine = CountDown(Result);
        StartCoroutine(currentCoroutine);
        WhatCharacter();
    }

    IEnumerator CountDown(System.Action Result)
    {
        talkBubbleObj.SetActive(true);
        countSlider.value = 0;
        countSlider.DOValue(1, 3).SetEase(Ease.Linear);

        countText.text = "3";
        yield return new WaitForSeconds(1);
        countText.text = "2";
        yield return new WaitForSeconds(1);
        countText.text = "1";
        yield return new WaitForSeconds(1);
        countText.text = "0";

        talkBubbleObj.SetActive(false);
        talkBubble.SetActive(false);

        Result();
    }

    string whatCharacter;
    string badSelect;
    void WhatCharacter()
    {
        character.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "rsp_ready", true);
        badSelect = "";
        int rand = Random.RandomRange(0, 6);
        switch (rand)
        {
            case 0:
                CharacterTalk.instance.Talk("가위없는 가위바위보!", 3);
                rand = Random.RandomRange(0, 2);
                badSelect = "Scissor";
                switch (rand)
                {
                    case 0:
                        whatCharacter = "Rock";
                        break;
                    case 1:
                        whatCharacter = "Paper";
                        break;
                }
                break;
            case 1:
                CharacterTalk.instance.Talk("바위없는 가위바위보!", 3);
                rand = Random.RandomRange(0, 2);
                badSelect = "Rock";
                switch (rand)
                {
                    case 0:
                        whatCharacter = "Scissor";
                        break;
                    case 1:
                        whatCharacter = "Paper";
                        break;
                }
                break;
            case 2:
                CharacterTalk.instance.Talk("보없는 가위바위보!", 3);
                rand = Random.RandomRange(0, 2);
                badSelect = "Paper";
                switch (rand)
                {
                    case 0:
                        whatCharacter = "Scissor";
                        break;
                    case 1:
                        whatCharacter = "Rock";
                        break;
                }
                break;
            case 3:
                CharacterTalk.instance.ThinkingTalk("이번에는 가위를 안 내야지!", 3);
                rand = Random.RandomRange(0, 10);
                if (rand < 1)
                    whatCharacter = "Scissor";
                else if (rand < 5)
                    whatCharacter = "Rock";
                else
                    whatCharacter = "Paper";
                break;
            case 4:
                CharacterTalk.instance.ThinkingTalk("이번에는 바위를 안 내야지!", 3);
                rand = Random.RandomRange(0, 10);
                if (rand < 1)
                    whatCharacter = "Rock";
                else if (rand < 5)
                    whatCharacter = "Scissor";
                else
                    whatCharacter = "Paper";
                break;
            case 5:
                CharacterTalk.instance.ThinkingTalk("이번에는 보를 안 내야지!", 3);
                rand = Random.RandomRange(0, 10);
                if (rand < 1)
                    whatCharacter = "Paper";
                else if (rand < 5)
                    whatCharacter = "Scissor";
                else
                    whatCharacter = "Rock";
                break;
            default:
                break;
        }
    }

    void Result()
    {
        switch (whatPlayer)
        {
            case "Rock":
                switch (whatCharacter)
                {
                    case "Rock":
                        character.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "rsp-r", false);
                        currentCoroutine = Draw();
                        StartCoroutine(currentCoroutine);
                        return;
                    case "Paper":
                        character.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "rsp-p", false);
                        currentCoroutine = Lose();
                        StartCoroutine(currentCoroutine);
                        return;
                    case "Scissor":
                        character.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "rsp-s", false);
                        currentCoroutine = Win();
                        StartCoroutine(currentCoroutine);
                        return;
                }
                return;
            case "Paper":
                switch (whatCharacter)
                {
                    case "Rock":
                        character.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "rsp-r", false);
                        currentCoroutine = Win();
                        StartCoroutine(currentCoroutine);
                        return;
                    case "Paper":
                        character.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "rsp-p", false);
                        currentCoroutine = Draw();
                        StartCoroutine(currentCoroutine);
                        return;
                    case "Scissor":
                        character.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "rsp-s", false);
                        currentCoroutine = Lose();
                        StartCoroutine(currentCoroutine);
                        return;
                }
                return;
            case "Scissor":
                switch (whatCharacter)
                {
                    case "Rock":
                        character.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "rsp-r", false);
                        currentCoroutine = Lose();
                        StartCoroutine(currentCoroutine);
                        return;
                    case "Paper":
                        character.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "rsp-p", false);
                        currentCoroutine = Win();
                        StartCoroutine(currentCoroutine);
                        return;
                    case "Scissor":
                        character.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "rsp-s", false);
                        currentCoroutine = Draw();
                        StartCoroutine(currentCoroutine);
                        return;
                }
                return;
        }

        currentCoroutine = Lose();
        StartCoroutine(currentCoroutine);
    }

    IEnumerator Win()
    {
        yield return new WaitForSeconds(.8f);
        winCount++;
        winCountText.text = "이긴횟수 " + winCount;

        character.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "game_lose", false);
        if (script != "")
        {
            CharacterTalk.instance.Talk(script, 1);
        }
        yield return new WaitForSeconds(1f);

        Init();

        Play();
    }

    IEnumerator Draw()
    {
        yield return new WaitForSeconds(.8f);

        Init();

        Play();
    }

    IEnumerator Lose()
    {
        yield return new WaitForSeconds(.8f);
        winCount = 0;
        winCountText.text = "이긴횟수 " + winCount;

        character.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "game_victory", false);
        CharacterTalk.instance.Talk("내가 이겼지!", 1);
        yield return new WaitForSeconds(1f);

        Init();

        Play();
    }

    string script;
    float fadeSpeed = 0.3f;
    public void PlayerTalkBtn()
    {
        string script01 = "script01";
        string script02 = "script02";
        string script03 = "script03";

        int rand = Random.RandomRange(0, 3);
        switch (rand)
        {
            case 0:
                script = script01;
                break;
            case 1:
                script = script02;
                break;
            case 2:
                script = script03;
                break;
        }

        talkBubble.SetActive(true);
        talkBubble.transform.Find("Text").GetComponent<Text>().text = script;

        Vector3 tempLocalSalce = talkBubble.transform.localScale;
        talkBubble.transform.localScale = new Vector2(tempLocalSalce.x * .7F, tempLocalSalce.y * .7F);
        talkBubble.transform.DOScale(tempLocalSalce, fadeSpeed);

        talkBubble.GetComponent<Image>().DOFade(0, 0);
        talkBubble.GetComponent<Image>().DOFade(1, fadeSpeed);
        for (int j = 0; j < talkBubble.transform.childCount; j++)
        {
            if (talkBubble.transform.GetChild(j).GetComponent<Image>() != null)
            {
                talkBubble.transform.GetChild(j).GetComponent<Image>().DOFade(0, 0);
                talkBubble.transform.GetChild(j).GetComponent<Image>().DOFade(1, fadeSpeed);
            }
            if (talkBubble.transform.GetChild(j).GetComponent<Text>() != null)
            {
                talkBubble.transform.GetChild(j).GetComponent<Text>().DOFade(0, 0);
                talkBubble.transform.GetChild(j).GetComponent<Text>().DOFade(1, fadeSpeed);
            }
        }
    }
}
