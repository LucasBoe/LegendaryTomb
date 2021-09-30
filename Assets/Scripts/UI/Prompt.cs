using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Prompt : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;

    public void Init(string txt, Transform target)
    {
        text.text = TextPrepper.Iconify(txt);
        this.target = target;
        Destroy(gameObject, 3);
    }

    internal void Hide()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Game.UIHandler.PromptHandler.Unregister(this);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(target.position);
    }
}
