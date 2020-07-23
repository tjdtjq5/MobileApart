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

    public GameObject alramPannel;
    public Text oderInfo;

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

    /*
[CharacterData]
장비하고 있는 옷 
스태미너  지능 ... 
호감도 
기분 
스토리 진행상황 
케릭터 상태 청결 등 ... 
     */


    /*
     * [Login씬]
처음 시작 - 회원가입 로그인 -> 닉네임 생성 저장 -> 초기 스킨 아이템 지급 -> characterSelect씬으로 이동 
이후 시작 - 로그인 -> CurrentCharacter정보가 있는지 확인 -> 있다면 [CharacterData] 정보 로드-> Loding 씬으로 이동 
                                                         -> 없다면 characterSelect씬으로 이동 
     */


    // 닉네임 정보 받아오기 순서 1 
    public void AsyncGetUserInfo()
    {
        alramPannel.SetActive(true);
        
        oderInfo.text = "닉네임 받아오는 중 ...";
        BackendAsyncClass.BackendAsync(Backend.BMember.GetUserInfo, (callback) =>
        {
            string[] userData = callback.GetReturnValue().Split('"');
            string inDate = userData[7];
            string nickname = userData[4];
            GameManager.instance.userInfoManager.inDate = inDate;

            GameManager.instance.userInfoManager.Initialized(); // 첫 시작이든 아니든 일단 초기정보 넣기 

            Debug.Log("닉네임 및 inDate 받아오기 완료");
            if (nickname == ":null,") // 닉네임이 안정해져 있을 경우 , 첫 시작 
            {
                NicknameSet();  // 닉네임 정보 저장 
                Param nicknameData = new Param();
                nicknameData.Add("nickname", idInput.text);
                BackEndGameInfo.instance.InsertData("UserInfo", nicknameData); //private테이블 생성 
                GameManager.instance.userInfoManager.nickname = idInput.text;

                oderInfo.text = "초기 스킨 아이템 저장중 ...";
                //초기 스킨 아이템 저장
                GameManager.instance.userInfoManager.SaveSkinItem(() => {
                    // 그 다음 돈  저장 
                    GameManager.instance.userInfoManager.SetUserMoney(MoneyKind.Crystal, 1000000);
                    GameManager.instance.userInfoManager.SetUserMoney(MoneyKind.Gold, 1000000);
                    oderInfo.text = "초기 돈 정보 저장중 ...";
                    GameManager.instance.userInfoManager.SaveUserMoney(() => SceneManager.LoadScene("CaracterSelect"));
                });

                //초기 돈 
            
            }
            else // 닉네임이 있을 경우 , 이후시작
            {
                GameManager.instance.userInfoManager.nickname = idInput.text;

                oderInfo.text = "기존 유저의 돈 정보 받아오는 중 ...";
                // 돈 불러오기 
                GameManager.instance.userInfoManager.LoadUserMoney(() =>
                {
                    oderInfo.text = "기존 유저의 스킨 정보 받아오는 중 ...";
                    // 스킨아이템 불러오기 
                    GameManager.instance.userInfoManager.LoadSkinItem(() => {

                        oderInfo.text = "현재 캐릭터 정보 받아오는중 ...";
                        BackendAsyncClass.BackendAsync(Backend.GameInfo.GetPrivateContents, "UserInfo", (callback2) =>
                        {
                            // 이후 처리
                            JsonData jsonData = callback2.GetReturnValuetoJSON()["rows"][0];
                            if (jsonData.Keys.Contains("CurrentCharacter")) // CurrentCharacter 정보가 존재 한다면 
                            {
                                //현재 캐릭터 불러오기
                                string temp = jsonData["inDate"]["S"].ToString();
                                string currentCharacter = jsonData["CurrentCharacter"][0].ToString();
                                GameManager.instance.userInfoManager.currentCharacter = currentCharacter;

                                oderInfo.text = "캐릭터의 장비정보 로드 ...";
                                // 그 캐릭터의 장비정보 로드 
                                GameManager.instance.userInfoManager.LoadUserEqip(currentCharacter, () => {
                                    oderInfo.text = "캐릭터의 스테이터스 로드 ...";
                                    // 그 캐릭터의 스테이터스 로드
                                    GameManager.instance.userInfoManager.LoadUserStatus(currentCharacter, () =>
                                    {
                                        oderInfo.text = "캐릭터의 욕구정보 로드 ...";
                                        // 그 캐릭터의 욕구 로드 
                                        GameManager.instance.userInfoManager.LoadUserNeed(currentCharacter, () =>
                                        {
                                            Debug.Log("성공");
                                            SceneManager.LoadScene("Loding");
                                        });
                                    }); 
                                });
                            }
                            else
                            {
                                SceneManager.LoadScene("CaracterSelect");// CurrentCharacter 정보가 없다면 
                            }
                        });
                    });

                });
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
