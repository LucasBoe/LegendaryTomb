using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Material matPlayer;
    [SerializeField] Transform plane;

    // Update is called once per frame
    void Update()
    {
        float direction = Vector3.SignedAngle(Vector3.right, playerController.direction, Vector3.up) / 360f;
        matPlayer.SetFloat("direction", direction);
        plane.localPosition = Vector3.up * Mathf.Sin(Time.time * 6) * 0.1f;
    }
}
