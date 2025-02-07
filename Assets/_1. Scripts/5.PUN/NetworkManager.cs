using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

//TCP (Transmission Control Protocol) : 신뢰성 있는 데이터 전송, 속도는 좀 느려도 데이터가 제대로 도착하는게 중요할 때 사용, 웹 브라우징, 이메일, 파일 전송
//UDP (User Datagram Protocol ): 빠른 속도가 중요하지만 데이터가 조금 날아가도 상관없을 때 사용, 실시간 게임, 영상 스트리밍, 음성 채팅

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject roomButtons;
    public List<DefaultRoom> defalultRooms;

    private void Start()
    {
        //ConnectToServer();
    }

    public void ConnectToServer()
    {
        //PhotonServerSettings 파일에서 서버 정보를 가져오고 Photon Cloud 서버에 연결 시도
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("서버 연결 시도");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("서버에 연결됨");
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("로비에 입장함");
        roomButtons.SetActive(true);
    }

    public void InitiliazeRoom(int roomIndex)
    {
        DefaultRoom room = defalultRooms[roomIndex];

        //씬 로드
        PhotonNetwork.LoadLevel(room.sceneIndex);

        //방 세팅
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = room.maxPlayer; //최대 10명까지 입장가능
        roomOptions.IsVisible = true; //로비에서 이 방이 보이도록 설정함
        roomOptions.IsOpen = true; //플레이어가 입장 가능하도록 방을 열어둠

        //Room 01이라는 방을 들어가고, 없으면 새로 만들어줌, 기본 로비에서 방을 찾음
        //방 이름만 같으면 설정이 달라도 들어감
        PhotonNetwork.JoinOrCreateRoom(room.Name, roomOptions, TypedLobby.Default);
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("방에 참여함");
        base.OnJoinedRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("새로운 플레이어가 참가함");
        base.OnPlayerEnteredRoom(newPlayer);
    }
}

[System.Serializable]
public class DefaultRoom
{
    public string Name;
    public int sceneIndex;
    public int maxPlayer;
}