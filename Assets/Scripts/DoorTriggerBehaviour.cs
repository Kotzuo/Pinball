﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggerBehaviour : MonoBehaviour
{
    public GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (!gameManager.isDoorClosed)
        {
            gameManager.isDoorClosed = true;
        }
    }
}
