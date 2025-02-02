using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameMenuManager : MonoBehaviour
{
    public float spawnDistance = 2f;
    public Transform head;
    public GameObject menu;
    public InputActionProperty menuButton;

    private void Update()
    {
        if (menuButton.action.WasPressedThisFrame())
        {
            menu.SetActive(!menu.activeSelf);
            menu.transform.position = head.transform.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * spawnDistance;
        }

        if (menu.activeSelf)
        {
            menu.transform.LookAt (new Vector3(head.transform.position.x, menu.transform.position.y, head.transform.position.z));
            menu.transform.forward = menu.transform.forward * -1f;
        }
        //Debug.Log(menuButton.action.WasPressedThisFrame());
    }
}
