using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FareRefresher : MonoBehaviour
{
    public GameObject farePrefab; // Fare prefabını buraya atayın
    public Vector3 spawnPoint;
    public GameObject patrolToDestroy;// Farenin spawn olacağı nokta

    private void Start()
    {
        spawnPoint= patrolToDestroy.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Fareyi yenile
            RefreshFare();
        }
    }

    private void RefreshFare()
    {
        GameObject newFare = Instantiate(farePrefab, spawnPoint, Quaternion.identity);
        Destroy(patrolToDestroy);
        patrolToDestroy = newFare;
    }
}
