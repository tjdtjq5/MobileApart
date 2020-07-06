using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

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
        }

        else
        {
            BackEndManager.MyInstance.ShowErrorUI(backendReturnObject);
        }

        Debug.Log("동기 방식============================================= ");
    }

    BackendReturnObject bro = new BackendReturnObject();
    bool isSuccess = false;

    void Update()
    {
        if (isSuccess)
        {
            // SaveToken( BackendReturnObject bro ) -> void
            // 비동기 메소드는 update()문에서 SaveToken을 꼭 적용해야 합니다.
            Backend.BMember.SaveToken(bro);
            isSuccess = false;
            bro.Clear();
        }
    }

    // 회원가입2 - 비동기 방식
    public void OnClickSignUp2()
    {
        Backend.BMember.CustomSignUp(idInput.text, paInput.text, "Test2", (callback) =>
        {
            isSuccess = callback.IsSuccess();
            bro = callback;

            if (isSuccess == true)
            {
                Debug.Log("[비동기방식] 회원가입 완료");
            }
            else
            {
                BackEndManager.MyInstance.ShowErrorUI(callback);
            }
        });

        Debug.Log("비동기 방식============================================= ");
    }

    // 로그인2 - 비동기 방식
    public void OnClickLogin2()
    {
        Backend.BMember.CustomLogin(idInput.text, paInput.text, (callback) =>
        {
            isSuccess = callback.IsSuccess();
            bro = callback;

            if (isSuccess == true)
            {
                Debug.Log("[비동기방식] 로그인 완료");
            }
            else
            {
                BackEndManager.MyInstance.ShowErrorUI(bro);
            }
        });
        Debug.Log("비동기 방식============================================= ");
    }


    // 자동 로그인 - 동기방식
    public void AutoLogin1()
    {
        BackendReturnObject backendReturnObject = Backend.BMember.LoginWithTheBackendToken();

        if (backendReturnObject.IsSuccess() == true)
        {
            Debug.Log("[동기방식] 자동로그인 완료");
        }
        else
        {
            BackEndManager.MyInstance.ShowErrorUI(backendReturnObject);
        }


        Debug.Log("동기 방식============================================= ");
    }

    // 자동 로그인 - 비동기방식
    public void AutoLogin2()
    {
        Backend.BMember.LoginWithTheBackendToken((callback) =>
        {
            isSuccess = callback.IsSuccess();
            bro = callback;

            if (isSuccess == true)
            {
                Debug.Log("[비동기방식] 자동로그인 완료");
            }
            else
            {
                BackEndManager.MyInstance.ShowErrorUI(bro);
            }

        });
    }
}
