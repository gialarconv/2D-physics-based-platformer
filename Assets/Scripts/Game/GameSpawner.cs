using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _gamePrefabs;
    public void InitGameSpawner()
    {
        GameObject game = Instantiate(_gamePrefabs[Random.Range(0, _gamePrefabs.Length)]);
    }
}