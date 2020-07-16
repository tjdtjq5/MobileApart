using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public UserInfoManager userInfoManager;
    public SpineSkinInfoManager spineSkinInfoManager;
    public DatabaseManager databaseManager;
    public ItemManager itemManager;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        instance = this;
    }
}

   
