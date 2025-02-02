using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.AI;

public class SuperHotEnemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    private Transform playerTarget;
    private Transform playerHead;
    private FireBulletOnActivate gun;
    public float stopDistance = 5f;

    private Quaternion localRotationGun;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        gun = GetComponentInChildren<FireBulletOnActivate>();
        playerTarget = FindObjectOfType<XROrigin>().transform;
        playerHead = FindObjectOfType<Camera>().transform;
    }

    private void Start()
    {
        SetUpRagDoll();

        localRotationGun = gun.spawnPoint.transform.localRotation;
    }

    private void Update()
    {
        agent.SetDestination(playerTarget.position);

        float distance = Vector3.Distance(gameObject.transform.position, playerTarget.transform.position);
        //Debug.Log(distance);
        if (distance < stopDistance)
        {
            Vector3 dir = playerHead.transform.position - transform.position;
            dir.y = 0;

            if (dir != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(dir);
            }

            agent.isStopped = true;
            anim.SetBool("Shoot", true);
        }
    }

    public void ThrowGun()
    {
        gun.spawnPoint.localRotation = localRotationGun;
        gun.transform.parent = null;
        Rigidbody rb = gun.GetComponent<Rigidbody>();
        rb.velocity = BallisticVelocityVector(gun.transform.position, playerHead.transform.position, 45);
        rb.angularVelocity = Vector3.zero;
    }

    private Vector3 BallisticVelocityVector(Vector3 source, Vector3 target, float angle)
    {
        //https://www.youtube.com/watch?v=8NLzuURxFwY&t=708s

        Vector3 dir = target - source;

        float h = dir.y;
        dir.y = 0;

        float dis = dir.magnitude;
        float a = angle * Mathf.Deg2Rad;

        dir.y = dis * Mathf.Tan(a);
        dis += h / Mathf.Tan(a);

        float velocity = Mathf.Sqrt(dis * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return velocity * dir.normalized;
    }

    public void ShootEnemy()
    {
        //애니메이션 이벤트로 호출됨

        Vector3 playerHeadPos = playerHead.position - Random.Range(0, 0.4f) * Vector3.up;

        gun.spawnPoint.forward = (playerHeadPos - gun.spawnPoint.position).normalized;

        gun.FireBullet();
    }

    public void SetUpRagDoll()
    {
        foreach (var item in GetComponentsInChildren<Rigidbody>())
        {
            item.isKinematic = true;
        }
    }

    public void Dead(Vector3 hitPos)
    {
        foreach (var item in GetComponentsInChildren<Rigidbody>())
        {
            item.isKinematic = false;
        }

        foreach (var item in Physics.OverlapSphere(hitPos, 0.3f))
        {
            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.AddExplosionForce(1000, hitPos, 0.3f);
            }
        }

        ThrowGun();
        anim.enabled = false;
        agent.enabled = false;
        this.enabled = false;
    }
}
