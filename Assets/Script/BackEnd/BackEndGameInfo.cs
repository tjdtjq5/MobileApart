using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

public class BackEndGameInfo : MonoBehaviour
{

    public static BackEndGameInfo instance;

    private void Awake()
    {
        instance = this;
    }

    public void OnClickTest()
    {
        List<string> userNickname = GetMyRank("82bca7d0-a949-11ea-b69a-31bb9942f2b1", "rank", 1);

        for (int i = 0; i < userNickname.Count; i++)
        {
            Debug.Log("유저 닉네임 : " + userNickname[i]);
        }
    }

    string key = "";
    List<string> outputDataList = new List<string>();

    // ===== 데이터 주고받기 ===== // 
    // Insert 는 '생성' 작업에 주로 사용된다. 
    public void InsertData(string table, Param param)
    {
        BackendReturnObject BRO = Backend.GameInfo.Insert(table, param);

        if (BRO.IsSuccess())
        {
            Debug.Log("indate : " + BRO.GetInDate());
        }
        else
        {
            switch (BRO.GetStatusCode())
            {
                case "404":
                    Debug.Log("존재하지 않는 tableName인 경우");
                    break;

                case "412":
                    Debug.Log("비활성화 된 tableName인 경우");
                    break;

                case "413":
                    Debug.Log("하나의 row( column들의 집합 )이 400KB를 넘는 경우");
                    break;

                default:
                    Debug.Log("서버 공통 에러 발생: " + BRO.GetMessage());
                    break;
            }
        }
    }

    public List<string> GetPublicDataList(string table, string nickname, string key)
    {
        outputDataList.Clear();
        this.key = key;
        GetPublicContentsByGamerIndate(table, nickname);
        return outputDataList;
    }

    // 공개 테이블에서 특정 유저의 정보 불러오기 
    public void GetPublicContentsByGamerIndate(string table, string nickname)
    {
        // 해당 유저의 닉네임으로 Indate 값을 불러올 수 있습니다. 
        // https://developer.thebackend.io/unity3d/guide/social/getuser/ 페이지에서 특정 유저의 Indate 값 불러오기 참고
        BackendReturnObject BRO = Backend.GameInfo.GetPublicContentsByGamerIndate(table, GetGamerIndateByNickname(nickname));

        if (BRO.IsSuccess())
        {
            GetGameInfo(BRO.GetReturnValuetoJSON());
        }
        else
        {
            CheckError(BRO);
        }
    }

    // 특정 닉네임으로 유저의 indates를 받아옴
    string GetGamerIndateByNickname(string nickname)
    {
        BackendReturnObject BRO = Backend.Social.GetGamerIndateByNickname(nickname);

        if (BRO.IsSuccess())
        {
            JsonData data = BRO.GetReturnValuetoJSON();
            for (int i = 0; i < data["rows"].Count; i++)
            {
                string indate = data["rows"][i]["inDate"]["S"].ToString();
                return indate;
            }
        }

        else
        {
            Debug.Log("서버 공통 에러 발생: " + BRO.GetMessage());
        }

        return "1";
    }

    void GetGameInfo(JsonData returnData)
    {
        // ReturnValue가 존재하고, 데이터가 있는지 확인
        if (returnData != null)
        {
            Debug.Log("데이터가 존재합니다.");

            // rows 로 전달받은 경우 
            if (returnData.Keys.Contains("rows"))
            {
                JsonData rows = returnData["rows"];
                for (int i = 0; i < rows.Count; i++)
                {
                    GetData(rows[i]);
                }
            }

            // row 로 전달받은 경우
            else if (returnData.Keys.Contains("row"))
            {
                JsonData row = returnData["row"];
                GetData(row[0]);
            }
        }
        else
        {
            Debug.Log("데이터가 없습니다.");
        }
    }


    // json parsing 
    void GetData(JsonData data)
    {
        if (!data.Keys.Contains(key))
        {
            Debug.Log("key값이 잘못 되었거나 없습니다.");
            return;
        }
        string str = data[key][0].ToString();
        if (str.Contains("array"))
        {
            string array_data = "";
            for (int i = 0; i < data[key][0].Count; i++)
            {
                array_data += data[key][0][i][0].ToString() + "/";
            }
            outputDataList.Add(array_data);
        }
        else
        {
            outputDataList.Add(str);
        }
    }

