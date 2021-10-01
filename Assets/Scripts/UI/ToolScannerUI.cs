using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolScannerUI : MonoBehaviour
{
    [SerializeField] ToolScanner scanner;
    [SerializeField] UILineRenderer lineRenderer;

    private void Update()
    {
        float[] data = scanner.ScanData;

        if (data != null)
        {
            int length = data.Length;
            lineRenderer.points.Clear();

            for (int i = 0; i < length; i++)
            {
                lineRenderer.points.Add(new Vector2(i / (float)length, data[i]));
            }

            lineRenderer.SetAllDirty();
        }
    }
}
