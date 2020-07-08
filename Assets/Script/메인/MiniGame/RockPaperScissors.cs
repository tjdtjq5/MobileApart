using DG.Tweening;
using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RockPaperScissors : MonoBehaviour
{
    public int sucessCount;
    public Text sucessText;

    public GameObject[] miniGameImg;
    public GameObject[] rockPaperScissorsImg;

    public Text counterText;
    public Image timeFillImg;

    public GameObject SetImg; 
    public GameObject rock;                   
    public GameObject paper;              
    public GameObject scissor;            

    public GameObject character;
    SkeletonAnimation skeletonAnimation;

    IEnumerator controllCoroutine;

    private void Start()
    {
        skeletonAnimation = character.GetComponent<SkeletonAnimation>();
    }


    public void GameStart()
    {
        controllCoroutine = GameStartCoroutine();
        StartCoroutine(controllCoroutine);
        sucessCount = 0;
        sucessText.text = sucessCount.ToString();
    }

    float fadeSpeed = 0.3f;
    IEnumerator GameStartCoroutine()
    {
        for (int i = 0; i < miniGameImg.Length; i++)
        {
            if (miniGameImg[i].GetComponent<Image>() != null)
            {
                miniGameImg[i].GetComponent<Image>().DOFade(0, fadeSpeed);
            }
        }
        yield return new WaitForSeconds(fadeSpeed);
        for (int i = 0; i < miniGameImg.Length; i++)
        {
            miniGameImg[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < rockPaperScissorsImg.Length; i++)
        {
            rockPaperScissorsImg[i].SetActive(true);

            if (rockPaperScissorsImg[i].GetComponent<Image>() != null)
            {
                rockPaperScissorsImg[i].GetComponent<Image>().DOFade(1, fadeSpeed);
            }
            if (rockPaperScissorsImg[i].GetComponent<Text>() != null)
            {
                rockPaperScissorsImg[i].GetComponent<Text>().DOFade(1, fadeSpeed);
            }
        }
        yield return new WaitForSeconds(fadeSpeed);

        SetImg.SetActive(true);

        Play();
    }

    public void GameExit()
    {
        StartCoroutine(GameExitCoroutine());
        StopCoroutine(controllCoroutine);
        skeletonAnimation.AnimationState.SetAnimation(0, "idle_01", true);
    }

    IEnumerator GameExitCoroutine()
    {
        SetImg.SetActive(false);

        for (int i = 0; i < rockPaperScissorsImg.Length; i++)
        {
            if (rockPaperScissorsImg[i].GetComponent<Image>() != null)
            {
                rockPaperScissorsImg[i].GetComponent<Image>().DOFade(0, fadeSpeed);
            }
            if (rockPaperScissorsImg[i].GetComponent<Text>() != null)
            {
                rockPaperScissorsImg[i].GetComponent<Text>().DOFade(0, fadeSpeed);
            }
        }
        yield return new WaitForSeconds(fadeSpeed);
        for (int i = 0; i < rockPaperScissorsImg.Length; i++)
        {
            rockPaperScissorsImg[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < miniGameImg.Length; i++)
        {
            miniGameImg[i].gameObject.SetActive(true);
            if (miniGameImg[i].GetComponent<Image>() != null)
            {
                miniGameImg[i].GetComponent<Image>().DOFade(1, fadeSpeed);
            }
        }
    }

    public void Play()
    {
        rock.GetComponent<Image>().color = Color.gray;
        paper.GetComponent<Image>().color = Color.gray;
        scissor.GetComponent<Image>().color = Color.gray;

        skeletonAnimation.AnimationState.SetAnimation(0, "rsp_ready", true);

        controllCoroutine = CounterDownCoroutine(Play02());
        StartCoroutine(controllCoroutine);
    }

    IEnumerator CounterDownCoroutine(IEnumerator coroutine)
    {
        counterText.gameObject.SetActive(true);
        counterText.text = "3";
        yield return new WaitForSeconds(1);
        counterText.text = "2";
        yield return new WaitForSeconds(1);
        counterText.text = "1";
        yield return new WaitForSeconds(1);
        counterText.gameObject.SetActive(false);

        controllCoroutine = coroutine;
        StartCoroutine(controllCoroutine);
    }

    IEnumerator Play02()
    {
        rock.GetComponent<Image>().color = Color.white;
        paper.GetComponent<Image>().color = Color.white;
        scissor.GetComponent<Image>().color = Color.white;

        //시간초 
        timeFillImg.fillAmount = 0;
        timeFillImg.DOFillAmount(1, 1.5f);
        yield return new WaitForSeconds(1.5f);
        Result();
    }

    public void Rock()
    {
        if (rock.GetComponent<Image>().color == Color.white)
        {
            rock.GetComponent<Image>().color = Color.red;
            paper.GetComponent<Image>().color = Color.white;
            scissor.GetComponent<Image>().color = Color.white;
        }
    }

    public void Paper()
    {
        if (paper.GetComponent<Image>().color == Color.white)
        {
            rock.GetComponent<Image>().color = Color.white;
            paper.GetComponent<Image>().color = Color.red;
            scissor.GetComponent<Image>().color = Color.white;
        }
    }

    public void Scissor()
    {
        if (scissor.GetComponent<Image>().color == Color.white)
        {
            rock.GetComponent<Image>().color = Color.white;
            paper.GetComponent<Image>().color = Color.white;
            scissor.GetComponent<Image>().color = Color.red;
        }
    }

    public void Result()
    {
        int randResult = Random.RandomRange(0, 3);
        string aniName = "";

        //플레이어가 묵을 냈을 경우
        if (rock.GetComponent<Image>().color == Color.red)
        {
            switch (randResult)
            {
                case 0:
                    aniName = "rsp-r";
                    controllCoroutine = Draw();
                    StartCoroutine(controllCoroutine);
                    break;
                case 1:
                    aniName = "rsp-s";
                    controllCoroutine = Win();
                    StartCoroutine(controllCoroutine);
                    break;
                case 2:
                    aniName = "rsp-p";
                    controllCoroutine = Lose();
                    StartCoroutine(controllCoroutine);
                    break;
            }
        }
        //플레이어가 찌을 냈을 경우
        if (scissor.GetComponent<Image>().color == Color.red)
        {
            switch (randResult)
            {
                case 0:
                    aniName = "rsp-r";
                    controllCoroutine = Lose();
                    StartCoroutine(controllCoroutine);
                    break;
                case 1:
                    aniName = "rsp-s";
                    controllCoroutine = Draw();
                    StartCoroutine(controllCoroutine);
                    break;
                case 2:
                    aniName = "rsp-p";
                    controllCoroutine = Win();
                    StartCoroutine(controllCoroutine);
                    break;
            }
        }
        //플레이어가 빠을 냈을 경우
        if (paper.GetComponent<Image>().color == Color.red)
        {
            switch (randResult)
            {
                case 0:
                    aniName = "rsp-r";
                    controllCoroutine = Win();
                    StartCoroutine(controllCoroutine);
                    break;
                case 1:
                    aniName = "rsp-s";
                    controllCoroutine = Lose();
                    StartCoroutine(controllCoroutine);
                    break;
                case 2:
                    aniName = "rsp-p";
                    controllCoroutine = Draw();
                    StartCoroutine(controllCoroutine);
                    break;
            }
        }
        if (aniName == "")
        {
            controllCoroutine = Lose();
            StartCoroutine(controllCoroutine);
        }
        else
        {
            skeletonAnimation.AnimationState.SetAnimation(0, aniName, false);

        }
    }

    IEnumerator Win()
    {
        Debug.Log("이김");
        yield return new WaitForSeconds(1f);
        skeletonAnimation.AnimationState.SetAnimation(0, "game_lose", false);
        sucessCount++;
        sucessText.text = sucessCount.ToString();
        yield return new WaitForSeconds(1f);
        Play();
    }

    IEnumerator Draw()
    {
        Debug.Log("비김");
        yield return new WaitForSeconds(1f);
        Play();
    }

    IEnumerator Lose()
    {
        Debug.Log("짐");
        yield return new WaitForSeconds(1f);
        sucessCount = 0;
        sucessText.text = sucessCount.ToString();
        skeletonAnimation.AnimationState.SetAnimation(0, "game_victory", false);
        yield return new WaitForSeconds(1f);
        Play();
    }
}
