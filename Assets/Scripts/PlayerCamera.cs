using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private float MovementDamp;

    private void OnEnable()
    {
        if (Player == null)
            Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(Player.position.x, Player.position.y, transform.position.z), Time.deltaTime * MovementDamp);
    }
}
