using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] FoodManager food;
    [SerializeField] MapGrid map;
    [SerializeField] TouchManager touch;
    [SerializeField] SnakeManager snake;
    [SerializeField] FactoryController factory;

    bool? isGameOver;

    public Action OnGameOver;

    private void Awake()
    {
        isGameOver = null;
    }

    private void OnEnable()
    {
        snake.OnDied += EndLevel;
        touch.OnChangeDirection += TryToChangeDirection;
        map.OnCellSizeCalculated += snake.SetInitialValues;
        Food.OnEaten += OnFoodEaten;

        //StartLevel();
    }

    private void OnDisable()
    {
        touch.OnChangeDirection -= TryToChangeDirection;
        map.OnCellSizeCalculated -= snake.SetInitialValues;
        Food.OnEaten -= OnFoodEaten;
    }

    public void StartLevel()
    {
        isGameOver = false;
        map.CalculateCellSize();
        SpawnLimits();
        SpawnFoodAtRandomCell();
    }

    void OnFoodEaten(Food food)
    {
        snake.Grow();
        SpawnFoodAtRandomCell();
    }

    void SpawnFoodAtRandomCell()
    {
        Transform[] snakeBodyArray = snake.GetBody().ToArray();

        var randomPosition = map.GetRandomAvailablePosition(snakeBodyArray);

        food.SpawnFood(randomPosition);
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
            newLimit.transform.localScale = snake.transform.localScale;
        }
    }
}
