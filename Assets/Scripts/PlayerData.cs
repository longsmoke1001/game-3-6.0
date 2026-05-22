using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject 
{
    // Start is called before the first frame update
     public float maxHealth = 20f;
     public float movSpeed = 5f;
     public float dashCooldown = 1f;
     public float attackPower = 5f;
     public float attackRange = 2f;
     public float dashDuration = 0.1f;
     public float dashSpeed = 20f;
     public float defendDuration = 0.5f;
     public float defendCooldown = 2f;
     public float attackSpeed = 0.5f;
     public float attackdelay = 0.2f;
     public float healthOnHit = 0f;
     public bool canMoveWhileAttacking = false;
     public bool canMoveWhileDefending = false;

    public void Reset()
    {
        maxHealth = 20f;
        movSpeed = 5f;
        dashCooldown = 1f;
        attackPower = 5f;
        attackRange = 2f;
        dashDuration = 0.1f;
        dashSpeed = 20f;
        defendDuration = 0.5f;
        defendCooldown = 2f;
        attackSpeed = 0.5f;
        attackdelay = 0.2f;
        healthOnHit = 0f;
        canMoveWhileAttacking = false;
        canMoveWhileDefending = false;
    }
}
