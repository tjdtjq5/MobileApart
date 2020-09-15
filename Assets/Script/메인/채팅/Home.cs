using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Home : MonoBehaviour
{
    public GameObject HomeObj;
    public TempCharacter tempCharacter;
    public Button goodBtn;
    public Text goodCountText;


    public void HomeOpen(string nickName ,List<UserSkin> userSkinList)
    {
        HomeObj.SetActive(true);
        tempCharacter.CharacterSet(userSkinList);
        OnClickGood(nickName);
        SetGoodCountText(nickName);
    }

    public void HomeClose()
    {
        HomeObj.SetActive(false);
    }

    void OnClickGood(string nickName)
    {
        goodBtn.onClick.RemoveAllListeners();
        goodBtn.onClick.AddListener(() => {
            BackendAsyncClass.BackendAsync(Backend.Social.GetGamerIndateByNickname, nickName, (callback) =>
            {
                string inDate = callback.GetReturnValuetoJSON()["rows"][0]["inDate"]["S"].ToString();
                BackendAsyncClass.BackendAsync(Backend.GameInfo.GetPublicContentsByGamerIndate, "Profile", inDate, (callback2) =>
                {
                    JsonData json = callback2.GetReturnValuetoJSON()["rows"][0];
                    inDate = json["inDate"]["S"].ToString();

                    if (!json.Keys.Contains("Good"))
                    {
                        Param goodPram = new Param();
                        goodPram.Add("Good", 1);
                        BackendAsyncClass.BackendAsync(Backend.GameInfo.Update, "Profile", inDate, goodPram, (callback3) =>
                        {
                            goodCountText.text = "1";
                        });
                    }
                    else
                    {
                        int goodPlus = int.Parse(json["Good"][0].ToString()) + 1;

                        Param goodPram = new Param();
                        goodPram.Add("Good", goodPlus);
                        BackendAsyncClass.BackendAsync(Backend.GameInfo.Update, "Profile", inDate, goodPram, (callback3) =>
                        {
                            goodCountText.text = goodPlus + "";
                        });
                    }
                });
            });
        });
    }

    void SetGoodCountText(string nickName)
    {
        BackendAsyncClass.BackendAsync(Backend.Social.GetGamerIndateByNickname, nickName, (callback) =>
        {
            string inDate = callback.GetReturnValuetoJSON()["rows"][0]["inDate"]["S"].ToString();
            BackendAsyncClass.BackendAsync(Backend.GameInfo.GetPublicContentsByGamerIndate, "Profile", inDate, (callback2) =>
            {
                JsonData json = callback2.GetReturnValuetoJSON()["rows"][0];
                inDate = json["inDate"]["S"].ToString();

                if (!json.Keys.Contains("Good"))
                {
                    goodCountText.text = 0 + "";
                }
                else
                {
                    int goodPlus = int.Parse(json["Good"][0].ToString());
                    goodCountText.text = goodPlus + "";

                }
            });
        });
    } 
}
