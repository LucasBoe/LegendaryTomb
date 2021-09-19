using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RoomCreator : MonoBehaviour
{
    [SerializeField] TileworldRenderer tileworldRenderer;

    public int stories = 1;
    public Vector3[] cornerpoints;
    public List<Vector3Int> tilesTouched = new List<Vector3Int>();
    public List<Vector3Int> tilesFilled = new List<Vector3Int>();

    public void UpdateTouchingTiles()
    {
        tilesTouched.Clear();

        for (int i = 0; i < cornerpoints.Length; i++)
        {
            Vector3 current = cornerpoints[i];
            Vector3 next = i + 1 >= cornerpoints.Length ? cornerpoints[0] : cornerpoints[i + 1];

            float distance = Vector3.Distance(current, next);

            for (int j = 0; j < distance; j++)
            {
                Vector3 pointToCheck = Vector3.Lerp(current, next, (float)j / distance);
                Vector3Int onGrid = new Vector3Int(Gridify(pointToCheck.x), Gridify(pointToCheck.y), Gridify(pointToCheck.z));

                if (!tilesTouched.Contains(onGrid))
                    tilesTouched.Add(onGrid);
            }
        }
    }

    public void UpdateFillingTiles()
    {
        tilesFilled.Clear();

        Vector3 midpoint = Vector3.zero;

        foreach (Vector3 corner in cornerpoints)
        {
            midpoint += corner;
        }

        midpoint /= cornerpoints.Length;

        Vector3Int origin = new Vector3Int(Gridify(midpoint.x), Gridify(midpoint.y), Gridify(midpoint.z));

        StopAllCoroutines();
        FillRecursively(origin);
    }

    public void CreateRoom()
    {
        GameObject newRoom = new GameObject("ROOM_");
        tileworldRenderer.CreateRoom(newRoom.transform, tilesTouched, tilesFilled, stories: stories);
    }

    private void FillRecursively(Vector3Int self)
    {
        if (tilesFilled.Count > 1000)
        {
            return;
        }

        if (!tilesFilled.Contains(self))
        {
            tilesFilled.Add(self);

            for (int i = 1; i <= 4; i++)
            {
                Vector3Int translated = self + (TranslateByInt(i) * 2);

                if (!tilesTouched.Contains(translated))
                {
                    FillRecursively(translated);
                }
            }
        }
    }

    private Vector3Int TranslateByInt(int i)
    {
        switch (i)
        {
            case 1:
                return Vector3Int.right;

            case 2:
                return Vector3Int.forward;
            case 3:
                return Vector3Int.left;
            case 4:
                return Vector3Int.back;
        }

        return Vector3Int.zero;
    }

    private int Gridify(float x)
    {
        return Mathf.RoundToInt(x * 0.5f) * 2;
    }
}
