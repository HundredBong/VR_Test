using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public VelocityEstimator head;
    public VelocityEstimator leftHand;
    public VelocityEstimator rightHand;
    

    public float sensitivity = 0.8f;
    public float minTimeScale = 0.05f;

    private float initailFixedDeltaTime;

    private void Start()
    {
        initailFixedDeltaTime = Time.fixedDeltaTime;        
    }

    private void Update()
    {
        float velocityMagnitude = head.GetVelocityEstimate().magnitude + leftHand.GetVelocityEstimate().magnitude + rightHand.GetVelocityEstimate().magnitude;

        Debug.Log($"Head : {head.GetVelocityEstimate().magnitude} \n Hands : {leftHand.GetVelocityEstimate().magnitude} , {rightHand.GetVelocityEstimate().magnitude}\n velocity : {velocityMagnitude}");

        Time.timeScale = Mathf.Clamp01(minTimeScale + velocityMagnitude * sensitivity);

        Time.fixedDeltaTime = initailFixedDeltaTime * Time.timeScale;
    }

}
