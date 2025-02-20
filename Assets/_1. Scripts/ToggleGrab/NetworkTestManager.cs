using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.InputSystem;
using ExitGames.Client.Photon;

public class NetworkTestManager : MonoBehaviourPunCallbacks
{
    public InputActionProperty rightTriggerButton;
    public TextMeshProUGUI text;

    private bool inRoom;

    private const byte EVENT_BUTTON_PRESSED = 1;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        text.text = "서버 연결됨";

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        text.text = "로비 입장함";

        RoomOptions options = new RoomOptions { MaxPlayers = 2, IsVisible = true, IsOpen = true };

        PhotonNetwork.JoinOrCreateRoom("테스트", options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        inRoom = true;
        text.text = "방에 입장함";
    }

    private void Update()
    {
        if (rightTriggerButton.action.WasPressedThisFrame() && inRoom)
        {
            SendButtonPressEvent();
        }
    }

    private void SendButtonPressEvent()
    {
        object content = true;
        RaiseEventOptions options = new RaiseEventOptions();
        options.Receivers = ReceiverGroup.All;
        SendOptions sendOptions = SendOptions.SendReliable;

        PhotonNetwork.RaiseEvent(EVENT_BUTTON_PRESSED, content, options, sendOptions);
        text.text = "버튼 눌림";
    }
}
