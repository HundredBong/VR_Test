using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using Photon.Pun;
using Unity.XR.CoreUtils;

public class NetworkPlayer : MonoBehaviour
{
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;
    private PhotonView photonview;

    public Animator leftHandAnimator;
    public Animator rightHandAnimator;

    private Transform headRig;
    private Transform leftHandRig;
    private Transform rightHandRig;

    private XROrigin origin;

    private void Awake()
    {
        photonview = GetComponent<PhotonView>();
        origin = FindObjectOfType<XROrigin>();
    }

    private void Start()
    {
        headRig = origin.transform.Find("Camera Offset/Main Camera");
        leftHandRig = origin.transform.Find("Camera Offset/Left Controller");
        rightHandRig = origin.transform.Find("Camera Offset/Right Controller");

        if (headRig == null) { Debug.LogError("headRig 할당 안됨"); }
        if (leftHandRig == null) { Debug.LogError("leftHandRig 할당 안됨"); }
        if (rightHandRig == null) { Debug.LogError("rightHandRig 할당 안됨"); }
        
        if (photonview.IsMine)
        {
            foreach (var item in GetComponentsInChildren<Renderer>())
            {
                item.enabled = false;
            }
        }
    }

    private void Update()
    {
        if (photonview.IsMine)
        {
            //헤드셋, 왼쪽 컨트롤러, 오른쪽 컨트롤러의 데이터를 가져와서 위치, 회전값을 업데이트함
            //MapPosition(head, XRNode.Head);
            //MapPosition(leftHand, XRNode.LeftHand);
            //MapPosition(rightHand, XRNode.RightHand);

            if (leftHandRig != null && rightHandRig != null)
            {
                MapPosition(head, headRig);
                MapPosition(leftHand, leftHandRig);
                MapPosition(rightHand, rightHandRig);
            }
            else
            {
                Debug.LogWarning("leftHandRig 또는 rightHandRig가 null임");
            }
            
            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.LeftHand), leftHandAnimator);
        }
    }

    private void UpdateHandAnimation(InputDevice targetDevice, Animator handAnimator)
    {
      
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

    private void MapPosition(Transform target, Transform rigTransform)
    {
        target.position = rigTransform.position;
        target.rotation = rigTransform.rotation;
    }


    private void MapPosition(Transform target, XRNode node)
    {
        //XRNode : VR장치의 특정 부분을 나타내는 열거형 타입
        //InputDevices.GetDeviceAtXRNode(node) : 이 메서드로 XR 장비의 정보를 가져옴
        //TryGetFeatureValue() : XR 장비의 특정 데이터를 가져오는 역할을 함
        //CommonUsages : XR 장비에서 자주 쓰이는 데이터 종류를 모아둔 집합.
        if (InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position))
        {
            target.position = position;
        }
        else
        {
            Debug.LogWarning($"{node}의 데이터를 가져올 수 없음");
        }


        if (InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation))
        {
            target.rotation = rotation;
        }
        else
        {
            Debug.LogWarning($"{node}의 데이터를 가져올 수 없음");
        }
    }
}
