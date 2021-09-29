using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AutoTile : MonoBehaviour
{
    [SerializeField] Material matFloor, matWalls, matCave, matTransparent;

    public Dictionary<Vector3, AutoTile> Neightbours = new Dictionary<Vector3, AutoTile>();
    [OnValueChanged("OnTypeChangedCallback")]
    public TileType Type;

    private void Start()
    {
        UpdateNeightbours();
    }

    public void UpdateNeightbours()
    {
        Neightbours.Clear();

        foreach (Collider collider in Physics.OverlapSphere(transform.position + Vector3.down, 2))
        {
            Debug.Log("Hit..." + collider);


            AutoTile autoTile = collider.GetComponent<AutoTile>();
            if (autoTile != null && autoTile != this && !Neightbours.ContainsValue(autoTile))
            {
                Vector3 direction = (autoTile.transform.position - transform.position).normalized;
                if (!Neightbours.ContainsKey(direction) && AutoTileDirection.IsAllowed(direction))
                {
                    Neightbours.Add(direction, autoTile);
                    Debug.DrawLine(transform.position + Vector3.down, collider.transform.position + Vector3.down, Color.gray, 0.1f);
                }
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

        DebugDraw.Cuboid(new Bounds(transform.position + Vector3.down, Vector3.one * 0.1f), Color.gray, 0.5f);

        meshRenderer.sharedMaterials = materials;
    }

    private Material GetMaterial(AutoTile neightbour, AutoTileDirection direction)
    {
        if (direction.Direction == Vector3.down)
            return (Type == TileType.Tomb) ? matFloor : matCave;
        else
            return (Type == TileType.Tomb) ? matWalls : matCave;
    }

    public void UpdateNeightboursNeightbours()
    {
        UpdateNeightbours();
        foreach (AutoTile n in Neightbours.Values)
        {
            n.UpdateNeightbours();
        }
    }

    public void OnTypeChangedCallback()
    {
        UpdateNeightboursNeightbours();
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

    public static bool IsAllowed(Vector3 directionToCheck)
    {
        foreach (AutoTileDirection dir in List)
        {
            if (dir.Direction == directionToCheck)
                return true;
        }

        return false;
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