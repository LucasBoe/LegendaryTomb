using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DataHolder : Singleton<DataHolder>
{
    [SerializeField] InteractionTypeInfo[] typeInfos;

    public InteractionTypeInfo GetInfoByType(InteractionType type)
    {
        foreach (InteractionTypeInfo info in typeInfos)
        {
            if (info.Type == type)
                return info;
        }

        return null;
    }
}

[System.Serializable]
public class InteractionTypeInfo
{
    public InteractionType Type;
    public Sprite Sprite;
    public string Name;
}

public static class InteractionTypeExtention
{
    public static Sprite ToSprite(this InteractionType type)
    {
        return Game.DataHolder.GetInfoByType(type).Sprite;
    }

    public static Sprite[] ToSpriteArray(this InteractionType[] types)
    {
        List<Sprite> sprites = new List<Sprite>();

        foreach (InteractionType type in types)
        {
            sprites.Add(type.ToSprite());
        }

        return sprites.ToArray();
    }
}

public enum InteractionType
{
    LookAt,
    Touch,
    Translate,
    Break,
}
