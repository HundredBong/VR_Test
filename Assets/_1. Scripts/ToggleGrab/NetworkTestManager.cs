using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.InputSystem;
using ExitGames.Client.Photon;
using UnityEngine.Networking;

public class NetworkTestManager : MonoBehaviourPunCallbacks
{
    public InputActionProperty rightTriggerButton;
    public TextMeshProUGUI text;

    private bool inRoom;
    private bool isReady = false;
    private const byte EVENT_BUTTON_PRESSED = 1; //이벤트 ID

    private void Start()
    {
        GetRouterIP();
        
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        text.text = "서버 연결됨";
        PhotonNetwork.NickName = "HundredBong";
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        text.text = "로비 입장함";

        RoomOptions options = new RoomOptions { MaxPlayers = 2, IsVisible = true, IsOpen = true };

        PhotonNetwork.JoinOrCreateRoom($"{routerIP}", options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        inRoom = true;
        text.text = "방에 입장함";
        Debug.Log($"방 이름 : {PhotonNetwork.CurrentRoom.Name}");
    }

    private void Update()
    {
        if (rightTriggerButton.action.WasPressedThisFrame() && inRoom && PhotonNetwork.InRoom)
        {
            SendButtonPressEvent();
        }
    }

    private void SendButtonPressEvent()
    {
        if (PhotonNetwork.InRoom == false)
        {
            Debug.Log("방에 입장하지 않음 ㅅㄱ");
            return;
        }
        isReady = !isReady;
        object content = isReady;
        RaiseEventOptions options = new RaiseEventOptions();
        options.Receivers = ReceiverGroup.All;
        SendOptions sendOptions = SendOptions.SendReliable;

        PhotonNetwork.RaiseEvent(EVENT_BUTTON_PRESSED, content, options, sendOptions);
        text.text = "버튼 눌림";
    }

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
}
