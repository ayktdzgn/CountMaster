using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSequencePlatformManager : MonoBehaviour
{
    [SerializeField] Horde horde;
    int hordeCountOnPlatform;
    public Horde Horde => horde;
    public int HordeCountOnPlatform
    {
        get => hordeCountOnPlatform;
        set
        {
            if (value >= horde.HordeManager.HordeCount)
            {
                FindObjectOfType<GameManager>().gameState = GameState.Win;
            }
            hordeCountOnPlatform = value;
        }
    }
}
