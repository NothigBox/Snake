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

    bool? isGameOver;

    public Action OnGameOver;

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
        SpawnFoodAtRandomCell();
    }

    void OnFoodEaten(Food food)
    {
        SpawnBody();
        SpawnFoodAtRandomCell();
    }

    void SpawnFoodAtRandomCell()
    {
        Transform[] snakeBodyArray = snake.GetBody().ToArray();

        var randomPosition = map.GetRandomAvailablePosition(snakeBodyArray);

        Apple newApple = factory.GetApple(randomPosition);
        newApple.transform.localScale = WorldScale;
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

    public void ResetLevel()
    {
        ClearLevel();

        StartLevel();
    }

    public void ClearLevel()
    {
        factory.DeactivateAllObjects();

        snake.gameObject.SetActive(false);
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
}
