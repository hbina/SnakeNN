using System;
using System.Collections.Generic;
using System.Drawing;

public enum SnakeDirection
{
    UP,
    DOWN,
    LEFT,
    RIGHT,
    NULL
};

public enum CartesianStates
{
    IS_EMPTY,
    IS_SNAKE,
    IS_FOOD,
    IS_LIGHT,
    IS_WALL
};

public class SnakeSettings
{

    public readonly int gameGridX;
    public readonly int gameGridY;
    public readonly int gameSpeed;
    public readonly int gameEatPoints;

    public int currentScore;
    public bool isGameOver;
    public SnakeDirection headDirection;

    public SnakeCartesian snakeCartesian;
    public Circle snakeBody;
    public Circle food;
    public List<Circle> snakeVision;

    public SnakeSettings(int givenGridX, int givenGridY, int givenSpeed, int givenEatPoints)
    {
        gameGridX = givenGridX;
        gameGridY = givenGridY;
        gameSpeed = givenSpeed;
        gameEatPoints = givenEatPoints;
        Clear();
    }

    public void Clear()
    {
        currentScore = 0;
        isGameOver = false;
        headDirection = SnakeDirection.UP;
        snakeCartesian = new SnakeCartesian(gameGridX, gameGridY);
        snakeBody = new Circle(gameGridX / 2, gameGridY / 2, CartesianStates.IS_SNAKE);
        food = new Circle(0, 0, CartesianStates.IS_FOOD);
        snakeVision = new List<Circle>();
        GenerateFood();
    }

    public void GenerateFood()
    {
        Random random = new Random();
        food.x = random.Next(0, gameGridX);
        food.y = random.Next(0, gameGridY);
    }

    public void Eat()
    {
        currentScore += gameEatPoints;
        GenerateFood();
    }

    public void UpdateWorld()
    {
        switch (headDirection)
        {
            case SnakeDirection.RIGHT:
                snakeBody.x++;
                break;
            case SnakeDirection.LEFT:
                snakeBody.x--;
                break;
            case SnakeDirection.UP:
                snakeBody.y--;
                break;
            case SnakeDirection.DOWN:
                snakeBody.y++;
                break;
        }

        if (snakeCartesian.GetWorld()[snakeBody.x, snakeBody.y].Equals(CartesianStates.IS_WALL))
        {
            Die();
        }
        else if (snakeCartesian.GetWorld()[snakeBody.x, snakeBody.y].Equals(CartesianStates.IS_FOOD))
        {
            Eat();
        }

        //UpdateVision();
        List<Circle> worldObjects = new List<Circle>();
        worldObjects.Add(snakeBody);
        worldObjects.Add(food);
        foreach (Circle circle in snakeVision)
        {
            worldObjects.Add(circle);
        }
        snakeCartesian.UpdateCartesian(worldObjects);
    }

    private void Die()
    {
        isGameOver = true;
    }

    public Brush GetColorOfObject(CartesianStates givenCartesianState)
    {
        if (givenCartesianState.Equals(CartesianStates.IS_EMPTY))
        {
            return Brushes.Pink;
        }
        else if (givenCartesianState.Equals(CartesianStates.IS_SNAKE))
        {
            return Brushes.Blue;
        }
        else if (givenCartesianState.Equals(CartesianStates.IS_FOOD))
        {
            return Brushes.Green;
        }
        else if (givenCartesianState.Equals(CartesianStates.IS_LIGHT))
        {
            return Brushes.White;
        }
        else if (givenCartesianState.Equals(CartesianStates.IS_WALL))
        {
            return Brushes.Black;
        }
        else
        {
            return Brushes.Yellow;
        }
    }

    public void UpdateVision()
    {
        snakeVision = new List<Circle>();
        int[] lightPosition = new int[] { snakeBody.x, snakeBody.y };
        if (headDirection.Equals(SnakeDirection.UP))
        {
            int counter = 0;
            while (lightPosition[1] >= 0)
            {
                snakeVision.Add(new Circle(lightPosition[0], lightPosition[1], CartesianStates.IS_LIGHT));
                for (int sideLength = 0; sideLength < counter; sideLength++)
                {
                    int checkedLeft = lightPosition[0] - sideLength;
                    int checkedRight = lightPosition[0] + sideLength;
                    if (checkedLeft >= 0)
                    {
                        snakeVision.Add(new Circle(checkedLeft, lightPosition[1], CartesianStates.IS_LIGHT));
                    }
                    if (checkedRight < snakeCartesian.GetWorld().GetLength(0))
                    {
                        snakeVision.Add(new Circle(checkedRight, lightPosition[1], CartesianStates.IS_LIGHT));
                    }
                }
                lightPosition[1]--;
                counter++;
            }
        }
        else if (headDirection.Equals(SnakeDirection.DOWN))
        {
            int counter = 0;
            while (lightPosition[1] < snakeCartesian.GetWorld().GetLength(1))
            {
                snakeVision.Add(new Circle(lightPosition[0], lightPosition[1], CartesianStates.IS_LIGHT));
                for (int sideLength = 0; sideLength < counter; sideLength++)
                {
                    int checkedLeft = lightPosition[0] - sideLength;
                    int checkedRight = lightPosition[0] + sideLength;
                    if (checkedLeft >= 0)
                    {
                        snakeVision.Add(new Circle(checkedLeft, lightPosition[1], CartesianStates.IS_LIGHT));
                    }
                    if (checkedRight < snakeCartesian.GetWorld().GetLength(0))
                    {
                        snakeVision.Add(new Circle(checkedRight, lightPosition[1], CartesianStates.IS_LIGHT));
                    }
                }
                lightPosition[1]++;
                counter++;
            }
        }
        else if (headDirection.Equals(SnakeDirection.LEFT))
        {
            int counter = 0;
            while (lightPosition[0] >= 0)
            {
                snakeVision.Add(new Circle(lightPosition[0], lightPosition[1], CartesianStates.IS_LIGHT));
                for (int sideLength = 0; sideLength < counter; sideLength++)
                {
                    int checkedUp = lightPosition[1] - sideLength;
                    int checkDown = lightPosition[1] + sideLength;
                    if (checkedUp >= 0)
                    {
                        snakeVision.Add(new Circle(checkedUp, lightPosition[1], CartesianStates.IS_LIGHT));
                    }
                    if (checkDown < snakeCartesian.GetWorld().GetLength(1))
                    {
                        snakeVision.Add(new Circle(checkDown, lightPosition[1], CartesianStates.IS_LIGHT));
                    }
                }
                lightPosition[1]--;
                counter++;
            }
        }
        else
        {
            int counter = 0;
            while (lightPosition[1] < snakeCartesian.GetWorld().GetLength(0))
            {
                snakeVision.Add(new Circle(lightPosition[0], lightPosition[1], CartesianStates.IS_LIGHT));
                for (int sideLength = 0; sideLength < counter; sideLength++)
                {
                    int checkedUp = lightPosition[0] - sideLength;
                    int checkedDown = lightPosition[0] + sideLength;
                    if (checkedUp >= 0)
                    {
                        snakeVision.Add(new Circle(checkedUp, lightPosition[1], CartesianStates.IS_LIGHT));
                    }
                    if (checkedDown < snakeCartesian.GetWorld().GetLength(1))
                    {
                        snakeVision.Add(new Circle(checkedDown, lightPosition[1], CartesianStates.IS_LIGHT));
                    }
                }
                lightPosition[1]--;
                counter++;
            }
        }
    }
}

