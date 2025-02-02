using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private int currentHoleNumber = 0;
    public List<Transform> startingPosition;
    public Rigidbody ballRigidbody;

    public int currentHitNumber = 0;
    private List<int> previousHitNumbers = new List<int>();

    public TMPro.TextMeshPro scoreText;

    private void Start()
    {
        ballRigidbody.transform.position = startingPosition[currentHoleNumber].position;

        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;

        scoreText.text = "";
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("다음 홀로 이동");
            GoToNextHole();
        }
    }

    public void GoToNextHole()
    {
        currentHoleNumber++;

        if (currentHoleNumber >= startingPosition.Count)
        {
            Debug.Log("끝에 도달함");
        }
        else
        {
            ballRigidbody.transform.position = startingPosition[currentHoleNumber].position;

            ballRigidbody.velocity = Vector3.zero;
            ballRigidbody.angularVelocity = Vector3.zero;
        }

        previousHitNumbers.Add(currentHoleNumber);
        currentHitNumber = 0;
        DisplayScore();
    }

    public void DisplayScore()
    {
        string tempScoreText = "";
        for (int i = 0; i < previousHitNumbers.Count; i++)
        {
            tempScoreText += "Hole" + (i + 1) + " - " + previousHitNumbers[i] + "<br>";
        }
        scoreText.text = tempScoreText;
    }
}
