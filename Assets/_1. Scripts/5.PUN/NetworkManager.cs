using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

//TCP (Transmission Control Protocol) : 신뢰성 있는 데이터 전송, 속도는 좀 느려도 데이터가 제대로 도착하는게 중요할 때 사용, 웹 브라우징, 이메일, 파일 전송
//UDP (User Datagram Protocol ): 빠른 속도가 중요하지만 데이터가 조금 날아가도 상관없을 때 사용, 실시간 게임, 영상 스트리밍, 음성 채팅

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject roomButtons;
    public List<DefaultRoom> defalultRooms;

    private void Start()
    {
        //ConnectToServer();
        //StartCoroutine(CheckNetworkStateCoroutine());
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

    //여기서 들어온 인자로 뭐 함
    //뭐하긴 방 선택하지
    //선택된 방 리스트에서 인덱스에 맞는 방을 초기화 하는데
    //0번째 방의 씬 인덱스가 1이면 같은 방 가는거 아닌가
    //ㅅ발왜안됨
    public void InitiliazeRoom(int roomIndex)
    {
        Debug.Log($"InitializeRoom 실행, roomIndex : {roomIndex}");
        DefaultRoom room = defalultRooms[roomIndex];

        //0번 인덱스 기준 
        //I Hate PUN, 1, 10


        //씬 로드
        PhotonNetwork.LoadLevel(room.sceneIndex);

        //방 세팅
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = room.maxPlayer; //최대 10명까지 입장가능
        roomOptions.IsVisible = true; //로비에서 이 방이 보이도록 설정함
        roomOptions.IsOpen = true; //플레이어가 입장 가능하도록 방을 열어둠

        Debug.Log($"방 생성 시도: {room.Name}");

        //Room 01이라는 방을 들어가고, 없으면 새로 만들어줌, 기본 로비에서 방을 찾음
        //방 이름만 같으면 설정이 달라도 들어감
        bool roomJoined =  PhotonNetwork.JoinOrCreateRoom(room.Name, roomOptions, TypedLobby.Default);

        Debug.Log($"방 생성 요청 결과 : {roomJoined}");
        Debug.Log($"현재 네트워크 상태 : {PhotonNetwork.NetworkClientState}");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log($"현재 방 인원: {PhotonNetwork.CurrentRoom.PlayerCount}");
        Debug.Log($"현재 네트워크 상태: {PhotonNetwork.NetworkClientState}");
        Debug.Log("방에 참여함");


    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"OnPlayerEnteredRoom 실행됨 - 현재 클라이언트 {PhotonNetwork.LocalPlayer.NickName}");

        Debug.Log("새로운 플레이어가 참가함");
        base.OnPlayerEnteredRoom(newPlayer);
    }
    
    private IEnumerator CheckNetworkStateCoroutine()
    {
        while (true)
        {
            Debug.Log($"현재 네트워크 상태 : {PhotonNetwork.NetworkClientState}");
            yield return new WaitForSeconds(1f);
        }
    }
}



[System.Serializable]
public class DefaultRoom
{
    public string Name;
    public int sceneIndex;
    public int maxPlayer;
}