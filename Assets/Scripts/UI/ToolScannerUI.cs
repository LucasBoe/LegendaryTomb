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
            int resolution = 8;
            int length = data.Length * resolution;
            lineRenderer.points.Clear();

            float y = 0;

            for (int i = 0; i < length; i++)
            {
                float x = i / (float)length;
                float yBaseNoise = 1 + (Mathf.Sin(x * 17f)) * 0.05f;
                float yNoise1 = Mathf.Sin(x * 37f + Time.time * 23f);
                float yNoise2 = -Mathf.Sin(x * 21f + -Time.time * 31f);
                float yNoiseScale = (0.5f + (1 - (Mathf.Abs(0.5f - data[i / resolution])))) * 0.01f;
                y = Mathf.MoveTowards(y, data[i / resolution] * yBaseNoise, 0.01f) + yNoise1 * yNoiseScale + yNoise2 * yNoiseScale;
                lineRenderer.points.Add(new Vector2(x, y));
            }

            lineRenderer.SetAllDirty();
        }
    }
}
