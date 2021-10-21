using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSequencePlatform : MonoBehaviour
{

    Horde _horde;
    EndSequencePlatformManager manager;

    private void Awake()
    {
        manager = GetComponentInParent<EndSequencePlatformManager>();
        _horde = manager.Horde;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var member = other.GetComponent<Member>();
            member.transform.SetParent(transform);
            member.Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            if (!member.IsTriggerByEndSequence)
                manager.HordeCountOnPlatform++;

            member.IsTriggerByEndSequence = true;
        }
    }


}
