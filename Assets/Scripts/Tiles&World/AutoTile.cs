using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AutoTile : MonoBehaviour
{
    [SerializeField] Material matFloor, matWalls, matCave, matTransparent;
    [SerializeField] MeshRenderer meshRenderer;

    public Dictionary<Vector3, AutoTile> Neightbours = new Dictionary<Vector3, AutoTile>();
    [HideInInspector] public TileType Type;
    [HideInInspector] public bool Air = true;
    [HideInInspector] public bool Visible = true;

    private void Awake()
    {
        Game.AutoTileHandler.Register(this);
    }

    private void Start()
    {
        UpdateNeightbours();
    }

    public void UpdateNeightbours()
    {
        if (Application.isPlaying)
            Neightbours = Game.AutoTileHandler.FetchNeightbours(transform.position);
        else
            Neightbours = DectedNeightboursByCollider();

        if (!Visible && Application.isPlaying)
            UpdateVisibility();

        DebugDraw.Cuboid(new Bounds(transform.position + Vector3.down, Vector3.one * 0.1f), Color.gray, 0.5f);

        if (Air && Visible)
        {
            SetVisualsActive(true);

            Material[] materials = meshRenderer.sharedMaterials;
            meshRenderer.sharedMaterials = UpdateMaterialsBasedOnNeightbours(materials);
        }
        else
        {
            SetVisualsActive(false);
            gameObject.layer = LayerMask.NameToLayer("Digable");
        }
    }

    public void UpdateVisibility()
    {
        foreach (var direction in AutoTileDirection.List)
        {
            if (Neightbours.ContainsKey(direction.Direction))
            {
                AutoTile tile = Neightbours[direction.Direction];
                if (tile.Air && tile.Visible)
                {
                    DebugDraw.Cuboid(new Bounds(transform.position, Vector3.one), Color.white, 0.1f);
                    Visible = true;
                }
            }
        }

        if (Visible)
        {
            foreach (AutoTile tile in Neightbours.Values)
            {
                tile.UpdateNeightbours();
            }
        }
    }

    private void SetVisualsActive(bool active)
    {
        meshRenderer.enabled = active;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(active);
        }
    }

    private Dictionary<Vector3, AutoTile> DectedNeightboursByCollider()
    {
        Dictionary<Vector3, AutoTile> neightboursDict = new Dictionary<Vector3, AutoTile>();

        foreach (Collider collider in Physics.OverlapSphere(transform.position + Vector3.down, 2))
        {
            AutoTile autoTile = collider.GetComponent<AutoTile>();
            if (autoTile != null && autoTile != this && !neightboursDict.ContainsValue(autoTile))
            {
                Vector3 direction = (autoTile.transform.position - transform.position).normalized;
                if (!neightboursDict.ContainsKey(direction) && AutoTileDirection.IsAllowed(direction))
                {
                    neightboursDict.Add(direction, autoTile);
                    Debug.DrawLine(transform.position + Vector3.down, collider.transform.position + Vector3.down, Color.gray, 0.1f);
                }
            }
        }

        return neightboursDict;
    }

    private Material[] UpdateMaterialsBasedOnNeightbours(Material[] materials)
    {
        foreach (var direction in AutoTileDirection.List)
        {
            bool hasNeightbour = Neightbours.ContainsKey(direction.Direction);
            AutoTile neightbour = hasNeightbour ? Neightbours[direction.Direction] : null;

            gameObject.layer = LayerMask.NameToLayer(!(direction.Direction == Vector3.down && hasNeightbour) ? "Walkable" : "Obstacle");

            if (direction.MaterialIndex != -1)
                materials[direction.MaterialIndex] = (hasNeightbour && neightbour.Air && neightbour.Visible) ? matTransparent : GetMaterial(neightbour, direction);
        }

        return materials;
    }

    public void StartDig()
    {
        PlayerAction digAction = new PlayerAction(InteractionType.Touch, 3, FinishDig);
        PlayerHandler.ActivePlayer.ActionManager.TryStart(digAction);
    }

    private void FinishDig()
    {
        Air = true;
        UpdateNeightboursNeightbours();
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


    private void OnDrawGizmos()
    {
        if (!Air)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawCube(transform.position, Vector3.one * 1.99f);
        }

        if (!Visible)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawCube(transform.position, Vector3.one * 1.99f);
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