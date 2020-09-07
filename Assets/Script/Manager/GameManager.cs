using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public UserInfoManager userInfoManager;
    public DatabaseManager databaseManager;
    public ItemManager itemManager;
    public StageManager stageManager;
    public MonsterManager monsterManager;
    public WeaponeManager weaponeManager;
    public ScriptManager scriptManager;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        instance = this;
    }
}

   
