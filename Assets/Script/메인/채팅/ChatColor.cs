using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatColor : MonoBehaviour
{
    public GameObject set;
    public Transform btn_color;
    public Image set_color;

    public void Open()
    {
        set.SetActive(true);
    }

    public void Close()
    {
        set.SetActive(false);
    }

    public void SetColor(int index)
    {
        Color color = btn_color.GetChild(index).GetComponent<Image>().color;
        set_color.color = color;
        Close();
    }
}
