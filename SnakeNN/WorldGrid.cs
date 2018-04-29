using System;
using System.Collections.Generic;

public class WorldGrid
{
    private WorldState[,] world;
    public WorldGrid(int gameGridX, int gameGridY)
    {
        world = new WorldState[gameGridX, gameGridY];
        ClearWorld();
    }

    public void ClearWorld(WorldState s)
    {
        for (int iterateX = 0; iterateX < world.GetLength(0); iterateX++)
        {
            for (int iterateY = 0; iterateY < world.GetLength(1); iterateY++)
            {
                if (iterateX.Equals(0) || iterateX.Equals(world.GetLength(1) - 1) ||
                    iterateY.Equals(0) || iterateY.Equals(world.GetLength(0) - 1))
                {
                    world[iterateX, iterateY] = WorldState.IS_WALL;
                }
                else
                {
                    world[iterateX, iterateY] = s;
                }
            }
        }
    }

    public void ClearWorld()
    {
        ClearWorld(WorldState.IS_EMPTY);
    }

    public void UpdateCartesian(List<WorldObject> lo)
    {
        ClearWorld();
        foreach (WorldObject circle in lo)
        {
            if (ValidMove(circle))
            {
                world[circle.X, circle.Y] = circle.State;
            }
        }
    }

    public int GetLength(int d)
    {
        return world.GetLength(d);
    }

    private WorldState[,] GetWorld()
    {
        return world;
    }

    public WorldState GetStateAt(int x, int y)
    {
        return world[x, y];
    }

    private Boolean ValidMove(WorldObject o)
    {
        return !(o.X < 0 || o.X >= world.GetLength(0) || o.Y < 0 || o.Y >= world.GetLength(1));
    }
}