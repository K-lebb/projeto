using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    public GameObject power, player;
    public float horizontalDistance, upDistance, downDistance, attackSpeed, rangePower;
    public Rigidbody2D rbPower;
    private int life;
    private float attackDurationTime, currentAttackDurationTime, fullAttackDurationTime, currentFullAttackDurationTime, powerDurationTime, currentPowerDurationTime, damageDurationTime, currentDamageDurationTime, deathDurationTime, currentDeathDurationTime;
    private bool attackPower, attacking, canThrow, damage, lowLife, death, deathEmpty;
    private Animator animator;
    private Vector2 playerPosition, powerPosition, facingRight, facingLeft, facingRightPower, facingLeftPower;

    void Start()
    {
        if (player.transform.localScale.x < 0)
        {
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                if (power.transform.localScale.x < 0)
                {
                    power.transform.localScale = new Vector2(-power.transform.localScale.x, power.transform.localScale.y);
                }

                if (power.transform.localScale.x > 0)
                {
                    power.transform.localScale = new Vector2(power.transform.localScale.x, power.transform.localScale.y);
                }
            }

            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
                if (power.transform.localScale.x < 0)
                {
                    power.transform.localScale = new Vector2(-power.transform.localScale.x, power.transform.localScale.y);
                }

                if (power.transform.localScale.x > 0)
                {
                    power.transform.localScale = new Vector2(power.transform.localScale.x, power.transform.localScale.y);
                }
            }

            facingLeft = new Vector2(-transform.localScale.x, transform.localScale.y);
            facingRight = new Vector2(transform.localScale.x, transform.localScale.y);
            facingLeftPower = new Vector2(-power.transform.localScale.x, power.transform.localScale.y);
            facingRightPower = new Vector2(power.transform.localScale.x, power.transform.localScale.y);
        }

        if (player.transform.localScale.x > 0)
        {
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
                if (power.transform.localScale.x < 0)
                {
                    power.transform.localScale = new Vector2(power.transform.localScale.x, power.transform.localScale.y);
                }

                if (power.transform.localScale.x > 0)
                {
                    power.transform.localScale = new Vector2(-power.transform.localScale.x, power.transform.localScale.y);
                }
            }

            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                if (power.transform.localScale.x < 0)
                {
                    power.transform.localScale = new Vector2(power.transform.localScale.x, power.transform.localScale.y);
                }

                if (power.transform.localScale.x > 0)
                {
                    power.transform.localScale = new Vector2(-power.transform.localScale.x, power.transform.localScale.y);
                }
            }

            facingLeft = new Vector2(transform.localScale.x, transform.localScale.y);
            facingRight = new Vector2(-transform.localScale.x, transform.localScale.y);
            facingLeftPower = new Vector2(power.transform.localScale.x, power.transform.localScale.y);
            facingRightPower = new Vector2(-power.transform.localScale.x, power.transform.localScale.y);
        }

        animator = GetComponent<Animator>();
        life = 4;
        attackDurationTime = 0.8f;
        currentAttackDurationTime = attackDurationTime;
        fullAttackDurationTime = 11f / 7f;
        currentFullAttackDurationTime = fullAttackDurationTime;
        powerDurationTime = 1.7f;
        currentPowerDurationTime = powerDurationTime;
        damageDurationTime = 1;
        currentDamageDurationTime = damageDurationTime;
        deathDurationTime = 1;
        currentDeathDurationTime = deathDurationTime;
        attackPower = false;
        attacking = false;
        canThrow = true;
        power.SetActive(false);
        damage = false;
        lowLife = false;
        death = false;
        deathEmpty = false;
    }

    void Update()
    {
        if (!death)
        {
            if (!attackPower)
            {
                playerPosition = player.transform.position;
                currentAttackDurationTime = attackDurationTime;
            }

            if ((player.transform.position.x <= transform.position.x) && !attackPower && !damage)
            {
                transform.localScale = facingLeft;
                if ((playerPosition.x >= (transform.position.x - horizontalDistance)) && (playerPosition.y <= (transform.position.y + upDistance)) && (playerPosition.y >= (transform.position.y - downDistance)) && !attackPower && !power.activeInHierarchy)
                {
                    animator.SetBool("Attacking", true);
                    animator.SetBool("Damage", false);
                    animator.SetBool("Death", false);
                    attackPower = true;
                    attacking = true;
                }
            }

            if ((player.transform.position.x >= transform.position.x) && !attackPower && !damage)
            {
                transform.localScale = facingRight;
                if ((playerPosition.x <= (transform.position.x + horizontalDistance)) && (playerPosition.y <= (transform.position.y + upDistance)) && (playerPosition.y >= (transform.position.y - downDistance)) && !attackPower && !power.activeInHierarchy)
                {
                    animator.SetBool("Attacking", true);
                    animator.SetBool("Damage", false);
                    animator.SetBool("Death", false);
                    attackPower = true;
                    attacking = true;
                }
            }

            if (attackPower && !damage)
            {
                currentAttackDurationTime -= Time.deltaTime;
                if (currentAttackDurationTime <= 0 && attacking && canThrow && transform.localScale.x < 0 && life != 0)
                {
                    power.SetActive(true);
                    powerPosition = power.transform.position;
                    rbPower.velocity = Vector2.left * attackSpeed;
                    canThrow = false;
                }

                if (currentAttackDurationTime <= 0 && attacking && canThrow && transform.localScale.x > 0 && life != 0)
                {
                    power.SetActive(true);
                    powerPosition = power.transform.position;
                    rbPower.velocity = Vector2.right * attackSpeed;
                    canThrow = false;
                }
            }

            if (attacking)
            {
                currentFullAttackDurationTime -= Time.deltaTime;
                if (currentFullAttackDurationTime <= 0)
                {
                    animator.SetBool("Attacking", false);
                    animator.SetBool("Damage", false);
                    animator.SetBool("Death", false);
                    attackPower = false;
                    attacking = false;
                    canThrow = true;
                    currentFullAttackDurationTime = fullAttackDurationTime;
                }
            }

            if (!Player.invincibility && (Math.Round(player.transform.position.x, 1)) >= (Math.Round(transform.position.x - 1.9f, 1)) && (Math.Round(player.transform.position.x, 1)) <= (Math.Round(transform.position.x + 1.4f, 1)) && (Math.Round(player.transform.position.y, 5)) >= (Math.Round(transform.position.y - 1.95778f, 5)) && (Math.Round(player.transform.position.y, 5)) <= (Math.Round(transform.position.y + 3.08414f, 5)))
            {
                Player.damage = true;
                Player.life -= 2;
                Debug.Log(Player.life);
                if (transform.localScale.x < 0)
                {
                    Player.damageLeft = true;
                }

                if (transform.localScale.x > 0)
                {
                    Player.damageRight = true;
                }

                Player.invincibility = true;
            }

            if (damage)
            {
                animator.SetBool("Damage", true);
                animator.SetBool("Attacking", false);
                currentDamageDurationTime -= Time.deltaTime;
                if (currentDamageDurationTime <= 0)
                {
                    animator.SetBool("Damage", false);
                    damage = false;
                    lowLife = true;
                    currentDamageDurationTime = damageDurationTime;
                }
            }

            if (life == 2 && !lowLife)
            {
                animator.SetBool("Damage", true);
                animator.SetBool("Attacking", false);
                damage = true;
            }

            if (life <= 0)
            {
                death = true;
                animator.SetBool("Death", true);
                animator.SetBool("Attacking", false);
                animator.SetBool("Damage", false);
            }
        }

        if (death)
        {
            currentDeathDurationTime -= Time.deltaTime;
            if (currentDeathDurationTime <= 0)
            {
                deathEmpty = true;
            }
        }

        if (deathEmpty)
        {
            if (power.activeInHierarchy)
            {
                animator.SetBool("Attacking", false);
                animator.SetBool("Damage", false);
                animator.SetBool("Death", false);
                animator.SetBool("Death Empty", true);
            }

            if (!power.activeInHierarchy)
            {
                Destroy(power);
                Destroy(gameObject);
            }
        }

        if (!power.activeInHierarchy)
        {
            currentPowerDurationTime = powerDurationTime;
            if (transform.localScale.x < 0)
            {
                power.transform.position = new Vector2(transform.position.x - 1.5f, transform.position.y + 0.5f);
                power.transform.localScale = facingLeftPower;
            }

            if (transform.localScale.x > 0)
            {
                power.transform.position = new Vector2(transform.position.x + 1.5f, transform.position.y + 0.5f);
                power.transform.localScale = facingRightPower;
            }
        }

        if (power.activeInHierarchy)
        {
            currentPowerDurationTime -= Time.deltaTime;
            if (power.transform.localScale.x < 0)
            {
                if (power.transform.position.x <= (powerPosition.x - rangePower))
                {
                    rbPower.velocity = Vector2.zero;
                    if (currentPowerDurationTime <= 0)
                    {
                        if (death)
                        {
                            Destroy(power);
                            Destroy(gameObject);
                        }

                        if (!death)
                        {
                            power.SetActive(false);
                        }
                    }
                }
            }

            if (power.transform.localScale.x > 0)
            {
                if (power.transform.position.x >= (powerPosition.x + rangePower))
                {
                    rbPower.velocity = Vector2.zero;
                    if (currentPowerDurationTime <= 0)
                    {
                        if (death)
                        {
                            Destroy(power);
                            Destroy(gameObject);
                        }

                        if (!death)
                        {
                            power.SetActive(false);
                        }
                    }
                }
            }
        }
    }
        
    private void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.gameObject.tag)
        {
            case "Sword":
                life --;
                break;
        }
    }
}