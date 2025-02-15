using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using PDollarGestureRecognizer;
using System.IO;
using UnityEngine.Events;

public class MovementRecognizer : MonoBehaviour
{
    //public XRNode inputSource;
    //public InputHelpers.Button inputButton;
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

    private void Start()
    {
        string[] gestureFiles = Directory.GetFiles(Application.persistentDataPath, "*.xml");

        foreach (var item in gestureFiles)
        {
            trainingSet.Add(GestureIO.ReadGestureFromFile(item));
        }
    }

    private void Update()
    {
        if (rightTrigger.action.ReadValue<float>() > inputThreshod && !isMoving)
        {
            StartMovement();
        }
        if (rightTrigger.action.WasReleasedThisFrame())
        {
            EndMoverment();
        }

        if (isMoving && rightTrigger.action.IsPressed())
        {
            UpdateMovement();
        }
    }

    private void StartMovement()
    {
        Debug.Log("Start Movement 실행됨");
        isMoving = true;
        positionList.Clear();
        positionList.Add(movementSource.transform.position);

        if (debugCubePrefab != null)
        {
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
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionList[i]);
            pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0);
        }

        Gesture newGesture = new Gesture(pointArray);
        if (creationMode)
        {
            newGesture.Name = newGestureName;
            trainingSet.Add(newGesture);

            string fileName = $"{Application.persistentDataPath}/{newGestureName}.xml";
            GestureIO.WriteGesture(pointArray, newGestureName, fileName);
        }
        else
        {
            Result result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray());
            Debug.Log(result.GestureClass + result.Score);
            if (result.Score > recognitionThreshold)
            {
                OnRecognized.Invoke(result.GestureClass);
            }
        }
    }

    private void UpdateMovement()
    {
        Debug.Log("Update Movement 실행됨");

        Vector3 lastPosition = positionList[positionList.Count - 1];

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
