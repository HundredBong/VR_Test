using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    private TriggerZone triggerZone;

    private void Awake()
    {
        triggerZone = GetComponent<TriggerZone>();
    }

    private void OnEnable()
    {
        triggerZone.OnEnterEvent.AddListener(InsideTrash);
    }

    private void OnDisable()
    {
        triggerZone.OnEnterEvent.RemoveListener(InsideTrash);
    }

    public void InsideTrash(GameObject obj)
    {
        obj.SetActive(false);
    }
}
