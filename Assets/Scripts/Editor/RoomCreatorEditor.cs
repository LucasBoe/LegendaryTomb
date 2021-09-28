using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomCreator))]
public class RoomCreatorEditor : Editor
{
    Tool LastTool = Tool.None;
    RoomCreator sceneTarget;
    void OnEnable()
    {
        LastTool = Tools.current;
        Tools.current = Tool.None;
    }

    void OnDisable()
    {
        Tools.current = LastTool;
    }

    public override void OnInspectorGUI()
    {
        sceneTarget = (RoomCreator)target;
        if (GUILayout.Button("CreateRoom"))
        {
            sceneTarget.CreateRoom();
        }
        DrawDefaultInspector();
    }

    public void OnSceneGUI()
    {
        sceneTarget = (RoomCreator)target;

        EditorGUI.BeginChangeCheck();

        if (sceneTarget.Mode == RoomCreatorMode.Room)
        {
            for (int i = 0; i < sceneTarget.cornerpoints.Length; i++)
            {
                EditorGUI.BeginChangeCheck();
                Vector3 newTargetPosition = Handles.PositionHandle(sceneTarget.cornerpoints[i], Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(sceneTarget, "Change Position");
                    sceneTarget.cornerpoints[i] = newTargetPosition;
                }

                Vector3 current = sceneTarget.cornerpoints[i];
                Vector3 next = i + 1 >= sceneTarget.cornerpoints.Length ? sceneTarget.cornerpoints[0] : sceneTarget.cornerpoints[i + 1];

                Handles.DrawDottedLine(current, next, 10);
            }
        }
        else
        {
            EditorGUI.BeginChangeCheck();
            Vector3 newTargetPosition = Handles.PositionHandle(sceneTarget.cornerpoints[0], Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(sceneTarget, "Change Position");
                sceneTarget.cornerpoints[0] = newTargetPosition;
            }
        }

        sceneTarget.UpdateTouchingTiles();
        sceneTarget.UpdateFillingTiles();

        Handles.color = Color.red;

        foreach (Vector3Int tile in sceneTarget.tilesTouched)
        {
            Handles.DrawWireCube(tile, new Vector3(2, 0.2f, 2));
        }

        Handles.color = Color.yellow;

        if (sceneTarget.fill)
        {
            foreach (Vector3Int tile in sceneTarget.tilesFilled)
            {
                Handles.DrawWireCube(tile, new Vector3(2, 0.2f, 2));
            }
        }
    }
}
