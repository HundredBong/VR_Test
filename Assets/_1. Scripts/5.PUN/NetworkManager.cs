using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

//TCP (Transmission Control Protocol) : �ŷڼ� �ִ� ������ ����, �ӵ��� �� ������ �����Ͱ� ����� �����ϴ°� �߿��� �� ���, �� ����¡, �̸���, ���� ����
//UDP (User Datagram Protocol ): ���� �ӵ��� �߿������� �����Ͱ� ���� ���ư��� ������� �� ���, �ǽð� ����, ���� ��Ʈ����, ���� ä��

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject roomButtons;
    public List<DefaultRoom> defalultRooms;

    private void Start()
    {
        //ConnectToServer();
    }

    public void ConnectToServer()
    {
        //PhotonServerSettings ���Ͽ��� ���� ������ �������� Photon Cloud ������ ���� �õ�
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("���� ���� �õ�");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("������ �����");
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("�κ� ������");
        roomButtons.SetActive(true);
    }

    public void InitiliazeRoom(int roomIndex)
    {
        DefaultRoom room = defalultRooms[roomIndex];

        //�� �ε�
        PhotonNetwork.LoadLevel(room.sceneIndex);

        //�� ����
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = room.maxPlayer; //�ִ� 10����� ���尡��
        roomOptions.IsVisible = true; //�κ񿡼� �� ���� ���̵��� ������
        roomOptions.IsOpen = true; //�÷��̾ ���� �����ϵ��� ���� �����

        //Room 01�̶�� ���� ����, ������ ���� �������, �⺻ �κ񿡼� ���� ã��
        //�� �̸��� ������ ������ �޶� ��
        PhotonNetwork.JoinOrCreateRoom(room.Name, roomOptions, TypedLobby.Default);
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("�濡 ������");
        base.OnJoinedRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("���ο� �÷��̾ ������");
        base.OnPlayerEnteredRoom(newPlayer);
    }
}

[System.Serializable]
public class DefaultRoom
{
    public string Name;
    public int sceneIndex;
    public int maxPlayer;
}