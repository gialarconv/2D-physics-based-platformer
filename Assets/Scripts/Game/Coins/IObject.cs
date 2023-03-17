using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObject
{
    GameCoinFactory gameCoinFactory { get; }
    Vector3 coordinate { get; }
    bool isSpecialItem { get; }

    void ObjectCollected();
    void RemoveObject();
}