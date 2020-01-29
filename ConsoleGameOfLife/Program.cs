using System;

namespace ConsoleGameOfLife
{
    public class Program
    {
        public static void Main()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Welcome to Game Of Life simulator. " + Environment.NewLine +
                              "Please give square size, from 5 to 25, 100 being a rectangular grid");
            int.TryParse(Console.ReadLine(), out var size);

            if (5 <= size && size <= 25 || size == 100)
            {
                var gameOfLife = size == 100 ? new GameOfLife(25, 100) : new GameOfLife(size);
                var play = true;
                while (play)
                {
                    gameOfLife.Game();
                    Console.WriteLine("Quit ? Or Restart ?");
                    play = Console.ReadKey().Key != ConsoleKey.Escape;
                }
            }
            else
            {
                Console.WriteLine("Wrong size.");
            }
        }
    }
}
