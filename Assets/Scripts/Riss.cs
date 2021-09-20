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
        interactions.Add(new Interaction(InteractionType.Translate, Translate));
    }

    private void LookAt()
    {
        Debug.Log("Look At.");
    }

    private void Touch()
    {
        Debug.Log("Touch.");
    }

    private void Translate()
    {
        Debug.Log("Translate.");
    }
}
