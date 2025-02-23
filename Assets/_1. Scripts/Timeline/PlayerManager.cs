using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Timeline
{
    public class PlayerManager : MonoBehaviourPunCallbacks
    {
        public GameObject waitingUI;

        private bool isWaiting = false;
        private bool isQuitting = false; //정상 종료 감지용 
        private bool isReconnecting = false;

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);

            if (isQuitting)
            {
                Debug.Log($"플레이어 {otherPlayer.ActorNumber}가 게임을 종료함");
                return;
            }

            //액터넘버는 방에 입장한 순서대로 1부터 증가함
            //중간에 플레이어가 나가도 나머지 플레이어의 번호가 바뀌지는 않음
            //새로운 플레이어가 들어오면 기존 번호를 재사용하지 않고 새로운 번호를 받음
            Debug.Log($"플레이어 {otherPlayer.ActorNumber} 퇴장함, 대기 시작");

            SaveGameState();

            StartCoroutine(WaitForReconnectCoroutine());
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            Debug.Log($"{newPlayer.NickName},{newPlayer.ActorNumber}가 재접속함, 게임 상태 복구 시도");
            LoadGameState();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            
            Debug.Log($"연결 끊김 감지함, 원인 : {cause}");

            //클라에 의해 종료되었을 경우
            if (cause == DisconnectCause.DisconnectByClientLogic)
            {
                Debug.Log("정상적으로 게임을 종료함");
            }

            if (isReconnecting == false)
            {
                isReconnecting = true;
                TryReconnect();
            }
        }

        private async void TryReconnect()
        {
            if (PhotonNetwork.IsConnected)
            {
                Debug.Log("이미 포톤 네트워크에 연결됨");
                return;
            }

            Debug.Log("자동 재접속 시도");
            try
            {
                bool success = PhotonNetwork.ReconnectAndRejoin();
                await Task.Delay(2000); //2초 대기 (비동기)

                if (success)
                {
                    Debug.Log("자동 재접속 성공");
                }
                else
                {
                    Debug.Log("자동 재접속 실패함, 서버 연결 시도");
                    PhotonNetwork.ConnectUsingSettings();
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"자동 재접속 중 오류 발생 : {ex.Message}");
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("서버 재연결 성공함");

            if (isReconnecting)
            {
                Debug.Log("기존 방으로 다시 입장 시도");
                PhotonNetwork.RejoinRoom(PhotonNetwork.CurrentRoom.Name);
            }
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("방 재입장 완료");
            isReconnecting = false;
        }

        private IEnumerator WaitForReconnectCoroutine()
        {
            //이미 대기중이면 중복 실행 방지
            if (isWaiting) { yield break; }
            isWaiting = true;

            waitingUI.SetActive(true);

            float timer = 10f;

            while (timer > 0)
            {
                //Debug.Log($"남은 시간 : {timer:F1}");
                timer -= Time.deltaTime;
                yield return null;
            }

            waitingUI.SetActive(false);
            isWaiting = false;
            Debug.Log("10초 경과함");
        }

        private void SaveGameState()
        {
            try
            {
                bool isObjectActive = GameObject.Find("TargetObject").activeSelf;

                //딕셔너리는 제네릭을 사용하여 키, 값의 타입이 고정되어있지만,
                //해쉬테이블은 그런거 없이 키, 값 둘 다 object타입 들어감
                Hashtable props = new Hashtable()
            {
                //bool을 object로 저장할때는 박싱
                { "ObjectActive", isObjectActive }
            };

                //룸 커스텀 프로퍼티는 방에 저장되는 키-값 형태의 데이터
                //방에 있는 모든 플레이어가 공유할 수 있음
                //서버가 데이터를 저장하고 관리하므로 플레이어가 나가도 데이터가 유지됨
                //방이 존재하는 동안 유지되며, 모든 플레이어가 나가서 방이 삭제되면 같이 삭제됨
                PhotonNetwork.CurrentRoom.SetCustomProperties(props);

                Debug.Log($"게임 상태 저장됨, 오브젝트 상태 : {isObjectActive}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"저장 중 오류 발생 : {ex.Message}");
            }
        }

        private void LoadGameState()
        {
            try
            {
                if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("ObjectActive", out object value))
                {
                    //object 타입은 다양한 데이터를 저장할 수 있지만, 꺼낼때는 원래 타입으로 변환하는 언박싱을 거쳐야 함
                    //object -> bool
                    bool isObjectActive = (bool)value;
                    GameObject targetObject = GameObject.Find("TargetObject");

                    if (targetObject != null)
                    {
                        targetObject.SetActive(isObjectActive);
                        Debug.Log($"게임 상태 복구 완료, 오브젝트 상태 : {isObjectActive}");
                    }
                    else
                    {
                        Debug.LogError("대상 오브젝트를 찾을 수 없음");
                    }
                }
                else
                {
                    Debug.LogError("저장된 게임 상태가 없음");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"로드 중 에러 발생 : {ex.Message}");
            }
        }

        private void OnApplicationQuit()
        {
            isQuitting = true;
        }
    }

}
