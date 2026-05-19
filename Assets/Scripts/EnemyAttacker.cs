using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacker : Enemy
{
    protected Vector2 targetPos;
    protected bool isWaiting;
    protected float startWaitingTime = -9999f;
    protected float waitingTime = 1f;
    protected Transform playerTransform;
    [SerializeField] protected float chaseRange = 5f;
    [SerializeField] protected LayerMask obstacleMask;
    [SerializeField] protected float attackRange = 1f;
    [SerializeField] protected float attackPower = 5f;
    [SerializeField] protected float attackSpeed = 0.5f;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        spawnPos = transform.position;
        targetPos = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)) + spawnPos;
        player = FindAnyObjectByType<Player>();
        if (player != null)
            playerTransform = player.transform;
    }

    protected void Patrol()
    {
        if (!isWaiting)
        {
            Vector2 toTarget = targetPos - (Vector2)(transform.position);
            if (toTarget.magnitude > 0.1f)
            {
                transform.Translate(toTarget.normalized * movSpeed * Time.deltaTime);
                if (anim != null)
                    anim.SetBool("1_Move", true);
            }
            else
            {
                targetPos = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)) + spawnPos;
                isWaiting = true;
                startWaitingTime = Time.time;
                if (anim != null)
                    anim.SetBool("1_Move", false);
            }
        }
        else
        {
            if (Time.time - startWaitingTime > waitingTime)
            {
                isWaiting = false;
            }

            if (anim != null)
                anim.SetBool("1_Move", false);
        }
    }

    protected bool CanSeePlayer()
    {
        if (playerTransform == null)
            return false;

        Vector2 from = transform.position;
        Vector2 to = playerTransform.position;
        if (Vector2.Distance(from, to) > chaseRange)
            return false;
        RaycastHit2D hit = Physics2D.Linecast(from, to, obstacleMask);
        Debug.DrawLine(from, to, Color.red);
        return hit.collider == player.GetComponent<BoxCollider2D>() && hit.collider.gameObject != gameObject;
    }
}