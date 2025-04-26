using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(10, 0, -10f);
    private int lastDirection = 1; // 1 = sağa, -1 = sola

    void LateUpdate()
    {
        if (target.localScale.x > 0)
            lastDirection = 1;
        else if (target.localScale.x < 0)
            lastDirection = -1;

        Vector3 dynamicOffset = new Vector3(offset.x * lastDirection, offset.y, offset.z);

        Vector3 desiredPosition = target.position + dynamicOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, dynamicOffset.z);
    }
}
