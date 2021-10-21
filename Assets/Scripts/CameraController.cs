using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    [SerializeField] float _followSpeed;
    [SerializeField] float endSeqYMargin;

    float distanceFromTarget;

    private void Awake()
    {
        EndSequence.OnCameraEndSequenceHandler += SetEndSequenceTarget;
    }
    private void OnDestroy()
    {
        EndSequence.OnCameraEndSequenceHandler -= SetEndSequenceTarget;
    }

    private void Start()
    {
        distanceFromTarget = target.transform.position.z - transform.position.z;
    }
    private void LateUpdate()
    {
        var xPos = Mathf.Lerp(transform.position.x , target.transform.position.x , _followSpeed * Time.deltaTime);
            if (target.transform.position.y + endSeqYMargin > transform.position.y)
            {
                transform.position = new Vector3(xPos, target.transform.position.y + endSeqYMargin, target.transform.position.z - distanceFromTarget);
            }
            else
            {
                transform.position = new Vector3(xPos, transform.position.y, target.transform.position.z - distanceFromTarget);
            }

    }

    void SetEndSequenceTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
