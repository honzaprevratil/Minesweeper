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
        private bool firstClick = true;

        private int Cols { get; set; }
        private int Rows { get; set; }
        private int MaxBombs { get; set; }

        private int[,] BombArr { get; set; }
        public BombStatus[,] StatusArr { get; set; }
        public int Flags { get; set; } = 0;

        public MinesweeperClass(int cols, int rows, int bombs)
        {
            Cols = cols;
            Rows = rows;
            MaxBombs = bombs;
        }

        public void GenerateBombs(int colIndex, int rowIndex)
        {
            Flags = 0;
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

            for (int i = 0; i < MaxBombs; i++)
            {
                int randCol = RNG.Next(0, Cols);
                int randRow = RNG.Next(0, Rows);

                if (BombArr[randCol, randRow] == 0 && !(randCol == colIndex && randRow == rowIndex))
                {
                    BombArr[randCol, randRow] = 1;
                }
                else
                    i--;
            }
        }

        public void CheckClick(int colIndex, int rowIndex)
        {
            if (!firstClick)
            {
                if (StatusArr[colIndex, rowIndex] != BombStatus.Marked)
                {
                    if (BombArr[colIndex, rowIndex] == 1)
                    {
                        // BOOM!
                        StatusArr[colIndex, rowIndex] = BombStatus.Boomed;
                        RevealAll();
                        firstClick = true;
                    }
                    else
                    {
                        CheckAdjencent(colIndex, rowIndex);
                    }
                }
            } else
            {
                GenerateBombs(colIndex, rowIndex);
                CheckAdjencent(colIndex, rowIndex);
                firstClick = false;
            }
        }

        public void MarkClick(int colIndex, int rowIndex)
        {
            if (!firstClick)
            {
                if (StatusArr[colIndex, rowIndex] == BombStatus.notClicked)
                {
                    StatusArr[colIndex, rowIndex] = BombStatus.Marked;
                    Flags++;
                }
                else if (StatusArr[colIndex, rowIndex] == BombStatus.Marked)
                {
                    StatusArr[colIndex, rowIndex] = BombStatus.notClicked;
                    Flags--;
                }
            }
            else
            {
                GenerateBombs(colIndex, rowIndex);
                CheckAdjencent(colIndex, rowIndex);
                firstClick = false;
            }
        }

        private void RevealAll()
        {
            for (int i = 0; i < Cols; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    if (StatusArr[i, j] != BombStatus.Marked && BombArr[i, j] == 1)
                        StatusArr[i, j] = BombStatus.Boomed;
                    else if (StatusArr[i, j] == BombStatus.Marked && BombArr[i, j] == 1)
                        StatusArr[i, j] = BombStatus.Defused;
                    else
                        CheckAdjencent(i, j);
                }
            }
        }

        private void CheckAdjencent(int colIndex, int rowIndex)
        {
            int bombsCount = 0;

            for (int i = (colIndex-1); i <= (colIndex+1); i++)
            {
                for (int j = (rowIndex-1); j <= (rowIndex+1); j++)
                {
                    // not the same 
                    if ( !(i == colIndex && j == rowIndex ) && i != -1 && j != -1 && i < Cols && j < Rows)
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

                for (int i = (colIndex-1); i <= (colIndex+1); i++)
                {
                    for (int j = (rowIndex-1); j <= (rowIndex+1); j++)
                    {
                        // not the same 
                        if (!(i == colIndex && j == rowIndex) && i != -1 && j != -1 && i < Cols && j < Rows && StatusArr[i, j] == BombStatus.notClicked)
                        {
                            CheckAdjencent(i, j);
                        }
                    }
                }
            }
        }
    }
}
