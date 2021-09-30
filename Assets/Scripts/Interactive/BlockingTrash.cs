using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingTrash : InteractableBase
{
    private void Start()
    {
        interactions.Add(new Interaction(InteractionType.LookAt, delegate () { PlayerUtility.Say("Rocks and and sand are blocking the passage here. I should clean it up."); }));
        interactions.Add(new Interaction(InteractionType.Touch, CleanUp));
    }

    private void CleanUp()
    {
        PlayerHandler.ActivePlayer.ActionManager.TryStart(new PlayerAction(InteractionType.Touch, 5, CleanedUp));
    }

    private void CleanedUp()
    {
        Destroy(gameObject);
    }
}
