using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using PDollarGestureRecognizer;
using System.IO;
using UnityEngine.Events;
using WebSocketSharp;

public class MovementRecognizer : MonoBehaviour
{
    public InputActionProperty rightTrigger;
    public float inputThreshod = 0.1f;
    public Transform movementSource;
    public float newPositionThresholdDistance = 0.05f;

    private bool isMoving = false;
    private List<Vector3> positionList = new List<Vector3>();

    public GameObject debugCubePrefab;
    public bool creationMode = true;
    public string newGestureName;
    private List<Gesture> trainingSet = new List<Gesture>();

    public float recognitionThreshold = 0.9f;

    [System.Serializable]
    public class UnityStringEvent : UnityEvent<string> { }
    public UnityStringEvent OnRecognized;

    public ObjectSpawner objectSpawner;

    private void Start()
    {
        //C:\Users\kag24\AppData\LocalLow\DefaultCompany\VR_Test
        string[] gestureFiles = Directory.GetFiles(Application.persistentDataPath, "*.xml");

        foreach (var item in gestureFiles)
        {
            trainingSet.Add(GestureIO.ReadGestureFromFile(item));
        }
    }

    private void Update()
    {
        //트리거 누르면
        if (rightTrigger.action.ReadValue<float>() > inputThreshod && !isMoving)
        {
            StartMovement();
        }
        //트리거 떼면
        if (rightTrigger.action.WasReleasedThisFrame())
        {
            EndMoverment();
        }
        //트리거 누르는 동안
        if (isMoving && rightTrigger.action.IsPressed())
        {
            UpdateMovement();
        }
    }

    private void StartMovement()
    {
        Debug.Log("Start Movement 실행됨");
        isMoving = true;
        //리스트를 비워주고 현재 위치값을 리스트에 추가, 다음 포지션 리스트 작성할 때 필요해서 시작할 때 Add해줘야 함
        positionList.Clear();
        positionList.Add(movementSource.transform.position);

        //디버그용 오브젝트 있으면
        if (debugCubePrefab != null)
        {
            //Destroy를 이렇게도 쓸 수 있다니
            Destroy(Instantiate(debugCubePrefab, movementSource.transform.position, Quaternion.identity), 3f);
        }
    }

    private void EndMoverment()
    {
        Debug.Log("End Movement 실행됨");
        isMoving = false;

        //포지션 리스트에서 제스쳐 만들기
        Point[] pointArray = new Point[positionList.Count];

        for (int i = 0; i < positionList.Count; i++)
        {
            //pointArray[i] = positionList[i];
            //에셋이 2D기반으로 만들어진거라 VR에서 쓸려면 변경이 필요함
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionList[i]);
            pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0);
        }

        Gesture newGesture = new Gesture(pointArray);

        //인스펙터에서 creationMode 선택하면 제스쳐를 저장함
        if (creationMode)
        {
            newGesture.Name = newGestureName;
            trainingSet.Add(newGesture);

            string fileName = $"{Application.persistentDataPath}/{newGestureName}.xml";
            GestureIO.WriteGesture(pointArray, newGestureName, fileName);
        }
        //제스쳐를 검사한 후 저장된 제스쳐와 비슷한지 점수를 책정함
        //점수가 일정 이상일 경우 이벤트 실행
        else
        {
            Result result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray());
            Debug.Log($"제스쳐 이름 : {result.GestureClass}, 제스쳐 점수 : {result.Score}");
            if (result.Score > recognitionThreshold)
            {
                //if (result.GestureClass.IsNullOrEmpty())
                //{
                //    Debug.LogError("아니 왜 또");
                //}
                //OnRecognized.Invoke((result.GestureClass).ToString());

                objectSpawner.Spawn(result.GestureClass);
                
            }
        }
    }

    private void UpdateMovement()
    {
        //Debug.Log("Update Movement 실행됨");

        //이전 위치값을 마지막 포인트로 지정
        Vector3 lastPosition = positionList[positionList.Count - 1];

        //마지막 위치값과 손의 위치의 거리가 0.05f보다 크면 새로운 포지션값을 추가함
        if (Vector3.Distance(movementSource.transform.position, lastPosition) > newPositionThresholdDistance)
        {
            positionList.Add(movementSource.transform.position);

            if (debugCubePrefab != null)
            {
                Destroy(Instantiate(debugCubePrefab, movementSource.transform.position, Quaternion.identity), 3f);
            }
        }
    }

}
