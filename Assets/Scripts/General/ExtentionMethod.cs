using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class ExtentionMethod
{
    public static bool IsPlayer(this Collider collider)
    {
        return collider.CompareTag("Player");
    }

    public static bool Contains(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
    public static int[] ToArray(this Vector3Int v3i)
    {
        return new int[3] { v3i.x, v3i.y, v3i.z };
    }
}
