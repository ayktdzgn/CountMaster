using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpRamp : MonoBehaviour
{
    [SerializeField]float yHeight;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Member>().Rigidbody.isKinematic = true;
            other.gameObject.transform.DOLocalJump(other.gameObject.transform.localPosition , yHeight , 1 , 2f);
        }
    }
}
