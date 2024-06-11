using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speedPlayer, speedSword, rangeSword, walkDurationTime, dashDurationTime, damageDurationTime, deathDurationTime, jumpForce, dashForce, xKnockback, yKnockback;
    public static bool damage, damageLeft, damageRight, invincibility;
    private byte jumpTimes, dashTimes;
    private bool jump, dashLeft, dashRight, doubleButton, jumpDash, attacking, attackingLeft, attackingRight, returnSword, death;
    public GameObject sword, floor;
    public static int life;
    public Rigidbody2D rbPlayer, rbSword;
    private Animator animator;
    private Vector2 facingLeft, facingRight, facingLeftSword, facingRightSword, swordPosition;
    private Collider2D scenarioCollider;
    private float scenarioLeftBound, scenarioRightBound, leftBoundary, rightBoundary, currentWalkDurationTime, swordThrowDurationTime, currentSwordThrowDurationTime, currentDashDurationTime, currentDamageDurationTime, currentDeathDurationTime;

    void Start()
    {
        if (transform.localScale.x < 0)
        {
            if (sword.transform.localScale.x < 0)
            {
                sword.transform.localScale = new Vector2(sword.transform.localScale.x, sword.transform.localScale.y);
            }

            if (sword.transform.localScale.x > 0)
            {
                sword.transform.localScale = new Vector2(-sword.transform.localScale.x, sword.transform.localScale.y);
            }

            facingLeft = new Vector2(transform.localScale.x, transform.localScale.y);
            facingRight = new Vector2(-transform.localScale.x, transform.localScale.y);
            facingLeftSword = new Vector2(sword.transform.localScale.x, sword.transform.localScale.y);
            facingRightSword = new Vector2(-sword.transform.localScale.x, sword.transform.localScale.y);
        }
        
        if (transform.localScale.x > 0)
        {
            if (sword.transform.localScale.x < 0)
            {
                sword.transform.localScale = new Vector2(-sword.transform.localScale.x, sword.transform.localScale.y);
            }

            if (sword.transform.localScale.x > 0)
            {
                sword.transform.localScale = new Vector2(sword.transform.localScale.x, sword.transform.localScale.y);
            }

            facingLeft = new Vector2(-transform.localScale.x, transform.localScale.y);
            facingRight = new Vector2(transform.localScale.x, transform.localScale.y);
            facingLeftSword = new Vector2(-sword.transform.localScale.x, sword.transform.localScale.y);
            facingRightSword = new Vector2(sword.transform.localScale.x, sword.transform.localScale.y);
        }

        swordPosition = sword.transform.position;
        animator = GetComponent<Animator>();
        scenarioCollider = floor.GetComponent<Collider2D>();
        scenarioLeftBound = scenarioCollider.bounds.min.x;
        scenarioRightBound = scenarioCollider.bounds.max.x;
        leftBoundary = scenarioLeftBound;
        rightBoundary = scenarioRightBound;
        life = 12;
        jumpTimes = 1;
        dashTimes = 1;
        jump = false;
        dashLeft = false;
        dashRight = false;
        jumpDash = false;
        doubleButton = false;
        sword.SetActive(false);
        attacking = false;
        returnSword = false;
        damage = false;
        damageLeft = false;
        damageRight = false;
        invincibility = false;
        death = false;
        walkDurationTime = 10f / 9f;
        currentWalkDurationTime = walkDurationTime;
        swordThrowDurationTime = 5f / 7f;
        currentSwordThrowDurationTime = swordThrowDurationTime;
        currentDashDurationTime = dashDurationTime;
        currentDamageDurationTime = damageDurationTime;
        deathDurationTime = 20f / 18f;
        currentDeathDurationTime = deathDurationTime;
    }

    void Update()
    {
        if (!death)
        {
            if (invincibility)
            {
                if (damage)
                {
                    animator.SetBool("Damage", true);
                    animator.SetBool("Walking", false);
                    animator.SetBool("Attacking", false);
                    animator.SetBool("Jumping", false);
                    animator.SetBool("Dashing", false);
                    animator.SetBool("Death", false);
                    animator.SetBool("Walk Continuation", false);
                    jumpTimes = 1;
                    dashTimes = 1;
                    jump = false;
                    dashLeft = false;
                    dashRight = false;
                    doubleButton = false;
                    currentDashDurationTime = dashDurationTime;
                    currentSwordThrowDurationTime = swordThrowDurationTime;
                }

                if (damageLeft)
                {
                    rbPlayer.velocity = Vector2.zero;
                    rbPlayer.AddForce(new Vector2(-xKnockback, yKnockback), ForceMode2D.Impulse);
                    damageLeft = false;
                }

                if (damageRight)
                {
                    rbPlayer.velocity = Vector2.zero;
                    rbPlayer.AddForce(new Vector2(xKnockback, yKnockback), ForceMode2D.Impulse);
                    damageRight = false;
                }

                if (!damageLeft || !damageRight || !damage)
                {
                    currentDamageDurationTime -= Time.deltaTime;
                    if (currentDamageDurationTime <= 0)
                    {
                        currentDamageDurationTime = damageDurationTime;
                        invincibility = false;
                    }
                }
            }

            if (Input.GetKey("left") && !attacking && !dashLeft && !damage)
            {
                dashTimes = 1;
                transform.localScale = facingLeft;
                if (!jump)
                {    
                    rbPlayer.velocity = Vector2.zero;
                    if (!sword.activeInHierarchy)
                    {
                        sword.transform.Translate(new Vector2(-1, 0) * Time.deltaTime * speedPlayer);
                    }

                    if (!doubleButton)
                    {
                        animator.SetBool("Walking", true);
                        animator.SetBool("Attacking", false);
                        animator.SetBool("Jumping", false);
                        animator.SetBool("Dashing", false);
                        animator.SetBool("Damage", false);
                        animator.SetBool("Death", false);
                        rbPlayer.velocity = Vector2.left * speedPlayer;
                        currentWalkDurationTime -= Time.deltaTime; 
                    }
                }

                if (jump)
                {
                    animator.SetBool("Jumping", true);
                    animator.SetBool("Walking", false);
                    animator.SetBool("Attacking", false);
                    animator.SetBool("Dashing", false);
                    animator.SetBool("Damage", false);
                    animator.SetBool("Death", false);
                    animator.SetBool("Walk Continuation", false);
                }
            }

            if (Input.GetKeyDown("left") && !attacking && dashRight && !damage)
            {
                rbPlayer.velocity = Vector2.zero;
                currentWalkDurationTime = walkDurationTime;
                dashRight = false;
            }

            if (Input.GetKeyUp("left") && !attacking && !damage && !jump)
            {
                animator.SetBool("Walking", false);
                animator.SetBool("Attacking", false);
                animator.SetBool("Jumping", false);
                animator.SetBool("Dashing", false);
                animator.SetBool("Damage", false);
                animator.SetBool("Death", false);
                animator.SetBool("Walk Continuation", false);
                currentWalkDurationTime = walkDurationTime;
                if (!jump && !dashLeft)
                {
                    rbPlayer.velocity = Vector2.zero;
                }

                doubleButton = false;
            }

            if (Input.GetKey("right") && !attacking && !dashRight && !damage)
            {
                dashTimes = 1;
                transform.localScale = facingRight;
                if (!jump)
                {    
                    rbPlayer.velocity = Vector2.zero;
                    if (!sword.activeInHierarchy)
                    {
                        sword.transform.Translate(new Vector2(1, 0) * Time.deltaTime * speedPlayer);
                    }

                    if (!doubleButton)
                    {
                        animator.SetBool("Walking", true);
                        animator.SetBool("Attacking", false);
                        animator.SetBool("Jumping", false);
                        animator.SetBool("Dashing", false);
                        animator.SetBool("Damage", false);
                        animator.SetBool("Death", false);
                        rbPlayer.velocity = Vector2.right * speedPlayer; 
                        currentWalkDurationTime -= Time.deltaTime;
                    }
                }

                if (jump)
                {
                    animator.SetBool("Jumping", true);
                    animator.SetBool("Walking", false);
                    animator.SetBool("Attacking", false);
                    animator.SetBool("Dashing", false);
                    animator.SetBool("Damage", false);
                    animator.SetBool("Death", false);
                    animator.SetBool("Walk Continuation", false);
                }
            }

            if (Input.GetKeyDown("right") && !attacking && dashLeft && !damage)
            {
                rbPlayer.velocity = Vector2.zero;
                currentWalkDurationTime = walkDurationTime;
                dashLeft = false;
            }

            if (Input.GetKeyUp("right") && !attacking && !damage && !jump)
            {
                animator.SetBool("Walking", false);
                animator.SetBool("Attacking", false);
                animator.SetBool("Jumping", false);
                animator.SetBool("Dashing", false);
                animator.SetBool("Damage", false);
                animator.SetBool("Death", false);
                animator.SetBool("Walk Continuation", false);
                currentWalkDurationTime = walkDurationTime;
                if (!jump && !dashRight)
                {
                    rbPlayer.velocity = Vector2.zero;
                }

                doubleButton = false;
            }

            if (Input.GetKey("left") && Input.GetKey("right") && !attacking && !jump && !dashLeft && !dashRight && !damage)
            {
                animator.SetBool("Walking", false);
                animator.SetBool("Attacking", false);
                animator.SetBool("Jumping", false);
                animator.SetBool("Dashing", false);
                animator.SetBool("Damage", false);
                animator.SetBool("Death", false);
                animator.SetBool("Walk Continuation", false);
                currentWalkDurationTime = walkDurationTime;
                doubleButton = true;
            }

            if (currentWalkDurationTime <= 0)
            {
                animator.SetBool("Walk Continuation", true);
                currentWalkDurationTime = walkDurationTime;
            }   

            if (!sword.activeInHierarchy)
            {
                swordPosition = sword.transform.position;
                if (transform.localScale.x == facingLeft.x)
                {
                    sword.transform.localScale = facingLeftSword;
                    sword.transform.position = new Vector2(transform.position.x - 1.5f, transform.position.y);
                }

                if (transform.localScale.x == facingRight.x)
                {
                    sword.transform.localScale = facingRightSword;
                    sword.transform.position = new Vector2(transform.position.x + 1.5f, transform.position.y);
                }
            }

            if (Input.GetKeyDown(KeyCode.E) && (sword.transform.localScale.x == facingLeftSword.x) && !sword.activeInHierarchy && !attacking && !dashLeft && !dashRight && !jumpDash && !damage)
            {
                rbPlayer.velocity = Vector2.zero;
                attacking = true;
                attackingLeft = true;
                animator.SetBool("Attacking", true);
                animator.SetBool("Walking", false);
                animator.SetBool("Jumping", false);
                animator.SetBool("Dashing", false);
                animator.SetBool("Damage", false);
                animator.SetBool("Death", false);
                animator.SetBool("Walk Continuation", false);
            }

            if (Input.GetKeyDown(KeyCode.E) && (sword.transform.localScale.x == facingRightSword.x) && !sword.activeInHierarchy && !attacking && !dashLeft && !dashRight && !jumpDash && !damage)
            {
                rbPlayer.velocity = Vector2.zero;
                attacking = true;
                attackingRight = true;
                animator.SetBool("Attacking", true);
                animator.SetBool("Walking", false);
                animator.SetBool("Jumping", false);
                animator.SetBool("Dashing", false);
                animator.SetBool("Damage", false);
                animator.SetBool("Death", false);
                animator.SetBool("Walk Continuation", false);
            }

            if (attacking && !damage)
            {
                currentSwordThrowDurationTime -= Time.deltaTime;
                if (currentSwordThrowDurationTime <= 0 && attackingLeft)
                {
                    sword.SetActive(true);
                    rbSword.velocity = Vector2.left * speedSword;
                    attacking = false;
                    attackingLeft = false;
                    animator.SetBool("Attacking", false);
                    animator.SetBool("Walking", false);
                    animator.SetBool("Jumping", false);
                    animator.SetBool("Dashing", false);
                    animator.SetBool("Damage", false);
                    animator.SetBool("Death", false);
                    animator.SetBool("Walk Continuation", false);
                    currentSwordThrowDurationTime = swordThrowDurationTime;
                }

                if (currentSwordThrowDurationTime <= 0 && attackingRight)
                {
                    sword.SetActive(true);
                    rbSword.velocity = Vector2.right * speedSword;
                    attacking = false;
                    attackingRight = false;
                    animator.SetBool("Attacking", false);
                    animator.SetBool("Walking", false);
                    animator.SetBool("Jumping", false);
                    animator.SetBool("Dashing", false);
                    animator.SetBool("Damage", false);
                    animator.SetBool("Death", false);
                    animator.SetBool("Walk Continuation", false);
                    currentSwordThrowDurationTime = swordThrowDurationTime;
                }
            }

            if (sword.activeInHierarchy)
            {
                if ((sword.transform.position.x <= (swordPosition.x - rangeSword)) || (sword.transform.position.x <= leftBoundary) || (sword.transform.position.x >= (swordPosition.x + rangeSword)) || (sword.transform.position.x >= rightBoundary) && !returnSword)
                {
                    rbSword.velocity = Vector2.zero;
                    returnSword = true;
                }

                if (returnSword)
                {
                    rbSword.velocity = Vector2.zero;
                    sword.transform.position = Vector2.MoveTowards(sword.transform.position, transform.position, speedSword * Time.deltaTime);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && (jumpTimes > 0) && ((!Input.GetKey("left") && !Input.GetKey("right")) || (Input.GetKey("left") && Input.GetKey("right"))) && !attacking && !dashLeft && !dashRight && !damage)
            {
                animator.SetBool("Jumping", true);
                animator.SetBool("Walking", false);
                animator.SetBool("Attacking", false);
                animator.SetBool("Dashing", false);
                animator.SetBool("Damage", false);
                animator.SetBool("Death", false);
                animator.SetBool("Walk Continuation", false);
                jumpTimes --;
                jump = true;
                rbPlayer.velocity = Vector2.zero;
                rbPlayer.velocity = Vector2.up * jumpForce;
            }

            if (Input.GetKeyDown(KeyCode.Space) && (jumpTimes > 0) && Input.GetKey("left") && !Input.GetKey("right") && !attacking && !dashLeft && !dashRight && !damage)
            {
                jump = true;
                animator.SetBool("Jumping", true);
                animator.SetBool("Walking", false);
                animator.SetBool("Attacking", false);
                animator.SetBool("Dashing", false);
                animator.SetBool("Damage", false);
                animator.SetBool("Death", false);
                animator.SetBool("Walk Continuation", false);
                jumpTimes --;
                rbPlayer.velocity = Vector2.zero;
                rbPlayer.AddForce(new Vector2(-speedPlayer, jumpForce), ForceMode2D.Impulse);
            }

            if (Input.GetKeyDown(KeyCode.Space) && (jumpTimes > 0) && !Input.GetKey("left") && Input.GetKey("right") && !attacking && !dashLeft && !dashRight && !damage)
            {
                jump = true;
                animator.SetBool("Jumping", true);
                animator.SetBool("Walking", false);
                animator.SetBool("Attacking", false);
                animator.SetBool("Dashing", false);
                animator.SetBool("Damage", false);
                animator.SetBool("Death", false);
                animator.SetBool("Walk Continuation", false);
                jumpTimes --;
                rbPlayer.velocity = Vector2.zero;
                rbPlayer.AddForce(new Vector2(speedPlayer, jumpForce), ForceMode2D.Impulse);
            }

            if (Input.GetKeyDown(KeyCode.RightShift) && (dashTimes > 0) && (transform.localScale.x == facingLeft.x) && !attacking && !jump && !damage && !doubleButton && !sword.activeInHierarchy)
            {
                animator.SetBool("Dashing", true);
                animator.SetBool("Walking", false);
                animator.SetBool("Attacking", false);
                animator.SetBool("Jumping", false);
                animator.SetBool("Damage", false);
                animator.SetBool("Death", false);
                animator.SetBool("Walk Continuation", false);
                dashTimes --;
                dashLeft = true;
                rbPlayer.velocity = Vector2.zero;
                rbPlayer.velocity = Vector2.left * dashForce;
            }

            if (Input.GetKeyDown(KeyCode.RightShift) && (dashTimes > 0) && (transform.localScale.x == facingRight.x) && !attacking && !jump && !damage && !doubleButton && !sword.activeInHierarchy)
            {
                animator.SetBool("Dashing", true);
                animator.SetBool("Walking", false);
                animator.SetBool("Attacking", false);
                animator.SetBool("Jumping", false);
                animator.SetBool("Damage", false);
                animator.SetBool("Death", false);
                animator.SetBool("Walk Continuation", false);
                dashTimes --;
                dashRight = true;
                rbPlayer.velocity = Vector2.zero;
                rbPlayer.velocity = Vector2.right * dashForce;
            }

            if (dashLeft || dashRight)
            {
                currentDashDurationTime -= Time.deltaTime;
            }

            if (!dashLeft && !dashRight)
            {
                currentDashDurationTime = dashDurationTime;
            }

            if (currentDashDurationTime <= 0)
            {
                rbPlayer.velocity = Vector2.zero;
                dashLeft = false;
                dashRight = false;
                dashTimes = 1;
                animator.SetBool("Dashing", false);
                animator.SetBool("Walking", false);
                animator.SetBool("Attacking", false);
                animator.SetBool("Jumping", false);
                animator.SetBool("Damage", false);
                animator.SetBool("Death", false);
                animator.SetBool("Walk Continuation", false);
                currentDashDurationTime = dashDurationTime;
            }

            if (currentDashDurationTime > 0 && dashLeft)
            {
                if (Input.GetKeyDown(KeyCode.Space) && Input.GetKey("left"))
                {
                    rbPlayer.velocity = Vector2.zero;
                    rbPlayer.AddForce(new Vector2(-dashForce, jumpForce), ForceMode2D.Impulse);
                    jump = true;
                    dashLeft = false;
                    jumpDash = true;
                    jumpTimes = 0;
                    dashTimes = 1;
                    animator.SetBool("Jumping", true);
                    animator.SetBool("Walking", false);
                    animator.SetBool("Attacking", false);
                    animator.SetBool("Dashing", false);
                    animator.SetBool("Damage", false);
                    animator.SetBool("Death", false);
                    animator.SetBool("Walk Continuation", false);
                }
            }

            if (currentDashDurationTime > 0 && dashRight)
            {
                if (Input.GetKeyDown(KeyCode.Space) && Input.GetKey("right"))
                {
                    rbPlayer.velocity = Vector2.zero;
                    rbPlayer.AddForce(new Vector2(dashForce, jumpForce), ForceMode2D.Impulse);
                    jump = true;
                    dashRight = false;
                    jumpDash = true;
                    jumpTimes = 0;
                    dashTimes = 1;
                    animator.SetBool("Jumping", true);
                    animator.SetBool("Walking", false);
                    animator.SetBool("Attacking", false);
                    animator.SetBool("Dashing", false);
                    animator.SetBool("Damage", false);
                    animator.SetBool("Death", false);
                    animator.SetBool("Walk Continuation", false);
                }
            }

            if (currentDashDurationTime > 0 && dashLeft || dashRight)
            {
                if (Input.GetKeyDown(KeyCode.Space) && !Input.GetKey("left") && !Input.GetKey("right"))
                {
                    rbPlayer.velocity = Vector2.zero;
                    rbPlayer.velocity = Vector2.up * jumpForce;
                    jump = true;
                    dashLeft = false;
                    dashRight = false;
                    jumpTimes --;
                    dashTimes = 1;
                    animator.SetBool("Jumping", true);
                    animator.SetBool("Walking", false);
                    animator.SetBool("Attacking", false);
                    animator.SetBool("Dashing", false);
                    animator.SetBool("Damage", false);
                    animator.SetBool("Death", false);
                    animator.SetBool("Walk Continuation", false);
                }
            }
        }

        if (death)
        {
            currentDeathDurationTime -= Time.deltaTime;
            animator.SetBool("Death", true);
            animator.SetBool("Walking", false);
            animator.SetBool("Attacking", false);
            animator.SetBool("Jumping", false);
            animator.SetBool("Dashing", false);
            animator.SetBool("Damage", false);
            animator.SetBool("Walk Continuation", false);
            if (currentDeathDurationTime <= 0)
            {
                sword.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.gameObject.tag)
        {
            case "Sword":
                sword.SetActive(false);
                returnSword = false;
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Floor":
                jump = false;
                jumpDash = false;
                jumpTimes = 1;
                rbPlayer.velocity = Vector2.zero;
                damage = false;
                animator.SetBool("Walking", false);
                if (!attacking)
                {
                    animator.SetBool("Attacking", false);
                }
                
                animator.SetBool("Jumping", false);
                animator.SetBool("Dashing", false);
                if (life > 0)
                {
                    animator.SetBool("Damage", false);
                }

                if (life <= 0)
                {
                    death = true;
                }
                animator.SetBool("Walk Continuation", false);
                break;
        }
    }
}