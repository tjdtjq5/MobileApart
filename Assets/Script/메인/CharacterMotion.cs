using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterMotion : MonoBehaviour, IPointerClickHandler, IDragHandler,IBeginDragHandler, IEndDragHandler
{
    public Camera characterCam;
    public Camera hitCam;

    [Header("파티클")]
    public GameObject clickParticle;
    public GameObject trailParticle;

    [HideInInspector]
    public bool stopFlag = false;
    bool flag = false;
    IEnumerator aniCoroutine;
    string currentAniName = "";

    SkeletonAnimation sa;

    void Start()
    {
        sa = this.GetComponent<SkeletonAnimation>();
    }
 
    // stop = true, play = false
    public void SetFlag(bool stopFlag)
    {
         this.stopFlag = stopFlag;

        flag = false;
        if (currentAniName != "")
            sa.AnimationState.SetAnimation(0, currentAniName, true);
        if (aniCoroutine != null)
            StopCoroutine(aniCoroutine);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (stopFlag || flag)
            return;

        Vector2 clickPos = characterCam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(clickPos, Vector2.zero);
        if (hit)
        {
            switch (hit.transform.name)
            {
                case "머리":
                    clickPos = hitCam.ScreenToWorldPoint(Input.mousePosition);
                    clickParticle.transform.position = clickPos;
                    clickParticle.GetComponent<ParticleSystem>().Play();
                    aniCoroutine = AniCoroutine("touch_head", 1f);
                    StartCoroutine(aniCoroutine);
                    break;
                case "가슴":
                    clickPos = hitCam.ScreenToWorldPoint(Input.mousePosition);
                    clickParticle.transform.position = clickPos;
                    clickParticle.GetComponent<ParticleSystem>().Play();
                    aniCoroutine = AniCoroutine("touch_br", 1f);
                    StartCoroutine(aniCoroutine);
                    break;
            }
        }
    }


    IEnumerator AniCoroutine(string aniName, float time)
    {
        flag = true;
        currentAniName = sa.AnimationName;
        sa.AnimationState.SetAnimation(0, aniName, false);
        yield return new WaitForSeconds(time);
        sa.AnimationState.SetAnimation(0, currentAniName, true);
        yield return new WaitForSeconds(0.5f);
        flag = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
       
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (stopFlag || flag)
            return;

        Vector2 clickPos = characterCam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(clickPos, Vector2.zero);
        if (hit)
        {
            switch (hit.transform.name)
            {
                case "머리":
                    aniCoroutine = AniCoroutine("stroke_hair1", 1f);
                    StartCoroutine(aniCoroutine);

                    dragFlag = true;
                    trailParticle.GetComponent<ParticleSystem>().Play();
                    break;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragFlag = false;
        trailParticle.GetComponent<ParticleSystem>().Stop();
    }

    bool dragFlag;
    void Update()
    {
        if (dragFlag)
        {
            Vector2 clickPos = hitCam.ScreenToWorldPoint(Input.mousePosition);
            trailParticle.transform.position = clickPos;
        }
    }
}
