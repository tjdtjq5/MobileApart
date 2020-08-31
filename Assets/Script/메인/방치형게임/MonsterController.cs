using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.U2D;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour
{
    public Transform monster; Vector2 originPos;

    public Hit hit;

    public Transform info;

    public SpriteAtlas monsterAtlas; public Image monsterImg;

    string[] monsterList;
    int originHp; int currentHp;
    int defence;

    IEnumerator HitCoroutine;
    int monsterCount = 0;

    [Header("코인")]
    public GameObject coinParticle;
    [Header("무기")]
    public WeaponController weaponController;

    private void Start()
    {
        originPos = monster.transform.localPosition;
        //이동
        MoveUp();
    }

    //초기 셋팅 
    public void Initialize(string[] monsterList, int hp, int def)
    {
        //정보 저장 
        this.monsterList = monsterList;
        originHp = hp;
        currentHp = originHp;
        defence = def;

        //정보
        HpSetting(currentHp);
        info.Find("몬스터 방어력").GetComponent<Text>().text = "몬스터 방어력 " + def;

        MonsterSetting(0);
        monsterCount = 0;
    }

    void MonsterSetting(int index)
    {
        if (monsterList.Length - 1 < index)
        {
            Debug.Log("몬스터 길이 초과 디버그");
            index = 0;
        }

        //몬스터 이름 
        string monsterName = monsterList[index];

        // 이미지 셋팅
        monsterImg.sprite = monsterAtlas.GetSprite(monsterName);
        monsterImg.SetNativeSize();

        // 정보 셋팅 
        currentHp = originHp;
        HpSetting(currentHp);
        info.Find("몬스터속성").GetComponent<Text>().text = "[" + GameManager.instance.monsterManager.GetMonsterInfo(monsterName).properties.ToString() + "]";
        info.Find("몬스터이름").GetComponent<Text>().text = "[" + monsterName + "]";

        //0.5초 뒤 0.5초 간격으로 공격 
        if (HitCoroutine != null)
        {
            StopCoroutine(HitCoroutine);
        }
        HitCoroutine = TempHitCoroutine();
        StartCoroutine(HitCoroutine);
    }

    void HpSetting(int currentHp)
    {
        info.Find("몬스터체력").GetComponent<Text>().text = currentHp + "HP";
        info.Find("피통 이미지").Find("fore").GetComponent<Image>().fillAmount = currentHp / (float)originHp;
    }
    IEnumerator TempHitCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(.5f);
            Hit();
        }
    }
    void Hit()
    {
        hit.SlashBlue01();
        monster.DOScale(.75f,0.05f).OnComplete(()=> {
            coinParticle.transform.localPosition = monsterImg.transform.localPosition;
            coinParticle.GetComponent<ParticleSystem>().Play();
            //coinParticle.GetComponent<ParticleSystem>().emission.burstCount = Atk();

            int atk = Atk();
            if (currentHp - atk <= 0)
            {
                atk = currentHp;
            }
            currentHp -= atk;
            HpSetting(currentHp);
            weaponController.GetWeaponCoin(atk);
            if (currentHp <= 0)
            {
                MonsterDead();
            }

            monster.DOScale(.8f, 0.05f); });
    }

    int Atk()
    {
        string weaponName = GameManager.instance.userInfoManager.userWeapon.weaponName;
        int weaponAtk = GameManager.instance.weaponeManager.GetWeaponInfo(weaponName).atk;
        int enhance = GameManager.instance.userInfoManager.userWeapon.enhance;
        int enhanceAtk = GameManager.instance.weaponeManager.GetWeaponInfo(weaponName).enhanceAtk;
        int num = GameManager.instance.userInfoManager.userWeapon.num;
        int numAtk = GameManager.instance.weaponeManager.GetWeaponInfo(weaponName).getForAtk;

        int atk = weaponAtk + (enhance * enhanceAtk) + (num * numAtk);

        atk -= defence;
        if (atk < 0)
        {
            atk = 0;
        }
        return atk;
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
        monsterCount++;
        MonsterSetting(monsterCount);
    }

}
