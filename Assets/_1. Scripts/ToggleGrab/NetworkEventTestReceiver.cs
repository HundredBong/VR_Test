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

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnDisable()
    {
        base.OnDisable();
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == EVENT_BUTTON_PRESSED)
        {
            bool isPressed = (bool)photonEvent.CustomData;

            text.text = $"다른 곳에서 버튼 눌림 {isPressed}";
        }
    }
}
