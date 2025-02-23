using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    private GameObject spawnPlayerPrefab;

    private void Update()
    {
        //Debug.Log($"방 참가 여부: {PhotonNetwork.InRoom}");
    }

    public override void OnJoinedRoom()
    {
        //네트워크에서 동기화된 오브젝트 생성, 모든 클라이언트에 동일하게 생성됨
        //첫 번째 인자로는 프리팹 이름, Resources/ 폴더 안에 동일한 이름의 프리팹이 있어야 함
        //두 번째 인자로는 오브젝트를 생성할 월드 좌표, 세 번째 인자로는 생성될 때 회전 값
        base.OnJoinedRoom();
        Debug.Log($"포톤 네트워크 플레이어 리스트 {PhotonNetwork.PlayerList.Length}");
        spawnPlayerPrefab = PhotonNetwork.Instantiate($"Network Player{PhotonNetwork.PlayerList.Length}", transform.position, transform.rotation);
        Debug.Log($"프리팹 생성 {spawnPlayerPrefab.name}");

    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        //모든 클라이언트에서 동시에 사라지나, 자기가 생성한 오브젝트만 삭제할 수 있음
        //다른 사람이 만든 네트워크 오브젝트는 삭제 못함
        PhotonNetwork.Destroy(spawnPlayerPrefab);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"OnPlayerEnteredRoom 실행됨 - 현재 클라이언트 {PhotonNetwork.LocalPlayer.NickName}");

        Debug.Log("새로운 플레이어가 참가함");
        base.OnPlayerEnteredRoom(newPlayer);
    }
}
