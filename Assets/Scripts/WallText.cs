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
        PlayerUtility.Say("This stone has been carefully encarved...");
    }

    private void Translate()
    {
        PlayerHandler.ActivePlayer.ActionManager.TryStart(new PlayerAction(InteractionType.Translate, 10, FinishedTranslation));
    }

    private void FinishedTranslation()
    {
        Read();
        interactions.OverrideFunction(InteractionType.LookAt, Read);
        interactions.OverrideFunction(InteractionType.Translate, delegate () { PlayerUtility.Say("I can read it already."); });
    }

    private void Read()
    {
        PlayerUtility.Say("The Text says: 'Danger! Hole in the center of the room!'.");
    }

    private void LookAt()
    {
        PlayerUtility.Say("Looks like readable Text, I should be able to translate it.");

        if (!interactions.Contains(InteractionType.Translate))
            interactions.Add(new Interaction(InteractionType.Translate, Translate));
    }
}
