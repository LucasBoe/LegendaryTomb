using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    InteractionType[] GetInteractions();
    void TryInteract(InteractionType type);
}
