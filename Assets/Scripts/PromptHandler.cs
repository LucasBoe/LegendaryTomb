using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptHandler : MonoBehaviour
{
    [SerializeField] Prompt promptPrefab;

    internal void Show(Transform target, string txt)
    {
        Instantiate(promptPrefab, transform).Init(txt, target);
    }
}
