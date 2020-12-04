using System;
using System.Collections.Generic;
using System.Linq;
using SimpleSnake.GameObjects.Foods;

namespace SimpleSnake.GameObjects
{
    public class Snake
    {
        private const char SnakeSymbol = '\u25CF';
        private const char EmptySpace = ' ';

        private readonly Queue<Point> snake;
        private readonly Food[] food;
        private Wall wall;
        private int nextLeftX;
        private int nextTopY;
        private int foodIndex;

        public Snake(Wall wall)
        {
            this.wall = wall;
            this.snake = new Queue<Point>();
            this.food = new Food[3];
            this.foodIndex = this.RandomFoodNumber;
            this.GetFoods();
            this.CreateSnake();
            this.GenerateNewFood();
            this.Score = 0;
            this.Level = 0;
        }

        public int RandomFoodNumber => new Random().Next(0, this.food.Length);

        public int Score { get; private set; }

        public int Level { get; private set; }

        public bool IsMoving(Point direction)
        {
            Point currentSnakeHead = this.snake.Last();
            this.GetNextPoint(direction, currentSnakeHead);

            if (this.BittenHerself())
            {
                return false;
            }

            Point snakeNewHead = new Point(this.nextLeftX, this.nextTopY);

            if (this.wall.IsPointOfWall(snakeNewHead))
            {
                return false;
            }

            if (this.food[this.foodIndex].IsFoodPoint(snakeNewHead))
            {
                this.Eat(direction, currentSnakeHead);
            }

            snakeNewHead.Draw(SnakeSymbol);
            this.snake.Enqueue(snakeNewHead);
            Point snakeTail = this.snake.Dequeue();
            snakeTail.Draw(EmptySpace);

            return true;
        }

        private void GetFoods()
        {
            this.food[0] = new FoodHash(this.wall);
            this.food[1] = new FoodDollar(this.wall);
            this.food[2] = new FoodAsterisk(this.wall);
        }

        private void CreateSnake()
        {
            for (int topY = 1; topY <= 6; topY++)
            {
                this.snake.Enqueue(new Point(2, topY));
            }
        }

        private void Eat(Point direction, Point currentSnakeHead)
        {
            var length = this.food[this.foodIndex].FoodPoints;
            this.Score += length;
            this.Level = this.Score / 10 + 1;

            for (int i = 0; i < length; i++)
            {
                this.snake.Enqueue(new Point(this.nextLeftX, this.nextTopY));
                this.GetNextPoint(direction, currentSnakeHead);
            }

            GenerateNewFood();
        }

        private void GenerateNewFood()
        {
            this.foodIndex = this.RandomFoodNumber;
            this.food[this.foodIndex].SetRandomPosition(this.snake);
        }

        private void GetNextPoint(Point direction, Point snakeHead)
        {
            this.nextLeftX = snakeHead.LeftX + direction.LeftX;
            this.nextTopY = snakeHead.TopY + direction.TopY;
        }

        private bool BittenHerself()
            => this.snake.Any(x => x.LeftX == this.nextLeftX && x.TopY == this.nextTopY);
    }
}
