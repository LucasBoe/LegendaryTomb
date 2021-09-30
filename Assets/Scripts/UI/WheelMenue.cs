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
    private Coroutine hidingRoutine;

    private bool isHiding => hidingRoutine != null;

    public void Hide(int indexToClickedOn = -1)
    {
        if (!isHiding)
            hidingRoutine = StartCoroutine(HidingRoutine(indexToClickedOn));
    }

    private IEnumerator HidingRoutine(int indexToClickedOn)
    {
        WheelMenueElement specialElement = null;

        yield return new WaitForSeconds(0.1f);

        foreach (WheelMenueElement element in elements)
        {
            if (indexToClickedOn != -1 && elements[indexToClickedOn] == element)
                specialElement = element;
            else
                Destroy(element.gameObject);
        }

        yield return new WaitForSeconds(0.5f);

        if (specialElement != null)
            Destroy(specialElement.gameObject);

        elements.Clear();
        hidingRoutine = null;
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
        if (!Open || isHiding)
            return;

        int elementCount = elements.Count;
        int hoverIndex = -1;

        foreach (WheelMenueElement element in elements)
        {
            if (element != null)
            {
                InputManager inputManager = Game.InputManager;

                Vector2 inputVector2 = inputManager.IsControllerConnected() ? inputManager.GetJoystickVector2() : inputManager.GetMouseVector2(transform.position);

                if (inputVector2.magnitude > 0f)
                {
                    float deltaAngle = Mathf.Abs(Mathf.DeltaAngle(Vector2.SignedAngle(inputVector2, Vector2.up), element.centerAngle));
                    element.SetHovered(1 - (deltaAngle / (360f / (float)elementCount)));

                    if (deltaAngle < (180f / (float)elementCount))
                        hoverIndex = element.index;
                }
                else
                {
                    element.SetHovered(-1);
                }
            }
        }

        if (Input.GetButtonDown("Fire1") && hoverIndex >= 0)
        {
            Hide(hoverIndex);
            ClickOn?.Invoke(hoverIndex);
        }

        if (Input.GetButtonDown("Fire2") && Open)
            Hide();
    }

    private void OnGUI()
    {
        string str = "";

        foreach (WheelMenueElement element in elements)
        {
            InputManager inputManager = Game.InputManager;

            Vector2 inputVector2 = inputManager.IsControllerConnected() ? inputManager.GetJoystickVector2() : inputManager.GetMouseVector2(transform.position);

            if (inputVector2.magnitude > 0f)
            {
                str += element.centerAngle;

                float deltaAngle = Mathf.Abs(Mathf.DeltaAngle(Vector2.SignedAngle(inputVector2, Vector2.up), element.centerAngle));

                if (deltaAngle < ((180f / (float)elements.Count)))
                    str += " - " + deltaAngle;
            }

            str += "\n";
        }


        GUI.Box(new Rect(100, 100, 100, 100), str);
    }
}
