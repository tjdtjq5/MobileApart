using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{

    bool flag = false;
    float y = 0;
    Sequence MySequence;

    private void Start()
    {
        MySequence = DOTween.Sequence();
        Move();
    }

    private void OnEnable()
    {
        y = this.transform.localPosition.y;
    }


    void Move()
    {
        if (flag)
        {
            MySequence.Append(this.transform.DOLocalMoveY(y - 35f, .5f).SetEase(Ease.InOutFlash).OnComplete(() =>
            {
                flag = false;
                Move();
            }));
           
        }
        else
        {
            MySequence.Append(this.transform.DOLocalMoveY(y + 35f, .5f).SetEase(Ease.InOutFlash).OnComplete(() =>
            {
                flag = true;
                Move();
            }));
        }
    }

    void OnDisable()
    {
       
    }
}
