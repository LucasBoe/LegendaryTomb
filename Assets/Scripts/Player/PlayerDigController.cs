using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDigController : MonoBehaviour
{
    [SerializeField] AutoTile tilePrefab;
    [SerializeField] PlayerController playerController;
    [SerializeField] Material targetIndicatorMaterial;
    bool facingTile;
    Vector3Int tileFacing;

    MeshRenderer targetIndicator;

    private void Awake()
    {

        targetIndicator = GameObject.CreatePrimitive(PrimitiveType.Cube).GetComponent<MeshRenderer>();
        targetIndicator.transform.localScale = new Vector3(1.99f, 1.99f, 1.99f);
        targetIndicator.material = targetIndicatorMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position - playerController.Direction.normalized;
        Vector3Int inReg = AutoTileHandler.WorldSpaceToRegistry(pos);
        Vector3Int ownReg = AutoTileHandler.WorldSpaceToRegistry(transform.position);
        facingTile = (
            inReg != ownReg
            && Vector3Int.Distance(inReg, ownReg) <= 1 //sort out diagonals
            && (Game.AutoTileHandler.IsEmpty(pos) || Game.AutoTileHandler.Fetch(pos).Air == false) //no tile or hidden tile
            );

        Vector3Int facingTilePosition = AutoTileHandler.RegistrySpaceToWorld(inReg);

        UpdateDigTarget(facingTilePosition);
    }

    private void UpdateDigTarget(Vector3Int newDigTarget)
    {
        if (newDigTarget != tileFacing)
        {
            targetIndicator.transform.position = newDigTarget;

            if (newDigTarget != null && facingTile)
            {
                targetIndicator.enabled = true;
                Game.UIHandler.PromptHandler.Show(targetIndicator.transform, "press #Fire1 to dig");
            }
            else
            {
                targetIndicator.enabled = false;
                Game.UIHandler.PromptHandler.Hide(targetIndicator.transform);
            }

            tileFacing = newDigTarget;
        }

        if (tileFacing != null && facingTile)
        {
            if (Input.GetButtonUp("Fire1"))
            {
                StartDig(tileFacing);
            }
        }
    }

    private void StartDig(Vector3Int tileFacing)
    {
        AutoTile targetTile = Game.AutoTileHandler.Fetch(tileFacing);
        if (targetTile == null)
        {
            targetTile = Instantiate(tilePrefab, tileFacing, Quaternion.identity);
            targetTile.Type = TileType.Cave;
            targetTile.Air = false;
        }

        targetTile.StartDig();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (facingTile)
        {
            Gizmos.DrawCube(tileFacing, new Vector3(1.99f, 1.99f, 1.99f));
        }
        else
        {
            Gizmos.DrawWireCube(tileFacing, new Vector3(1.99f, 1.99f, 1.99f));
        }
    }
}
