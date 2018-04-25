using System;
using System.Collections.Generic;

namespace SnakeNN
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    };

    public class SnakeSettings
    {
        public readonly int width;
        public readonly int height;
        public readonly int speed;
        public readonly int points;

        public int score;
        public bool gameOver;
        public Direction direction;

        private int[,] snakeCartesian;
        public List<Circle> Snake = new List<Circle>();
        public Circle food = new Circle();

        public SnakeSettings(int givenWidth, int givenHeight, int givenSpeed, int givenPoint)
        {
            width = givenWidth;
            height = givenHeight;
            speed = givenSpeed;
            points = givenPoint;

            Clear();
        }

        public void Clear()
        {
            score = 0;
            gameOver = false;
            direction = Direction.Down;
            snakeCartesian = new int[width, height];
            Snake.Clear(); // clear the List
        }
    }


}
