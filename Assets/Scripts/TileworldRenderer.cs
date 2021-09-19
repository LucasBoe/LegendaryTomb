using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class TileworldRenderer : MonoBehaviour
{
    [SerializeField] TileworldData world;
    [SerializeField] MeshRenderer tilePrefab;
    [SerializeField] Material matFloor, matHWalls, matVWalls, matTransparent;

    public void CreateRoom(Transform parent, Vector3Int[] roomTiles)
    {
        List<MeshRenderer> newMeshRenderers = new List<MeshRenderer>();

        foreach (Vector3Int newTile in roomTiles)
        {
            MeshRenderer meshRenderer = Instantiate(tilePrefab, newTile, Quaternion.identity, parent);
            UpdateMeshRendererAccordingToNeightbours(meshRenderer, newTile, roomTiles);
        }
    }

    private void UpdateMeshRendererAccordingToNeightbours(MeshRenderer meshRenderer, Vector3Int tile, Vector3Int[] otherTiles)
    {
        Material[] materials = meshRenderer.sharedMaterials;

        if (otherTiles.Contains(tile + Vector3Int.down * 2))
        {
            materials[2] = matTransparent;
            DestroyImmediate(meshRenderer.GetComponent<BoxCollider>());
            tile = tile + Vector3Int.down * 2;
        }
        else
        {
            materials[2] = matFloor;
        }

        materials[4] = otherTiles.Contains(tile + Vector3Int.back * 2) ? matTransparent : matHWalls;
        materials[3] = otherTiles.Contains(tile + Vector3Int.left * 2) ? matTransparent : matVWalls;
        materials[0] = otherTiles.Contains(tile + Vector3Int.forward * 2) ? matTransparent : matHWalls;
        materials[1] = otherTiles.Contains(tile + Vector3Int.right * 2) ? matTransparent : matVWalls;

        meshRenderer.sharedMaterials = materials;
    }

    internal void CreateRoom(Transform parent, List<Vector3Int> tilesTouched, List<Vector3Int> tilesFilled, int stories)
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

        CreateRoom(parent, list.ToArray());
    }
}
