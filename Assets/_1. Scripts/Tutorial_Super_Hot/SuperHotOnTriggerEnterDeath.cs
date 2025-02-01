using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SuperHotOnTriggerEnterDeath : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SuperHotEnemyDeath death = other.GetComponent<SuperHotEnemyDeath>();
        
        if (death != null)
        {
            death.enemy.Dead(transform.position);
        }
    }
}
