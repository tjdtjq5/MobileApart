using BackEnd;
using BackEnd.Tcp;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackEndChat : MonoBehaviour
{

    [ContextMenu("테스트")]
    public void Test()
    {
        GetChatList();
    }

    // 이벤트 수신 
    // * Backend.Chat.Poll(); *
    // 업데이트문에 넣거나 별도의 쓰레드에서 주기적으로 수신 
    // 30초 이상 수신이 없을 경우 채널 강제로 퇴장 됨 

    //일반채널 리스트 가져오기
    void GetChatList()
    {
        Backend.Chat.GetChannelList((callback) =>
        {
            if (callback.IsSuccess())
            {
                JsonData jsonData = callback.GetReturnValuetoJSON()["rows"];
                for (int i = 0; i < jsonData.Count; i++)
                {
                    Debug.Log(i + "번째 채널");
                    JsonData channelJsonData = jsonData[i];
                    Debug.Log(channelJsonData["uuid"].ToString());
                    Debug.Log(channelJsonData["alias"].ToString());
                    Debug.Log(channelJsonData["registrationDate"].ToString());
                    Debug.Log(channelJsonData["serverHostName"].ToString());
                    Debug.Log(channelJsonData["serverPort"].ToString());
                    Debug.Log(channelJsonData["joinedUserCount"].ToString());
                    Debug.Log(channelJsonData["maxUserCount"].ToString());
                }
            }
            else
            {
                Debug.Log("일반채널 리스트 가져오기 실패 (에러코드)" + callback.GetErrorCode());
            }
        });
    }

    //채널에 입장하기
    void ChannelJoin(string hostName, int port, string uuid)
    {
        ErrorInfo errorInfo = new ErrorInfo();
        Backend.Chat.JoinChannel(ChannelType.Public, hostName, (ushort)port, uuid, out errorInfo);
    }

    //채널 퇴장하기
    void ChannelLeave()
    {
        Backend.Chat.LeaveChannel(ChannelType.Public);
    }

    // 채널에 입장 시 최초 한번, 해당 채널에 접속하고 있는 모든 게이머들의 정보 콜백
    void OnSessionListInChannel()
    {
       
    }

}
