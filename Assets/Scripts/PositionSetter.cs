using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PositionSetter 
{
    public static List<Vector3> GetPositionListAround(Vector3 startPos, float[] ringDistanceArray, int[] ringPositionCountArray)
    {
        List<Vector3> positionList = new List<Vector3>();
        positionList.Add(startPos);
        for (int i = 0; i < ringDistanceArray.Length; i++)
        {
            positionList.AddRange(GetPositionListAround(startPos, ringDistanceArray[i], ringPositionCountArray[i]));
        }
        return positionList;
    }

    public static List<Vector3> GetPositionListAround(Vector3 startPos, float distance, int positionCount)
    {
        List<Vector3> positionList = new List<Vector3>();
        for (int i = 0; i < positionCount; i++)
        {
            float angle = i * (360 / positionCount);
            Vector3 dir = ApplyRotationVector(new Vector3(1, 0), angle);
            Vector3 pos = startPos + dir * distance;
            positionList.Add(pos);
        }
        return positionList;
    }

    public static Vector3 ApplyRotationVector(Vector3 vec, float angle)
    {
        return Quaternion.Euler(0, angle, 0) * vec;
    }
}
