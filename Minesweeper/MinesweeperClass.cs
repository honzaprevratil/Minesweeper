using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public class MinesweeperClass : INotifyPropertyChanged
    {
        private Random RNG = new Random();
        public bool firstClick = true;

        public event PropertyChangedEventHandler PropertyChanged;

        private int Cols { get; set; }
        private int Rows { get; set; }
        private int MaxBombs { get; set; }

        private int[,] BombArr { get; set; }
        public BombStatus[,] StatusArr { get; set; }
        public int Flags { get; set; } = 0;

        private int time = 999;
        public string Time
        {
            get
            {
                return time.ToString("000");
            }
            set
            {
                time = int.Parse(value);
                OnPropertyChanged("Time");
            }
        }

        private string gameStatus = "";
        public string GameStatus
        {
            get { return gameStatus; }
            set
            {
                gameStatus = value;
                OnPropertyChanged("GameStatus");
            }
        }

        public void DecreaseTime(int sec = 1)
        {
            time = time - sec;
            if (time <= 999)
                OnPropertyChanged("Time");
        }

        public MinesweeperClass(int cols, int rows, int bombs)
        {
            Cols = cols;
            Rows = rows;
            MaxBombs = bombs;
        }

        public void GenerateBombs(int colIndex, int rowIndex)
        {
            Flags = 0;
            time = 0;
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

        public void LeftClick(int colIndex, int rowIndex)
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
                        GameStatus = "You have been boomed!";
                    }
                    else
                    {
                        CheckAdjencent(colIndex, rowIndex);
                        CheckWin();
                    }
                }
            } else
            {
                GenerateBombs(colIndex, rowIndex);
                CheckAdjencent(colIndex, rowIndex);
                firstClick = false;
                Time = "0";
                GameStatus = "";
            }
        }

        public void RightClick(int colIndex, int rowIndex)
        {
            if (!firstClick)
            {
                if (StatusArr[colIndex, rowIndex] == BombStatus.notClicked)
                {
                    if (Flags < MaxBombs)
                    {
                        StatusArr[colIndex, rowIndex] = BombStatus.Marked;
                        Flags++;
                    }
                }
                else if (StatusArr[colIndex, rowIndex] == BombStatus.Marked)
                {
                    StatusArr[colIndex, rowIndex] = BombStatus.notClicked;
                    Flags--;
                }
                CheckWin();
            }
            else
            {
                GenerateBombs(colIndex, rowIndex);
                CheckAdjencent(colIndex, rowIndex);
                firstClick = false;
                Time = "0";
                GameStatus = "";
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
                    else if (StatusArr[i, j] == BombStatus.Marked && BombArr[i, j] == 0)
                        StatusArr[i, j] = BombStatus.Marked;
                    else
                        CheckAdjencent(i, j);
                }
            }

            firstClick = true;
        }

        private void CheckWin()
        {
            bool WinOrNot = false;

            if (Flags == MaxBombs)
            {
                WinOrNot = true;
                for (int i = 0; i < Cols; i++)
                {
                    for (int j = 0; j < Rows; j++)
                    {
                        if ((StatusArr[i, j] != BombStatus.Marked && BombArr[i, j] == 1) || StatusArr[i, j] == BombStatus.notClicked)
                            WinOrNot = false;
                    }
                }
            }

            if (WinOrNot)
            {
                GameStatus = "You won!";
                RevealAll();
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

        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
