using System.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;

namespace reverse
{
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

    public class IKFollowTargetRigVR : MonoBehaviour
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

        public Transform eyePos;

        public float avatarEyeHeight = 1.6f; //아바타의 눈높이
        private Vector3 initialHeadBodyOffset;
        private XROrigin origin;

        private void Awake()
        {
            origin = FindObjectOfType<XROrigin>();
        }

        private IEnumerator Start()
        {
            avatarEyeHeight = eyePos.transform.position.y;
            // 퀘스트 초기화 대기 시간
            yield return new WaitForSeconds(1.5f);
            headMap();

            AdjustPlayerHeightToAvatar();

            //초기 위치 오프셋 값 저장
            initialHeadBodyOffset = headBodyPositionOffset;
        }

        private void AdjustPlayerHeightToAvatar()
        {
            //플레이어의 현재 눈높이
            float playerEyeHeight = vrCamera.position.y;

            //아바타의 눈높이를 기준으로 XR Origin 높이 보정
            float targetHeight = avatarEyeHeight;

            //XR Origin의 Y값 보정
            float heightDifference = targetHeight - playerEyeHeight;


            Vector3 originPos = transform.position;
            originPos.y += heightDifference;
            //transform.position = originPos;
            origin.transform.position = new Vector3(0, (origin.transform.position.y - heightDifference) / 2, 0);
            Debug.Log($"아바타 높이 : {targetHeight}, 플레이어 높이 : {playerEyeHeight}, 보정값 : {heightDifference}");
        }

        private void LateUpdate()
        {
            transform.position = vrCamera.position + headBodyPositionOffset;

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
}


