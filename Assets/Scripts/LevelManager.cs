using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelManager", order = 1)]
public class LevelManager : ScriptableObject
{
   [Serializable]
   public struct Level
   {
        public string levelName;
   }

    public List<Level> levelSet;
}
