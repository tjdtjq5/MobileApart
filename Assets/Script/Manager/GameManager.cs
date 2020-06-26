using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public ItemManager itemManager;
    public UserInfoManager userInfoManager;
    public SpineSkinInfoManager spineSkinInfoManager;
    void Awake()
    {
        instance = this;
    }
}

   
