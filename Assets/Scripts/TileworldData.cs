using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TileworldData : ScriptableObject
{
    public const int xSize = 40;
    public const int ySize = 40;
    public const int zSize = 10;

    public int zOffset => xSize * ySize;
    public int yOffset => xSize;

    public Tile[] Tiles = new Tile[xSize * ySize * zSize];

    public Tile this[int x, int y, int z]
    {
        get => Tiles[z * zOffset + y * yOffset + x];
        set => Tiles[z * zOffset + y * yOffset + x] = value;
    }

    public Vector3Int this[int i]
    {
        get
        {
            int z = (int)(i / (float)zOffset);
            i -= (int)(z * (float)zOffset);
            int y = (int)(i / (float)yOffset);
            int x = (int)(i % (float)yOffset);
            return new Vector3Int( x * 2, y * 2, z * 2);
        }
    }

    public Tile[] Test = new Tile[10];

    internal Tile GetTile(int x, int y, int z)
    {
        return this[x, y, z];
    }

    public Vector3Int locationToSet;
    public bool airToSet;
    public bool visibleToSet;
    public TileType typeToSet;

    [Button]
    public void Set()
    {
        if (this[locationToSet.x, locationToSet.y, locationToSet.z] == null)
            this[locationToSet.x, locationToSet.y, locationToSet.z] = new Tile();

        this[locationToSet.x, locationToSet.y, locationToSet.z].Air = airToSet;
        this[locationToSet.x, locationToSet.y, locationToSet.z].Visible = visibleToSet;
        this[locationToSet.x, locationToSet.y, locationToSet.z].Type = typeToSet;
    }

    [Button]
    void Fill()
    {
        //Tiles = new Tile[xSize, ySize, zSize];

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                for (int z = 0; z < zSize; z++)
                {
                    this[x, y, z] = new Tile();
                }
            }
        }
    }

}

[System.Serializable]
public class Tile
{
    public TileType Type;
    public bool Air;
    public bool Visible;
    public MeshRenderer MeshRenderer;

    public static Vector3 ToVector(int x, int y, int z)
    {
        return new Vector3(x * 2, y * 2, z * 2);
    }
}

public enum TileType
{
    Tomb,
    Cave,
}
