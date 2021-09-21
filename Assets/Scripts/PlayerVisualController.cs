using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Material matPlayer;
    [SerializeField] Transform plane;
    [SerializeField] MeshRenderer healthBarRenderer;

    Material healthBarMaterial;

    private void Start()
    {
        healthBarMaterial = new Material(healthBarRenderer.material);
        healthBarRenderer.material = healthBarMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        float direction = Vector3.SignedAngle(Vector3.right, playerController.direction, Vector3.up) / 360f;
        matPlayer.SetFloat("direction", direction);
        plane.localPosition = Vector3.up * Mathf.Sin(Time.time * 6) * 0.1f;

        if (healthBarMaterial != null)
            healthBarMaterial.SetFloat("value", (float)playerController.PlayerData.health / (float)playerController.PlayerData.maxHealth);
    }
}
