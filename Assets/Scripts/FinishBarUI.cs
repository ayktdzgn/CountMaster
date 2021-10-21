using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishBarUI : MonoBehaviour
{
    [SerializeField] Slider slider;

    public void ValueChangeOnUI(Horde horde, FinishPoint finishPoint)
    {
        slider.value = horde.transform.position.z / finishPoint.transform.position.z;
    }
}