    // 게임 정보 읽기 관련 에러처리를
    // 하나의 메소드로 묶었습니다. (내용을 이해하시는데 어려움이 있을거같아)
    void CheckError(BackendReturnObject BRO)
    {
        switch (BRO.GetStatusCode())
        {
            case "200":
                Debug.Log("해당 유저의 데이터가 테이블에 없습니다.");
                break;

            case "404":
                if (BRO.GetMessage().Contains("gamer not found"))
                {
                    Debug.Log("gamerIndate가 존재하지 gamer의 indate인 경우");
                }
                else if (BRO.GetMessage().Contains("table not found"))
                {
                    Debug.Log("존재하지 않는 테이블");
                }
                break;

            case "400":
                if (BRO.GetMessage().Contains("bad limit"))
                {
                    Debug.Log("limit 값이 100이상인 경우");
                }

                else if (BRO.GetMessage().Contains("bad table"))
                {
                    // public Table 정보를 얻는 코드로 private Table 에 접근했을 때 또는
                    // private Table 정보를 얻는 코드로 public Table 에 접근했을 때 
                    Debug.Log("요청한 코드와 테이블의 공개여부가 맞지 않습니다.");
                }
                break;

            case "412":
                Debug.Log("비활성화된 테이블입니다.");
                break;

            default:
                Debug.Log("서버 공통 에러 발생: " + BRO.GetMessage());
                break;

        }
    }
    public void GamePublicInfoUpdate(string table, string nickname, Param param)
    {
        BackendReturnObject BRO = Backend.GameInfo.Update(table, GetPublicDataList(table, nickname, "inDate")[0], param);
        if (BRO.IsSuccess())
        {
            Debug.Log("수정 완료");
        }
        else
        {
            switch (BRO.GetStatusCode())
            {
                case "405":
                    Debug.Log("param에 partition, gamer_id, inDate, updatedAt 네가지 필드가 있는 경우");
                    break;

                case "403":
                    Debug.Log("퍼블릭테이블의 타인정보를 수정하고자 하였을 경우");
                    break;

                case "404":
                    Debug.Log("존재하지 않는 tableName인 경우");
                    break;

                case "412":
                    Debug.Log("비활성화 된 tableName인 경우");
                    break;

                case "413":
                    Debug.Log("하나의 row( column들의 집합 )이 400KB를 넘는 경우");
                    break;
            }
        }
    }
    public void GamePrivateInfoUpdate(string table, Param param)
    {
        BackendReturnObject BRO = Backend.GameInfo.Update(table, GetPrivateContents(table, "inDate")[0], param);
        if (BRO.IsSuccess())
        {
            Debug.Log("수정 완료");
        }
        else
        {
            switch (BRO.GetStatusCode())
            {
                case "405":
                    Debug.Log("param에 partition, gamer_id, inDate, updatedAt 네가지 필드가 있는 경우");
                    break;

                case "403":
                    Debug.Log("퍼블릭테이블의 타인정보를 수정하고자 하였을 경우");
                    break;

                case "404":
                    Debug.Log("존재하지 않는 tableName인 경우");
                    break;

                case "412":
                    Debug.Log("비활성화 된 tableName인 경우");
                    break;

                case "413":
                    Debug.Log("하나의 row( column들의 집합 )이 400KB를 넘는 경우");
                    break;
            }
        }
    }

    // 비공개 테이블에서 본인 정보 가져오기
    public List<string> GetPrivateContents(string table, string key)
    {
        outputDataList.Clear();
        this.key = key;
        BackendReturnObject BRO = Backend.GameInfo.GetPrivateContents(table);

        if (BRO.IsSuccess())
        {
            GetGameInfo(BRO.GetReturnValuetoJSON());
        }
        else
        {
            CheckError(BRO);
        }

        return outputDataList;
    }

    // ===== 랭킹 ===== // 

