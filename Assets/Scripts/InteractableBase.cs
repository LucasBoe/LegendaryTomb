using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBase : MonoBehaviour, IInteractable
{
    [SerializeField] protected List<Interaction> interactions = new List<Interaction>();


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
        foreach (Interaction interaction in interactions)
        {
            if (interaction.type == type)
                interaction.function?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isPlayer())
        {
            Game.UIHandler.PromptHandler.Show(transform, "press (A) to interact");
            playerIsInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isPlayer())
        {
            playerIsInTrigger = false;
        }
    }

    protected virtual void Update()
    {
        if (!playerIsInTrigger)
            return;

        WheelMenue wheelMenue = Game.UIHandler.WheelMenu;

        if (Input.GetButtonDown("Fire1"))
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
