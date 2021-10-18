using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateManager : MonoBehaviour
{
    Gate[] gateArray;

    private void Awake()
    {
        gateArray = GetComponentsInChildren<Gate>();
    }
    public void GateTriggered()
    {
        for (int i = 0; i < gateArray.Length; i++)
        {
            gateArray[i].Collider.enabled = false;
        }
    }
}
