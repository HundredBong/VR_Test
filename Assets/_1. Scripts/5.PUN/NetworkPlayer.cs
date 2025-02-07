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

        if (headRig == null) { Debug.LogError("headRig �Ҵ� �ȵ�"); }
        if (leftHandRig == null) { Debug.LogError("leftHandRig �Ҵ� �ȵ�"); }
        if (rightHandRig == null) { Debug.LogError("rightHandRig �Ҵ� �ȵ�"); }
        
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
            //����, ���� ��Ʈ�ѷ�, ������ ��Ʈ�ѷ��� �����͸� �����ͼ� ��ġ, ȸ������ ������Ʈ��
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
                Debug.LogWarning("leftHandRig �Ǵ� rightHandRig�� null��");
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
        //XRNode : VR��ġ�� Ư�� �κ��� ��Ÿ���� ������ Ÿ��
        //InputDevices.GetDeviceAtXRNode(node) : �� �޼���� XR ����� ������ ������
        //TryGetFeatureValue() : XR ����� Ư�� �����͸� �������� ������ ��
        //CommonUsages : XR ��񿡼� ���� ���̴� ������ ������ ��Ƶ� ����.
        if (InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position))
        {
            target.position = position;
        }
        else
        {
            Debug.LogWarning($"{node}�� �����͸� ������ �� ����");
        }


        if (InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation))
        {
            target.rotation = rotation;
        }
        else
        {
            Debug.LogWarning($"{node}�� �����͸� ������ �� ����");
        }
    }
}
