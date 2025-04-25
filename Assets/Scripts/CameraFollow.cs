using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 0, -10f);

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 desiredTransform = target.position + offset;
        Vector3 smoothed = Vector3.Lerp(transform.position, desiredTransform, smoothSpeed);
        transform.position= new Vector3(smoothed.x, smoothed.y, offset.z);
    }
}
