using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sudo2
{
    internal class MapGener
    {
        // Возвращает значение false, если заданный блок размером 3x3 содержит num
        static bool unUsedInBox(int[,] grid, int rowStart,
                                int colStart, int num)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (grid[rowStart + i, colStart + j]
                        == num)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        // Заполните матрицу размером 3х3
        static void fillBox(int[,] grid, int row, int col)
        {
            Random rand = new Random();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int num;
                    do
                    {
                        num = rand.Next(1, 10);
                    } while (!unUsedInBox(grid, row, col, num));
                    grid[row + i, col + j] = num;
                }
            }
        }

        // Проверяем, безопасно ли помещать num в строку i
        static bool unUsedInRow(int[,] grid, int i, int num)
        {
            for (int j = 0; j < 9; j++)
            {
                if (grid[i, j] == num)
                {
                    return false;
                }
            }
            return true;
        }

        // Проверяем, безопасно ли указывать число в столбце j
        static bool unUsedInCol(int[,] grid, int j, int num)
        {
            for (int i = 0; i < 9; i++)
            {
                if (grid[i, j] == num)
                {
                    return false;

                }
            }
            return true;
        }

        // Проверяем, безопасно ли помещать в камеру
        static bool checkIfSafe(int[,] grid, int i, int j,
                                int num)
        {
            return unUsedInRow(grid, i, num)
                && unUsedInCol(grid, j, num)
                && unUsedInBox(grid, i - i % 3, j - j % 3, num);
        }

        //  Заполните диагональные матрицы размером 3х3
        static void fillDiagonal(int[,] grid)
        {
            for (int i = 0; i < 9; i += 3)
            {
                fillBox(grid, i, i);
            }
        }

        //Заполните оставшиеся блоки
        static bool fillRemaining(int[,] grid, int i, int j)
        {
            if (j >= 9 && i < 8)
            {
                i++;
                j = 0;
            }
            if (i >= 9 && j >= 9)
            {
                return true;
            }
            if (i < 3)
            {
                if (j < 3)
                {
                    j = 3;
                }
            }
            else if (i < 6)
            {
                if (j == (i / 3) * 3)
                {
                    j += 3;
                }
            }
            else
            {
                if (j == 6)
                {
                    i++;
                    j = 0;
                    if (i >= 9)
                    {
                        return true;
                    }
                }
            }

            for (int num = 1; num <= 9; num++)
            {
                if (checkIfSafe(grid, i, j, num))
                {
                    grid[i, j] = num;
                    if (fillRemaining(grid, i, j + 1))
                    {
                        return true;
                    }
                    grid[i, j] = 0;
                }
            }
            return false;
        }

        // Рандомно задаем массив в соответствии со сложностью
        static void removeKDigits(int[,] grid)
        {
            
            Random rand = new Random();
            switch (Game.comp) 
            {
                case 1:
                    Game.zero=rand.Next(35, 45);
                    Game.mistMax = 5;
                    break;
                case 2:
                    Game.zero = rand.Next(45, 55);
                    Game.mistMax = 3;
                    break;
                case 3:
                    Game.zero = rand.Next(55, 65);
                    Game.mistMax = 1;
                    break;

            }
            int t = Game.zero;
            while ( t> 0)
            {
            m1:
                int i = rand.Next(0, 9);
                int j = rand.Next(0, 9);

                if (grid[i, j] != 0)
                {
                    grid[i, j] = 0;
                    t--;
                }
                else { goto m1; }
            }
        }

        // Создаем сетку 
        public static int[,] sudokuGenerator(int k)
        {
            int[,] grid = new int[9, 9];

            fillDiagonal(grid);
            fillRemaining(grid, 0, 3);
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    n[i, j] = grid[i, j];
                }
            }

            removeKDigits(grid);

            return grid;
        }

       public static int[,] n = new int[9, 9];
    }
}
