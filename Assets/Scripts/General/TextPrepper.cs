using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextPrepper
{
    public static string Iconify(string input)
    {
        string output = "";
        string signal = "";
        bool interpreting = false;

        foreach (char c in input)
        {
            if (!interpreting)
            {
                if (c == '#')
                    interpreting = true;
                else
                    output += c;
            } else
            {
                if (c == ' ')
                {
                    interpreting = false;
                    output += TryConvert(signal) + c;
                    signal = "";
                } else
                {
                    signal += c;
                }
            }
        }

        return output;
    }

    private static string TryConvert(string signal)
    {
        bool controllerConnected = Game.InputManager.IsControllerConnected();

        switch (signal)
        {
            case "Fire1":
                if (controllerConnected)
                    return "<sprite=0>";
                else
                    return "<sprite=1>";

            case "Fire2":
                if (controllerConnected)
                    return "<sprite=2>";
                else
                    return "<sprite=3>";

            case "Fire3":
                if (controllerConnected)
                    return "<sprite=4>";
                else
                    return "<sprite=5>";

            case "Fire4":
                if (controllerConnected)
                    return "<sprite=6>";
                else
                    return "<sprite=7>";
        }

        return "";
    }
}
