using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class Portal : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    private void Start()
    {
        if (gameManager == null)
            gameManager=FindAnyObjectByType<GameManager>();
    }
    void OnTriggerEnter2D(){
        gameManager.Winning();
    }
}
