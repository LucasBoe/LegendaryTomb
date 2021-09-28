using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TileworldRenderer : MonoBehaviour
{
    [SerializeField] TileworldData world;
    [SerializeField] MeshRenderer tilePrefab;
    [SerializeField] Material matFloor, matHWalls, matVWalls, matTransparent;

    public void CreateRoom(Vector3Int[] roomTiles)
    {
        List<MeshRenderer> newMeshRenderers = new List<MeshRenderer>();

        foreach (Vector3Int newTile in roomTiles)
        {
            int x = newTile.x / 2;
            int y = newTile.y / 2;
            int z = newTile.z / 2;

            world[x, y, z].Air = true;
            world[x, y, z].Visible = true;
        }

        UpdateAllTiles();
    }

    internal void CreateRoom(List<Vector3Int> tilesTouched, int stories)
    {
        List<Vector3Int> list = new List<Vector3Int>();
        list.AddRange(tilesTouched);

        for (int i = 0; i < stories; i++)
        {
            foreach (Vector3Int touched in tilesTouched)
            {
                list.Add(touched + Vector3Int.up * 2 * i);
            }
        }

        CreateRoom(list.ToArray());
    }

    internal void CreateRoom(List<Vector3Int> tilesTouched, List<Vector3Int> tilesFilled, int stories)
    {
        List<Vector3Int> list = new List<Vector3Int>();
        list.AddRange(tilesFilled);
        list.AddRange(tilesTouched);

        for (int i = 0; i < stories; i++)
        {
            foreach (Vector3Int touched in tilesTouched)
            {
                list.Add(touched + Vector3Int.up * 2 * i);
            }
        }
   
        CreateRoom(list.ToArray());
    }

    [Button]
    public void UpdateAllTiles()
    {
        for (int x = 0; x < TileworldData.xSize; x++)
        {
            for (int y = 0; y < TileworldData.ySize; y++)
            {
                for (int z = 0; z < TileworldData.zSize; z++)
                {
                    UpdateTile(x, y, z);
                }
            }
        }
    }

    private void UpdateTile(int x, int y, int z)
    {
        Tile tile = world[x, y, z];

        if (tile.Air && tile.MeshRenderer == null)
            tile.MeshRenderer = Instantiate(tilePrefab, Tile.ToVector(x, y, z), Quaternion.identity, transform);


        if (tile.Air && tile.Visible)
        {
            tile.MeshRenderer.gameObject.SetActive(true);

            Material[] materials = tile.MeshRenderer.sharedMaterials;

            bool notBelow = y == 0 || !world[x, y - 1, z].Air;
            bool back = z == 0 || !world[x, y, z - 1].Air;
            bool front = z == TileworldData.zSize || !world[x, y, z + 1].Air;
            bool left = x == 0 || !world[x - 1, y, z].Air;
            bool right = x == TileworldData.xSize || !world[x + 1, y, z].Air;

            if (!notBelow)
            {
                materials[2] = matTransparent;
                DestroyImmediate(tile.MeshRenderer.GetComponent<BoxCollider>());
                y--;
            }
            else
            {
                materials[2] = matFloor;
            }

            materials[4] = back ? matHWalls : matTransparent;
            materials[3] = left ? matVWalls : matTransparent;
            materials[0] = front ? matHWalls : matTransparent;
            materials[1] = right ? matVWalls : matTransparent;

            tile.MeshRenderer.sharedMaterials = materials;
        }
        else
        {
            if (tile.MeshRenderer != null)
                tile.MeshRenderer.gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (world.Tiles == null)
        {
            Debug.Log("No Tiles...");
            return;
        }

        int count = 0;

        for (int i = 0; i < world.Tiles.Length; i++)
        {
            Tile tile = world.Tiles[i];
            if (tile != null && tile.Air)
            {
                count++;
                Gizmos.color = tile.Type == TileType.Tomb ? Color.yellow : Color.white;
                Gizmos.DrawCube(world[i], Vector3.one * 2);
            }
        }

        Debug.Log("count: " + count);

        Gizmos.DrawWireCube(new Vector3(TileworldData.xSize, TileworldData.ySize, TileworldData.zSize), new Vector3(TileworldData.xSize * 2, TileworldData.ySize * 2, TileworldData.zSize * 2));
    }

}
