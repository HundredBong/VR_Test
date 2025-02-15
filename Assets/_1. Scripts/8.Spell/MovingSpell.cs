using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpell : BasicSpell
{
    public Vector3 movingAxis;
    public float speed;
    public float collisionRadius = 0.2f;
    public LayerMask collsionLayer;

    private Vector3 movingWorldAxis;
    private bool hasExplode = false;

    public ParticleSystem bolt;
    public ParticleSystem trail;
    public ParticleSystem explosion;

    public override void Initialize(Transform wandTip)
    {
        base.Initialize(wandTip);
        movingWorldAxis = wandTip.TransformDirection(movingAxis);
    }

    private void Update()
    {
        transform.position += Time.deltaTime * movingWorldAxis * speed;
    }

    private void FixedUpdate()
    {
        if (hasExplode == false)
        {
            Collider[] results = Physics.OverlapSphere(transform.position, collisionRadius, collsionLayer);

            if (results.Length > 0)
            {
                Explode();
            }
        }
    }

    public void Explode()
    {
        hasExplode = true;

        explosion.Play();
        trail.Stop();
        bolt.Stop();

        Destroy(gameObject, 1f);
    }
}
