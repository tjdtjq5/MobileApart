using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    public Text coinText;

    public Text weaponNameText;
    public Text atkText;
    public Text enhanceText;
    public Text enhanceCostText;

    private void Start()
    {
        GameManager.instance.userInfoManager.LoadWeapon(()=> { WeaponUI(); GameManager.instance.userInfoManager.LoadWeaponCoin(() => {  WeaponCoinUI(); }); });
    }
    void WeaponCoinUI()
    {
        coinText.text = "[" + GameManager.instance.userInfoManager.GetWeaponCoin() + "]";
    }
    public void GetWeaponCoin(int coin)
    {
        GameManager.instance.userInfoManager.SetWeaponCoin(GameManager.instance.userInfoManager.GetWeaponCoin() + coin);
        WeaponCoinUI();
        GameManager.instance.userInfoManager.SaveWeaponCoin();
    }

    void WeaponUI()
    {
        string weaponName = GameManager.instance.userInfoManager.userWeapon.weaponName;
        int enhance = GameManager.instance.userInfoManager.userWeapon.enhance;
        int num = GameManager.instance.userInfoManager.userWeapon.num;

        weaponNameText.text = weaponName;
        int enhanceAtk = GameManager.instance.weaponeManager.GetWeaponInfo(weaponName).enhanceAtk;
        int numAtk = GameManager.instance.weaponeManager.GetWeaponInfo(weaponName).getForAtk;
        int originAtk = GameManager.instance.weaponeManager.GetWeaponInfo(weaponName).atk;

        int atk = originAtk + (enhance * enhanceAtk) + (numAtk * num);
        atkText.text = "ATK" + atk;
        enhanceText.text = enhance + "강";

        int enhanceCost = GameManager.instance.weaponeManager.GetWeaponInfo(weaponName).enhancePrice;
        enhanceCostText.text = "강화 코스트\n" + enhanceCost;
    }

    //무기를 얻을때
    public void GetWeapon(string weaponName)
    {
        GameManager.instance.userInfoManager.PushWeapon(weaponName);
        WeaponUI();
        GameManager.instance.userInfoManager.SaveWeapon();
    }

    public void Enhance()
    {
        string weaponName = GameManager.instance.userInfoManager.userWeapon.weaponName;
        int enhancePrice = GameManager.instance.weaponeManager.GetWeaponInfo(weaponName).enhancePrice;
        if (GameManager.instance.userInfoManager.GetWeaponCoin() >= enhancePrice)
        {
            GameManager.instance.userInfoManager.SetWeaponCoin(GameManager.instance.userInfoManager.GetWeaponCoin() - enhancePrice);
            GameManager.instance.userInfoManager.SaveWeaponCoin();
            GameManager.instance.userInfoManager.WeaponEnhance();
            GameManager.instance.userInfoManager.SaveWeapon();
            WeaponUI();
            WeaponCoinUI();
        }
    }
}
