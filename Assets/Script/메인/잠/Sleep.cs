using DG.Tweening;
using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Multiple BG")]
    public GameObject multipleBg;

    [Header("욕구 상태 아이콘")]
    public Sprite best_icon;
    public Sprite good_icon;
    public Sprite soso_icon;
    public Sprite notBad_icon;
    public Sprite bad_icon;
    [Header("연기 파티클")]
    public GameObject smokeParticle;

    IEnumerator GhostCoroutine;
    bool flag;
    bool isComplete;

    public void SleepOpen()
    {
        flag = true;

        //초기화 
        gage.fillAmount = 0;
        fillGage = 0;
        isComplete = false;

        //고스트 생성
        GhostCoroutine = GhostStart();
        StartCoroutine(GhostCoroutine);

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

        // 활력 셋팅
        int needV = GameManager.instance.userInfoManager.GetUserNeed(NeedKind.활력);
        this.transform.Find("활력").GetChild(1).GetChild(2).GetComponent<Image>().sprite = GetNeedIcon(needV);
        this.transform.Find("활력").GetChild(1).GetChild(0).GetComponent<Text>().text = needV.ToString();
        StartCoroutine(VitalityUp(50));
    }

    public Sprite GetNeedIcon(int needPercent)
    {
        if (needPercent >= 80)
        {
            return best_icon;
        }
        else if (needPercent >= 60)
        {
            return good_icon;
        }
        else if (needPercent >= 40)
        {
            return soso_icon;
        }
        else if (needPercent >= 20)
        {
            return notBad_icon;
        }
        else
        {
            return bad_icon;
        }
    }

    public void SleepClose()
    {
        if (flag)
            return;

        StartCoroutine(SleepCloseCoroutine());
    }

    IEnumerator SleepCloseCoroutine()
    {
        flag = true;

        if (GhostCoroutine != null)
            StopCoroutine(GhostCoroutine);

        for (int i = 0; i < this.transform.Find("유령").childCount; i++)
        {
            if (this.transform.Find("유령").GetChild(i).gameObject.activeSelf)
            {
                this.transform.Find("유령").GetChild(i).GetChild(0).GetComponent<Image>().DOFade(0, .5f);
            }
        }

        character.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "sleep_wakeup", false);
        Color originMultipleColor = multipleBg.GetComponent<Image>().color;
        multipleBg.GetComponent<Image>().DOFade(0, 1.3f);

        yield return new WaitForSeconds(2.4f);

        for (int i = 0; i < this.transform.Find("유령").childCount; i++)
        {
            this.transform.Find("유령").GetChild(i).gameObject.SetActive(false);
        }

        character.position = originPos;
        character.localScale = new Vector3(originSize, originSize, originSize);
        theCam.position = originCamPos;
        character.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, originAni, true);
        multipleBg.GetComponent<Image>().color = originMultipleColor;

        for (int i = 0; i < setOff.Length; i++)
        {
            setOff[i].SetActive(true);
        }
        for (int i = 0; i < setOn.Length; i++)
        {
            setOn[i].SetActive(false);
        }

        flag = false;
    }

    [Header("향초구매")]
    public GameObject candlePurchase;

    public void CandleOpen()
    {
        flag = true;
        candlePurchase.SetActive(true);
        Time.timeScale = 0;
    }

    public void SelectCandle(int index)
    {
        for (int i = 0; i < this.transform.Find("향초").Find("CandleKind").childCount; i++)
        {
            this.transform.Find("향초").Find("CandleKind").GetChild(i).gameObject.SetActive(false);
        }
        this.transform.Find("향초").Find("CandleKind").Find("기본").gameObject.SetActive(true);
        // 구매 
        switch (index)
        {
            case 1: // 라벤더
                int money = 1000;
                MoneyKind moneykind = MoneyKind.Gold;
                if (GameManager.instance.userInfoManager.GetUserMoney(moneykind) < money)
                {
                    return;
                }
                else
                {
                    GameManager.instance.userInfoManager.SetUserMoney(moneykind, GameManager.instance.userInfoManager.GetUserMoney(moneykind) - money);
                    GameManager.instance.userInfoManager.SaveUserMoney();
                    for (int i = 0; i < this.transform.Find("향초").Find("CandleKind").childCount; i++)
                    {
                        this.transform.Find("향초").Find("CandleKind").GetChild(i).gameObject.SetActive(false);
                    }
                    this.transform.Find("향초").Find("CandleKind").Find("라벤더").gameObject.SetActive(true);
                }
                break;
            case 2: // 장미 
                int money2 = 1000;
                MoneyKind moneykind2 = MoneyKind.Crystal;
                if (GameManager.instance.userInfoManager.GetUserMoney(moneykind2) < money2)
                {
                    return;
                }
                else
                {
                    GameManager.instance.userInfoManager.SetUserMoney(moneykind2, GameManager.instance.userInfoManager.GetUserMoney(moneykind2) - money2);
                    GameManager.instance.userInfoManager.SaveUserMoney();
                    for (int i = 0; i < this.transform.Find("향초").Find("CandleKind").childCount; i++)
                    {
                        this.transform.Find("향초").Find("CandleKind").GetChild(i).gameObject.SetActive(false);
                    }
                    this.transform.Find("향초").Find("CandleKind").Find("장미").gameObject.SetActive(true);
                }
                break;
        }

        flag = false;
        candlePurchase.SetActive(false);
        Time.timeScale = 1;

        switch (index)
        {
            case 1:
                flag = true;
                // 활력 셋팅
                StartCoroutine(VitalityUp(GameManager.instance.userInfoManager.GetUserNeed(NeedKind.활력) + 20));

                break;
            case 2:
                isComplete = true;
                flag = true;
                // 활력 셋팅
                StartCoroutine(VitalityUp(GameManager.instance.userInfoManager.GetUserNeed(NeedKind.활력) + 40));
                fillGage = 1;
                gage.DOFillAmount(fillGage, 1F);
                if (GhostCoroutine != null)
                    StopCoroutine(GhostCoroutine);

                for (int i = 0; i < this.transform.Find("유령").childCount; i++)
                {
                    if (this.transform.Find("유령").GetChild(i).gameObject.activeSelf)
                    {
                        this.transform.Find("유령").GetChild(i).GetChild(0).GetComponent<Image>().DOFade(0, .5f);
                    }
                }
                break;
        }
    }

    IEnumerator GhostStart()
    {
        while (true)
        {
            SpawnGhost();
            yield return new WaitForSeconds(1);
        }
    }

    void SpawnGhost()
    {
        for (int i = 0; i < this.transform.Find("유령").childCount; i++)
        {
            if (!this.transform.Find("유령").GetChild(i).gameObject.activeSelf)
            {
                float randomX = Random.RandomRange(-130, 130);
                float randomY = Random.RandomRange(-200, 200);
                this.transform.Find("유령").GetChild(i).localPosition = new Vector2(randomX, randomY);
                this.transform.Find("유령").GetChild(i).GetChild(0).GetComponent<Image>().DOFade(0, 0);
                this.transform.Find("유령").GetChild(i).GetChild(0).GetComponent<Image>().DOFade(1, .5f);
                this.transform.Find("유령").GetChild(i).gameObject.SetActive(true);

                if (i == this.transform.Find("유령").childCount - 1)
                {
                    Invoke("GameOverCheck", 3);
                }
                return;
            }
        }
    }

    void GameOverCheck()
    {
        for (int i = 0; i < this.transform.Find("유령").childCount; i++)
        {
            if (!this.transform.Find("유령").GetChild(i).gameObject.activeSelf)
            {
                return;
            }
        }
        SleepClose();
    }

    public void GhostTouch(int index)
    {
        if (this.transform.Find("유령").GetChild(index).GetChild(0).GetComponent<Image>().color.a < 1)
        {
            return;
        }

        Instantiate(smokeParticle, new Vector3(this.transform.Find("유령").GetChild(index).position.x , this.transform.Find("유령").GetChild(index).position.y + 0.5f, this.transform.Find("유령").GetChild(index).position.z), Quaternion.identity, this.transform.Find("유령"));
        GageUp(0.1F);
        this.transform.Find("유령").GetChild(index).GetChild(0).GetComponent<Image>().DOFade(0, .5f).OnComplete(()=> {
            this.transform.Find("유령").GetChild(index).gameObject.SetActive(false);
        });
    }

    [Header("게이지")]
    public Image gage;
    float fillGage;

    void GageUp(float percent)
    {
        fillGage += percent;
        gage.DOFillAmount(fillGage, 1F).OnComplete(()=> {
            if (fillGage >= 1 && !isComplete)
            {
                isComplete = true;
                flag = true;

                if (GhostCoroutine != null)
                    StopCoroutine(GhostCoroutine);

                // 활력 셋팅
                StartCoroutine(VitalityUp(GameManager.instance.userInfoManager.GetUserNeed(NeedKind.활력) + 20));
                StartCoroutine(GageUpCoroutine());
            }
        });
    }

    IEnumerator GageUpCoroutine()
    {
        this.transform.Find("악몽퇴치").gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        this.transform.Find("악몽퇴치").gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        this.transform.Find("악몽퇴치").gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        this.transform.Find("악몽퇴치").gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        this.transform.Find("악몽퇴치").gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        this.transform.Find("악몽퇴치").gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        this.transform.Find("악몽퇴치").gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        this.transform.Find("악몽퇴치").gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        this.transform.Find("악몽퇴치").gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        this.transform.Find("악몽퇴치").gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        this.transform.Find("악몽퇴치").gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator VitalityUp(int num, System.Action callback = null)
    {
        if (GameManager.instance.userInfoManager.GetUserNeed(NeedKind.활력) >= num)
        {
            flag = false;
            yield break;
        }
        // 활력 셋팅
        float countSpeed = 0.03f;
        WaitForSeconds wait = new WaitForSeconds(countSpeed);
        int needV = GameManager.instance.userInfoManager.GetUserNeed(NeedKind.활력);
        this.transform.Find("활력").GetChild(1).GetChild(2).GetComponent<Image>().sprite = GetNeedIcon(needV);
        this.transform.Find("활력").GetChild(1).GetChild(0).GetComponent<Text>().text = needV.ToString();

        while (needV < num)
        {
            if (needV > 100)
                break;
            this.transform.Find("활력").GetChild(1).GetChild(0).GetComponent<Text>().text = needV.ToString();
            Sprite tempSprite = this.transform.Find("활력").GetChild(1).GetChild(2).GetComponent<Image>().sprite;
            this.transform.Find("활력").GetChild(1).GetChild(2).GetComponent<Image>().sprite = GetNeedIcon(needV);
            if (tempSprite != GetNeedIcon(needV))
            {
                this.transform.Find("활력").GetChild(1).GetChild(2).DOScale(new Vector3(.45f, .45f, .45f), .35f).OnComplete(() => {
                    this.transform.Find("활력").GetChild(1).GetChild(2).DOScale(new Vector3(.3f, .3f, .3f), .35f);
                });
            }
            yield return wait;
            needV++;
        }

        GameManager.instance.userInfoManager.SetUserNeed(GameManager.instance.userInfoManager.GetUserNeed(NeedKind.즐거움),
            GameManager.instance.userInfoManager.GetUserNeed(NeedKind.청결함),
            GameManager.instance.userInfoManager.GetUserNeed(NeedKind.포만감),
            needV);
        this.transform.Find("활력").GetChild(1).GetChild(0).GetComponent<Text>().text = GameManager.instance.userInfoManager.GetUserNeed(NeedKind.활력).ToString();

        GameManager.instance.userInfoManager.SaveUserNeed(GameManager.instance.userInfoManager.currentCharacter);
        flag = false;
        if (callback != null)
        {
            callback();
        }

    }
}