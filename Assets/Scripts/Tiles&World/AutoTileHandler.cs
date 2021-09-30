using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTileHandler : Singleton<AutoTileHandler>
{
    [SerializeField] private int regiesteredTileCounter = 0;

    private const int size = 100;
    private AutoTile[,,] tiles = new AutoTile[size * 2, size * 2, size * 2];

    public void Register(AutoTile autoTile)
    {
        Vector3Int position = WorldSpaceToRegistry(autoTile.transform.position);
        tiles[position.x, position.y, position.z] = autoTile;
        regiesteredTileCounter++;
    }

    public bool IsEmpty(Vector3 pos)
    {
        return Fetch(pos) == null;
    }

    public AutoTile Fetch(Vector3 pos)
    {
        Vector3Int position = WorldSpaceToRegistry(pos);

        if (OutsideRange(position))
        {
            Debug.Log("Tried to fetch tile from outside of range.");
            return null;
        }

        return tiles[position.x, position.y, position.z];
    }

    internal Dictionary<Vector3, AutoTile> FetchNeightbours(Vector3 position)
    {
        Dictionary<Vector3, AutoTile> d = new Dictionary<Vector3, AutoTile>();

        foreach (var dir in AutoTileDirection.List)
        {
            Vector3 pos = position + dir.Direction * 2;
            AutoTile fetch = Fetch(pos);

            if (fetch != null)
                d.Add(dir.Direction, fetch);
        }

        return d;
    }


    private bool OutsideRange(Vector3Int position)
    {
        foreach (int i in position.ToArray())
        {
            if (i < 0 || i >= (size * 2))
                return true;
        }

        return false;
    }

    public static Vector3Int WorldSpaceToRegistry(Vector3 input)
    {
        return new Vector3Int(
            Mathf.RoundToInt(input.x / 2f) + size,
            Mathf.RoundToInt(input.y / 2f) + size,
            Mathf.RoundToInt(input.z / 2f) + size);
    }

    public static Vector3Int RegistrySpaceToWorld(Vector3Int input)
    {
        Vector3Int output = (input - new Vector3Int(size, size, size)) * 2;
        return output;
    }
}
