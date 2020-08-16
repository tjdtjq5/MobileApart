using DG.Tweening;
using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Bath : MonoBehaviour
{
    [Header("캐릭터 카메라")]
    public Transform characterCam;   Vector3 originCamPos;  public Vector3 camMovePos;
    public Acceleration acceleration;
    [Header("케릭터")]
    public Transform character;      Vector3 originCharacterPos; public Vector3 characterMovePos;
    Vector3 originSize;               public Vector3 moveSize;
    string currentAni;

    [Header("파티클")]
    public GameObject touchParticleWater;
    public GameObject clickParticleWater;

    [Header("목욕씬때 꺼놔야 하는 것들")]
    public GameObject[] setOff;
    [Header("켜지는 것들")]
    public GameObject[] setOn;

    [Header("청결")]
    public Transform cleanliness;

    [Header("욕구 상태 아이콘")]
    public Sprite best_icon;
    public Sprite good_icon;
    public Sprite soso_icon;
    public Sprite notBad_icon;
    public Sprite bad_icon;

    [Header("게이지")]
    public Transform gage;
    float gagePercent;

    public void BathOpen()
    {
        originCamPos = characterCam.position;
        characterCam.position = camMovePos;

        originCharacterPos = character.position;
        character.position = characterMovePos;

        originSize = character.transform.localScale;
        character.transform.localScale = moveSize;

        character.GetComponent<CharacterMotion>().SetFlag(true);

        currentAni = character.GetComponent<SkeletonAnimation>().AnimationName;
        character.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0 ,"bath", true);

        character.GetComponent<TransformSkin>().acc("");
        character.GetComponent<TransformSkin>().top("");
        character.GetComponent<TransformSkin>().set("");
        character.GetComponent<TransformSkin>().outt("");

        for (int i = 0; i < setOff.Length; i++)
        {
            setOff[i].SetActive(false);
        }

        for (int i = 0; i < setOn.Length; i++)
        {
            setOn[i].SetActive(true);
        }

        AniCoroutine = Bath3Coroutine(1.5f);

        touchParticleWater.SetActive(true);
        clickParticleWater.SetActive(true);

        StartCoroutine(CleanlinessUp(50));

        gage.GetChild(0).GetComponent<Image>().fillAmount = 0;
        gagePercent = 0;
    }

    public void BathClose()
    {
        if (cleanlinessUpFlag || gageUpFlag)
        {
            return;
        }

        characterCam.position = originCamPos;
        character.position = originCharacterPos;

        character.transform.localScale = originSize;

        character.GetComponent<CharacterMotion>().SetFlag(false);

        character.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, currentAni, true);

        character.GetComponent<TransformSkin>().UserEqipInfoSetting();

        for (int i = 0; i < setOff.Length; i++)
        {
            setOff[i].SetActive(true);
        }

        for (int i = 0; i < setOn.Length; i++)
        {
            setOn[i].SetActive(false);
        }

        StopCoroutine(AniCoroutine);

        touchParticleWater.SetActive(false);
        clickParticleWater.SetActive(false);
    }

    bool touchFlag = false;
    //물뿌리기 
    public void TouchDownWater()
    {
        StopCoroutine(AniCoroutine);

        touchFlag = true;
        touchParticleWater.GetComponent<ParticleSystem>().Play();
        aniName = "";
    }

    public void TouchUpWater()
    {
        touchFlag = false;
        touchParticleWater.GetComponent<ParticleSystem>().Stop();
        touchParticleWater.transform.position = new Vector3(2000, 2000, touchParticleWater.transform.position.z);
        character.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "bath", true);
    }
    string aniName;
    private void Update()
    {
        if (touchFlag)
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 touchPos = characterCam.GetComponent<Camera>().ScreenToWorldPoint(mousePos);
            touchParticleWater.transform.position = new Vector3(touchPos.x, touchPos.y, touchParticleWater.transform.position.z);

            if (Mathf.Abs(character.position.x + 4  - touchPos.x) < 1.3f)
            {
                if (aniName != "bath2")
                {
                    character.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "bath2", true);
                    aniName = "bath2";
                }
            }
            else
            {
                if (aniName != "bath")
                {
                   character.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "bath", true);
                    aniName = "bath";
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 touchPos = characterCam.GetComponent<Camera>().ScreenToWorldPoint(mousePos);

            RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero, 10);
            if (hit && hit.transform.tag == "꽃병")
            {
               
            }
        }
    }

    IEnumerator AniCoroutine;
    // 물 터지기 파티클
    public void ClickWaterParticle()
    {
        if (character.GetComponent<SkeletonAnimation>().AnimationName == "bath3")
        {
            return;
        }

        AniCoroutine = Bath3Coroutine(1.5f);
        StartCoroutine(AniCoroutine);
    }

    IEnumerator Bath3Coroutine(float time)
    {
        float tempTime = 0.3f;
        character.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "bath3", false);
        yield return new WaitForSeconds(tempTime);
        clickParticleWater.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(time - tempTime);
        character.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "bath", true);
    }

    bool cleanlinessUpFlag = false;
    IEnumerator CleanlinessUp(float num, System.Action callback = null)
    {
   
        if (GameManager.instance.userInfoManager.GetUserNeed(NeedKind.청결함) >= num)
        {
            yield break;
        }

        cleanlinessUpFlag = true;

        // 청결 셋팅
        float countSpeed = 0.03f;
        WaitForSeconds wait = new WaitForSeconds(countSpeed);
        int needC = GameManager.instance.userInfoManager.GetUserNeed(NeedKind.청결함);
        cleanliness.GetChild(1).GetChild(2).GetComponent<Image>().sprite = GetNeedIcon(needC);
        cleanliness.GetChild(1).GetChild(0).GetComponent<Text>().text = needC.ToString();

        while (needC < num)
        {
            if (needC > 100)
                break;
            cleanliness.GetChild(1).GetChild(0).GetComponent<Text>().text = needC.ToString();
            Sprite tempSprite = cleanliness.GetChild(1).GetChild(2).GetComponent<Image>().sprite;
            cleanliness.GetChild(1).GetChild(2).GetComponent<Image>().sprite = GetNeedIcon(needC);
            if (tempSprite != GetNeedIcon(needC))
            {
                cleanliness.GetChild(1).GetChild(2).DOScale(new Vector3(.45f, .45f, .45f), .35f).OnComplete(() => {
                    cleanliness.GetChild(1).GetChild(2).DOScale(new Vector3(.3f, .3f, .3f), .35f);
                });
            }
            yield return wait;
            needC++;
        }

        GameManager.instance.userInfoManager.SetUserNeed(GameManager.instance.userInfoManager.GetUserNeed(NeedKind.즐거움),
                                                     GameManager.instance.userInfoManager.GetUserNeed(NeedKind.포만감),
                                                     needC,
                                                       GameManager.instance.userInfoManager.GetUserNeed(NeedKind.활력));
        cleanliness.GetChild(1).GetChild(0).GetComponent<Text>().text = GameManager.instance.userInfoManager.GetUserNeed(NeedKind.청결함).ToString();

        GameManager.instance.userInfoManager.SaveUserNeed(GameManager.instance.userInfoManager.currentCharacter);

        if (callback != null)
        {
            callback();
        }

        cleanlinessUpFlag = false;
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

    bool gageUpFlag = false;
    IEnumerator GageUp(int num)
    {
        yield return null;

        gageUpFlag = true;
        float countSpeed = 0.03f;
        WaitForSeconds wait = new WaitForSeconds(countSpeed);

        while (gagePercent > num)
        {
            gagePercent += countSpeed;
            gage.GetChild(0).GetComponent<Image>().fillAmount = gagePercent / 100f;
            yield return wait;

            if (gagePercent >= 100)
            {
                gagePercent = 100;
                break;
            }
        }
        gageUpFlag = false;
    }
}
