using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AutoTile : MonoBehaviour
{
    [SerializeField] Material matFloor, matWalls, matCave, matTransparent;

    public Dictionary<Vector3, AutoTile> Neightbours = new Dictionary<Vector3, AutoTile>();

    public void UpdateNeightbours()
    {
        Neightbours.Clear();

        foreach (Collider collider in Physics.OverlapSphere(transform.position + Vector3.down, 2))
        {
            Debug.Log("Hit..." + collider);

            Debug.DrawLine(transform.position, collider.transform.position, Color.green, 0.5f);

            AutoTile autoTile = collider.GetComponent<AutoTile>();
            if (autoTile != null && autoTile != this && !Neightbours.ContainsValue(autoTile))
            {
                Vector3 direction = (autoTile.transform.position - transform.position).normalized;
                if (!Neightbours.ContainsKey(direction))
                    Neightbours.Add(direction, autoTile);
            }
        }

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        Material[] materials = meshRenderer.sharedMaterials;

        foreach (var direction in AutoTileDirection.List)
        {

            bool hasNeightbour = Neightbours.ContainsKey(direction.Direction);
            AutoTile neightbour = hasNeightbour ? Neightbours[direction.Direction] : null;

            Debug.Log("check direction has neightbour:" + hasNeightbour);

            if (direction.MaterialIndex != -1)
                materials[direction.MaterialIndex] = (hasNeightbour) ? matTransparent : GetMaterial(neightbour, direction);
        }

        DebugDraw.Cuboid(new Bounds(transform.position, Vector3.one), Color.green, 0.5f);

        meshRenderer.sharedMaterials = materials;
    }

    private Material GetMaterial(AutoTile neightbour, AutoTileDirection direction)
    {
        return matCave;

        if (direction.Direction == Vector3.down)
            return matFloor;
        else
            return matWalls;
    }

    public void UpdateNeightboursNeightbours()
    {
        UpdateNeightbours();
        foreach (AutoTile n in Neightbours.Values)
        {
            n.UpdateNeightbours();
        }
    }
}

[System.Serializable]
public class AutoTileDirection
{
    public Vector3 Direction;
    public string Symbol;
    public int MaterialIndex;

    public AutoTileDirection(Vector3 direction, string symbol, int materialIndex)
    {
        Direction = direction;
        Symbol = symbol;
        MaterialIndex = materialIndex;
    }

    public static AutoTileDirection[] List = new AutoTileDirection[6]
    {
        new AutoTileDirection(Vector3.forward, "\u2196", 0),
        new AutoTileDirection(Vector3.right, "\u2197", 1),
        new AutoTileDirection(Vector3.up, "\u21D1", -1),
        new AutoTileDirection(Vector3.left, "\u2199", 3),
        new AutoTileDirection(Vector3.back, "\u2198", 4),
        new AutoTileDirection(Vector3.down, "\u21D3", 2)
    };
}