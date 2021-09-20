using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugDraw
{
    public static void Cuboid(Bounds bounds, Color color)
    {
        Vector3[] points = new Vector3[8];

        points[0] = bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, bounds.extents.z);
        points[1] = bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, bounds.extents.z);
        points[2] = bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, bounds.extents.z);
        points[3] = bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, bounds.extents.z);
        points[4] = bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, -bounds.extents.z);
        points[5] = bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, -bounds.extents.z);
        points[6] = bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, -bounds.extents.z);
        points[7] = bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, -bounds.extents.z);

        int[] edges = {
            0,1,
            1,2,
            2,3,
            3,0,
            4,5,
            5,6,
            6,7,
            7,4,
            1,5,
            2,6,
            3,7,
            0,4
        };

        for (int i = 0; i < edges.Length; i += 2)
        {
            Vector3 self = points[edges[i]];
            Vector3 other = points[edges[i + 1]];

            Debug.DrawLine(self, other, color);
        }
    }
}
