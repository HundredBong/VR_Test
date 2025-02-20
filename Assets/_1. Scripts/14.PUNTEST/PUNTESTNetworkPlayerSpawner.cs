using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUNTESTNetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    private GameObject spawnPlayerPrefab;

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("PlayerSpawner, 방에 참여함");
        Debug.Log(PhotonNetwork.CurrentRoom.Name);
        spawnPlayerPrefab = PhotonNetwork.Instantiate("PUNTEST Network Player", transform.position, transform.rotation);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        Debug.Log("PlayerSpawndj, 방에서 탈출함");
        PhotonNetwork.Destroy(spawnPlayerPrefab);
    }
}
