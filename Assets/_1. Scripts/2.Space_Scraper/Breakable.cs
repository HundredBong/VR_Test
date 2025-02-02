using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Breakable : MonoBehaviour
{
    public List<GameObject> breakablePieces;
    public float timeToBreak = 2f;
    private float timer = 0f;
    public UnityEvent OnBreak;

    private void Start()
    {
        foreach (GameObject item in breakablePieces)
        {
            item.SetActive(false);
        }
    }

    public void Break()
    {
        timer = timer + Time.deltaTime;

        if (timer > timeToBreak)
        {
            foreach (GameObject item in breakablePieces)
            {
                item.SetActive(true);
                item.transform.parent = null;
            }

            OnBreak?.Invoke();
            gameObject.SetActive(false);
        }
    }
}

