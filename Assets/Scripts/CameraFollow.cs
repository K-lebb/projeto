using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player, lowerLimit, upperLimit, leftLimit, rightLimit;
    public float timeLerp;

    private void LateUpdate()
    {
        if (player)
        {
            transform.position = Vector3.Lerp(transform.position, player.position, timeLerp) + new Vector3(0, 0, player.position.z);
        }

        transform.position = new Vector3(Mathf.Clamp(player.position.x, leftLimit.position.x + 14.012f, rightLimit.position.x - 14.012f), Mathf.Clamp(player.position.y, lowerLimit.position.y + -lowerLimit.position.y, upperLimit.position.y - upperLimit.position.y), Mathf.Clamp(0, 0, -10));
    }
}