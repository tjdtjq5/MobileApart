using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public UserInfoManager userInfoManager;
    public SpineSkinInfoManager spineSkinInfoManager;
    public DatabaseManager databaseManager;
    public IconManager iconManager;
    void Awake()
    {
        instance = this;
        Screen.SetResolution(720, 1280, true);
    }

}

   
