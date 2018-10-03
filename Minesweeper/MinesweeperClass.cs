using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public class MinesweeperClass
    {
        private Random RNG = new Random();
        public int Cols { get; set; }
        public int Rows { get; set; }

        public int[,] BombArr { get; set; }
        public BombStatus[,] StatusArr { get; set; }

        public MinesweeperClass(int cols, int rows, int bombs)
        {
            Cols = cols;
            Rows = rows;

            BombArr = new int[Cols, Rows];
            StatusArr = new BombStatus[Cols, Rows];
            for (int i = 0; i < Cols; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    BombArr[i, j] = 0;
                    StatusArr[i, j] = BombStatus.notClicked;
                }
            }

            GenerateBombs(bombs);
        }

        public void GenerateBombs(int bombs)
        {
            for (int i = 0; i < bombs; i++)
            {
                int randCol = RNG.Next(0, Cols);
                int randRow = RNG.Next(0, Rows);

                if (BombArr[randCol, randRow] == 0)
                {
                    BombArr[randCol, randRow] = 1;
                }
                else
                    i--;
            }
        }

        public void CheckClick(int colIndex, int rowIndex)
        {
            if (BombArr[colIndex, rowIndex] == 1)
            {
                // GAME OVER
            }
            else
            {
                CheckAdjencent(colIndex, rowIndex);
            }
        }

        private void CheckAdjencent(int colIndex, int rowIndex)
        {
            int bombsCount = 0;

            for (int i = (colIndex-1); i < (colIndex+1); i++)
            {
                for (int j = (rowIndex-1); j < (rowIndex+1); j++)
                {
                    // not the same 
                    if (i != colIndex && j != rowIndex && i != -1 && j != -1 && i < Cols && j < Rows)
                    {
                        if (BombArr[i, j] == 1)
                            bombsCount++;
                    }
                }
            }

            if (bombsCount > 0)
            {
                // bomb found, set status
                StatusArr[colIndex, rowIndex] = (BombStatus)bombsCount;
            }
            else
            {
                //  bomb not found,
                StatusArr[colIndex, rowIndex] = BombStatus.Clicked;
                // check adjencent
                for (int i = (colIndex - 1); i < (colIndex + 1); i++)
                {
                    for (int j = (rowIndex - 1); j < (rowIndex + 1); j++)
                    {
                        // not the same 
                        if (i != colIndex && j != rowIndex && i != -1 && j != -1 && i < Cols && j < Rows)
                        {
                            CheckAdjencent(i, j);
                        }
                    }
                }
            }
        }
    }
}
