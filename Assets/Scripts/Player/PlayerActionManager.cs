using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionManager : MonoBehaviour
{
    private PlayerAction current;
    public bool InAction => current != null;

    public bool TryStart(PlayerAction actionToStart)
    {
        if (InAction)
            return false;

        current = actionToStart;
        StartCoroutine(ActionRoutine());
        return true;
    }

    private IEnumerator ActionRoutine()
    {
        float duration = current.DurationInSeconds;
        int lastDurationInSeconds = int.MaxValue;

        while (InAction && duration > 0f)
        {
            duration -= Time.deltaTime;

            if (lastDurationInSeconds != Mathf.RoundToInt(duration))
            {
                lastDurationInSeconds = Mathf.RoundToInt(duration);
                Game.UIHandler.PromptHandler.Show(transform, current.InteractionType.ToString() + "... " + lastDurationInSeconds + "s");
            }

            yield return null;
        }

        Game.UIHandler.PromptHandler.Hide(transform);

        current.CallbackOnFinish?.Invoke();
        current = null;
    }
}

public class PlayerAction
{
    public InteractionType InteractionType;
    public int DurationInSeconds;
    public System.Action CallbackOnFinish;

    public PlayerAction(InteractionType interactionType, int durationInSeconds, System.Action callbackOnFinish)
    {
        InteractionType = interactionType;
        DurationInSeconds = durationInSeconds;
        CallbackOnFinish = callbackOnFinish;
    }
}
