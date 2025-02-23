using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine.Networking;

public class PUNTESTNetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject roomUI;
    public List<PUNTESTDefaultRoom> defaultRooms;

    #region IP테스트
    [System.Serializable]
    private class IPResponse
    {
        public string ip;
    }

    private string routerIP;
    public void GetRouterIP()
    {
        //나중에 테스트할 때 버튼에 ConnectToServer대신 이 메서드 연결해보기
        StartCoroutine(GetRouterIPCoroutine());
    }

    private IEnumerator GetRouterIPCoroutine()
    {
        //리소스 자동 정리용 using문 사용
        using (UnityWebRequest www = UnityWebRequest.Get("https://api64.ipify.org?format=json"))
        {
            //웹 요청이 끝날때까지 대기
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("아 이게 안되네");
            }
            else
            {
                routerIP = JsonUtility.FromJson<IPResponse>(www.downloadHandler.text).ip;
                Debug.Log($"가져온 아이피 주소 : {routerIP}");

                //TODO : 정상적으로 가져와지면 JoinOrCreateRoom의 방 이름에 routerIP넣고 돌려보기
            }
        }
    }

    #endregion
    private void Start()
    {

        //GetRouterIP();
    }


    public void ConnectToServer()
    {
        Debug.Log("서버 연결 시도");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnected();
        Debug.Log("서버 연결됨");
        PhotonNetwork.JoinLobby();
    }

    public void InitializeRoom(int defaultRoomIndex)
    {
        PUNTESTDefaultRoom roomSetting = defaultRooms[defaultRoomIndex];

        PhotonNetwork.LoadLevel(roomSetting.sceneIndex);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = roomSetting.maxPlayer;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom(roomSetting.Name, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("로비에 입장함");
        roomUI.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("방에 참여함");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("새로운 플레이어가 방에 참여함");
    }
}

[System.Serializable]
public class PUNTESTDefaultRoom
{
    public string Name;
    public int sceneIndex;
    public int maxPlayer;
}