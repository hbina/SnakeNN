using System;
using System.Collections.Generic;

public class WorldGrid
{
    public WorldStates[,] world;
    public WorldGrid(int gameGridX, int gameGridY)
    {
        world = new WorldStates[gameGridX, gameGridY];
        ClearWorld();
    }

    public void ClearWorld(WorldStates s)
    {
        for (int iterateX = 0; iterateX < world.GetLength(0); iterateX++)
        {
            for (int iterateY = 0; iterateY < world.GetLength(1); iterateY++)
            {
                if (iterateX.Equals(0) || iterateX.Equals(world.GetLength(1) - 1) ||
                    iterateY.Equals(0) || iterateY.Equals(world.GetLength(0) - 1))
                {
                    world[iterateX, iterateY] = WorldStates.IS_WALL;
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
        ClearWorld(WorldStates.IS_EMPTY);
    }

    public void UpdateCartesian(List<WorldObject> lo)
    {
        ClearWorld();
        foreach (WorldObject circle in lo)
        {
            if (ValidMove(circle))
            {
                world[circle.x, circle.y] = circle.type;
            }
        }
    }

    public int GetLength(int d)
    {
        return world.GetLength(d);
    }

    public WorldStates[,] GetWorld()
    {
        return world;
    }

    private Boolean ValidMove(WorldObject o)
    {
        return !(o.x < 0 || o.x >= world.GetLength(0) || o.y < 0 || o.y >= world.GetLength(1));
    }
}