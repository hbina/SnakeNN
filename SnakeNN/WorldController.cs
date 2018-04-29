using System;
using System.Collections.Generic;
using System.Drawing;

public enum WorldState
{
    IS_EMPTY,
    IS_SNAKE,
    IS_FOOD,
    IS_LIGHT,
    IS_WALL
};

public class WorldController
{

    public readonly int GameGridX;
    public readonly int GameGridY;
    public readonly int GameSpeed;
    public readonly int gameEatPoints;

    private int CurrentScore;
    private bool IsGameOver;

    private WorldGrid WorldGrid;
    public WorldObject Self;
    public WorldObject Target;
    private List<WorldObject> SelfVision;

    private Random RNG;
    private readonly int RANDOM_NUMBER = 181983123;

    public WorldController(int GivenGridX, int GivenGridY, int GivenSpeed, int GivenEatPoints)
    {
        RNG = new Random(RANDOM_NUMBER);
        GameGridX = GivenGridX;
        GameGridY = GivenGridY;
        GameSpeed = GivenSpeed;
        gameEatPoints = GivenEatPoints;
        Clear();
    }

    public void Clear()
    {
        CurrentScore = 0;
        IsGameOver = false;
        WorldGrid = new WorldGrid(GameGridX, GameGridY);
        Self = new WorldObject(GameGridX / 2, GameGridY / 2, WorldState.IS_SNAKE, ObjectDirection.UP);
        Target = new WorldObject(0, 0, WorldState.IS_FOOD);
        SelfVision = new List<WorldObject>();
        GenerateFood();
    }

    public void GenerateFood()
    {
        int x = RNG.Next(1, GameGridX);
        int y = RNG.Next(1, GameGridY);
        Console.WriteLine($"food({x}, {y})");
        if (WorldGrid.GetStateAt(x, y).Equals(WorldState.IS_EMPTY))
        {
            Target = new WorldObject(x, y, WorldState.IS_FOOD);
        }
        else
        {
            GenerateFood();
        }
    }

    public void Eat()
    {
        CurrentScore += gameEatPoints;
        GenerateFood();
    }

    public void UpdateWorld()
    {
        switch (Self.Direction)
        {
            case ObjectDirection.RIGHT:
                Self.X++;
                break;
            case ObjectDirection.LEFT:
                Self.X--;
                break;
            case ObjectDirection.UP:
                Self.Y--;
                break;
            case ObjectDirection.DOWN:
                Self.Y++;
                break;
        }

        if (WorldGrid.GetStateAt(Self.X, Self.Y).Equals(WorldState.IS_WALL))
        {
            Die();
        }
        else if (WorldGrid.GetStateAt(Self.X, Self.Y).Equals(WorldState.IS_FOOD))
        {
            Eat();
        }

        //UpdateVision();
        List<WorldObject> worldObjects = new List<WorldObject>();
        worldObjects.Add(Self);
        worldObjects.Add(Target);
        foreach (WorldObject circle in SelfVision)
        {
            worldObjects.Add(circle);
        }
        WorldGrid.UpdateCartesian(worldObjects);
    }

    private void Die()
    {
        IsGameOver = true;
    }

