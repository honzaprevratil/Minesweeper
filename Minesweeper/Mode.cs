using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public class Mode
    {
        public string Name { get; set; }

        public int Cols { get; set; }
        public int Rows { get; set; }
        public int Bombs { get; set; }
        public override string ToString()
        {
            return Name + " (" + Cols + "x" + Rows + " - B:"+ Bombs +")";
        }

        public Mode(string name, int cols, int rows, int bombs)
        {
            Name = name;
            Cols = cols;
            Rows = rows;
            Bombs = bombs;
        }
    }
}
