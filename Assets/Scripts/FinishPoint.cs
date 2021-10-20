using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    [SerializeField] FinishBarUI finishBarUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<GameManager>().gameState = GameState.Win;
        }
    }

    public void ValueChangeOnUI(Vector3 horde )
    {
        finishBarUI.slider.value = horde.z / transform.position.z;
    }
}
