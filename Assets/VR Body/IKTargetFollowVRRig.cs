using System.Collections;
using UnityEngine;

[System.Serializable]
public class VRMap
{
    public Transform vrTarget;
    public Transform ikTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;
    public void Map()
    {
        ikTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        ikTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }

}

public class IKTargetFollowVRRig : MonoBehaviour
{
    [Range(0, 1)]
    public float turnSmoothness = 0.1f;
    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;

    public Vector3 headBodyPositionOffset;
    public float headBodyYawOffset;


    public Transform headIK;
    public Vector3 headIKOffset;

    public Transform avatarRoot; 
    public Transform vrCamera; 
    private float avatarEyeHeight = 1.6f; //아바타 눈높이
    private Vector3 initialHeadBodyOffset; 

    private IEnumerator Start()
    {
        //퀘스트 초기화 대기 시간
        yield return new WaitForSeconds(1.5f);
        headMap();

        //플레이어의 눈 높이를 가져옴
        float playerEyeHeight = vrCamera.position.y;

        //아바타의 눈 높이를 기준으로 플레이어의 실제 눈높이에 비례해 아바타 크기를 조정함
        //너무 작은 값이 들어가면 문제가 생길 수 있어서 최소 값을 0.01로 설정
        float scaleFactor = Mathf.Max(playerEyeHeight / avatarEyeHeight, 0.01f);
        avatarRoot.localScale = Vector3.one * scaleFactor;

        //초기 위치 오프셋 값 저장
        initialHeadBodyOffset = headBodyPositionOffset;

        //스케일에 맞춰서 오프셋도 다시 조정, 안하면 머리만 둥둥 떠다님 
        headBodyPositionOffset = initialHeadBodyOffset * scaleFactor;
        headIKOffset *= scaleFactor;
    }

    private void LateUpdate()
    {
        //아바타의 몸통을 플레이어의 머리 위치에 맞게 보정함
        transform.position = vrCamera.position + headBodyPositionOffset;

        //Y축 회전값 보정, 무조건 시선과 몸의 방향이 정면을 향하지 않고, 자연스럽게 따라가도록 함
        float yaw = vrCamera.eulerAngles.y + headBodyYawOffset;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, yaw, 0f), turnSmoothness);

        head.Map();
        leftHand.Map();
        rightHand.Map();

        headIK.transform.position = head.ikTarget.position + headIKOffset;
    }

    public void headMap()
    {
        headIK.transform.position += headIKOffset;
    }
}
