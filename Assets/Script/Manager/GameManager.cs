using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public UserInfoManager userInfoManager;
    public SpineSkinInfoManager spineSkinInfoManager;
    public DatabaseManager databaseManager;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        instance = this;
        Screen.SetResolution(720, 1280, true);
    }

  

}

   
