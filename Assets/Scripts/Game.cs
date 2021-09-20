using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private static Game instance;
    private static Game Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<Game>();

            if (instance == null)
                instance = new GameObject("GAME").AddComponent<Game>();

            return instance;
        }
    }

    public static UIHandler UIHandler
    {
        get
        {
            return UIHandler.GetInstance();
        }
    }

    public static InputManager InputManager
    {
        get
        {
            return InputManager.GetInstance();
        }
    }

    public static Game GetInstance()
    {
        return Instance;
    }

    public static DataHolder DataHolder
    {
        get
        {
            return DataHolder.GetInstance(usePrefab: true);
        }
    }
}
