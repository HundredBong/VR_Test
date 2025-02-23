using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Timeline
{
    public class PhotonManager : MonoBehaviourPunCallbacks
    {
        public GameObject playerPrefab;

        private void Start()
        {
            if (PhotonNetwork.IsConnected == false)
            {
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            Debug.Log("서버 연결됨");

            //로비를 거치지않고 바로 이동함
            PhotonNetwork.JoinOrCreateRoom("TestRoom", new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            Debug.Log($"방에 참여함, 현재 플레이어 수 : {PhotonNetwork.CurrentRoom.PlayerCount}");

            Debug.Log("플레이어 스포너 실행됨");

            if (PhotonNetwork.IsConnectedAndReady)
            {
                Debug.Log("포톤 네트워크에 연결됨, 플레이어 생성");
                Vector3 spawnPosition = new Vector3(Random.Range(-3, 3), 1, Random.Range(-3, 3));
                PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogError("포톤 네트워크에 연결되지 않음");
            }
        }
    }
}

