using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallText : InteractableBase
{
    private void Start()
    {
        interactions.Add(new Interaction(InteractionType.LookAt, LookAt));
        interactions.Add(new Interaction(InteractionType.Touch, Touch));
    }

    private void Touch()
    {
        Game.UIHandler.PromptHandler.Show(PlayerHandler.ActivePlayer.transform, "This stone has been carefully encarved...");
    }

    private void Translate()
    {
        throw new NotImplementedException();
    }

    private void LookAt()
    {
        Game.UIHandler.PromptHandler.Show(PlayerHandler.ActivePlayer.transform, "Looks like readable Text, I should be able to translate it.");
        interactions.Add(new Interaction(InteractionType.Translate, Translate));
    }
}
