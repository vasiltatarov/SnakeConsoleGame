using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using SimpleSnake.Enums;
using SimpleSnake.GameObjects;

namespace SimpleSnake.Core
{
    public class Engine
    {
        private const string FilePath = "../../../Database/BestScore.txt";
        private const string BestScoreMsg = "{0} - {1}";

        private readonly Dictionary<Direction, Point> pointsOfDirection;
        private Wall wall;
        private Snake snake;
        private Direction direction;
        private double horizontalSpeed;
        private double verticalSpeed;

        public Engine(Wall wall, Snake snake)
        {
            this.wall = wall;
            this.snake = snake;
            this.direction = Direction.Right;
            this.pointsOfDirection = new Dictionary<Direction, Point>();
            this.CreateDirections();
            this.horizontalSpeed = 100;
            this.verticalSpeed = 150;
        }

        public void Run()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            this.ShowBestScore();

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    this.GetNextDirection();
                }

                bool isMoving = this.snake.IsMoving(this.pointsOfDirection[this.direction]);

                if (!isMoving)
                {
                    this.AskUserForRestart();
                }

                this.ShowScore();
                this.ShowLevel();

                this.horizontalSpeed -= 0.01;
                this.verticalSpeed -= 0.01;

                if (this.direction == Direction.Left || this.direction == Direction.Right)
                {
                    Thread.Sleep((int)this.horizontalSpeed);
                }
                else
                {
                    Thread.Sleep((int)this.verticalSpeed);
                }

            }
        }

        private void GetNextDirection()
        {
            ConsoleKeyInfo userInput = Console.ReadKey();

            switch (userInput.Key)
            {
                case ConsoleKey.RightArrow:
                    if (this.direction != Direction.Left)
                    {
                        this.direction = Direction.Right;
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    if (this.direction != Direction.Right)
                    {
                        this.direction = Direction.Left;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (this.direction != Direction.Up)
                    {
                        this.direction = Direction.Down;
                    }
                    break;
                case ConsoleKey.UpArrow:
                    if (this.direction != Direction.Down)
                    {
                        this.direction = Direction.Up;
                    }
                    break;
            }

            Console.CursorVisible = false;
        }

        private void CreateDirections()
        {
            this.pointsOfDirection.Add(Direction.Right, new Point(1, 0));
            this.pointsOfDirection.Add(Direction.Left, new Point(-1, 0));
            this.pointsOfDirection.Add(Direction.Down, new Point(0, 1));
            this.pointsOfDirection.Add(Direction.Up, new Point(0, -1));
        }

        private void AskUserForRestart()
        {
            this.ShowBestScore();

            var leftX = this.wall.LeftX / 2;
            var topY = this.wall.TopY / 2;

            Console.SetCursorPosition(leftX, topY);
            Console.Write("Would you like to continue? y/n");

            var input = Console.ReadLine();

            if (input == "y" || input == "Y")
            {
                Console.Clear();
                StartUp.Main();
            }
            else
            {
                this.StopGame();
            }

        }

        private void StopGame()
        {
            var leftX = this.wall.LeftX / 2 + 2;
            var topY = this.wall.TopY / 2 + 2;

            Console.SetCursorPosition(leftX, topY);
            Console.Write("Game over!");

            Console.SetCursorPosition(0, this.wall.TopY);
            Environment.Exit(0);
        }

        private void ShowGameStatus()
        {
            Console.SetCursorPosition(this.wall.LeftX / 2, this.wall.TopY / 2);
            Console.Write($"HAh... You are dead! With Score: {this.snake.Score}! On level: {this.snake.Level}");
            Console.SetCursorPosition(this.wall.LeftX, this.wall.TopY);
        }

        private void ShowLevel()
        {
            Console.SetCursorPosition(this.wall.LeftX + 2, 4);
            Console.Write($"Player level: {this.snake.Level}");
        }

        private void ShowScore()
        {
            Console.SetCursorPosition(this.wall.LeftX + 2, 2);
            Console.Write($"Player points: {this.snake.Score}");
        }

        private void ShowBestScore()
        {
            var databaseScore = File.ReadAllText(FilePath).Split(" - ");
            var scores = int.Parse(databaseScore[0]);
            var date = databaseScore[1];

            if (this.snake.Score > scores)
            {
                File.WriteAllText(FilePath, string.Format(BestScoreMsg, this.snake.Score.ToString(), DateTime.Now));
                scores = this.snake.Score;
                date = DateTime.Now.ToString();
            }

            Console.SetCursorPosition(this.wall.LeftX + 2, 6);
            Console.Write($"BestScore - {scores} - {date}");
        }
    }
}
