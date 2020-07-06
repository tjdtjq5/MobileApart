using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject shopPannel;

    public void ShopOpen()
    {
        shopPannel.gameObject.SetActive(true);
    }

    public void ShopClose()
    {
        shopPannel.gameObject.SetActive(false);
    }
}
