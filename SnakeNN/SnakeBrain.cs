using System;

public class SnakeBrain
{
    public SnakeBrain(int neuronsCount)
    {

    }

    // Update Statistics like hunger
    public void UpdateStatistics()
    {
        UpdateVision();
    }

    // Update whats in vision
    public void UpdateVision()
    {

    }
    public class SnakeStatistics
    {
        public static int hunger = 0;
        public static List<VisionObjects> snakeVision;
    }

    public class VisionObjects
    {
        public int objectId;
        public VisionObjects(int objectId)
        {
            objectId = objectId;
        }
    }
}
