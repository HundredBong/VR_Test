using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    private GameObject spawnPlayerPrefab;

    public override void OnJoinedRoom()
    { 
        //��Ʈ��ũ���� ����ȭ�� ������Ʈ ����, ��� Ŭ���̾�Ʈ�� �����ϰ� ������
        //ù ��° ���ڷδ� ������ �̸�, Resources/ ���� �ȿ� ������ �̸��� �������� �־�� ��
        //�� ��° ���ڷδ� ������Ʈ�� ������ ���� ��ǥ, �� ��° ���ڷδ� ������ �� ȸ�� ��
        spawnPlayerPrefab = PhotonNetwork.Instantiate("Network Player", transform.position, transform.rotation);
        Debug.Log($"������ ���� {spawnPlayerPrefab.name}");

        base.OnJoinedRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        //��� Ŭ���̾�Ʈ���� ���ÿ� �������, �ڱⰡ ������ ������Ʈ�� ������ �� ����
        //�ٸ� ����� ���� ��Ʈ��ũ ������Ʈ�� ���� ����
        PhotonNetwork.Destroy(spawnPlayerPrefab);
    }
}
