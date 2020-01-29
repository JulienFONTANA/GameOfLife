using System;
using System.Threading;

namespace ConsoleGameOfLife
{
    public class GameOfLife : IGameOfLife
    {
        private Random Rand { get; }
        private static int SizeH { get; set; }
        private static int SizeL { get; set; }

        public GameOfLife(int size)
        {
            Rand = new Random();
            SizeH = size;
            SizeL = size;
        }

        public GameOfLife(int sizeH, int sizeL)
        {
            Rand = new Random();
            SizeH = sizeH;
            SizeL = sizeL;
        }

        public void Game()
        {
            var board = InitBoard();
            var turnCount = 0;

            Console.Clear();
            Console.CursorVisible = false;
            var cpyBoard = new int[SizeH, SizeL];
            while (!Console.KeyAvailable || Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
                var olderBoard = cpyBoard;
                cpyBoard = board;
                Display(board, ++turnCount);
                board = Update(board);

                // In some cases, frequency of boards are more than 2... So this is not perfect.
                if (AreBoardEquals(board, cpyBoard) || AreBoardEquals(board, olderBoard))
                {
                    Console.WriteLine($"The game is over after {turnCount} turns !!!");
                    return;
                }

                Thread.Sleep(250);
            }
        }

        private static bool AreBoardEquals(int[,] board, int[,] cpyBoard)
        {
            for (var i = 0; i < SizeH; i++)
            {
                for (var j = 0; j < SizeL; j++)
                {
                    if (board[i, j] != cpyBoard[i, j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static int[,] Update(int[,] board)
        {
            var updatedBoard = new int[SizeH, SizeL];
            for (var i = 0; i < SizeH; i++)
            {
                for (var j = 0; j < SizeL; j++)
                {
                    updatedBoard[i, j] = CountAliveNeigbourgs(board, i, j);
                }
            }

            return updatedBoard;
        }

        private static int CountAliveNeigbourgs(int[,] board, int i, int j)
        {
            var aliveCount = 0;
            for (var k = -1; k <= 1; k++)
            {
                if (i + k < 0 || i + k >= SizeH)
                    continue;

                for (var l = -1; l <= 1; l++)
                {
                    if (k == 0 && l == 0)
                        continue;

                    if (j + l < 0 || j + l >= SizeL)
                        continue;

                    if (board[i + k, j + l] == 1)
                        aliveCount++;
                }
            }

            if (board[i, j] == 0 && aliveCount == 3)
                return 1;

            if (board[i, j] == 1 && (aliveCount == 2 || aliveCount == 3))
                return 1;

            return 0;
        }

        private int[,] InitBoard()
        {
            var board = new int[SizeH, SizeL];
            for (var i = 0; i < SizeH; i++)
            {
                for (var j = 0; j < SizeL; j++)
                {
                    board[i, j] = Rand.Next(2);
                }
            }

            return board;
        }

        private static void Display(int[,] board, int turnCount)
        {
            var str = "";
            double aliveCount = 0;

            str += $"-----======= Turn {turnCount} =======-----" + Environment.NewLine;
            for (var i = 0; i < SizeH; i++)
            {
                for (var j = 0; j < SizeL; j++)
                {
                    var alive = board[i, j] == 1;
                    if (alive) aliveCount++;
                    str += alive ? "#" : "_";
                }
                str += Environment.NewLine;
            }

            var perc = aliveCount / (SizeH * SizeL);
            str += $"Alive cells : {aliveCount} - which is {perc:0.0%}     "; // Spaces here to clean print
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(str);
        }
    }
}