using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Minesweeper
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int Cols = 10;
        public int Rows = 10;
        public int Bombs = 10;

        public int SizePx = 50;
        public MinesweeperClass Minesweeper { get; set; }
        private BrushConverter BC = new BrushConverter();
        Grid DynamicGrid = new Grid();

        public MainWindow()
        {
            InitializeComponent();

            Minesweeper = new MinesweeperClass(Cols, Rows, Bombs);

            DynamicGrid.Width = (Cols * SizePx);
            DynamicGrid.HorizontalAlignment = HorizontalAlignment.Left;
            DynamicGrid.VerticalAlignment = VerticalAlignment.Top;

            // Create Columns
            for (int i = 0; i < Cols; i++)
            {
                ColumnDefinition gridCol = new ColumnDefinition();
                gridCol.Width = new GridLength(SizePx);
                DynamicGrid.ColumnDefinitions.Add(gridCol);
            }

            // Create Rows
            for (int i = 0; i < Rows; i++)
            {
                RowDefinition gridRow = new RowDefinition();
                gridRow.Height = new GridLength(SizePx);
                DynamicGrid.RowDefinitions.Add(gridRow);

            }

            int count = 1;

            for (int i = 0; i < Cols; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    Button MyControl1 = new Button();
                    MyControl1.Content = "";
                    MyControl1.Name = "Button" + count.ToString();
                    MyControl1.Click += Button_Click;

                    Grid.SetColumn(MyControl1, j);
                    Grid.SetRow(MyControl1, i);
                    DynamicGrid.Children.Add(MyControl1);

                    count++;
                }
            }

            // Add the Grid as the Content of the Parent Window Object
            this.Content = DynamicGrid;
            this.Height = (Rows * SizePx) + SizePx;
            this.Width = (Cols * SizePx) + SizePx;
            this.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            int col = Grid.GetColumn(b);
            int row = Grid.GetRow(b);
            Minesweeper.CheckClick(col, row);

            Render();
        }

        private void Render()
        {
            foreach (Button child in DynamicGrid.Children)
            {
                int col = Grid.GetColumn(child);
                int row = Grid.GetRow(child);

                if (Minesweeper.StatusArr[col, row] == BombStatus.notClicked)
                    child.Content = "";
                else if (Minesweeper.StatusArr[col, row] == BombStatus.Clicked)
                    child.Background = (Brush)BC.ConvertFrom("#BDBDBD");
                else if (Minesweeper.StatusArr[col, row] == BombStatus.Marked)
                    child.Content = "X";
                else
                    child.Content = (int)Minesweeper.StatusArr[col, row];
            }
        }
    }
}
