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

public class Character01 : MonoBehaviour
{
    SkeletonAnimation skeletonAnimation;
    List<string> skinList = new List<string>();

    private void Start()
    {
        skeletonAnimation = transform.GetChild(0).GetComponent<SkeletonAnimation>();

        SkinChange(SkinKind.Body, "Body");
        SkinChange(SkinKind.eye, "eye/eye_01");
        SkinChange(SkinKind.face, "face/face_01");
        SkinChange(SkinKind.haF, "haF/hair_f_01");
        SkinChange(SkinKind.haB, "haB/hair_b_01");
        SkinChange(SkinKind.pan, "pan/clo_under_01");
        SkinChange(SkinKind.sho, "sho/shoes_01");
        SkinChange(SkinKind.top, "top/clo_top_01");
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

            // 캐릭터 01 정보 저장 
            BackendAsyncClass.BackendAsync(Backend.GameInfo.Update, "UserInfo", dataIndate, currentCharacterData, (callback2) =>
            {
                // 이후 처리
                BackendAsyncClass.BackendAsync(Backend.GameInfo.GetPrivateContents, "UserInfo", (callback3) =>
                {
                    // 정보 로드 : 캐릭터 셀렉을 선택했던 사람
                    JsonData jsonData2 = callback3.GetReturnValuetoJSON()["rows"][0];
                    if (jsonData2.Keys.Contains("Caracter01" + "Eqip"))
                    {
                        GameManager.instance.userInfoManager.LoadUserEqip("Caracter01", ()=> {
                            GameManager.instance.userInfoManager.LoadUserNeed("Caracter01", () => {
                                SceneManager.LoadScene("Loding");
                            });
                        });
                    }
                    else
                    {
                        // 정보 로드 : 캐릭터 셀렉을 선택하지 않고 나갔던 사람 + 처음 
                        GameManager.instance.userInfoManager.SaveUserEqip("Caracter01", () => {
                            GameManager.instance.userInfoManager.SaveUserNeed("Caracter01", () => {
                                SceneManager.LoadScene("Loding");
                            });
                        });
                    }
                });

            });
        });
    }
    public void SkinChange(SkinKind skinKind, string skinName)
    {
        skeletonAnimation.skeleton.Skin = null;
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.LateUpdate();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].Contains(skinKind.ToString()))
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
