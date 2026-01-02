using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    Vector2 OUT_OF_MAP = new Vector2 (0f, 1000f);

    [SerializeField] FoodManager food;
    [SerializeField] MapGrid map;
    [SerializeField] TouchManager touch;
    [SerializeField] SnakeManager snake;
    [SerializeField] FactoryController factory;
    [SerializeField] ScoreManager score;

    private int initialApplesCount = 1;

    bool? isGameOver;

    public Action OnGameOver;
    public Action<int> OnScoreUpdated;

    private Vector3 WorldScale => Vector3.one * map.CellSize;

    private void Awake()
    {
        isGameOver = null;
    }

    private void OnEnable()
    {
        snake.OnDied += EndLevel;
        snake.OnInitialGrow += SpawnBody;

        touch.OnChangeDirection += TryToChangeDirection;
        map.OnCellSizeCalculated += snake.SetInitialValues;
        Food.OnEaten += OnFoodEaten;
    }

    private void OnDisable()
    {
        snake.OnDied -= EndLevel;
        snake.OnInitialGrow = null;

        touch.OnChangeDirection -= TryToChangeDirection;
        map.OnCellSizeCalculated -= snake.SetInitialValues;
        Food.OnEaten -= OnFoodEaten;
    }

    public void StartLevel()
    {
        snake.gameObject.SetActive(true);
        isGameOver = false;
        map.CalculateCellSize();
        SpawnLimits();
        SpawnInitialApples();
        FillTiles();

        OnScoreUpdated?.Invoke(0);
    }

    void OnFoodEaten(Food food)
    {
        score.AddScore(1);
        
        SpawnBody();
        SpawnFoodAtRandomCell();

        OnScoreUpdated?.Invoke(score.CurrentScore);

        ValidateWinCondition()
    }

    void SpawnFoodAtRandomCell()
    {
        List<Transform> unavailableCells = new List<Transform>();
        List<Transform> bodyParts = snake.GetBody();
        List<Apple> apples = factory.ActiveApples;

        List<Transform> applesList = new List<Transform>();

        for (int i = 0; i < apples.Count; i++)
        {
            applesList.Add(apples[i].transform);
        }

        unavailableCells.AddRange(bodyParts);
        unavailableCells.AddRange(applesList);

        var randomPosition = map.GetRandomAvailablePosition(unavailableCells.ToArray());

        if(randomPosition != null)
        {
            Apple newApple = factory.GetApple(randomPosition.Value);
            newApple.transform.localScale = WorldScale;
        }
    }

    void EndLevel()
    {
        OnGameOver?.Invoke();
        isGameOver = true;
    }

    void TryToChangeDirection(ESnakeDirection newDirection)
    {
        if (isGameOver == false)
        {
            snake.TryToChangeDirection(newDirection);
        }
    }

    void SpawnLimits()
    {
        List<Vector2> limitPositions = map.GetLimitPositions();

        for (int i = 0; i < limitPositions.Count; i++)
        {
            MapLimit newLimit = factory.GetLimit(limitPositions[i]);
            newLimit.transform.localScale = WorldScale;
        }
    }

    void SpawnBody()
    {
        Body newBody = factory.GetBody(OUT_OF_MAP);
        newBody.transform.localScale = WorldScale;
        snake.Grow(newBody);
    }

    void SpawnInitialApples()
    {
        for (int i = 0; i < initialApplesCount; i++)
        {
            SpawnFoodAtRandomCell();
        }
    }

    public void ResetLevel()
    {
        ClearLevel();

        StartLevel();
    }

    public void ClearLevel()
    {
        score.ClearScore();

        factory.DeactivateAllObjects();

        snake.gameObject.SetActive(false);

        isGameOver = null;
    }

    public void SetMapSize(int mapIndex)
    {
        Vector2 size = default;

        switch (mapIndex)
        {
            case 0:
                size = new Vector2(5, 11);
                break;

            case 1:
                size = new Vector2(10, 22);
                break;

            case 2:
                size = new Vector2(15, 33);
                break;
        }

        if(size != default)
        {
            map.SetMapSize(size);
        }
    }

    public void SetSnakeSpeed(float speed)
    {
        snake.SetSpeed(speed);
    }

    public void SetInitialApplesCount(int initialApplesCount)
    {
        this.initialApplesCount = initialApplesCount;
    }

    public void FillTiles()
    {
        var gridCells = map.GetFullGridCells();
        int evenIndex = 0;

        for (int i = 0; i < gridCells.Count; i++)
        {
            var position = map.FromCellToPosition(gridCells[i]);
            MapTile newTile = default;

            //  Fills the map as in chess, with alternating cells
            if (gridCells.Count % 2 == 0)
            {
                if(i % 2 == 0)
                {
                    newTile = factory.GetTileA(position);
                }
                else
                {
                    newTile = factory.GetTileB(position);
                }
            }
            //  Fills the map as columns of the same type of tiles when the total amount of tiles is even
            else
            {
                int rowIndex = Mathf.FloorToInt(evenIndex / map.Width);
                if (rowIndex % 2 != 0)
                {
                    evenIndex = 1;
                }

                if (evenIndex % 2 == 0)
                {
                    newTile = factory.GetTileA(position);
                }
                else
                {
                    newTile = factory.GetTileB(position);
                }

                evenIndex++;
            }

                newTile.transform.localScale = WorldScale;
        }
    }
}
