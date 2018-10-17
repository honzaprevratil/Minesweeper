using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    [DelimitedRecord(";")]
    public class Score
    {
        public string Mode { get; set; }
        public int Time { get; set; }

        public Score() { }
        public Score(string mode, int time)
        {
            Time = time;
            Mode = mode;
        }
        public override string ToString()
        {
            return Mode + " " + Time;
        }
    }
}
