using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUNTESTAvatarSelector : MonoBehaviour
{
    public void SetAvatarID(int index)
    {
        PlayerPrefs.SetInt("AvatarID", index);
    }
}
