using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YLimiter : MonoBehaviour
{
    private Camera _camera;

    public float limitY = 0.2f;

    private void Awake()
    {
        _camera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        //카메라 포지션이 Y리미트보다 커지면 -> 아바타가 공중에 뜨면
        if (_camera.transform.position.y > limitY)
        {
            //아바타 포지션.y - 리미트y = 둘의 차이가 나오니까
            //그만큼 리미터의 포지션을 조정해주면?
            float correction = _camera.transform.position.y - limitY;

            transform.position = new Vector3(0,correction,0);
        }
    }
}
