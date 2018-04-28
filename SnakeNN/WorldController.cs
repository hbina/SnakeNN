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

public enum WorldStates
{
    IS_EMPTY,
    IS_SNAKE,
    IS_FOOD,
    IS_LIGHT,
    IS_WALL
};

public class WorldController
{

    public readonly int gameGridX;
    public readonly int gameGridY;
    public readonly int gameSpeed;
    public readonly int gameEatPoints;

    public int currentScore;
    public bool isGameOver;
    public SnakeDirection headDirection;

    public WorldGrid worldGrid;
    public WorldObject self;
    public WorldObject target;
    public List<WorldObject> selfVision;

    private Random random;
    private readonly int RANDOM_NUMBER = 181983123;

    public WorldController(int givenGridX, int givenGridY, int givenSpeed, int givenEatPoints)
    {
        random = new Random(RANDOM_NUMBER);
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
        worldGrid = new WorldGrid(gameGridX, gameGridY);
        self = new WorldObject(gameGridX / 2, gameGridY / 2, WorldStates.IS_SNAKE);
        target = new WorldObject(0, 0, WorldStates.IS_FOOD);
        selfVision = new List<WorldObject>();
        GenerateFood();
    }

    public void GenerateFood()
    {
        int x = random.Next(1, gameGridX);
        int y = random.Next(1, gameGridY);
        Console.WriteLine($"food({x}, {y})");
        if (worldGrid.GetWorld()[x, y].Equals(WorldStates.IS_EMPTY))
        {
            target = new WorldObject(x, y, WorldStates.IS_FOOD);
        }
        else
        {
            GenerateFood();
        }
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
                self.x++;
                break;
            case SnakeDirection.LEFT:
                self.x--;
                break;
            case SnakeDirection.UP:
                self.y--;
                break;
            case SnakeDirection.DOWN:
                self.y++;
                break;
        }

        if (worldGrid.GetWorld()[self.x, self.y].Equals(WorldStates.IS_WALL))
        {
            Die();
        }
        else if (worldGrid.GetWorld()[self.x, self.y].Equals(WorldStates.IS_FOOD))
        {
            Eat();
        }

        //UpdateVision();
        List<WorldObject> worldObjects = new List<WorldObject>();
        worldObjects.Add(self);
        worldObjects.Add(target);
        foreach (WorldObject circle in selfVision)
        {
            worldObjects.Add(circle);
        }
        worldGrid.UpdateCartesian(worldObjects);
    }

    private void Die()
    {
        isGameOver = true;
    }

    public Brush GetColorOfObject(WorldStates givenCartesianState)
    {
        if (givenCartesianState.Equals(WorldStates.IS_EMPTY))
        {
            return Brushes.Pink;
        }
        else if (givenCartesianState.Equals(WorldStates.IS_SNAKE))
        {
            return Brushes.Blue;
        }
        else if (givenCartesianState.Equals(WorldStates.IS_FOOD))
        {
            return Brushes.Green;
        }
        else if (givenCartesianState.Equals(WorldStates.IS_LIGHT))
        {
            return Brushes.White;
        }
        else if (givenCartesianState.Equals(WorldStates.IS_WALL))
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
        selfVision = new List<WorldObject>();
        int[] lightPosition = new int[] { self.x, self.y };
        if (headDirection.Equals(SnakeDirection.UP))
        {
            int counter = 0;
            while (lightPosition[1] >= 0)
            {
                selfVision.Add(new WorldObject(lightPosition[0], lightPosition[1], WorldStates.IS_LIGHT));
                for (int sideLength = 0; sideLength < counter; sideLength++)
                {
                    int checkedLeft = lightPosition[0] - sideLength;
                    int checkedRight = lightPosition[0] + sideLength;
                    if (checkedLeft >= 0)
                    {
                        selfVision.Add(new WorldObject(checkedLeft, lightPosition[1], WorldStates.IS_LIGHT));
                    }
                    if (checkedRight < worldGrid.GetWorld().GetLength(0))
                    {
                        selfVision.Add(new WorldObject(checkedRight, lightPosition[1], WorldStates.IS_LIGHT));
                    }
                }
                lightPosition[1]--;
                counter++;
            }
        }
        else if (headDirection.Equals(SnakeDirection.DOWN))
        {
            int counter = 0;
            while (lightPosition[1] < worldGrid.GetWorld().GetLength(1))
            {
                selfVision.Add(new WorldObject(lightPosition[0], lightPosition[1], WorldStates.IS_LIGHT));
                for (int sideLength = 0; sideLength < counter; sideLength++)
                {
                    int checkedLeft = lightPosition[0] - sideLength;
                    int checkedRight = lightPosition[0] + sideLength;
                    if (checkedLeft >= 0)
                    {
                        selfVision.Add(new WorldObject(checkedLeft, lightPosition[1], WorldStates.IS_LIGHT));
                    }
                    if (checkedRight < worldGrid.GetWorld().GetLength(0))
                    {
                        selfVision.Add(new WorldObject(checkedRight, lightPosition[1], WorldStates.IS_LIGHT));
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
                selfVision.Add(new WorldObject(lightPosition[0], lightPosition[1], WorldStates.IS_LIGHT));
                for (int sideLength = 0; sideLength < counter; sideLength++)
                {
                    int checkedUp = lightPosition[1] - sideLength;
                    int checkDown = lightPosition[1] + sideLength;
                    if (checkedUp >= 0)
                    {
                        selfVision.Add(new WorldObject(checkedUp, lightPosition[1], WorldStates.IS_LIGHT));
                    }
                    if (checkDown < worldGrid.GetWorld().GetLength(1))
                    {
                        selfVision.Add(new WorldObject(checkDown, lightPosition[1], WorldStates.IS_LIGHT));
                    }
                }
                lightPosition[1]--;
                counter++;
            }
        }
        else
        {
            int counter = 0;
            while (lightPosition[1] < worldGrid.GetWorld().GetLength(0))
            {
                selfVision.Add(new WorldObject(lightPosition[0], lightPosition[1], WorldStates.IS_LIGHT));
                for (int sideLength = 0; sideLength < counter; sideLength++)
                {
                    int checkedUp = lightPosition[0] - sideLength;
                    int checkedDown = lightPosition[0] + sideLength;
                    if (checkedUp >= 0)
                    {
                        selfVision.Add(new WorldObject(checkedUp, lightPosition[1], WorldStates.IS_LIGHT));
                    }
                    if (checkedDown < worldGrid.GetWorld().GetLength(1))
                    {
                        selfVision.Add(new WorldObject(checkedDown, lightPosition[1], WorldStates.IS_LIGHT));
                    }
                }
                lightPosition[1]--;
                counter++;
            }
        }
    }
}

