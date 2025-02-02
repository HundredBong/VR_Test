using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperHotEnemyDeath : MonoBehaviour
{
    public string targetTag = "Weapon";
    [HideInInspector]public SuperHotEnemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<SuperHotEnemy>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(targetTag))
        {
            enemy.Dead(other.contacts[0].point);
        }
    }
}
