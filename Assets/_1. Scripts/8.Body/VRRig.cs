using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HundredBong
{
    public class VRRig : MonoBehaviour
    {
        public float turnSmoothness = 1;

        public VRMap head;
        public VRMap leftHand;
        public VRMap rightHand;

        public Transform headConstraint;
        public Vector3 headBodyOffset;

        private void Start()
        {
            headBodyOffset = transform.position - headConstraint.position;
        }

        private void LateUpdate()
        {
            transform.position = headConstraint.position + headBodyOffset;

            transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(headConstraint.up, Vector3.up).normalized * -1, Time.deltaTime * turnSmoothness);

            head.Map();
            leftHand.Map();
            rightHand.Map();
        }
    }

    [System.Serializable]
    public class VRMap
    {
        public Transform VRTarget;
        public Transform rigTarget;
        public Vector3 trackingPositionOffset;
        public Vector3 trackingRotationOffset;

        public void Map()
        {
            rigTarget.position = VRTarget.TransformPoint(trackingPositionOffset);
            rigTarget.rotation = VRTarget.rotation * Quaternion.Euler(trackingRotationOffset);
        }
    }
}
