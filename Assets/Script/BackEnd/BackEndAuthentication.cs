using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using UnityEngine.SceneManagement;
using LitJson;
using System.Runtime.CompilerServices;

public class BackEndAuthentication : MonoBehaviour
{
    public InputField idInput;
    public InputField paInput;

    // 회원가입1 - 동기 방식
    public void OnClickSignUp1()
    {
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
                GameManager.instance.userInfoManager.nickname = "";
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


}
