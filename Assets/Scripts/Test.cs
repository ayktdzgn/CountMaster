using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    List<int> stepList = new List<int>();
    List<GameObject> objects = new List<GameObject>();
    
    public GameObject res;
    public int count;

    private void Start()
    {
        CreateObjects(count);
        Calculate(count);
        ReadList();

        //MakeTower();
        StartCoroutine(MakeTowerStepByStep());
        
    }

    void MakeTower()
    {
        int objectIndex = 0;
        for (int i = stepList.Count - 1; i >= 0; i--)
        {
            if (stepList[i] % 2 == 0)
            {
                int cift = 1;
                int tek = 1;
                for (int objectInStep = 1; objectInStep <= stepList[i]; objectInStep++)
                {
                    if (objectInStep % 2 == 0)
                    {
                        objects[objectIndex].transform.position = new Vector3(0 + (cift * -0.5f), objects[objectIndex].transform.position.y, 0);
                        objects[objectIndex].transform.DOMoveY(objects[objectIndex].transform.position.y + (stepList.Count - 1) - i, 1);
                        cift += 2;
                    }
                    else
                    {
                        objects[objectIndex].transform.position = new Vector3(0 + (tek * 0.5f), objects[objectIndex].transform.position.y, 0);
                        objects[objectIndex].transform.DOMoveY(objects[objectIndex].transform.position.y + (stepList.Count - 1) - i, 1);
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
                        objects[objectIndex].transform.position = new Vector3(0 + (cift * -0.5f), objects[objectIndex].transform.position.y, 0);
                        objects[objectIndex].transform.DOMoveY(objects[objectIndex].transform.position.y + (stepList.Count - 1) - i, 1);

                        cift += 2;
                    }
                    else
                    {
                        objects[objectIndex].transform.position = new Vector3(0 + (tek * 0.5f), objects[objectIndex].transform.position.y, 0);
                        objects[objectIndex].transform.DOMoveY(objects[objectIndex].transform.position.y + (stepList.Count - 1) - i, 1);

                        tek += 2;
                    }

                    objectIndex++;
                }
            }
        }
    }

    IEnumerator MakeTowerStepByStep()
    {
        int step = 0;
        for (int s = 0; s < stepList.Count; s++)
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
                            objects[objectIndex].transform.position = new Vector3(0 + (cift * -0.5f), objects[objectIndex].transform.position.y, 0);
                            //objects[objectIndex].transform.DOMove(new Vector3(objects[objectIndex].transform.position.x, objects[objectIndex].transform.position.y + 1, objects[objectIndex].transform.position.z), 1);
                            objects[objectIndex].transform.DOMoveY(objects[objectIndex].transform.position.y + 1, 1);
                            cift += 2;
                        }
                        else
                        {
                            objects[objectIndex].transform.position = new Vector3(0 + (tek * 0.5f), objects[objectIndex].transform.position.y, 0);
                            objects[objectIndex].transform.DOMoveY(objects[objectIndex].transform.position.y + 1, 1);
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
                            objects[objectIndex].transform.position = new Vector3(0 + (cift * -0.5f), objects[objectIndex].transform.position.y, 0);
                            objects[objectIndex].transform.DOMoveY(objects[objectIndex].transform.position.y + 1, 1);

                            cift += 2;
                        }
                        else
                        {
                            objects[objectIndex].transform.position = new Vector3(0 + (tek * 0.5f), objects[objectIndex].transform.position.y, 0);
                            objects[objectIndex].transform.DOMoveY(objects[objectIndex].transform.position.y + 1, 1);

                            tek += 2;
                        }

                        objectIndex++;
                    }
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    void CreateObjects(int count)
    {
        for (int i = 0; i < count; i++)
        {
            objects.Add(Instantiate(res));
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

    void ReadList()
    {
        stepList.Sort();
        //foreach (var item in stepList)
        //{
        //    Debug.Log(item);
        //}
    }
}
