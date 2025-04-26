using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVertigo : MonoBehaviour
{
    private bool isVertigo = false;
    private float vertigoTimer = 0f;
    private float rotationAmount = 0f;
    private Transform camTransform;
    private Quaternion originalRotation;

    public float maxRotationAngle = 15f; // Maksimum sağa-sola dönme açısı
    public float rotationSpeed = 2f; // Saniyedeki kaç defa ileri-geri sallansın
    private void Start()
    {
        camTransform = Camera.main.transform;
        originalRotation = camTransform.rotation;
    }

    public void StartVertigo(float duraiton)
    {
        vertigoTimer = duraiton;
        originalRotation = camTransform.rotation;
        isVertigo = true;
    }


    private void Update()
    {
        if (isVertigo)
        {
            vertigoTimer -= Time.deltaTime;
            rotationAmount = Mathf.Sin(Time.time * rotationSpeed) * maxRotationAngle;
            camTransform.rotation = Quaternion.Euler(0, 0, rotationAmount);

        }

        if (vertigoTimer <= 0)
        {
            isVertigo = false;
            camTransform.rotation = originalRotation;
        }
    }
}
