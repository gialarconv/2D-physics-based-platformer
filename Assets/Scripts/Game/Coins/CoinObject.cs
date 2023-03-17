using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CoinObject : MonoBehaviour, IObject
{
    [Tooltip("If you leave this value at 0, it means that the amount this object will give will be in percentage, which is set in GameManager.")]
    [SerializeField] private int _amount;
    [SerializeField] private bool _isSpecialItem = false;

    private GameCoinFactory _gameCoinFactory;
    private Vector3 _coordinate;

    public GameCoinFactory gameCoinFactory => _gameCoinFactory;
    public Vector3 coordinate => _coordinate;
    public bool isSpecialItem => _isSpecialItem;

    public virtual void Init(GameCoinFactory coinFactory, string objectName, Vector3 coordinate)
    {
        _gameCoinFactory = coinFactory;

        this.name = objectName;
        _coordinate = coordinate;

        transform.position = coordinate;
    }
    public virtual void ObjectCollected()
    {
        GameDelegates.OnPlayPooledFX?.Invoke(_coordinate);

        RemoveObject();

        if (_amount == 0)
            GameDelegates.OnAddCoinPercentage?.Invoke();
        else
            GameDelegates.OnAddCoinAmount?.Invoke(_amount);
    }
    public void RemoveObject()
    {
        _gameCoinFactory.ChangeEmptyCellState(_coordinate);

        if (_isSpecialItem)
        {
            _gameCoinFactory.DiscountChest();
        }

        this.gameObject.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            ObjectCollected();
        }
    }
}