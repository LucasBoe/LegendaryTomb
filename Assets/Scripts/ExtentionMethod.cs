using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class ExtentionMethod
{
    public static bool isPlayer (this Collider collider)
    {
        return collider.CompareTag("Player");
    }
}
