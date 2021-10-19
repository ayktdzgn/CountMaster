using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpRamp : MonoBehaviour
{
    [SerializeField]float yHeight;
    [SerializeField] float duration = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var randomHeight = Random.Range(yHeight, yHeight+1);
            other.GetComponent<Member>().Rigidbody.isKinematic = true;
            other.gameObject.transform.DOLocalJump(other.gameObject.transform.localPosition , randomHeight, 1 , duration);
        }
    }
}
