using System;
using System.Collections.Generic;

public class SnakeCartesian
{
    public CartesianStates[,] snakeCartesian;
    public SnakeCartesian(int gameGridX, int gameGridY)
    {
        snakeCartesian = new CartesianStates[gameGridX, gameGridY];
        ClearWorld();
    }

    public void clearWorld(CartesianStates givenStates)
    {
        for (int iterateX = 0; iterateX < snakeCartesian.GetLength(0); iterateX++)
        {
            for (int iterateY = 0; iterateY < snakeCartesian.GetLength(1); iterateY++)
            {
                snakeCartesian[iterateX, iterateY] = givenStates;
            }
        }
    }

    public void ClearWorld()
    {
        clearWorld(CartesianStates.IS_EMPTY);
    }

    public void UpdateCartesian(List<Circle> givenWorldObjects)
    {
        ClearWorld();
        foreach (Circle circle in givenWorldObjects)
        {
            if (ValidMove(circle))
            {
                snakeCartesian[circle.x, circle.y] = circle.type;
            }
        }
    }

    public int GetLength(int dimension)
    {
        return snakeCartesian.GetLength(dimension);
    }

    public CartesianStates[,] GetWorld()
    {
        return snakeCartesian;
    }

    private Boolean ValidMove(Circle circle)
    {
        return !(circle.x < 0 || circle.x >= snakeCartesian.GetLength(0) || circle.y < 0 || circle.y >= snakeCartesian.GetLength(1));
    }
}