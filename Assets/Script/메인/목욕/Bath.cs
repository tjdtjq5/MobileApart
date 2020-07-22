using Spine.Unity;
using System.Collections;
using UnityEngine;

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
    }

    public void BathClose()
    {
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
}
