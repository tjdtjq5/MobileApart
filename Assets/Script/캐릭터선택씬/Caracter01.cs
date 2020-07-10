using Spine;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using UnityEngine.SceneManagement;
using LitJson;

/*
 * [characterSelect씬]
케릭터 선택 버튼 -> currentCharacter정보 업데이트 -> [CharacterData] 정보가 있는지 확인 (key가 있는지 확인) -> 있다면  [CharacterData] 정보 로드-> Loding 씬으로 이동 
                                                                                                            -> 없다면  [CharacterData] 정보 초기정보 저장 및 로드 -> Loding씬으로 이동 
 */

public class Caracter01 : MonoBehaviour
{
    SkeletonAnimation skeletonAnimation;
    List<string> skinList = new List<string>();

    private void Start()
    {
        skeletonAnimation = transform.GetChild(0).GetComponent<SkeletonAnimation>();

        body("body");
        eye("eye/eye_01");
        face("face/face_01");
        haF("haF/hair_f_01");
        haB("haB/hair_b_01");
        pan("pan/clo_under_01");
        sho("sho/shoes_01");
        top("top/clo_top_01");
    }

    public void SelectCaracter01()
    {
        Param currentCharacterData = new Param();
        currentCharacterData.Add("CurrentCharacter", "Caracter01");

        BackendAsyncClass.BackendAsync(Backend.GameInfo.GetPrivateContents, "UserInfo", (callback) =>
        {
            // 이후 처리
            JsonData jsonData = callback.GetReturnValuetoJSON()["rows"][0];
            string dataIndate = jsonData["inDate"]["S"].ToString();

            BackendAsyncClass.BackendAsync(Backend.GameInfo.Update, "UserInfo", dataIndate, currentCharacterData, (callback2) =>
            {
                // 이후 처리
                BackendAsyncClass.BackendAsync(Backend.GameInfo.GetPrivateContents, "UserInfo", (callback3) =>
                {
                    // 이후 처리
                    JsonData jsonData2 = callback3.GetReturnValuetoJSON()["rows"][0];
                    if (jsonData2.Keys.Contains("Caracter01" + "Eqip"))
                    {

                        GameManager.instance.userInfoManager.LoadUserEqip("Caracter01", ()=> SceneManager.LoadScene("Loding"));
                    }
                    else
                    {
                        GameManager.instance.userInfoManager.SaveUserEqip("Caracter01", () => SceneManager.LoadScene("Loding"));
                    }
                });

            });
        });
    }

    void body(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains("body"))
            {
                skinList.RemoveAt(i);
            }
        }
        skinList.Add(skinName);
        SetEquip(skinList);
    }
    void acc(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains("acc"))
            {
                skinList.RemoveAt(i);
            }
        }
        skinList.Add(skinName);
        SetEquip(skinList);
    }

    void top(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains("top"))
            {
                skinList.RemoveAt(i);
            }
        }
        skinList.Add(skinName);
        SetEquip(skinList);

    }

    void pan(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains("pan"))
            {
                skinList.RemoveAt(i);
            }
        }
        skinList.Add(skinName);
        SetEquip(skinList);
    }

    void eye(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains("eye"))
            {
                skinList.RemoveAt(i);
            }
        }
        skinList.Add(skinName);
        SetEquip(skinList);

    }

    void face(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains("face"))
            {
                skinList.RemoveAt(i);
            }
        }
        skinList.Add(skinName);
        SetEquip(skinList);

    }
    void haF(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains("haF"))
            {
                skinList.RemoveAt(i);
            }
        }
        skinList.Add(skinName);
        SetEquip(skinList);

    }
    void haB(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains("haB"))
            {
                skinList.RemoveAt(i);
            }
        }
        skinList.Add(skinName);
        SetEquip(skinList);

    }
    void outt(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains("outt"))
            {
                skinList.RemoveAt(i);
            }
        }
        skinList.Add(skinName);
        SetEquip(skinList);

    }
    void sho(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains("sho"))
            {
                skinList.RemoveAt(i);
            }
        }
        skinList.Add(skinName);
        SetEquip(skinList);
    }

    void cap(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains("cap"))
            {
                skinList.RemoveAt(i);
            }
        }
        skinList.Add(skinName);
        SetEquip(skinList);
    }

    void set(string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains("set"))
            {
                skinList.RemoveAt(i);
            }
        }
        skinList.Add(skinName);
        SetEquip(skinList);
    }

    public void SetEquip(List<string> SkinList)
    {
        Skin combined = new Skin("combined");

        foreach (var skinName in SkinList)
        {
            Skin skin = skeletonAnimation.skeleton.Data.FindSkin(skinName);

            if (skin != null)
            {
                combined.AddFromSkin(skin);
            }
        }

        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.skeleton.SetSkin(combined);
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();
    }
}
