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

        MakeTower();
    }

    void MakeTower()
    {
        int objectIndex = 0;
        for (int i = 0; i < stepList.Count; i++)
        {
            if(stepList[i] % 2 == 0)
            {
                int cift = 1;
                int tek = 1;
                for (int objectInStep = 1; objectInStep <= stepList[i]; objectInStep++)
                {
                    if (objectInStep % 2 == 0)
                    {
                        objects[objectIndex].transform.position = new Vector3(0 + (cift * -0.5f), i, 0);
                        cift+= 2;
                    }
                    else
                    {
                        objects[objectIndex].transform.position = new Vector3(0 + (tek * 0.5f), i, 0);
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
                        objects[objectIndex].transform.position = new Vector3(0 + (cift * -0.5f), i, 0);
                        cift += 2;
                    }
                    else
                    {
                        objects[objectIndex].transform.position = new Vector3(0 + (tek * 0.5f), i, 0);
                        tek += 2;
                    }

                    objectIndex++;
                }
            }          
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
        foreach (var item in stepList)
        {
            Debug.Log(item);
        }
    }
}
