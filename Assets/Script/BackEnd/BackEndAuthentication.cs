using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using UnityEngine.SceneManagement;
using LitJson;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

public class BackEndAuthentication : MonoBehaviour
{
    public InputField idInput;
    public InputField paInput;

    // 회원가입1 - 동기 방식
    public void OnClickSignUp1()
    {
        if (!CheckNickname())
        {
            return; 
        }

        // 회원 가입을 한뒤 결과를 BackEndReturnObject 타입으로 반환한다.
        BackendReturnObject backendReturnObject = Backend.BMember.CustomSignUp(idInput.text, paInput.text, "Test1");

        if (backendReturnObject.IsSuccess() == true)
        {
            Debug.Log("[동기방식] 회원 가입 완료");
        }

        else
        {
            BackEndManager.MyInstance.ShowErrorUI(backendReturnObject);
        }

        Debug.Log("동기 방식============================================= ");
    }

    // 로그인1 - 동기 방식
    public void OnClickLogin1()
    {
        BackendReturnObject backendReturnObject = Backend.BMember.CustomLogin(idInput.text, paInput.text);

        if (backendReturnObject.IsSuccess() == true)
        {
            Debug.Log("[동기방식] 로그인 완료");
            // SceneManager.LoadScene("Loding");


            AsyncGetUserInfo();


        }

        else
        {
            BackEndManager.MyInstance.ShowErrorUI(backendReturnObject);
        }

        Debug.Log("동기 방식============================================= ");
    }


    // 닉네임 정보 받아오기 순서 1 
    public void AsyncGetUserInfo()
    {
        BackendAsyncClass.BackendAsync(Backend.BMember.GetUserInfo, (callback) =>
        {
            Debug.Log(callback.GetReturnValue());
            string[] userData = callback.GetReturnValue().Split('"');
            string inDate = userData[7];
            string nickname = userData[4];
            GameManager.instance.userInfoManager.inDate = inDate;

            Debug.Log("닉네임 및 inDate 받아오기 완료");
            if (nickname == ":null,") // 닉네임이 안정해져 있을 경우
            {
                NicknameSet();
                Param nicknameData = new Param();
                nicknameData.Add("nickname", idInput.text);
                BackEndGameInfo.instance.InsertData("UserInfo", nicknameData);
                GameManager.instance.userInfoManager.nickname = idInput.text;

                GameManager.instance.userInfoManager.Initialized();
                GameManager.instance.userInfoManager.SaveSkinItem();
                GameManager.instance.userInfoManager.SaveUserEqip();

                SceneManager.LoadScene("CaracterSelect");
            }
            else // 닉네임이 있을 경우 
            {
                GameManager.instance.userInfoManager.nickname = callback.GetReturnValuetoJSON()["row"]["nickname"].ToString();

                GameManager.instance.userInfoManager.LoadSkinItem();
                GameManager.instance.userInfoManager.LoadUserEqip();

                SceneManager.LoadScene("Loding");
            }
        });
    }

    private bool CheckNickname()
    {
        return Regex.IsMatch(idInput.text, "^[0-9a-zA-Z가-힣]*$");
    }

    public void NicknameSet()
    {
        BackendReturnObject BRO = Backend.BMember.CreateNickname(idInput.text);

        if (BRO.IsSuccess())
        {
            Debug.Log("닉네임 생성 완료");
 
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
}
