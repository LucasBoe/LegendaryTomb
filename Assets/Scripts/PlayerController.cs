using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask walkable;
    [SerializeField] float maxSpeed = 10, accelerationMultiplier = 0.5f;

    public Vector3 direction;

    Vector3 velocity = Vector3.zero;
    Vector3 rotationVelocity = Vector3.zero;

    private void Update()
    {
        Vector3 targetDir = Game.UIHandler.WheelMenu.Open ? Vector3.zero : Game.InputManager.GetJoystickVector3();

        if (targetDir != Vector3.zero)
            direction = Vector3.SmoothDamp(direction, -targetDir, ref rotationVelocity, Time.deltaTime, 25f);


        bool groundInFront = false;
        bool obstacleInFront = false;

        Ray ray = new Ray(transform.position + (direction * 0.1f) + Vector3.up, Vector3.down - direction * 0.6f);
        foreach (RaycastHit hit in Physics.RaycastAll(ray, 2, LayerMask.GetMask("Walkable", "Obstacle")))
        {

            int layer = hit.collider.gameObject.layer;

            if (layer == LayerMask.NameToLayer("Walkable"))
                groundInFront = true;
            else if (layer == LayerMask.NameToLayer("Obstacle"))
                obstacleInFront = true;
        }

        if (groundInFront && !obstacleInFront)
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.green);
            transform.position = Vector3.SmoothDamp(transform.position, transform.position + targetDir, ref velocity, Time.deltaTime * accelerationMultiplier, maxSpeed);
        } else
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.red);
        }
    }
}
