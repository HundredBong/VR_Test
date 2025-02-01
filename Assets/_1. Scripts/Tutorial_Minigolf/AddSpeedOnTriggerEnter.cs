using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSpeedOnTriggerEnter : MonoBehaviour
{
    public string targetTag;

    private GameManager gameManager;
    private Collider clubCollider;
    private Vector3 previousPosition;
    private Vector3 velocity;


    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        clubCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        previousPosition = transform.position;
    }

    private void Update()
    {
        //속도 = 거리 / 시간 공식 활용
        velocity = (transform.position - previousPosition) / Time.deltaTime;
        previousPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(targetTag))
        {
            Debug.Log("Colliding");
            Vector3 collisionPosition = clubCollider.ClosestPoint(other.transform.position);
            Vector3 collisionNormal = other.transform.position - collisionPosition;
            Vector3 projectedVelocity = Vector3.Project(velocity, collisionNormal);
            Rigidbody rb = other.attachedRigidbody;
            rb.velocity = projectedVelocity;
            gameManager.currentHitNumber++;
        }
    }
}
