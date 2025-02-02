using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    private GameManager gameManager;

    public string targetTag = "Ball";

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(targetTag))
        {
            gameManager.GoToNextHole();
        }
    }

}
