using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using BackEnd;

public class NicknameSet : MonoBehaviour
{
    public GameObject nickPannel;
    public InputField nicknameField;

    void Awake()
    {
        if (GameManager.instance.userInfoManager.nickname == "")
        {
            nickPannel.SetActive(true);

            GameManager.instance.userInfoManager.Initialized();
        }
    }



    public void NicknameSettingBtn()
    {
        if (nicknameField.text == "" && !CheckNickname())
        {
            return;
        }


        BackendReturnObject BRO = Backend.BMember.CreateNickname(nicknameField.text);

        if (BRO.IsSuccess())
        {
            Debug.Log("닉네임 생성 완료");

            GameManager.instance.userInfoManager.SaveSkinItem();
            GameManager.instance.userInfoManager.SaveUserEqip();

            nickPannel.SetActive(false);
        }

        else
        {
            switch (BRO.GetStatusCode())
            {
                case "409":
                    Debug.Log("이미 중복된 닉네임이 있는 경우");
                    break;

                case "400":
                    if (BRO.GetMessage().Contains("too long")) Debug.Log("20자 이상의 닉네임인 경우");
                    else if (BRO.GetMessage().Contains("blank")) Debug.Log("닉네임에 앞/뒤 공백이 있는경우");
                    break;

                default:
                    Debug.Log("서버 공통 에러 발생: " + BRO.GetErrorCode());
                    break;
            }
        }
    }

    private bool CheckNickname()
    {
        return Regex.IsMatch(nicknameField.text, "^[0-9a-zA-Z가-힣]*$");
    }
}
