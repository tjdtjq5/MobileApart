using DG.Tweening;
using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RockPaperScissors : MonoBehaviour
{
    public int sucessCount = 10;

    public GameObject[] miniGameImg;
    public GameObject[] rockPaperScissorsImg;

    public GameObject rock;                    Vector3 originRock;
    public GameObject paper;                   Vector3 originPaper;
    public GameObject scissor;                 Vector3 originScissor;

    public Text sucessCountText;
    int scuessCount;
    public Text test;

    public GameObject character;
    SkeletonAnimation skeletonAnimation;

    int count;                  // 가위바위보 게임 시작시 랜덤 설정 카운트
    int currentCount;           // count 와 동일한 수가 되면 10연승 시작

    private void Start()
    {
        skeletonAnimation = character.GetComponent<SkeletonAnimation>();
    }

    void CountReSet()
    {
        currentCount = 0;
        scuessCount = 0;
        count = Random.RandomRange(0, 13);
        test.text = count.ToString();
    }

    public void GameStart()
    {
        CountReSet();

        StartCoroutine(GameStartCoroutine());

        skeletonAnimation.AnimationState.SetAnimation(0, "rsp_ready", true);

        originRock = rock.transform.position;
        originPaper = paper.transform.position;
        originScissor = scissor.transform.position;

        scuessCount = 0;
        sucessCountText.text = "X" + scuessCount;
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
            rockPaperScissorsImg[i].gameObject.SetActive(true);
            if (rockPaperScissorsImg[i].GetComponent<Image>() != null)
            {
                rockPaperScissorsImg[i].GetComponent<Image>().DOFade(1, fadeSpeed);
            }
        }
    }

    public void GameExit()
    {
        StartCoroutine(GameExitCoroutine());
        skeletonAnimation.AnimationState.SetAnimation(0, "idle_01", true);
    }

    IEnumerator GameExitCoroutine()
    {
        for (int i = 0; i < rockPaperScissorsImg.Length; i++)
        {
            if (rockPaperScissorsImg[i].GetComponent<Image>() != null)
            {
                rockPaperScissorsImg[i].GetComponent<Image>().DOFade(0, fadeSpeed);
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


    bool flag = false;
    public void Rock()
    {
        if (flag)
            return;
        flag = true;

        rock.transform.DOMoveX(0, fadeSpeed);
        paper.gameObject.SetActive(false);
        scissor.gameObject.SetActive(false);
        ReSult("rsp-r");
    }

    public void Paper()
    {
        if (flag)
            return;
        flag = true;

        paper.transform.DOMoveX(0, fadeSpeed);
        rock.gameObject.SetActive(false);
        scissor.gameObject.SetActive(false);
        ReSult("rsp-p");
    }

    public void Scissor()
    {
        if (flag)
            return;
        flag = true;

        scissor.transform.DOMoveX(0, fadeSpeed);
        paper.gameObject.SetActive(false);
        rock.gameObject.SetActive(false);
        ReSult("rsp-s");
    }

    public void ReSult(string playerWhatRSP)
    {
        //count 계산 
        if (count == currentCount) // 카운트가 같으면 무조건 이기게 하기 
        {
            if (scuessCount > sucessCount)  // 10번 이기면 빠져나오기
            {
                CountReSet(); // 10번 이긴후에 다시 리셋
            }

            int tempRandom = Random.RandomRange(0, 2);
            switch (tempRandom)
            {
                case 0: // 0나오면 이김
                    scuessCount++; // 10번 이겼는지 확인하기 위해 카운트를 셈 
                    StartCoroutine(SuccecsPlayCoroutine(playerWhatRSP));
                    break;
                case 1: // 1나오면 비김 
                    StartCoroutine(DrawPlayCoroutine(playerWhatRSP));
                    break;
                default:
                    StartCoroutine(SuccecsPlayCoroutine(playerWhatRSP));
                    break;
            }
        }
        else // 10연승 카운트가 아닐 경우 
        {
            int tempRandom = Random.RandomRange(0, 3);
            switch (tempRandom)
            {
                case 0: // 0나오면 이김
                    StartCoroutine(SuccecsPlayCoroutine(playerWhatRSP));
                    break;
                case 1: // 1나오면 비김 
                    StartCoroutine(DrawPlayCoroutine(playerWhatRSP));
                    break;
                case 2: // 2나오면 짐 
                    StartCoroutine(FailPlayCoroutine(playerWhatRSP));
                    currentCount++;
                    break;
                default:
                    StartCoroutine(DrawPlayCoroutine(playerWhatRSP));
                    break;
            }
        }
    }

    IEnumerator SuccecsPlayCoroutine(string playerWhatRSP)
    {
        yield return new WaitForSeconds(fadeSpeed);
        switch (playerWhatRSP)
        {
            case "rsp-r":
                skeletonAnimation.AnimationState.SetAnimation(0, "rsp-s", false);
                break;
            case "rsp-s":
                skeletonAnimation.AnimationState.SetAnimation(0, "rsp-p", false);
                break;
            case "rsp-p":
                skeletonAnimation.AnimationState.SetAnimation(0, "rsp-r", false);
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(0.5f);
        skeletonAnimation.AnimationState.SetAnimation(0, "game_lose", false);
        scuessCount++;
        sucessCountText.text = "X" + scuessCount;
        yield return new WaitForSeconds(1f);
        StartCoroutine(RePlay());
    }

    IEnumerator DrawPlayCoroutine(string playerWhatRSP)
    {
        yield return new WaitForSeconds(fadeSpeed);
        switch (playerWhatRSP)
        {
            case "rsp-r":
                skeletonAnimation.AnimationState.SetAnimation(0, "rsp-r", false);
                break;
            case "rsp-s":
                skeletonAnimation.AnimationState.SetAnimation(0, "rsp-s", false);
                break;
            case "rsp-p":
                skeletonAnimation.AnimationState.SetAnimation(0, "rsp-p", false);
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(RePlay());
    }
    IEnumerator FailPlayCoroutine(string playerWhatRSP)
    {
        yield return new WaitForSeconds(fadeSpeed);
        switch (playerWhatRSP)
        {
            case "rsp-r":
                skeletonAnimation.AnimationState.SetAnimation(0, "rsp-p", false);
                break;
            case "rsp-s":
                skeletonAnimation.AnimationState.SetAnimation(0, "rsp-r", false);
                break;
            case "rsp-p":
                skeletonAnimation.AnimationState.SetAnimation(0, "rsp-s", false);
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(0.5f);
        skeletonAnimation.AnimationState.SetAnimation(0, "game_victory", false);
        scuessCount = 0;
        sucessCountText.text = "X" + scuessCount;
        yield return new WaitForSeconds(1f);
        StartCoroutine(RePlay());
    }
    IEnumerator RePlay()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, "rsp_ready", true);

        yield return new WaitForSeconds(0.1f);

        rock.SetActive(true);
        rock.transform.position = originRock;
        paper.SetActive(true);
        paper.transform.position = originPaper;
        scissor.SetActive(true);
        scissor.transform.position = originScissor;

        flag = false;
    }
}
