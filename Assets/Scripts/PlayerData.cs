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
    public float defendDuration = 1f;
    public float skillCooldown = 2f;
    public float attackSpeed = 0.5f;
    public float attackdelay = 0.2f;
    public float healthOnHit = 0f;
    public float reflectedDamageMultiplier = 1f;
    public bool canMoveWhileAttacking = false;
    public bool canMoveWhileDefending = false;
    public float defendHealMultiplier = 0f;
    public int projectileCount = 1;
    public bool pierce = false;
    public bool projReturn = false;
    public bool movSpeedScaleDamage = false;
    public float projSpeed=10f;
}
