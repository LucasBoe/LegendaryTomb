using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCrack : InteractableBase
{
    private void Start()
    {
        interactions.Add(new Interaction(InteractionType.LookAt, LookAt));
        interactions.Add(new Interaction(InteractionType.Touch, Touch));
    }

    private void LookAt()
    {
        Game.UIHandler.PromptHandler.Show(PlayerHandler.ActivePlayer.transform, "A crack in the wall.");
    }

    private void Touch()
    {
        Game.UIHandler.PromptHandler.Show(PlayerHandler.ActivePlayer.transform, "The crack reaches deep into the stone wall.");
    }
}
