using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PromptHandler : MonoBehaviour
{
    [SerializeField] Prompt promptPrefab;
    Dictionary<Prompt, Transform> prompts = new Dictionary<Prompt, Transform>();

    internal void Show(Transform target, string txt)
    {
        if (prompts.ContainsValue(target))
        {
            foreach (var element in prompts.Where(kvp => kvp.Value == target))
            {
                element.Key.Hide();
            }
        }

        Prompt newPrompt = Instantiate(promptPrefab, transform);
        newPrompt.Init(txt, target);
        prompts.Add(newPrompt, target);
    }

    internal void Hide(Transform target)
    {
        if (prompts.ContainsValue(target))
        {
            foreach (var element in prompts.Where(kvp => kvp.Value == target))
            {
                element.Key.Hide();
            }
        }
    }

    internal void Unregister(Prompt prompt)
    {
        if (prompts.ContainsKey(prompt))
        {
            prompts.Remove(prompt);
        }
    }
}
