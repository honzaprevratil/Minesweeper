using FileHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public class CsvHelper
    {
        private FileHelperEngine<Score> engine = new FileHelperEngine<Score>();
        private string path = "score.csv";

        public List<Score> ReadCsv()
        {
            Score[] res;
            List<Score> ScoreList = new List<Score>();

            if (File.Exists(path))
            {
                res = engine.ReadFile(path);

                foreach (var record in res)
                {
                    ScoreList.Add(record);
                }
            }
            return ScoreList;
        }
        public void WriteCsvFile(List<Score> ScoreList)
        {
            if (!File.Exists(path))
            {
                FileStream x = File.Create(path);
                x.Close();
            }
            engine.WriteFile(path, ScoreList);
        }
    }
}
