using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector2 checkpointPosition;
    void Start()
    {
        checkpointPosition = transform.position;
    }

    public void SetCheckpoint(Vector2 newCheckpoint)
    {
        checkpointPosition = newCheckpoint;
    }

    public void Respawn()
    {
        transform.position = checkpointPosition+ new Vector2(0, 1f);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            Respawn();
        }
    }
}
