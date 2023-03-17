using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public const float TILE_ANCHOR = 0.5f;
    [SerializeField] private GridSystem _gridSystem;
    [SerializeField] private CharacterController _characterController;

    private void OnEnable()
    {
        GameDelegates.OnInitCharacterSpawner += Init;
    }
    private void OnDisable()
    {
        GameDelegates.OnInitCharacterSpawner -= Init;
    }
    private void Init()
    {
        if (!_gridSystem.gameObject.activeInHierarchy)
            return;

        this.DoAfter(() => _gridSystem.CellsToRemove.Count == 4, () =>
        {
            CreateCharacter();
        });
    }
    private void CreateCharacter()
    {
        CharacterController character = Instantiate(_characterController);
        character.transform.position = AveragePosition();
    }
    private Vector3 AveragePosition()
    {
        Vector2 midPoint = GetCenterBetweenFourPoints(_gridSystem.CellsToRemove[0], _gridSystem.CellsToRemove[1], _gridSystem.CellsToRemove[2], _gridSystem.CellsToRemove[3]);
        return new Vector3(midPoint.x - ((_gridSystem.TileSize.x / 2) - TILE_ANCHOR), midPoint.y - ((_gridSystem.TileSize.y / 2) - TILE_ANCHOR));
    }
    private Vector2 GetCenterBetweenFourPoints(Vector2 point1, Vector2 point2, Vector2 point3, Vector2 point4)
    {
        Vector2 midpoint1 = Vector2.Lerp(point1, point2, 0.5f);
        Vector2 midpoint2 = Vector2.Lerp(point3, point4, 0.5f);
        return Vector2.Lerp(midpoint1, midpoint2, 0.5f);
    }
}
