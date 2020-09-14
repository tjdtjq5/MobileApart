using BackEnd;
using BackEnd.Tcp;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    public GameObject chatObj;
    public Transform chatContext;
    public InputField input;
    public Image textColorImg;

    public GameObject textBtn;
    public GameObject characterBtn;

    [Header("말풍선프리팹")]
    public GameObject onChatPrepab;
    public GameObject myChatPrepab;
    public GameObject systemMessage;

    List<SessionInfo> participants = new List<SessionInfo>();
    IEnumerator PollCoroutine;

    bool chatOpenFlag = false;

    private void Start()
    {
        Handler();
    }

    public void ChatOpen()
    {
        chatObj.SetActive(true);

        PollCoroutine = Poll();
        StartCoroutine(PollCoroutine);

        ChannelJoin();

        chatOpenFlag = true;
    }

    public void ChatClose()
    {
        chatObj.SetActive(false);
        StopCoroutine(PollCoroutine);

        ChannelLeave();
        Backend.Chat.Poll();

        chatOpenFlag = false;
    }

    //채팅입력버튼
    public void ChatEnter()
    {
        if (input.text == "")
        {
            return;
        }
        string message = input.text;
        input.text = "";
        MyChatMessage(message);
    }
    // 캐릭터 입력 버튼
    public void CharacterEnter()
    {
        if (input.text != "")
        {
            return;
        }
    }

    IEnumerator Poll()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        while (true)
        {
            Backend.Chat.Poll();
            yield return wait;
        }
    }

    int channelLength = 0;
    void GetChannelLength(System.Action action)
    {
        Backend.Chat.GetChannelList((callback) =>
        {
            if (callback.IsSuccess())
            {
                JsonData jsonData = callback.GetReturnValuetoJSON()["rows"];
                channelLength = jsonData.Count;
                action();
            }
            else
            {
                Debug.Log("일반채널 리스트 가져오기 실패 (에러코드)" + callback.GetErrorCode());
            }
        });
    }

    // 채널입장
    void ChannelJoin()
    {
        Backend.Chat.GetChannelList((callback) =>
        {
            if (callback.IsSuccess())
            {
                JsonData jsonData = callback.GetReturnValuetoJSON()["rows"];
                string hostName = "";
                int port = 0;
                string uuid = "";
                for (int i = 0; i < jsonData.Count; i++)
                {
                    Debug.Log(i + "번째 채널");
                    JsonData channelJsonData = jsonData[i];
                    uuid = channelJsonData["uuid"].ToString();
                    hostName = channelJsonData["serverHostName"].ToString();
                    port = int.Parse(channelJsonData["serverPort"].ToString());
                }

                ErrorInfo errorInfo = new ErrorInfo();
                Backend.Chat.JoinChannel(ChannelType.Public, hostName, (ushort)port, uuid, out errorInfo);
            }
            else
            {
                Debug.Log("일반채널 리스트 가져오기 실패 (에러코드)" + callback.GetErrorCode());
            }
        });
    } 
    void ChannelJoin(int index)
    {
        if (index < 0)
        {
            return;
        }

        GetChannelLength(() => {
            if (index > channelLength - 1)
            {
                Debug.Log("해당 채널이 없습니다.");
            }
            else
            {
                Backend.Chat.GetChannelList((callback) =>
                {
                    if (callback.IsSuccess())
                    {
                        JsonData jsonData = callback.GetReturnValuetoJSON()["rows"];
                        string hostName = "";
                        int port = 0;
                        string uuid = "";
                        JsonData channelJsonData = jsonData[index];
                        uuid = channelJsonData["uuid"].ToString();
                        hostName = channelJsonData["serverHostName"].ToString();
                        port = int.Parse(channelJsonData["serverPort"].ToString());

                        ErrorInfo errorInfo = new ErrorInfo();
                        Backend.Chat.JoinChannel(ChannelType.Public, hostName, (ushort)port, uuid, out errorInfo);
                    }
                    else
                    {
                        Debug.Log("일반채널 리스트 가져오기 실패 (에러코드)" + callback.GetErrorCode());
                    }
                });
            }
        
        
        });
    }

    //채널 퇴장하기
    void ChannelLeave()
    {
        Backend.Chat.LeaveChannel(ChannelType.Public);
    }

    //채팅 메세지 전송
    public void OnChatMessage(string nickName,string message)
    {
        if (message.Split('/').Length == 2)
        {
            string commend = message.Split('/')[0];
            switch (commend)
            {
              
            }
        }

        GameObject prepab = Instantiate(onChatPrepab, Vector3.zero, Quaternion.identity, chatContext);
        prepab.transform.Find("말풍선").Find("닉네임").GetComponent<Text>().text = "[" + nickName + "]";
        prepab.transform.Find("말풍선").Find("메세지").GetComponent<Text>().text = message;

        CheckMessageNumber();
    }
    public void MyChatMessage(string message)
    {
        if (message.Split('/').Length == 2)
        {
            switch (message.Split('/')[0])
            {
                case "운영자":
                    message = message.Split('/')[1];
                    Backend.Chat.ChatToGlobal(message);
                    return;
            }
        }

        Color color = textColorImg.color;
        message = Change_String_Color(message, color);

        Backend.Chat.ChatToChannel(ChannelType.Public, message);

        GameObject prepab = Instantiate(myChatPrepab, Vector3.zero, Quaternion.identity, chatContext);
        prepab.transform.Find("말풍선").Find("닉네임").GetComponent<Text>().text = "[" + GameManager.instance.userInfoManager.nickname + "]";
        prepab.transform.Find("말풍선").Find("메세지").GetComponent<Text>().text = message;

        CheckMessageNumber();
    }
    public void SystemChatMessage(string message)
    {
        GameObject prepab = Instantiate(systemMessage, Vector3.zero, Quaternion.identity, chatContext);
        prepab.transform.Find("말풍선").GetChild(0).GetComponent<Text>().text = message;

        CheckMessageNumber();
    }

    // 말풍선 수 체크 
    void CheckMessageNumber()
    {
        int deleteNumber = 10;
        if (chatContext.childCount > deleteNumber)
        {
            int iCount = 0;
            for (int i = deleteNumber; i < chatContext.childCount; i++)
            {
                Destroy(chatContext.GetChild(iCount).gameObject);
                iCount++;
            }
        }
    }
 


    ///
    //핸들러
    //
    void Handler()
    {
        OnSessionListInChannel();
        OnJoinChannel();
        OnLeaveChannel();
        OnSessionOfflineChannel();
        OnSessionOnlineChannel();
        OnChat();
        OnException();

        OnNotification();
        OnGlobalChat();
    }

    // 채널에 입장 시 최초 한번, 해당 채널에 접속하고 있는 모든 게이머들의 정보 콜백
    void OnSessionListInChannel()
    {
        Backend.Chat.OnSessionListInChannel = (args) =>
        {
            participants.Clear();
            // 게이머 정보를 참여자 리스트에 추가
            foreach (SessionInfo session in args.SessionList)
            {
                participants.Add(session);
            }
        };
    }

    //자기자신 혹은 다른 게이머가 채널에 입장한 경우, 자기자신이 채널에 재접속 한 경우
    void OnJoinChannel()
    {
        Backend.Chat.OnJoinChannel = (args) =>
        {
            if (!participants.Contains(args.Session))
            {
                participants.Add(args.Session);
            }
        };
    }

    //자기자신 혹은 다른 게이머가 채널에서 퇴장한 경우
    void OnLeaveChannel()
    {
        Backend.Chat.OnLeaveChannel = (args) =>
        {
            if (participants.Contains(args.Session))
            {
                participants.Remove(args.Session);
            }
        };
    }

    // 자기자신 혹은 다른게이머가 채팅 채널과 접속이 일시적으로 끊어진 경우
    void OnSessionOfflineChannel()
    {
        Backend.Chat.OnSessionOfflineChannel = (args) =>
        {
            if (participants.Contains(args.Session))
            {
                participants.Remove(args.Session);
            }
        };
    }
    // 다른게이머가 채팅 채널에 재접속 한 경우
    void OnSessionOnlineChannel()
    {
        Backend.Chat.OnSessionOnlineChannel = (args) =>
        {
            if (!participants.Contains(args.Session))
            {
                participants.Add(args.Session);
            }
        };
    }
    //같은 채널의 게이머들이 전송한 메시지가 도착한 경우
    void OnChat()
    {
        Backend.Chat.OnChat = (args) =>
        {
            if (args.From.NickName == GameManager.instance.userInfoManager.nickname)
            {
                return;
            }
            OnChatMessage(args.From.NickName, args.Message);
        };
    }
    // 채팅 관련 내부 기능에 예외가 발생한 경우
    void OnException()
    {
        Backend.Chat.OnException = (args) =>
        {

        };
    }

    // 공지사항 받기
    void OnNotification()
    {
        Backend.Chat.OnNotification = (NotificationEventArgs args) =>
        {
            string subject = args.Subject;
            string messge = args.Message;

            this.transform.Find("채팅창").Find("공지사항").GetChild(0).GetChild(0).GetComponent<Text>().text = "[" + subject + "] " + messge;
        };
    }

    //운영자 공지 받기
    void OnGlobalChat()
    {
        Backend.Chat.OnGlobalChat = (GlobalChatEventArgs args) =>
        {
            SessionInfo from = args.From; // 보낸 사람의 정보
            string message = args.Message; // 공지 메세지 정보
            SystemChatMessage("[" + from.NickName + "] " + message);
        };
    }

    /// 기타 작업 
    /// 

    public string Change_String_Color(string ThisString, Color ThisColor)
    {
        string TextToReturn = "<color=#[Color_Code]>[Insert_HERE]</color>";
        TextToReturn = TextToReturn.Replace("[Color_Code]", UnityEngine.ColorUtility.ToHtmlStringRGBA(ThisColor));
        TextToReturn = TextToReturn.Replace("[Insert_HERE]", ThisString);
        return TextToReturn;
    }


    private void Update()
    {
        if (chatOpenFlag)
        {
            if (input.text == "")
            {
                textBtn.SetActive(false);
                characterBtn.SetActive(true);
            }
            else
            {
                textBtn.SetActive(true);
                characterBtn.SetActive(false);
            }
        }
    }
}
