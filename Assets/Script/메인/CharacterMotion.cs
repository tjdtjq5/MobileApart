using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterMotion : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    public Camera characterCam;

    public bool stopFlag = false;
    bool flag = false;
    IEnumerator aniCoroutine;
    string currentAniName = "";

    SkeletonAnimation sa;

    void Start()
    {
        sa = this.GetComponent<SkeletonAnimation>();
    }
 
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
                    aniCoroutine = AniCoroutine("touch_head", 1f);
                    StartCoroutine(aniCoroutine);
                    break;
                case "가슴":
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
                    break;
            }
        }
    }
}
