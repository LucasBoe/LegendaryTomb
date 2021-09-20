using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WheelMenueElement : MonoBehaviour
{
    [SerializeField] Image icon;

    public float centerAngle;
    public int index;


    WheelMenue menue;

    public void Init(int index, Sprite sprite, float angle, WheelMenue menue)
    {
        GetComponent<Image>().fillAmount = (angle - 10) / 360f;
        transform.rotation = Quaternion.Euler(0, 0, angle * index - 5);
        icon.rectTransform.rotation = Quaternion.Euler(0, 0, 0);
        icon.rectTransform.localPosition = Quaternion.AngleAxis(((angle - 180f) / 2.25f) - angle, Vector3.forward) * (Vector3.right * 75f);

        centerAngle = Vector2.SignedAngle(icon.transform.position - menue.transform.position, Vector2.up);

        icon.sprite = sprite;

        this.index = index;
        this.menue = menue;
    }

    public void Click()
    {
        menue.ClickOn?.Invoke(index);
    }

    internal void SetHovered(float hoverAmount)
    {
        hoverAmount -= 0.5f;
        hoverAmount = Mathf.Max(0, hoverAmount);

        bool isHovered = hoverAmount > 0f;

        if (isHovered)
            hoverAmount = 0.25f + (hoverAmount / 2);


        transform.localScale = Vector3.one * (1 + hoverAmount * 0.25f);
        icon.transform.localScale = Vector3.one * (1 + (isHovered ? 0.5f: 0f));

        Image image = GetComponent<Image>();
        Color c = image.color;
        image.color = new Color(c.r, c.g, c.b, isHovered ? 1 : 0.5f);
    }
}
