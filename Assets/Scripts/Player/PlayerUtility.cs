using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerUtility
{
    /// <summary>
    /// Creates a prompt for the currently active player.
    /// </summary>
    /// <param name="toSay">The text the player says.</param>
    public static void Say(string toSay)
    {
        Debug.Log("Player Says: " + toSay);
        Game.UIHandler.PromptHandler.Show(PlayerHandler.ActivePlayer.transform, toSay);
    }
}
