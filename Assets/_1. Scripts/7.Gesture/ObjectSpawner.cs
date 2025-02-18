using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public List<GameObject> objects;

    public void Spawn(string objectName)
    {
        Debug.Log($"아이템 : {""}, 오브젝트 : {objectName}");

        foreach (var item in objects)
        {
            Debug.Log($"아이템 : {item.name}, 오브젝트 : {objectName}");
            if (item.name == objectName)
                item.SetActive(false);
        }
    }
}
