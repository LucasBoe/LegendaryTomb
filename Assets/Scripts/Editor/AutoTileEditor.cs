using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(AutoTile))]
public class AutoTileEditor : Editor
{
    Tool LastTool = Tool.None;
    AutoTile sceneTarget;

    private int[] controllId;

    void OnEnable()
    {
        LastTool = Tools.current;
        Tools.current = Tool.None;
        controllId = new int[6] {
            GUIUtility.GetControlID(FocusType.Passive),
            GUIUtility.GetControlID(FocusType.Passive),
            GUIUtility.GetControlID(FocusType.Passive),
            GUIUtility.GetControlID(FocusType.Passive),
            GUIUtility.GetControlID(FocusType.Passive),
            GUIUtility.GetControlID(FocusType.Passive)
        };

        sceneTarget = (AutoTile)target;
        sceneTarget.UpdateNeightbours();
    }

    void OnDisable()
    {
        Tools.current = LastTool;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();
        for (int i = 0; i < 3; i++)
            HandleButton(i);

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        for (int i = 3; i < 6; i++)
            HandleButton(i);

        GUILayout.EndHorizontal();

        GUILayout.Space(4);


        GUILayout.BeginHorizontal();

        int tileTypeIndex = (int)sceneTarget.Type;

        List<string> names = new List<string>();
        foreach (var value in Enum.GetValues(typeof(TileType)))
            names.Add(value.ToString());

        int newTileTypeIndex = GUILayout.Toolbar(tileTypeIndex, names.ToArray());

        if (newTileTypeIndex != tileTypeIndex)
        {
            sceneTarget.Type = (TileType)newTileTypeIndex;
            sceneTarget.UpdateNeightboursNeightbours();
        }

        bool newAir = GUILayout.Toggle(sceneTarget.Air, "Is Digged Out");

        if (newAir != sceneTarget.Air)
        {
            sceneTarget.Air = newAir;
            sceneTarget.UpdateNeightboursNeightbours();
        }

        GUILayout.EndHorizontal();

        GUILayout.Space(4);

        GUILayout.BeginHorizontal();
        foreach (var keyValuePair in sceneTarget.Neightbours)
        {
            if (GUILayout.Button(keyValuePair.Key.ToString()))
                Selection.activeObject = keyValuePair.Value;
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(4);

        if (GUILayout.Button("UpdateNeightbours"))
        {
            sceneTarget.UpdateNeightbours();
        }
        DrawDefaultInspector();
    }

    private void HandleButton(int index)
    {
        AutoTileDirection dir = AutoTileDirection.List[index];
        bool tileExists = sceneTarget.Neightbours.ContainsKey(dir.Direction);
        if (GUILayout.Button(dir.Symbol + (tileExists ? "" : " (null)")))
        {
            if (tileExists)
                Select(sceneTarget.Neightbours[dir.Direction]);
            else
                Expand(dir.Direction);
        }
    }

    public void OnSceneGUI()
    {
        sceneTarget = (AutoTile)target;

        for (int i = 0; i < 6; i++)
        {
            AutoTileDirection dir = AutoTileDirection.List[i];
            bool exists = sceneTarget.Neightbours.ContainsKey(dir.Direction);
            Handles.color = exists ? Color.yellow : Color.red;

            EventType current = Event.current.type;
            if (current == EventType.Repaint || current == EventType.Layout)
            {
                DrawHandle(dir, exists, current, i);
            }
            else if (current == EventType.MouseDown)
            {
                int id = HandleUtility.nearestControl;
                if (id == controllId[i])
                {
                    if (exists)
                        Select(sceneTarget.Neightbours[dir.Direction]);
                    else
                        Expand(dir.Direction);
                }
            }
        }
    }

    private void Expand(Vector3 direction)
    {
        Debug.LogWarning("try expanding into " + direction.ToString());
        AutoTile newAutoTile = Instantiate(sceneTarget, sceneTarget.transform.position + direction * 2, Quaternion.identity, sceneTarget.transform.parent);
        newAutoTile.name = sceneTarget.name;
        newAutoTile.Neightbours.Clear();
        newAutoTile.UpdateNeightboursNeightbours();
        Select(newAutoTile);
    }

    private void Select(AutoTile autoTile)
    {
        Debug.LogWarning("try select auto tile...");
        Selection.activeObject = autoTile;
    }

    private void DrawHandle(AutoTileDirection dir, bool exists, EventType type, int controlIdIndex)
    {
        if (exists)
        {
            if (dir.Direction != Vector3.up && dir.Direction != Vector3.down)
                Handles.SphereHandleCap(controllId[controlIdIndex], sceneTarget.transform.position + dir.Direction + Vector3.down, Quaternion.LookRotation(dir.Direction), 0.25f, type);
        }
        else
        {
            Handles.ArrowHandleCap(controllId[controlIdIndex], sceneTarget.transform.position + dir.Direction, Quaternion.LookRotation(dir.Direction), 0.5f, type);
        }
    }
}