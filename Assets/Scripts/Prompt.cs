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
        text.text = txt;
        this.target = target;
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
        transform.forward = Camera.main.transform.forward;
    }
}
