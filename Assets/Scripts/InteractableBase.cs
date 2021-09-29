using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractableBase : MonoBehaviour, IInteractable
{
    [SerializeField] protected InteractionList interactions = new InteractionList();

    private bool playerIsInTrigger = false;


    public InteractionType[] GetInteractions()
    {
        List<InteractionType> interactionTypesSupported = new List<InteractionType>();
        foreach (Interaction interaction in interactions)
        {
            interactionTypesSupported.Add(interaction.type);
        }

        return interactionTypesSupported.ToArray();
    }

    public void TryInteract(InteractionType type)
    {
        Interaction interaction = interactions.GetInteractionByType(type);

        if (interaction != null)
        {
            Debug.Log(type.ToString());
            interaction.function?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.IsPlayer())
        {
            Game.UIHandler.PromptHandler.Show(transform, "press #Fire1 to interact");
            playerIsInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.IsPlayer())
        {
            playerIsInTrigger = false;
        }
    }

    protected virtual void Update()
    {
        if (!playerIsInTrigger || PlayerHandler.ActivePlayer.ActionManager.InAction)
            return;

        WheelMenue wheelMenue = Game.UIHandler.WheelMenu;

        if (Input.GetButtonUp("Fire1"))
        {

            if (wheelMenue.Open == false)
            {
                wheelMenue.Show(this, GetInteractions());
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!playerIsInTrigger)
            return;

        Collider collider = GetComponent<Collider>();
        Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
    }
}

[System.Serializable]
public class Interaction
{
    public InteractionType type;
    public System.Action function;

    public Interaction(InteractionType type, System.Action function)
    {
        this.type = type;
        this.function = function;
    }
}

public class InteractionList : List<Interaction>
{
    public Interaction GetInteractionByType(InteractionType type)
    {
        foreach (Interaction interaction in this.Where(i => i.type == type))
        {
            return interaction;
        }

        return null;
    }

    public void OverrideFunction(InteractionType type, Action toOverride)
    {
        Interaction interaction = GetInteractionByType(type);
        if (interaction != null)
            interaction.function = toOverride;
    }

    public bool Contains(InteractionType type)
    {
        return GetInteractionByType(type) != null;
    }
}
