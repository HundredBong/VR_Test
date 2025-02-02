using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SetTurnType : MonoBehaviour
{
    public ActionBasedSnapTurnProvider snapTurnProvider;
    public ActionBasedContinuousTurnProvider continuousTurnProvider;

    public void SetTypeFromIndex(int index)
    {
        switch (index)
        {
            case 0:
                snapTurnProvider.enabled = true;
                continuousTurnProvider.enabled = false;
                break;
            case 1:
                snapTurnProvider.enabled = false;
                continuousTurnProvider.enabled = true;
                break;
            default:
                break;
        }
    }
}
