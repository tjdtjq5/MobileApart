using BackEnd;
using BackEnd.Tcp;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    public GameObject chatObj;
    public Transform chatContext;

    [Header("말풍선프리팹")]
    public GameObject onChatPrepab;
    public GameObject myChatPrepab;
    public GameObject systemMessage;

    List<SessionInfo> participants = new List<SessionInfo>();
    IEnumerator PollCoroutine;

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
    }

    public void ChatClose()
    {
        chatObj.SetActive(false);
        StopCoroutine(PollCoroutine);

        ChannelLeave();
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
    public void OnChatMessage()
    {

    }
    public void MyChatMessage(string message)
    {
        Backend.Chat.ChatToChannel(ChannelType.Public, message);

        GameObject prepab = Instantiate(myChatPrepab, Vector3.zero, Quaternion.identity, chatContext);
        prepab.transform.Find("말풍선").Find("닉네임").GetComponent<Text>().text = "[" + GameManager.instance.userInfoManager.nickname + "]";
        prepab.transform.Find("말풍선").Find("메세지").GetComponent<Text>().text = message;

        CheckMessageNumber();
    }
    public void SystemChatMessage(string message)
    {

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
            // 참여자 목록 출력
            for (int i = 0; i < participants.Count; i++)
            {
                Debug.Log("닉네임 : " + participants[i].NickName);
            }
        };
    }

    //자기자신 혹은 다른 게이머가 채널에 입장한 경우, 자기자신이 채널에 재접속 한 경우
    void OnJoinChannel()
    {
        Backend.Chat.OnJoinChannel = (args) =>
        {
            participants.Add(args.Session);
        };
    }

    //자기자신 혹은 다른 게이머가 채널에서 퇴장한 경우
    void OnLeaveChannel()
    {
        Backend.Chat.OnLeaveChannel = (args) =>
        {
            participants.Remove(args.Session);
        };
    }

    // 자기자신 혹은 다른게이머가 채팅 채널과 접속이 일시적으로 끊어진 경우
    void OnSessionOfflineChannel()
    {
        Backend.Chat.OnSessionOfflineChannel = (args) =>
        {
            participants.Remove(args.Session);
        };
    }
    // 다른게이머가 채팅 채널에 재접속 한 경우
    void OnSessionOnlineChannel()
    {
        Backend.Chat.OnSessionOnlineChannel = (args) =>
        {
            participants.Add(args.Session);
        };
    }
    //같은 채널의 게이머들이 전송한 메시지가 도착한 경우
    void OnChat()
    {
        Backend.Chat.OnChat = (args) =>
        {

        };
    }
    // 채팅 관련 내부 기능에 예외가 발생한 경우
    void OnException()
    {
        Backend.Chat.OnException = (args) =>
        {

        };
    }
}
