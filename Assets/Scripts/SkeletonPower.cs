using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonPower : MonoBehaviour
{
    public Transform skeleton, player;
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.gameObject.tag)
        {
            case "Player":
                if (!Player.invincibility && transform.position.x >= player.position.x)
                {
                    gameObject.SetActive(false);
                    Player.life -= 5;
                    Player.invincibility = true;
                    Player.damage = true;
                    Player.damageLeft = true;
                }

                if (!Player.invincibility && transform.position.x <= player.position.x)
                {
                    gameObject.SetActive(false);
                    Player.life -= 5;
                    Player.invincibility = true;
                    Player.damage = true;
                    Player.damageRight = true;
                }

                break;
        }
    }
}