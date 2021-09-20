using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelMenue : MonoBehaviour
{
    [SerializeField] WheelMenueElement elementPrefab;

    public System.Action<int> ClickOn;
    public bool Open = false;

    private List<WheelMenueElement> elements = new List<WheelMenueElement>();

    public void Hide()
    {
        foreach (WheelMenueElement element in elements)
        {
            Destroy(element.gameObject);
        }

        elements.Clear();

        Open = false;
    }

    public void Show(InteractableBase origin, InteractionType[] interactionType)
    {
        transform.position = Camera.main.WorldToScreenPoint(origin.transform.position);
        ClickOn = delegate (int index)
        {
            (origin as IInteractable).TryInteract(interactionType[index]);
        };

        SpawnElements(interactionType.ToSpriteArray());

        Open = true;
    }
    private void SpawnElements(Sprite[] icons)
    {
        float singleElementAngleSize = 360f / icons.Length;

        for (int i = 0; i < icons.Length; i++)
        {
            WheelMenueElement element = Instantiate(elementPrefab, transform);
            element.Init(i, icons[i], singleElementAngleSize, this);
            elements.Add(element);
        }
    }

    private void Update()
    {
        if (!Open)
            return;

        int elementCount = elements.Count;
        int hoverIndex = -1;

        foreach (WheelMenueElement element in elements)
        {
            InputManager inputManager = Game.InputManager;

            Vector2 inputVector2 = inputManager.IsControllerConnected() ? inputManager.GetJoystickVector2() : inputManager.GetMouseVector2(transform.position);

            if (inputVector2.magnitude > 0f)
            {
                float deltaAngle = Mathf.Abs(Mathf.DeltaAngle(Vector2.SignedAngle(inputVector2, Vector2.up), element.centerAngle));
                element.SetHovered(1 - (deltaAngle / (360f / (float)elementCount)));

                if (deltaAngle < (360f / (float)elementCount))
                    hoverIndex = element.index;
            } else
            {
                element.SetHovered(-1);
            }
        }

        if (Input.GetButtonDown("Fire1") && hoverIndex >= 0)
            ClickOn?.Invoke(hoverIndex);

        if (Input.GetButtonDown("Fire2") && Open)
            Hide();
    }
}
