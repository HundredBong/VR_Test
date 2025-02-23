using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

namespace Timeline
{
    public class EventManager : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        public Button readyButton;

        private const byte TIMELINE_EVENT = 1;
        private int playersReady = 0;

        public override void OnEnable()
        {
            base.OnEnable();
            readyButton.onClick.AddListener(PlayerPressedButton);
            PhotonNetwork.AddCallbackTarget(this);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            readyButton.onClick.RemoveListener(PlayerPressedButton);
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void PlayerPressedButton()
        {
            try
            {
                //배열로 저장하는 이유 : Photon의 RaiseEvent는 object 배열을 전송할 수 있기 때문
                //int, bool, string같은 기본 타입은 그대로 보낼 수 있지만, 여러 개의 데이터를 한 번에 보내려면 object[] 배열을 사용해야함
                object[] content = new object[] { PhotonNetwork.LocalPlayer.ActorNumber };

                //이 비벤트를 방에 있는 모든 플레이어가 받을 수 있도록 설정함
                //누른 플레이어뿐만 아니라 모든 플레이어가 이벤트를 수신함
                RaiseEventOptions options = new RaiseEventOptions();
                options.Receivers = ReceiverGroup.All;


                bool success = PhotonNetwork.RaiseEvent(TIMELINE_EVENT, content, options, SendOptions.SendReliable);
                if (success == false)
                {
                    Debug.Log("레이즈 이벤트 호출 실패");
                }
            }
            catch(System.Exception ex)
            {
                Debug.LogError($"이벤트 전송 중 에러 발생 : {ex.Message}");
            }

        }

        public void OnEvent(EventData photonEvent)
        {
            Debug.Log($"타임라인 이벤트 수신됨, 이벤트 코드 {photonEvent.Code}");

            if (photonEvent.Code == TIMELINE_EVENT)
            {
                Debug.Log("타임라인 이벤트 수신됨");

                //보낼때 배열로 보냈으니 받을때도 이렇게 해야함
                object[] data = (object[])photonEvent.CustomData;

                if (data != null && data.Length > 0)
                {
                    int playerID = (int)data[0];
                    Debug.Log($"플레이어 {playerID}가 버튼을 누름");
                }

                playersReady++;
                Debug.Log($"준비된 플레이어 수 : {playersReady}");

                if (playersReady >= 2)
                {
                    Debug.Log("타임라인 실행");
                    FindObjectOfType<TimelineController>().PlayTimeline();
                    playersReady = 0;
                }
            }
        }
    }
}