using SimpleSnake.Core;
using SimpleSnake.GameObjects;

namespace SimpleSnake
{
    using Utilities;

    public class StartUp
    {
        public static void Main()
        {
            ConsoleWindow.CustomizeConsole();

            var wall = new Wall(100, 40);
            var snake = new Snake(wall);

            var engine = new Engine(wall, snake);
            engine.Run();
        }
    }
}
