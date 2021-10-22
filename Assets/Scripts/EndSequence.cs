using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSequence : MonoBehaviour
{
    Horde _horde;
    List<int> stepList = new List<int>();
    List<Member> objects = new List<Member>();
    float hordeMemberHeight = 2.2f;

    public delegate void HordeEndSequenceHandler(bool status);
    public static HordeEndSequenceHandler OnHordeEndSequenceHandler;

    public delegate void CameraEndSequenceHandler(Transform target);
    public static CameraEndSequenceHandler OnCameraEndSequenceHandler;

    public void PlayEndSequence(Horde horde)
    {
        _horde = horde;
        OnHordeEndSequenceHandler?.Invoke(true);

        objects = _horde.HordeManager.HordeList;
        foreach (var obj in objects)
        {
            obj.transform.localPosition = new Vector3(obj.transform.localPosition.x,0, obj.transform.localPosition.z);
        }

        OnCameraEndSequenceHandler?.Invoke(objects[0].transform);

        Calculate(_horde.HordeManager.HordeCount);
        SortList();
        StartCoroutine(MakeTowerStepByStep());
    }
    IEnumerator MakeTowerStepByStep()
    {
        int step = 0;
        for (int s = 0; s <= stepList.Count; s++)
        {
            int objectIndex = 0;
            step++;
            for (int i = 0; i < s; i++)  //---> Basamak
            {
                if (stepList[i] % 2 == 0)
                {
                    int cift = 1;
                    int tek = 1;
                    for (int objectInStep = 1; objectInStep <= stepList[i]; objectInStep++)
                    {
                        if (objectInStep % 2 == 0)
                        {
                            objects[objectIndex].transform.position = new Vector3(0 + (cift * -0.5f), objects[objectIndex].transform.position.y, _horde.transform.position.z);
                            if (s < stepList.Count) objects[objectIndex].transform.position = new Vector3(objects[objectIndex].transform.position.x , objects[objectIndex].transform.position.y + hordeMemberHeight, _horde.transform.position.z); /*objects[objectIndex].transform.DOMoveY(objects[objectIndex].transform.position.y + 3.15f, 0.2f);*/
                            objects[objectIndex].PlayIdleAnimation();
                            cift += 2;
                        }
                        else
                        {
                            objects[objectIndex].transform.position = new Vector3(0 + (tek * 0.5f), objects[objectIndex].transform.position.y, _horde.transform.position.z);
                            if (s < stepList.Count) objects[objectIndex].transform.position = new Vector3(objects[objectIndex].transform.position.x, objects[objectIndex].transform.position.y + hordeMemberHeight, _horde.transform.position.z);
                            objects[objectIndex].PlayIdleAnimation();
                            tek += 2;
                        }

                        objectIndex++;
                    }
                }
                else
                {
                    int cift = 0;
                    int tek = 2;
                    for (int objectInStep = 0; objectInStep < stepList[i]; objectInStep++)
                    {

                        if (objectInStep % 2 == 0)
                        {
                            objects[objectIndex].transform.position = new Vector3(0 + (cift * -0.5f), objects[objectIndex].transform.position.y, _horde.transform.position.z);
                            if (s < stepList.Count) objects[objectIndex].transform.position = new Vector3(objects[objectIndex].transform.position.x, objects[objectIndex].transform.position.y + hordeMemberHeight, _horde.transform.position.z);
                            objects[objectIndex].PlayIdleAnimation();
                            cift += 2;
                        }
                        else
                        {
                            objects[objectIndex].transform.position = new Vector3(0 + (tek * 0.5f), objects[objectIndex].transform.position.y, _horde.transform.position.z);
                            if (s < stepList.Count) objects[objectIndex].transform.position = new Vector3(objects[objectIndex].transform.position.x, objects[objectIndex].transform.position.y + hordeMemberHeight, _horde.transform.position.z);
                            objects[objectIndex].PlayIdleAnimation();
                            tek += 2;
                        }

                        objectIndex++;
                    }
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Calculate(int count)
    {
        var tempCount = count;
        int i = 1;

        while (tempCount > 0)
        {
            for (int j = 0; j < 2; j++)
            {
                if (i > tempCount) break;
                stepList.Add(i);
                tempCount -= i;
            }

            if (i > tempCount)
            {
                stepList.Add(tempCount);
                tempCount -= tempCount;
            }
            else
            {
                i++;
            }
        }
    }

    void SortList()
    {
        stepList.Sort();
    }
}
