using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishBarUI : MonoBehaviour
{
    [SerializeField] Horde horde;
    [SerializeField] FinishPoint finishPoint;
    public Slider slider;

    private void Update()
    {
        ValueChangeOnUI();
    }

    public void ValueChangeOnUI()
    {
        slider.value = horde.transform.position.z / finishPoint.transform.position.z;
    }
}
