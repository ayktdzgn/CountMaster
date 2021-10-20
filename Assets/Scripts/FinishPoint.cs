using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    [SerializeField]EndSequence endSequence;
    bool isEndSequenceStart;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isEndSequenceStart)
            {
                isEndSequenceStart = true;
                endSequence.PlayEndSequence();
            }
            //FindObjectOfType<GameManager>().gameState = GameState.Win;
        }
    }
}
