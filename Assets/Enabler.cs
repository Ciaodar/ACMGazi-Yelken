using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enabler : MonoBehaviour
{
    [SerializeField]private GameObject[] Lands;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Lands != null)
        {
            foreach (GameObject land in Lands) 
            {
                land.SetActive(true);
            }
        }
    }
}
