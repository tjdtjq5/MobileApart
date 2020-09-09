using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public UserInfoManager userInfoManager;
    public DatabaseManager databaseManager;
    public ItemManager itemManager;
    public ScriptManager scriptManager;
    public SetManager setManager;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        instance = this;

        PlayerPrefs.DeleteAll();
    }

    
    
}

   
