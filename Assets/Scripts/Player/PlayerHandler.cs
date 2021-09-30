using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : Singleton<PlayerHandler>
{
    [SerializeField] PlayerController activePlayer;
    public static PlayerController ActivePlayer => GetInstance().activePlayer;
}
