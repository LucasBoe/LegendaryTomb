using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask walkable;
    [SerializeField] float maxSpeed = 10, accelerationMultiplier = 0.5f;

    public Vector3 direction;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            LayerMask layerMask = walkable;
            if (Physics.Raycast(ray, out hit, 100, walkable))
            {
                StartWalking(hit.point);
            }
        }
    }

    private void StartWalking(Vector3 target)
    {
        StopAllCoroutines();
        StartCoroutine(WalkingRoutine(target));
    }

    private IEnumerator WalkingRoutine(Vector3 target)
    {
        Vector3 velocity = Vector3.zero;
        Vector3 rotationVelocity = Vector3.zero;
        Vector3 targetDir = (transform.position - target).normalized;

        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            Debug.DrawLine(transform.position, transform.position + direction, Color.yellow);
            Debug.DrawLine(transform.position, transform.position + transform.forward, Color.green);

            transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, Time.deltaTime * accelerationMultiplier, maxSpeed);
            direction = Vector3.SmoothDamp(direction, targetDir, ref rotationVelocity, Time.deltaTime, 25f);
            yield return null;
        }
    }
}
