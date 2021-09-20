using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Riss : InteractableBase
{
    private void Start()
    {
        interactions.Add(new Interaction(InteractionType.LookAt, LookAt));
        interactions.Add(new Interaction(InteractionType.Touch, Touch));
    }

    private void LookAt()
    {
        Game.UIHandler.PromptHandler.Show(transform, "A crack in the wall.");
    }

    private void Touch()
    {
        Game.UIHandler.PromptHandler.Show(transform, "The crack reaches deep into the stone wall.");
    }
}