    public Brush GetColorOfObject(WorldState givenCartesianState)
    {
        if (givenCartesianState.Equals(WorldState.IS_EMPTY))
        {
            return Brushes.Pink;
        }
        else if (givenCartesianState.Equals(WorldState.IS_SNAKE))
        {
            return Brushes.Blue;
        }
        else if (givenCartesianState.Equals(WorldState.IS_FOOD))
        {
            return Brushes.Green;
        }
        else if (givenCartesianState.Equals(WorldState.IS_LIGHT))
        {
            return Brushes.White;
        }
        else if (givenCartesianState.Equals(WorldState.IS_WALL))
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
        SelfVision = new List<WorldObject>();
        int[] lightPosition = new int[] { Self.X, Self.Y };
        if (Self.Direction.Equals(ObjectDirection.UP))
        {
            int counter = 0;
            while (lightPosition[1] >= 0)
            {
                SelfVision.Add(new WorldObject(lightPosition[0], lightPosition[1], WorldState.IS_LIGHT));
                for (int sideLength = 0; sideLength < counter; sideLength++)
                {
                    int checkedLeft = lightPosition[0] - sideLength;
                    int checkedRight = lightPosition[0] + sideLength;
                    if (checkedLeft >= 0)
                    {
                        SelfVision.Add(new WorldObject(checkedLeft, lightPosition[1], WorldState.IS_LIGHT));
                    }
                    if (checkedRight < WorldGrid.GetLength(0))
                    {
                        SelfVision.Add(new WorldObject(checkedRight, lightPosition[1], WorldState.IS_LIGHT));
                    }
                }
                lightPosition[1]--;
                counter++;
            }
        }
        else if (Self.Direction.Equals(ObjectDirection.DOWN))
        {
            int counter = 0;
            while (lightPosition[1] < WorldGrid.GetLength(1))
            {
                SelfVision.Add(new WorldObject(lightPosition[0], lightPosition[1], WorldState.IS_LIGHT));
                for (int sideLength = 0; sideLength < counter; sideLength++)
                {
                    int checkedLeft = lightPosition[0] - sideLength;
                    int checkedRight = lightPosition[0] + sideLength;
                    if (checkedLeft >= 0)
                    {
                        SelfVision.Add(new WorldObject(checkedLeft, lightPosition[1], WorldState.IS_LIGHT));
                    }
                    if (checkedRight < WorldGrid.GetLength(0))
                    {
                        SelfVision.Add(new WorldObject(checkedRight, lightPosition[1], WorldState.IS_LIGHT));
                    }
                }
                lightPosition[1]++;
                counter++;
            }
        }
        else if (Self.Direction.Equals(ObjectDirection.LEFT))
        {
            int counter = 0;
            while (lightPosition[0] >= 0)
            {
                SelfVision.Add(new WorldObject(lightPosition[0], lightPosition[1], WorldState.IS_LIGHT));
                for (int sideLength = 0; sideLength < counter; sideLength++)
                {
                    int checkedUp = lightPosition[1] - sideLength;
                    int checkDown = lightPosition[1] + sideLength;
                    if (checkedUp >= 0)
                    {
                        SelfVision.Add(new WorldObject(checkedUp, lightPosition[1], WorldState.IS_LIGHT));
                    }
                    if (checkDown < WorldGrid.GetLength(1))
                    {
                        SelfVision.Add(new WorldObject(checkDown, lightPosition[1], WorldState.IS_LIGHT));
                    }
                }
                lightPosition[1]--;
                counter++;
            }
        }
        else
        {
            int counter = 0;
            while (lightPosition[1] < WorldGrid.GetLength(0))
            {
                SelfVision.Add(new WorldObject(lightPosition[0], lightPosition[1], WorldState.IS_LIGHT));
                for (int sideLength = 0; sideLength < counter; sideLength++)
                {
                    int checkedUp = lightPosition[0] - sideLength;
                    int checkedDown = lightPosition[0] + sideLength;
                    if (checkedUp >= 0)
                    {
                        SelfVision.Add(new WorldObject(checkedUp, lightPosition[1], WorldState.IS_LIGHT));
                    }
                    if (checkedDown < WorldGrid.GetLength(1))
                    {
                        SelfVision.Add(new WorldObject(checkedDown, lightPosition[1], WorldState.IS_LIGHT));
                    }
                }
                lightPosition[1]--;
                counter++;
            }
        }
    }

    public int GetCurrentScore()
    {
        return CurrentScore;
    }

    public Boolean GetIsGameOver()
    {
        return IsGameOver;
    }

    public int GetGameSpeed()
    {
        return GameSpeed;
    }

    public WorldGrid GetWorldGrid()
    {
        return WorldGrid;
    }
}

