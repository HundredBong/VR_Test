using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellCastManager : MonoBehaviour
{
    public InputActionProperty spellInput;

    public BasicSpell defaultSpell;

    public Transform wandTip;

    private void Update()
    {
        if (spellInput.action.WasPressedThisFrame())
        {
            StartCasting();
        }
        else if (spellInput.action.WasReleasedThisFrame())
        {
            StopCasting();
        }
    }

    private void StartCasting()
    {

    }

    private void StopCasting()
    {
        BasicSpell spawnSpell = Instantiate(defaultSpell);
        spawnSpell.Initialize(wandTip);
    }
}
