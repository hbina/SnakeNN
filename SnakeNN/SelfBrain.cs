using System;

public class SelfBrain
{
    private int selfX, selfY, targetX, targetY;

    public SelfBrain()
    {
        selfX = 0;
        selfY = 0;
    }

    // change this to vector later?
    public void feed(int selfPositionX, int selfPositionY, int targetPositionX, int targetPositionY)
    {
        selfX = selfPositionX;
        selfY = selfPositionY;
        targetX = targetPositionX;
        targetY = targetPositionY;
    }

    public SnakeDirection decide()
    {
        int distanceX = Math.Abs(selfX - targetX);
        int distanceY = Math.Abs(selfY - targetY);
        if (selfX < targetX)
        {
            return SnakeDirection.RIGHT;
        } else if (selfX > targetX)
        {
            return SnakeDirection.LEFT;
        } else if (selfY < targetY)
        {
            return SnakeDirection.DOWN;
        } else
        {
            return SnakeDirection.UP;
        }
    }
}
