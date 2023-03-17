using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameCoinFactory : MonoBehaviour
{
    private const float TILE_ANCHOR = 0.5f;

    [SerializeField] private GridSystem _gridSystem;
    [SerializeField] private CoinObject[] _coinsPrefabs;

    private int _chestRespawnerCount;
    private List<GridCellData> _gridCellsData;
    private WaitForSeconds _timeForCoins;
    private WaitForSeconds _timeForChests;

    public List<GridCellData> GridCellsData
    {
        get => _gridCellsData;
        set => _gridCellsData = value;
    }
    private void OnEnable()
    {
        GameDelegates.OnGameOver += GameOver;
    }
    private void OnDisable()
    {
        GameDelegates.OnGameOver -= GameOver;
    }
    void Start()
    {
        _gridCellsData = new List<GridCellData>();

        _timeForCoins = new WaitForSeconds(GameManager.Instance.TimeToRespawnCoins);
        _timeForChests = new WaitForSeconds(GameManager.Instance.TimeToRespawnChests);

        this.DoAfter(() => GameManager.Instance != null && _gridSystem.GridComplete, () =>
        {
            Init();

            StartCoroutine(IEGenerateRespawnedNormalCoin());
            StartCoroutine(IEGenerateRespawnedChests());
        });
    }
    private void Init()
    {
        GameObject coins = new GameObject("Coins");
        coins.transform.SetParent(this.transform);

        for (int i = 0; i < _coinsPrefabs.Length; i++)
        {
            PoolManager.Instance.CreatePool(_coinsPrefabs[i].gameObject, _gridSystem.EmptyCells.Count);
        }

        for (int i = 0; i < _gridSystem.EmptyCells.Count; i++)
        {
            int index = i;
            GridCellData cellData = new GridCellData(new Vector3(_gridSystem.EmptyCells[index].x - ((_gridSystem.TileSize.x / 2) - TILE_ANCHOR), _gridSystem.EmptyCells[index].y - ((_gridSystem.TileSize.y / 2) - TILE_ANCHOR)), false);

            CoinObject coinObject = _coinsPrefabs[Random.Range(0, _coinsPrefabs.Length)];

            if (coinObject.isSpecialItem)
            {
                if (_chestRespawnerCount < GameManager.Instance.MaxChestsToCreate)
                {
                    _chestRespawnerCount++;
                    CreateCoinFromPool(coinObject, cellData);
                }
                else
                    CreateCoinFromPool(_coinsPrefabs.First(), cellData);
            }
            else
                CreateCoinFromPool(_coinsPrefabs.First(), cellData);
        }
    }
    private void CreateCoinFromPool(CoinObject coinObject, GridCellData cellData)
    {
        GameObject coinFromPool = PoolManager.Instance.ReuseObject(coinObject.gameObject, cellData.coordinate, Quaternion.identity);

        coinFromPool.GetComponent<CoinObject>().Init(this, $"Tile {cellData.coordinate.x},{cellData.coordinate.y}", cellData.coordinate);

        AddTileToCell(cellData);
    }

    public void AddTileToCell(GridCellData gridCellData)
    {
        _gridCellsData.Add(gridCellData);
    }
    public void ChangeEmptyCellState(Vector3 tileCoordinate)
    {
        _gridCellsData.Find(x => x.coordinate == tileCoordinate).isEmpty = true;
    }
    public void DiscountChest()
    {
        _chestRespawnerCount--;
    }

    private IEnumerator IEGenerateRespawnedNormalCoin()
    {
        while (true)
        {
            yield return _timeForCoins;

            int randomItemsToCreate = Random.Range(0, GameManager.Instance.RandomNormalCointsToRespawn);

            for (int i = 0; i < randomItemsToCreate; i++)
            {
                GenerateRespawnedObject(_coinsPrefabs.First());
            }
        }
    }
    private IEnumerator IEGenerateRespawnedChests()
    {
        while (true)
        {
            yield return _timeForChests;
            GenerateRespawnedChests(_coinsPrefabs.Last());
        }
    }
    private void GenerateRespawnedObject(CoinObject coinObject)
    {
        if (GetRandomPosition() == Vector3.zero)
            return;

        ReuseObjectCoin(coinObject, GetRandomPosition());
    }
    private void GenerateRespawnedChests(CoinObject coinObject)
    {
        if (_chestRespawnerCount < GameManager.Instance.MaxChestsToCreate)
        {
            ReuseObjectCoin(coinObject, GetRandomPosition());
            _chestRespawnerCount++;
        }
    }
    private Vector3 GetRandomPosition()
    {
        List<GridCellData> emptyTiles = new List<GridCellData>();

        foreach (GridCellData cell in _gridCellsData)
        {
            if (cell.isEmpty)
                emptyTiles.Add(cell);
        }

        return emptyTiles.Count > 0 ? emptyTiles[Random.Range(0, emptyTiles.Count)].coordinate : Vector3.zero;
    }
    public void ReuseObjectCoin(CoinObject coinObject, Vector3 coordinate)
    {
        GameObject coinFromPool = PoolManager.Instance.ReuseObject(coinObject.gameObject, coordinate, Quaternion.identity);

        _gridCellsData.Find(x => x.coordinate == coordinate).isEmpty = false;

        coinFromPool.GetComponent<CoinObject>().Init(this, $"Tile {coordinate.x},{coordinate.y}", coordinate);
    }

    private void GameOver()
    {
        StopAllCoroutines();
    }
}