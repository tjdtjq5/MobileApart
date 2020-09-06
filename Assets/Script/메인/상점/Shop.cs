using DG.Tweening;
using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject shopPannel;
    public Transform context;
    public Sprite goldSprite;
    public Sprite crystalSprite;
    public Transform AlramSucess;

    public DailyGoods dailyGoods;
    public RandomGoodsInfo[] randomGoodsInfo;
    public RandomGoods[] randomGoods;

    public GameObject[] setOff;

    IEnumerator DailyChkCoroutine;

    [System.Serializable]
    public struct DailyGoods
    {
        public string GoodsName;
        public Sprite GoodsSprite;
        [Header("상품 설명")]
        [Multiline(3)]
        public string explanation;
        [Header("판매 종료 날짜")]
        public int year;
        public int month;
        public int day;
        [Header("가격")]
        public MoneyKind MoneyKind;
        public int price;
    }

    [System.Serializable]
    public struct RandomGoodsInfo
    {
        public string GoodsName;
        public string itemName;
        public GameObject GoodsObj;
        public ItemKind itemKind;
        [Header("상품 설명")]
        [Multiline(3)]
        public string explanation;
        [Header("가격")]
        public MoneyKind MoneyKind;
        public int price;
    }

    [System.Serializable]
    public struct RandomGoods
    {
        public string GoodsName;
        public string itemName;
        public ItemKind itemKind;
        [Header("컬러")]
        public Color color01;
        public Color color02;
        [Header("상품 설명")]
        public string explanation;
        [Header("가격")]
        public MoneyKind MoneyKind;
        public int price;
    }

    private void Start()
    {
        RandomGoodsInfoSetting();
        RandomGoodsSetting();
    }

    void RandomGoodsInfoSetting()
    {
        int Length = GameManager.instance.databaseManager.DailyShop_DB.GetLineSize();
        randomGoodsInfo = new RandomGoodsInfo[Length];
        for (int i = 0; i < Length; i++)
        {
            List<string> dataList = GameManager.instance.databaseManager.DailyShop_DB.GetRowData(i);
            randomGoodsInfo[i].GoodsName = dataList[1];
            randomGoodsInfo[i].itemName = dataList[0];
            randomGoodsInfo[i].GoodsObj = GameManager.instance.itemManager.GetItemInfo(dataList[0]).iconObj;
            randomGoodsInfo[i].itemKind = (ItemKind)System.Enum.Parse(typeof(ItemKind), dataList[2]);
            string[] tempExplanation = dataList[5].Split('#');
            for (int j = 0; j < tempExplanation.Length; j++)
            {
                randomGoodsInfo[i].explanation += tempExplanation[j] + "\n";
            }
            randomGoodsInfo[i].explanation = randomGoodsInfo[i].explanation.Substring(0, randomGoodsInfo[i].explanation.Length - 1);
            randomGoodsInfo[i].MoneyKind = (MoneyKind)System.Enum.Parse(typeof(MoneyKind), dataList[3]);
            randomGoodsInfo[i].price = int.Parse(dataList[4]);
        }
    }

    public void RandomGoodsSetting()
    {
        for (int i = 0; i < context.childCount; i++)
        {
            Transform tempGoodsTransform = context.GetChild(i);
            for (int j = 0; j < tempGoodsTransform.childCount; j++)
            {
                if (tempGoodsTransform.GetChild(j).name == "상품" || tempGoodsTransform.GetChild(j).name == "그림자")
                {
                    if (tempGoodsTransform.GetChild(j).childCount > 0)
                    {
                        Destroy(tempGoodsTransform.GetChild(j).GetChild(0).gameObject);
                    }
                }
            }
        }

        List<int> randomList = new List<int>();
        while (randomList.Count < 6)
        {
            int tempRandom = Random.RandomRange(0, randomGoodsInfo.Length);
            if (randomGoodsInfo[tempRandom].itemKind == ItemKind.스킨 && !randomList.Contains(tempRandom))
            {
                randomList.Add(tempRandom);
                Debug.Log(randomGoodsInfo[tempRandom].itemName);
            }
        }
        randomGoods = new RandomGoods[9];

        // 1
        randomGoods[0].GoodsName = randomGoodsInfo[randomList[0]].GoodsName;
        randomGoods[0].itemName = randomGoodsInfo[randomList[0]].itemName;
        randomGoods[0].itemKind = randomGoodsInfo[randomList[0]].itemKind;
        randomGoods[0].color01 = GameManager.instance.userInfoManager.RandColor();
        randomGoods[0].color02 = GameManager.instance.userInfoManager.RandColor();
        randomGoods[0].explanation = randomGoodsInfo[randomList[0]].explanation;
        randomGoods[0].MoneyKind = randomGoodsInfo[randomList[0]].MoneyKind;
        randomGoods[0].price = randomGoodsInfo[randomList[0]].price;
        GameObject itemObj = Instantiate(randomGoodsInfo[randomList[0]].GoodsObj, context.Find("상품1").position, Quaternion.identity, context.Find("상품1").Find("상품"));
        for (int k = 0; k < itemObj.transform.childCount; k++)
        {
            if (itemObj.transform.GetChild(k).name == "color_01")
            {
                itemObj.transform.GetChild(k).GetComponent<Image>().color = randomGoods[0].color01;
            }
            if (itemObj.transform.GetChild(k).name == "color_02")
            {
                itemObj.transform.GetChild(k).GetComponent<Image>().color = randomGoods[0].color02;
            }
        }
        GameObject shadowItemObj = Instantiate(randomGoodsInfo[randomList[0]].GoodsObj, new Vector2(context.Find("상품1").Find("그림자").position.x  , context.Find("상품1").Find("그림자").position.y ), Quaternion.identity, context.Find("상품1").Find("그림자"));
        for (int k = 0; k < shadowItemObj.transform.childCount; k++)
        {
            if (shadowItemObj.transform.GetChild(k).GetComponent<Image>() != null)
            {
                shadowItemObj.transform.GetChild(k).GetComponent<Image>().color = new Color(0, 0, 0, 76 / 255f);
            }
        }
        context.Find("상품1").Find("이름").GetComponent<Text>().text = randomGoods[0].GoodsName;
        context.Find("상품1").Find("설명").GetComponent<Text>().text = randomGoods[0].explanation;
        switch (randomGoods[0].MoneyKind)
        {
            case MoneyKind.Gold:
                context.Find("상품1").Find("재화").GetComponent<Image>().sprite = goldSprite;
                context.Find("상품1").Find("재화").GetChild(0).GetComponent<Text>().text = randomGoods[0].price.ToString();
                context.Find("상품1").Find("재화").GetChild(0).GetComponent<Text>().color = new Color(229 / 255f, 191 / 255f, 0, 1);
                break;
            case MoneyKind.Crystal:
                context.Find("상품1").Find("재화").GetComponent<Image>().sprite = crystalSprite;
                context.Find("상품1").Find("재화").GetChild(0).GetComponent<Text>().text = randomGoods[0].price.ToString();
                context.Find("상품1").Find("재화").GetChild(0).GetComponent<Text>().color = new Color(69 / 255f, 237 / 255f, 234 / 255f, 1);
                break;
        }

        // 2 
        randomGoods[1].GoodsName = randomGoodsInfo[randomList[1]].GoodsName;
        randomGoods[1].itemName = randomGoodsInfo[randomList[1]].itemName;
        randomGoods[1].itemKind = randomGoodsInfo[randomList[1]].itemKind;
        randomGoods[1].color01 = GameManager.instance.userInfoManager.RandColor();
        randomGoods[1].color02 = GameManager.instance.userInfoManager.RandColor();
        randomGoods[1].explanation = randomGoodsInfo[randomList[1]].explanation;
        randomGoods[1].MoneyKind = randomGoodsInfo[randomList[1]].MoneyKind;
        randomGoods[1].price = randomGoodsInfo[randomList[1]].price;
        GameObject itemObj02 = Instantiate(randomGoodsInfo[randomList[1]].GoodsObj, context.Find("상품2").position, Quaternion.identity, context.Find("상품2").Find("상품"));
        for (int k = 0; k < itemObj02.transform.childCount; k++)
        {
            if (itemObj02.transform.GetChild(k).name == "color_01")
            {
                itemObj02.transform.GetChild(k).GetComponent<Image>().color = randomGoods[1].color01;
            }
            if (itemObj02.transform.GetChild(k).name == "color_02")
            {
                itemObj02.transform.GetChild(k).GetComponent<Image>().color = randomGoods[1].color02;
            }
        }
        GameObject shadowItemObj02 = Instantiate(randomGoodsInfo[randomList[1]].GoodsObj, new Vector2(context.Find("상품2").Find("그림자").position.x, context.Find("상품2").Find("그림자").position.y), Quaternion.identity, context.Find("상품2").Find("그림자"));
        for (int k = 0; k < shadowItemObj02.transform.childCount; k++)
        {
            if (shadowItemObj02.transform.GetChild(k).GetComponent<Image>() != null)
            {
                shadowItemObj02.transform.GetChild(k).GetComponent<Image>().color = new Color(0, 0, 0, 76 / 255f);
            }
        }
        context.Find("상품2").Find("이름").GetComponent<Text>().text = randomGoods[1].GoodsName;
        context.Find("상품2").Find("설명").GetComponent<Text>().text = randomGoods[1].explanation;
        switch (randomGoods[1].MoneyKind)
        {
            case MoneyKind.Gold:
                context.Find("상품2").Find("재화").GetComponent<Image>().sprite = goldSprite;
                context.Find("상품2").Find("재화").GetChild(0).GetComponent<Text>().text = randomGoods[1].price.ToString();
                context.Find("상품2").Find("재화").GetChild(0).GetComponent<Text>().color = new Color(229 / 255f, 191 / 255f, 0, 1);
                break;
            case MoneyKind.Crystal:
                context.Find("상품2").Find("재화").GetComponent<Image>().sprite = crystalSprite;
                context.Find("상품2").Find("재화").GetChild(0).GetComponent<Text>().text = randomGoods[1].price.ToString();
                context.Find("상품2").Find("재화").GetChild(0).GetComponent<Text>().color = new Color(69 / 255f, 237 / 255f, 234 / 255f, 1);
                break;
        }

        // 3 
        randomGoods[2].GoodsName = randomGoodsInfo[randomList[2]].GoodsName;
        randomGoods[2].itemName = randomGoodsInfo[randomList[2]].itemName;
        randomGoods[2].itemKind = randomGoodsInfo[randomList[2]].itemKind;
        randomGoods[2].color01 = GameManager.instance.userInfoManager.RandColor();
        randomGoods[2].color02 = GameManager.instance.userInfoManager.RandColor();
        randomGoods[2].explanation = randomGoodsInfo[randomList[2]].explanation;
        randomGoods[2].MoneyKind = randomGoodsInfo[randomList[2]].MoneyKind;
        randomGoods[2].price = randomGoodsInfo[randomList[2]].price;
        GameObject itemObj03 = Instantiate(randomGoodsInfo[randomList[2]].GoodsObj, context.Find("상품3").position, Quaternion.identity, context.Find("상품3").Find("상품"));
        for (int k = 0; k < itemObj03.transform.childCount; k++)
        {
            if (itemObj03.transform.GetChild(k).name == "color_01")
            {
                itemObj03.transform.GetChild(k).GetComponent<Image>().color = randomGoods[2].color01;
            }
            if (itemObj03.transform.GetChild(k).name == "color_02")
            {
                itemObj03.transform.GetChild(k).GetComponent<Image>().color = randomGoods[2].color02;
            }
        }
        GameObject shadowItemObj03 = Instantiate(randomGoodsInfo[randomList[2]].GoodsObj, new Vector2(context.Find("상품3").Find("그림자").position.x, context.Find("상품3").Find("그림자").position.y), Quaternion.identity, context.Find("상품3").Find("그림자"));
        for (int k = 0; k < shadowItemObj03.transform.childCount; k++)
        {
            if (shadowItemObj03.transform.GetChild(k).GetComponent<Image>() != null)
            {
                shadowItemObj03.transform.GetChild(k).GetComponent<Image>().color = new Color(0, 0, 0, 76 / 255f);
            }
        }
        context.Find("상품3").Find("이름").GetComponent<Text>().text = randomGoods[2].GoodsName;
        context.Find("상품3").Find("설명").GetComponent<Text>().text = randomGoods[2].explanation;
        switch (randomGoods[2].MoneyKind)
        {
            case MoneyKind.Gold:
                context.Find("상품3").Find("재화").GetComponent<Image>().sprite = goldSprite;
                context.Find("상품3").Find("재화").GetChild(0).GetComponent<Text>().text = randomGoods[2].price.ToString();
                context.Find("상품3").Find("재화").GetChild(0).GetComponent<Text>().color = new Color(229 / 255f, 191 / 255f, 0, 1);
                break;
            case MoneyKind.Crystal:
                context.Find("상품3").Find("재화").GetComponent<Image>().sprite = crystalSprite;
                context.Find("상품3").Find("재화").GetChild(0).GetComponent<Text>().text = randomGoods[2].price.ToString();
                context.Find("상품3").Find("재화").GetChild(0).GetComponent<Text>().color = new Color(69 / 255f, 237 / 255f, 234 / 255f, 1);
                break;
        }


        // 4
        randomGoods[3].GoodsName = randomGoodsInfo[0].GoodsName;
        randomGoods[3].itemName = randomGoodsInfo[0].itemName;
        randomGoods[3].itemKind = randomGoodsInfo[0].itemKind;
        randomGoods[3].color01 = Color.clear;
        randomGoods[3].color02 = Color.clear;
        randomGoods[3].explanation = randomGoodsInfo[0].explanation;
        randomGoods[3].MoneyKind = randomGoodsInfo[0].MoneyKind;
        randomGoods[3].price = randomGoodsInfo[0].price;
        GameObject itemObj04 = Instantiate(randomGoodsInfo[0].GoodsObj, context.Find("상품4").position, Quaternion.identity, context.Find("상품4").Find("상품"));
        GameObject shadowItemObj04 = Instantiate(randomGoodsInfo[0].GoodsObj, new Vector2(context.Find("상품4").Find("그림자").position.x, context.Find("상품4").Find("그림자").position.y), Quaternion.identity, context.Find("상품4").Find("그림자"));
        for (int k = 0; k < shadowItemObj04.transform.childCount; k++)
        {
            if (shadowItemObj04.transform.GetChild(k).GetComponent<Image>() != null)
            {
                shadowItemObj04.transform.GetChild(k).GetComponent<Image>().color = new Color(0, 0, 0, 76 / 255f);
            }
        }
        context.Find("상품4").Find("이름").GetComponent<Text>().text = randomGoods[3].GoodsName;
        context.Find("상품4").Find("설명").GetComponent<Text>().text = randomGoods[3].explanation;
        switch (randomGoods[3].MoneyKind)
        {
            case MoneyKind.Gold:
                context.Find("상품4").Find("재화").GetComponent<Image>().sprite = goldSprite;
                context.Find("상품4").Find("재화").GetChild(0).GetComponent<Text>().text = randomGoods[3].price.ToString();
                context.Find("상품4").Find("재화").GetChild(0).GetComponent<Text>().color = new Color(229 / 255f, 191 / 255f, 0, 1);
                break;
            case MoneyKind.Crystal:
                context.Find("상품4").Find("재화").GetComponent<Image>().sprite = crystalSprite;
                context.Find("상품4").Find("재화").GetChild(0).GetComponent<Text>().text = randomGoods[3].price.ToString();
                context.Find("상품4").Find("재화").GetChild(0).GetComponent<Text>().color = new Color(69 / 255f, 237 / 255f, 234 / 255f, 1);
                break;
        }

        // 5
        randomGoods[4].GoodsName = randomGoodsInfo[0].GoodsName;
        randomGoods[4].itemName = randomGoodsInfo[0].itemName;
        randomGoods[4].itemKind = randomGoodsInfo[0].itemKind;
        randomGoods[4].color01 = Color.clear;
        randomGoods[4].color02 = Color.clear;
        randomGoods[4].explanation = randomGoodsInfo[0].explanation;
        randomGoods[4].MoneyKind = randomGoodsInfo[0].MoneyKind;
        randomGoods[4].price = randomGoodsInfo[0].price;
        GameObject itemObj05 = Instantiate(randomGoodsInfo[0].GoodsObj, context.Find("상품5").position, Quaternion.identity, context.Find("상품5").Find("상품"));
        GameObject shadowItemObj05 = Instantiate(randomGoodsInfo[0].GoodsObj, new Vector2(context.Find("상품5").Find("그림자").position.x, context.Find("상품5").Find("그림자").position.y), Quaternion.identity, context.Find("상품5").Find("그림자"));
        for (int k = 0; k < shadowItemObj05.transform.childCount; k++)
        {
            if (shadowItemObj05.transform.GetChild(k).GetComponent<Image>() != null)
            {
                shadowItemObj05.transform.GetChild(k).GetComponent<Image>().color = new Color(0, 0, 0, 76 / 255f);
            }
        }
        context.Find("상품5").Find("이름").GetComponent<Text>().text = randomGoods[4].GoodsName;
        context.Find("상품5").Find("설명").GetComponent<Text>().text = randomGoods[4].explanation;
        switch (randomGoods[4].MoneyKind)
        {
            case MoneyKind.Gold:
                context.Find("상품5").Find("재화").GetComponent<Image>().sprite = goldSprite;
                context.Find("상품5").Find("재화").GetChild(0).GetComponent<Text>().text = randomGoods[4].price.ToString();
                context.Find("상품5").Find("재화").GetChild(0).GetComponent<Text>().color = new Color(229 / 255f, 191 / 255f, 0, 1);
                break;
            case MoneyKind.Crystal:
                context.Find("상품5").Find("재화").GetComponent<Image>().sprite = crystalSprite;
                context.Find("상품5").Find("재화").GetChild(0).GetComponent<Text>().text = randomGoods[4].price.ToString();
                context.Find("상품5").Find("재화").GetChild(0).GetComponent<Text>().color = new Color(69 / 255f, 237 / 255f, 234 / 255f, 1);
                break;
        }

        // 6
        randomGoods[5].GoodsName = randomGoodsInfo[0].GoodsName;
        randomGoods[5].itemName = randomGoodsInfo[0].itemName;
        randomGoods[5].itemKind = randomGoodsInfo[0].itemKind;
        randomGoods[5].color01 = Color.clear;
        randomGoods[5].color02 = Color.clear;
        randomGoods[5].explanation = randomGoodsInfo[0].explanation;
        randomGoods[5].MoneyKind = randomGoodsInfo[0].MoneyKind;
        randomGoods[5].price = randomGoodsInfo[0].price;
        GameObject itemObj06 = Instantiate(randomGoodsInfo[0].GoodsObj, context.Find("상품6").position, Quaternion.identity, context.Find("상품6").Find("상품"));
        GameObject shadowItemObj06 = Instantiate(randomGoodsInfo[0].GoodsObj, new Vector2(context.Find("상품6").Find("그림자").position.x, context.Find("상품6").Find("그림자").position.y), Quaternion.identity, context.Find("상품6").Find("그림자"));
        for (int k = 0; k < shadowItemObj06.transform.childCount; k++)
        {
            if (shadowItemObj06.transform.GetChild(k).GetComponent<Image>() != null)
            {
                shadowItemObj06.transform.GetChild(k).GetComponent<Image>().color = new Color(0, 0, 0, 76 / 255f);
            }
        }
        context.Find("상품6").Find("이름").GetComponent<Text>().text = randomGoods[5].GoodsName;
        context.Find("상품6").Find("설명").GetComponent<Text>().text = randomGoods[5].explanation;
        switch (randomGoods[5].MoneyKind)
        {
            case MoneyKind.Gold:
                context.Find("상품6").Find("재화").GetComponent<Image>().sprite = goldSprite;
                context.Find("상품6").Find("재화").GetChild(0).GetComponent<Text>().text = randomGoods[5].price.ToString();
                context.Find("상품6").Find("재화").GetChild(0).GetComponent<Text>().color = new Color(229 / 255f, 191 / 255f, 0, 1);
                break;
            case MoneyKind.Crystal:
                context.Find("상품6").Find("재화").GetComponent<Image>().sprite = crystalSprite;
                context.Find("상품6").Find("재화").GetChild(0).GetComponent<Text>().text = randomGoods[5].price.ToString();
                context.Find("상품6").Find("재화").GetChild(0).GetComponent<Text>().color = new Color(69 / 255f, 237 / 255f, 234 / 255f, 1);
                break;
        }

        // 7 
        randomGoods[6].GoodsName = randomGoodsInfo[randomList[3]].GoodsName;
        randomGoods[6].itemName = randomGoodsInfo[randomList[3]].itemName;
        randomGoods[6].itemKind = randomGoodsInfo[randomList[3]].itemKind;
        randomGoods[6].color01 = GameManager.instance.userInfoManager.RandColor();
        randomGoods[6].color02 = GameManager.instance.userInfoManager.RandColor();
        randomGoods[6].explanation = randomGoodsInfo[randomList[3]].explanation;
        randomGoods[6].MoneyKind = randomGoodsInfo[randomList[3]].MoneyKind;
        randomGoods[6].price = randomGoodsInfo[randomList[3]].price;
        GameObject itemObj07 = Instantiate(randomGoodsInfo[randomList[3]].GoodsObj, context.Find("상품7").position, Quaternion.identity, context.Find("상품7").Find("상품"));
        for (int k = 0; k < itemObj07.transform.childCount; k++)
        {
            if (itemObj07.transform.GetChild(k).name == "color_01")
            {
                itemObj07.transform.GetChild(k).GetComponent<Image>().color = randomGoods[6].color01;
            }
            if (itemObj07.transform.GetChild(k).name == "color_02")
            {
                itemObj07.transform.GetChild(k).GetComponent<Image>().color = randomGoods[6].color02;
            }
        }
        GameObject shadowItemObj07 = Instantiate(randomGoodsInfo[randomList[3]].GoodsObj, new Vector2(context.Find("상품7").Find("그림자").position.x, context.Find("상품7").Find("그림자").position.y), Quaternion.identity, context.Find("상품7").Find("그림자"));
        for (int k = 0; k < shadowItemObj07.transform.childCount; k++)
        {
            if (shadowItemObj07.transform.GetChild(k).GetComponent<Image>() != null)
            {
                shadowItemObj07.transform.GetChild(k).GetComponent<Image>().color = new Color(0, 0, 0, 76 / 255f);
            }
        }
        context.Find("상품7").Find("이름").GetComponent<Text>().text = randomGoods[6].GoodsName;
        context.Find("상품7").Find("설명").GetComponent<Text>().text = randomGoods[6].explanation;
        switch (randomGoods[6].MoneyKind)
        {
            case MoneyKind.Gold:
                context.Find("상품7").Find("재화").GetComponent<Image>().sprite = goldSprite;
                context.Find("상품7").Find("재화").GetChild(0).GetComponent<Text>().text = randomGoods[6].price.ToString();
                context.Find("상품7").Find("재화").GetChild(0).GetComponent<Text>().color = new Color(229 / 255f, 191 / 255f, 0, 1);
                break;
            case MoneyKind.Crystal:
                context.Find("상품7").Find("재화").GetComponent<Image>().sprite = crystalSprite;
                context.Find("상품7").Find("재화").GetChild(0).GetComponent<Text>().text = randomGoods[6].price.ToString();
                context.Find("상품7").Find("재화").GetChild(0).GetComponent<Text>().color = new Color(69 / 255f, 237 / 255f, 234 / 255f, 1);
                break;
        }

        // 8 
        randomGoods[7].GoodsName = randomGoodsInfo[randomList[4]].GoodsName;
        randomGoods[7].itemName = randomGoodsInfo[randomList[4]].itemName;
        randomGoods[7].itemKind = randomGoodsInfo[randomList[4]].itemKind;
        randomGoods[7].color01 = GameManager.instance.userInfoManager.RandColor();
        randomGoods[7].color02 = GameManager.instance.userInfoManager.RandColor();
        randomGoods[7].explanation = randomGoodsInfo[randomList[4]].explanation;
        randomGoods[7].MoneyKind = randomGoodsInfo[randomList[4]].MoneyKind;
        randomGoods[7].price = randomGoodsInfo[randomList[4]].price;
        GameObject itemObj08 = Instantiate(randomGoodsInfo[randomList[4]].GoodsObj, context.Find("상품8").position, Quaternion.identity, context.Find("상품8").Find("상품"));
        for (int k = 0; k < itemObj08.transform.childCount; k++)
        {
            if (itemObj08.transform.GetChild(k).name == "color_01")
            {
                itemObj08.transform.GetChild(k).GetComponent<Image>().color = randomGoods[7].color01;
            }
            if (itemObj08.transform.GetChild(k).name == "color_02")
            {
                itemObj08.transform.GetChild(k).GetComponent<Image>().color = randomGoods[7].color02;
            }
        }
        GameObject shadowItemObj08 = Instantiate(randomGoodsInfo[randomList[4]].GoodsObj, new Vector2(context.Find("상품8").Find("그림자").position.x, context.Find("상품8").Find("그림자").position.y), Quaternion.identity, context.Find("상품8").Find("그림자"));
        for (int k = 0; k < shadowItemObj08.transform.childCount; k++)
        {
            if (shadowItemObj08.transform.GetChild(k).GetComponent<Image>() != null)
            {
                shadowItemObj08.transform.GetChild(k).GetComponent<Image>().color = new Color(0, 0, 0, 76 / 255f);
            }
        }
        context.Find("상품8").Find("이름").GetComponent<Text>().text = randomGoods[7].GoodsName;
        context.Find("상품8").Find("설명").GetComponent<Text>().text = randomGoods[7].explanation;
        switch (randomGoods[7].MoneyKind)
        {
            case MoneyKind.Gold:
                context.Find("상품8").Find("재화").GetComponent<Image>().sprite = goldSprite;
                context.Find("상품8").Find("재화").GetChild(0).GetComponent<Text>().text = randomGoods[7].price.ToString();
                context.Find("상품8").Find("재화").GetChild(0).GetComponent<Text>().color = new Color(229 / 255f, 191 / 255f, 0, 1);
                break;
            case MoneyKind.Crystal:
                context.Find("상품8").Find("재화").GetComponent<Image>().sprite = crystalSprite;
                context.Find("상품8").Find("재화").GetChild(0).GetComponent<Text>().text = randomGoods[7].price.ToString();
                context.Find("상품8").Find("재화").GetChild(0).GetComponent<Text>().color = new Color(69 / 255f, 237 / 255f, 234 / 255f, 1);
                break;
        }

        // 9
        randomGoods[8].GoodsName = randomGoodsInfo[randomList[5]].GoodsName;
        randomGoods[8].itemName = randomGoodsInfo[randomList[5]].itemName;
        randomGoods[8].itemKind = randomGoodsInfo[randomList[5]].itemKind;
        randomGoods[8].color01 = GameManager.instance.userInfoManager.RandColor();
        randomGoods[8].color02 = GameManager.instance.userInfoManager.RandColor();
        randomGoods[8].explanation = randomGoodsInfo[randomList[5]].explanation;
        randomGoods[8].MoneyKind = randomGoodsInfo[randomList[5]].MoneyKind;
        randomGoods[8].price = randomGoodsInfo[randomList[5]].price;
        GameObject itemObj09 = Instantiate(randomGoodsInfo[randomList[5]].GoodsObj, context.Find("상품9").position, Quaternion.identity, context.Find("상품9").Find("상품"));
        for (int k = 0; k < itemObj09.transform.childCount; k++)
        {
            if (itemObj09.transform.GetChild(k).name == "color_01")
            {
                itemObj09.transform.GetChild(k).GetComponent<Image>().color = randomGoods[8].color01;
            }
            if (itemObj09.transform.GetChild(k).name == "color_02")
            {
                itemObj09.transform.GetChild(k).GetComponent<Image>().color = randomGoods[8].color02;
            }
        }
        GameObject shadowItemObj09 = Instantiate(randomGoodsInfo[randomList[5]].GoodsObj, new Vector2(context.Find("상품9").Find("그림자").position.x, context.Find("상품9").Find("그림자").position.y), Quaternion.identity, context.Find("상품9").Find("그림자"));
        for (int k = 0; k < shadowItemObj09.transform.childCount; k++)
        {
            if (shadowItemObj09.transform.GetChild(k).GetComponent<Image>() != null)
            {
                shadowItemObj09.transform.GetChild(k).GetComponent<Image>().color = new Color(0, 0, 0, 76 / 255f);
            }
        }
        context.Find("상품9").Find("이름").GetComponent<Text>().text = randomGoods[8].GoodsName;
        context.Find("상품9").Find("설명").GetComponent<Text>().text = randomGoods[8].explanation;
        switch (randomGoods[8].MoneyKind)
        {
            case MoneyKind.Gold:
                context.Find("상품9").Find("재화").GetComponent<Image>().sprite = goldSprite;
                context.Find("상품9").Find("재화").GetChild(0).GetComponent<Text>().text = randomGoods[8].price.ToString();
                context.Find("상품9").Find("재화").GetChild(0).GetComponent<Text>().color = new Color(229 / 255f, 191 / 255f, 0, 1);
                break;
            case MoneyKind.Crystal:
                context.Find("상품9").Find("재화").GetComponent<Image>().sprite = crystalSprite;
                context.Find("상품9").Find("재화").GetChild(0).GetComponent<Text>().text = randomGoods[8].price.ToString();
                context.Find("상품9").Find("재화").GetChild(0).GetComponent<Text>().color = new Color(69 / 255f, 237 / 255f, 234 / 255f, 1);
                break;
        }
    }

    public void ShopOpen()
    {
        shopPannel.gameObject.SetActive(true);
        SetMoneyText();
        SetDailyGoods();

        for (int i = 0; i < setOff.Length; i++)
        {
            setOff[i].SetActive(false);
        }
    }

    public void ShopClose()
    {
        shopPannel.gameObject.SetActive(false);

        if (DailyChkCoroutine != null)
        {
            StopCoroutine(DailyChkCoroutine);
        }

        for (int i = 0; i < setOff.Length; i++)
        {
            setOff[i].SetActive(true);
        }
    }

    public void SetMoneyText()
    {
        shopPannel.transform.Find("재화UI").Find("골드").GetChild(0).GetComponent<Text>().text = string.Format("{0:#,###0}", GameManager.instance.userInfoManager.GetUserMoney(MoneyKind.Gold));
        shopPannel.transform.Find("재화UI").Find("보석").GetChild(0).GetComponent<Text>().text = string.Format("{0:#,###0}", GameManager.instance.userInfoManager.GetUserMoney(MoneyKind.Crystal));
    }
 
    public void SetDailyGoods()
    {
        context.Find("데일리 상품").Find("상품그림자").GetComponent<Image>().sprite = dailyGoods.GoodsSprite;
        context.Find("데일리 상품").Find("상품그림자").GetChild(0).GetComponent<Image>().sprite = dailyGoods.GoodsSprite;
        context.Find("데일리 상품").Find("상품설명").GetComponent<Text>().text = dailyGoods.explanation;
        DailyChkCoroutine = DailyChk();
        StartCoroutine(DailyChkCoroutine); // 남은시간 계산
        switch (dailyGoods.MoneyKind)
        {
            case MoneyKind.Gold:
                context.Find("데일리 상품").Find("재화").GetComponent<Image>().sprite = goldSprite;
                context.Find("데일리 상품").Find("재화").GetChild(0).GetComponent<Text>().color = new Color(229 / 255f, 191 / 255f, 0, 1);
                break;
            case MoneyKind.Crystal:
                context.Find("데일리 상품").Find("재화").GetComponent<Image>().sprite = crystalSprite;
                context.Find("데일리 상품").Find("재화").GetChild(0).GetComponent<Text>().color = new Color(69 / 255f, 237 / 255f, 234/255f, 1);
                break;
        }
        context.Find("데일리 상품").Find("재화").GetChild(0).GetComponent<Text>().text = string.Format("{0:#,###0}", dailyGoods.price);
    }

    IEnumerator DailyChk()
    {
        while (true)
        {
            UnityWebRequest request = new UnityWebRequest();
            using (request = UnityWebRequest.Get("www.naver.com"))
            {
                yield return request.SendWebRequest();

                if (request.isNetworkError)
                {
                    Debug.Log(request.error);
                }
                else
                {
                    string date = request.GetResponseHeader("date");
                    System.DateTime dateTime = System.DateTime.Parse(date).ToUniversalTime();
                    System.TimeSpan timestamp = new System.DateTime(dailyGoods.year, dailyGoods.month, dailyGoods.day, 0, 0, 0) - dateTime;
                    context.Find("데일리 상품").Find("판매종료시간").GetComponent<Text>().text = "판매종료 " + timestamp.Days + "일 " + timestamp.Hours + "시간 " + timestamp.Minutes + "분 남음";
                }
            }

            yield return new WaitForSeconds(10f);
        }
      
    }

    public void DailyPurchase()
    {

    }

    public void RandomGoodsPurchase(int index)
    {
        // 돈이 없으면 리턴
        if (GameManager.instance.userInfoManager.GetUserMoney(randomGoods[index].MoneyKind) < randomGoods[index].price)
        {
            return;
        }

        string itemName = randomGoods[index].itemName;
        ItemKind itemKind = randomGoods[index].itemKind;
        Color color_01 = randomGoods[index].color01;
        Color color_02 = randomGoods[index].color02;
        MoneyKind moneyKind = randomGoods[index].MoneyKind;
        int price = randomGoods[index].price;
        OverrideCanvas.instance.PurchaseAlram(()=> PerChase(itemName, itemKind, color_01, color_02, moneyKind, price));
    }

    public void PerChase(string itemName, ItemKind itemKind, Color color_01, Color color_02, MoneyKind moneyKind, int price)
    {

        AlramSucess.gameObject.SetActive(true);
        AlramSucess.GetComponent<Image>().DOFade(1, 0);
        AlramSucess.GetChild(0).GetComponent<Text>().DOFade(1, 0);
        AlramSucess.GetComponent<Image>().DOFade(0, 1.3f).OnComplete(() => { AlramSucess.gameObject.SetActive(false); });
        AlramSucess.GetChild(0).GetComponent<Text>().DOFade(0, 1.3f);

        GameManager.instance.userInfoManager.SetUserMoney(moneyKind, GameManager.instance.userInfoManager.GetUserMoney(moneyKind) - price);
        SetMoneyText();
        GameManager.instance.userInfoManager.SaveUserMoney();

        switch (itemKind)
        {
            case ItemKind.골드:
                break;
            case ItemKind.크리스탈:
                break;
            case ItemKind.랜덤염색약:
                GameManager.instance.userInfoManager.PushColorItem(Color.clear);
                break;
            case ItemKind.염색약:
                GameManager.instance.userInfoManager.PushColorItem(color_01);
                break;
            case ItemKind.스킨:
                GameManager.instance.userInfoManager.PushSkinItem(new UserSkin(itemName, color_01, color_02));
                break;
            default:
                break;
        }

        //옷 저장
        GameManager.instance.userInfoManager.SaveSkinItem();
    }

}

public enum MoneyKind
{
    Gold,
    Crystal
}
