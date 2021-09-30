using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolScanner : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    private void Update()
    {
        transform.right = (playerController.Direction).normalized;
        Scan();
    }

    private void Scan()
    {
        float length = 6;
        Vector3 startingPoint = transform.position;
        Vector3 endPoint = startingPoint + transform.right * -length;

        List<float> airScan = new List<float>();

        for (int i = 0; i < 32; i++)
        {
            Vector3 toMeasureAt = Vector3.Lerp(startingPoint, endPoint, i / 32f);
            AutoTile tile = Game.AutoTileHandler.Fetch(toMeasureAt);
            if (tile != null)
            {
                if (tile.Air)
                    airScan.Add(0);
                else
                    airScan.Add(0.5f);
            }
            else
            {
                airScan.Add(0.9f);
            }
        }

        for (int i = 0; i < 31; i++)
        {
            Vector3 p1 = Vector3.Lerp(startingPoint, endPoint, i / 32f);
            Vector3 p2 = Vector3.Lerp(startingPoint, endPoint, i + 1 / 32f);

            Debug.DrawLine(p1, p2, Color.Lerp(Color.green, Color.red, airScan[i]));
        }
    }
}
