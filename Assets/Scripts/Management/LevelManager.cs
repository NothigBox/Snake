using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] FoodManager food;
    [SerializeField] MapGrid map;
    [SerializeField] TouchManager touch;
    [SerializeField] SnakeManager snake;

    private void OnEnable()
    {
        touch.OnChangeDirection += snake.TryToChangeDirection;
        Food.OnEaten += OnFoodEaten;
    }

    private void OnDisable()
    {
        touch.OnChangeDirection -= snake.TryToChangeDirection;
        Food.OnEaten -= OnFoodEaten;
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
}
