using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask walkable;
    [SerializeField] LayerMask obstacle;
    [SerializeField] float maxSpeed = 10, accelerationMultiplier = 0.5f;

    public Vector3 Direction;
    [SerializeField] public PlayerActionManager ActionManager;
    [SerializeField] public PlayerData PlayerData;

    Vector3 velocity = Vector3.zero;
    Vector3 rotationVelocity = Vector3.zero;

    private void Update()
    {
        if (PlayerHandler.ActivePlayer == this&& !ActionManager.InAction)
            MovementUpdate();
    }

    private void MovementUpdate()
    {
        Vector3 targetDir = Game.UIHandler.WheelMenu.Open ? Vector3.zero : Quaternion.Euler(0, 45, 0) * Game.InputManager.GetJoystickVector3();

        if (targetDir != Vector3.zero)
            Direction = Vector3.SmoothDamp(Direction, -targetDir, ref rotationVelocity, Time.deltaTime, 25f);


        bool groundInFront = false;
        bool obstacleInFront = false;

        Ray ray = new Ray(transform.position + (Direction * 0.1f) + Vector3.up, Vector3.down - Direction * 0.6f);
        foreach (RaycastHit hit in Physics.RaycastAll(ray, 2, LayerMask.GetMask("Walkable", "Obstacle")))
        {

            int layer = hit.collider.gameObject.layer;

            if (walkable.Contains(layer))
                groundInFront = true;
            else if (obstacle.Contains(layer))
                obstacleInFront = true;
        }

        if (groundInFront && !obstacleInFront)
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.green);
            transform.position = Vector3.SmoothDamp(transform.position, transform.position + targetDir, ref velocity, Time.deltaTime * accelerationMultiplier, maxSpeed * Direction.magnitude);
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.red);
        }
    }
}
