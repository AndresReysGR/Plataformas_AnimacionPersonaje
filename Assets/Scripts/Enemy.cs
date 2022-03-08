using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    SpriteRenderer spr;
    Animator anim;
    [SerializeField, Range(0.1f, 15.0f)]
    float moveSpeed = 2f;

    [SerializeField]
    Vector2 direction = Vector2.right;

    [SerializeField, Range(0.1f, 10f)]
    float patrolTimeLimit = 3f;

    [SerializeField, Range(0.1f, 10f)]    
    float idleTimelimit = 2f;
    float patrolTimer = 0f;

    IEnumerator idleRoutine;

    IEnumerator patrolRoutine;

    void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {   
        StartIdle();
    }

    /*void Update()
    {
        MoveX();
        patrolTimer += Time.deltaTime;
        if(patrolTimer >= patrolTimeLimit)
        {
            patrolTimer = 0f;
            ChangeDirection();
            FlipSpriteX();
        }
    }*/

    IEnumerator patroling()
    {
        ChangeDirection();
        FlipSpriteX();
        while(true)
        {
            patrolTimer += Time.deltaTime;
            MoveX();
            if(patrolTimer >= patrolTimeLimit)
            {
                patrolTimer = 0f;
                StartIdle();
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator idling()
    {
        yield return new WaitForSeconds(idleTimelimit);
        StartPatrol();
    }

    void StartIdle()
    {
        anim.SetFloat("move", 0);
        idleRoutine = idling();
        StartCoroutine(idleRoutine);
    }

    void StartPatrol()
    {
        anim.SetFloat("move", 1f);
        patrolRoutine = patroling();
        StartCoroutine(patrolRoutine);
    }

    void MoveX() => transform.Translate(direction * moveSpeed * Time.deltaTime);
    void ChangeDirection() => direction.x = -direction.x; 
    void FlipSpriteX() => spr.flipX = !spr.flipX;

    void OnTriggerEnter2D(Collider2D other)
    {
       if(other.CompareTag("Player"))
       {
           PLayer player = other.GetComponent<PLayer>();
           player.GetDamage(Damage);
       } 
    }

}