    public void RankingRegist(string table, string key, long score)
    {
        BackendReturnObject BRO = Backend.GameInfo.UpdateRTRankTable(table, key, score, GetPrivateContents(table, "inDate")[0]);
        if (BRO.IsSuccess())
        {
            Debug.Log("수정 완료");
        }
        else
        {
            switch (BRO.GetStatusCode())
            {
                case "403":
                    Debug.Log("콘솔에서 실시간 랭킹을 활성화 하지 않고 갱신 요청을 한 경우");
                    Debug.Log("퍼블릭테이블의 타인정보를 수정하고자 하였을 경우");
                    break;

                case "400":
                    Debug.Log("콘솔에서 실시간 랭킹을 생성하지 않고 갱신 요청을 한 경우");
                    Debug.Log("콘솔에서 Public 테이블로 실시간 랭킹을 생성한 경우");
                    Debug.Log("테이블 명 혹은 colum명이 존재하지 않는 경우");
                    break;
                case "428":
                    Debug.Log("한국시간(UTC+9) 4시 ~ 5시 사이에 실시간 랭킹 갱신 요청을 한 경우");
                    break;
                case "412":
                    Debug.Log("비활성화 된 tableName인 경우");
                    break;
                case "404":
                    Debug.Log("존재하지 않는 tableName인 경우");
                    break;
            }
        }
    }

    // key : gamer_id , gamerInDate, nickname, score, rank
    public List<string> RankList(string uuid, string key, int start = -1, int end = -1)
    {
        outputDataList.Clear();
        this.key = key;
        BackendReturnObject BRO = null;
        if (start == -1)
        {
            // (default) 상위 10명 랭킹 정보 조회 (1-10)
            BRO = Backend.RTRank.GetRTRankByUuid(uuid);
        }
        if (start != -1 && end == -1)
        {
            // 상위 2명 랭킹 정보 조회 (1-2)
            BRO = Backend.RTRank.GetRTRankByUuid(uuid, start);
        }
        if (start != -1 && end != -1)
        {
            // 처음 2명 이후의 상위 5명 랭킹 정보 정보 (3-7)
            BRO = Backend.RTRank.GetRTRankByUuid(uuid, start, end);
        }

        if (BRO == null && !BRO.IsSuccess())
        {
            switch (BRO.GetErrorCode())
            {
                case "200":
                    Debug.Log("랭킹이 없는 경우");
                    break;
                case "404":
                    Debug.Log("랭킹 Uuid가 틀린 경우");
                    break;
            }
        }
        else
        {
            GetRankingGameInfo(BRO.GetReturnValuetoJSON());
            return outputDataList;
        }
        return null;
    }

    public List<string> GetMyRank(string uuid, string key, int gap = 0)
    {
        outputDataList.Clear();
        this.key = key;
        BackendReturnObject BRO = null;

        if (gap == 0)
        {
            BRO = Backend.RTRank.GetMyRTRank(uuid);
        }
        else
        {
            BRO = Backend.RTRank.GetMyRTRank(uuid, gap);
        }

        if (BRO == null && !BRO.IsSuccess())
        {
            switch (BRO.GetErrorCode())
            {
                case "200":
                    Debug.Log("랭킹이 없는 경우");
                    break;
                case "404":
                    Debug.Log("랭킹 Uuid가 틀린 경우");
                    break;
            }
        }
        else
        {
            GetRankingGameInfo(BRO.GetReturnValuetoJSON());
            return outputDataList;
        }
        return null;
    }

    void GetRankingGameInfo(JsonData returnData)
    {
        // ReturnValue가 존재하고, 데이터가 있는지 확인
        if (returnData != null)
        {
            Debug.Log("데이터가 존재합니다.");

            // rows 로 전달받은 경우 
            if (returnData.Keys.Contains("rows"))
            {
                JsonData rows = returnData["rows"];
                for (int i = 0; i < rows.Count; i++)
                {
                    GetRankingData(rows[i]);
                }
            }

            // row 로 전달받은 경우
            else if (returnData.Keys.Contains("row"))
            {
                JsonData row = returnData["row"];
                GetRankingData(row[0]);
            }
        }
        else
        {
            Debug.Log("데이터가 없습니다.");
        }
    }
    void GetRankingData(JsonData data)
    {
        if (!data.Keys.Contains(key))
        {
            Debug.Log("key값이 잘못 되었거나 없습니다.");
            return;
        }
        string str = "";
        if (key == "score" || key == "rank")
        {
            str = data[key]["N"].ToString();
        }
        else
        {
            str = data[key].ToString();
        }

        if (str.Contains("array"))
        {
            string array_data = "";
            for (int i = 0; i < data[key][0].Count; i++)
            {
                array_data += data[key][0][i][0].ToString() + "/";
            }
            outputDataList.Add(array_data);
        }
        else
        {
            outputDataList.Add(str);
        }
    }


    // === 비동기 데이터 주고받기 === //
    


}