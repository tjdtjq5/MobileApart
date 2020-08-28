using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MonsterController : MonoBehaviour
{
    public Transform monster; Vector2 originPos;

    public Hit hit;

    public Transform info;

    private void Start()
    {
        originPos = monster.transform.localPosition;
        InvokeRepeating("Hit", 0.5f, 0.5f);
        MoveUp();
    }

    public void Hit()
    {
        hit.SlashBlue01();
        monster.DOScale(.75f,0.05f).OnComplete(()=> { monster.DOScale(.8f, 0.05f); });
    }

    void MoveUp()
    {
        monster.transform.DOLocalMoveY(originPos.y + 60, 1).SetEase(Ease.Linear).OnComplete(()=> {
            monster.transform.DOLocalMoveY(originPos.y, 1).SetEase(Ease.Linear).OnComplete(()=> { MoveDown(); });
        });
    }

    void MoveDown()
    {
        monster.transform.DOLocalMoveY(originPos.y - 60, 1).SetEase(Ease.Linear).OnComplete(() => {
            monster.transform.DOLocalMoveY(originPos.y, 1).SetEase(Ease.Linear).OnComplete(() => { MoveUp(); });
        });
    }

    void MonsterDead()
    {

    }

    void MonsterSet(string monsterName)
    {

    }
}
