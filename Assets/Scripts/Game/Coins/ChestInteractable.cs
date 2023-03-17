using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInteractable : CoinObject
{
    [SerializeField] protected float _timeToExpire;

    private Coroutine _expireCoroutine;
    private WaitForSeconds _waitToExpire;

    public override void Init(GameCoinFactory coinFactory, string objectName, Vector3 coordinate)
    {
        base.Init(coinFactory, objectName, coordinate);
        _waitToExpire = new WaitForSeconds(_timeToExpire);

        if (_timeToExpire > 0f)
        {
            if (_expireCoroutine == null)
            {
                _expireCoroutine = StartCoroutine(ExpireObject());
            }
            else
            {
                StopCoroutine(_expireCoroutine);
                _expireCoroutine = StartCoroutine(ExpireObject());
            }
        }
    }

    public override void ObjectCollected()
    {
        base.ObjectCollected();
        if (_expireCoroutine != null)
            StopCoroutine(ExpireObject());
    }

    private IEnumerator ExpireObject()
    {
        yield return _waitToExpire;
        RemoveObject();
    }
}