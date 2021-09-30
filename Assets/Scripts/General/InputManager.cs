using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public bool IsControllerConnected()
    {
        foreach (string name in Input.GetJoystickNames())
        {
            if (name != "")
                return true;
        }

        return false;
    }

    public Vector2 GetJoystickVector2()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
    public Vector3 GetJoystickVector3()
    {
        return new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

    public Vector2 GetMouseVector2(Vector3 position)
    {
        return (Vector2)(Input.mousePosition - position);
    }

}
