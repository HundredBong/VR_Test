using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using TMPro;

public class NetworkEventTestReceiver : MonoBehaviourPunCallbacks, IOnEventCallback
{
    private const byte EVENT_BUTTON_PRESSED = 1;
    public TextMeshProUGUI text;
    private int readyCount = 0;
    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);// <- 이미 베이스 OnEnable에 들어가있음 
        //base.OnEnable();
    }

    public override void OnDisable()
    {
        //base.OnDisable();
    }

    public void OnEvent(EventData photonEvent)
    {
        


        if (photonEvent.Code == EVENT_BUTTON_PRESSED)
        {
            bool isPressed = (bool)photonEvent.CustomData;
Debug.Log($"이벤트 호출됨{PhotonNetwork.CurrentRoom.PlayerCount}, {photonEvent.Sender}");
            text.text = $"다른 곳에서 버튼 눌림 {isPressed}";
            if (isPressed)
            {
                readyCount++;
                if (readyCount == PhotonNetwork.CurrentRoom.PlayerCount)
                {
                    text.text = $"모든 플레이어가 준비 완료 됨 {PhotonNetwork.CurrentRoom.PlayerCount}";
                }
            }
        }
    }
}
