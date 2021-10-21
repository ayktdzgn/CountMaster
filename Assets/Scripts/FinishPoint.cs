using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    EndSequence _endSequence;
    [SerializeField]Horde horde;
    [SerializeField] FinishBarUI finishBarUI;
    bool isEndSequenceStart;

    private void Awake()
    {
        _endSequence = GetComponent<EndSequence>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isEndSequenceStart)
            {
                isEndSequenceStart = true;
                _endSequence.PlayEndSequence(horde);
            }
        }
    }

    private void Update()
    {
        finishBarUI.ValueChangeOnUI(horde , this);
    }
}
