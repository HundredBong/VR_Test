using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using Unity.XR.CoreUtils;

public class PUNTESTNetwokPlayer : MonoBehaviour
{
    private PhotonView photonView;
    private XROrigin origin;

    private Transform headRig;
    private Transform leftHandRig;
    private Transform rightHandRig;

    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    public Animator leftHandAnimator;
    public Animator rightHandAnimator;

    public List<GameObject> avatars;
    private GameObject spawnedAvatar;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        origin = FindObjectOfType<XROrigin>();
    }

    private void Start()
    {
        headRig = origin.transform.Find("Camera Offset/Main Camera");
        leftHandRig = origin.transform.Find("Camera Offset/Left Controller");
        rightHandRig = origin.transform.Find("Camera Offset/Right Controller");

        if (photonView.IsMine)
        {
            photonView.RPC("LoadAvatar", RpcTarget.AllBuffered, PlayerPrefs.GetInt("AvatarID")); 
        }
    }

    [PunRPC]
    public void LoadAvatar(int index)
    {
        if (spawnedAvatar) { Destroy(spawnedAvatar); }

        spawnedAvatar = Instantiate(avatars[index], transform);
        PUNTESTAvatarInfo avatarInfo = spawnedAvatar.GetComponent<PUNTESTAvatarInfo>();

        //두 번째 인자에 fasle를 넣으면 부모로 이동할 때 움직이지 않고 고유한 로컬 포지션을 유지함
        avatarInfo.head.SetParent(head, false);
        avatarInfo.leftHand.SetParent(leftHand, false);
        avatarInfo.rightHand.SetParent(rightHand, false);
        leftHandAnimator = avatarInfo.leftHandAnimator;
        rightHandAnimator = avatarInfo.rightHandAnimator;
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            MapPosition(head, headRig);
            MapPosition(leftHand, leftHandRig);
            MapPosition(rightHand, rightHandRig);

            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.LeftHand), leftHandAnimator);
            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.RightHand), rightHandAnimator);
        }

    }

    private void UpdateHandAnimation(InputDevice targetDevice, Animator handAnimator)
    {
        if (handAnimator == null) { return; }

        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggetValue))
        {
            handAnimator.SetFloat("Trigger", triggetValue);
            //Debug.Log($"Trigger Value : {triggetValue}");
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
            //Debug.LogWarning($"Trigger Value : {triggetValue}");
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
            //Debug.Log($"Grip Value : {gripValue}");
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
            //Debug.LogWarning($"Grip Value : {gripValue}");
        }
    }

    public void MapPosition(Transform target, Transform rigTransform)
    {
        target.position = rigTransform.position;
        target.rotation = rigTransform.rotation;
    }
}
