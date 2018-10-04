﻿using System;
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
        public int Cols = 20;
        public int Rows = 20;
        public int Bombs = 5;

        public int SizePx = 25;
        public MinesweeperClass Minesweeper { get; set; }

        private BrushConverter BC = new BrushConverter();
        private string[] Colors = new string[] {"", "#1565C0", "#558B2F", "#c62828", "#311B92", "#3E2723", "#004D40", "#263238", "#212121" };
        private Grid DynamicGrid = new Grid();

        public MainWindow()
        {
            InitializeComponent();

            Minesweeper = new MinesweeperClass(Cols, Rows, Bombs);

            DynamicGrid.HorizontalAlignment = HorizontalAlignment.Center;
            DynamicGrid.VerticalAlignment = VerticalAlignment.Center;
            DynamicGrid.Margin = new Thickness(5, 0, 5, 5);
            Grid.SetRow(DynamicGrid, 1);

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
                    MyControl1.Name = "Button" + count.ToString();
                    MyControl1.FontWeight = FontWeights.Bold;
                    MyControl1.FontSize = 16.5;

                    MyControl1.Click += Button_Click;
                    MyControl1.MouseRightButtonDown += Button_Right_Click;

                    Grid.SetColumn(MyControl1, i);
                    Grid.SetRow(MyControl1, j);
                    DynamicGrid.Children.Add(MyControl1);

                    count++;
                }
            }

            // Add the Grid as the Content of the Parent Window Object
            MainGrid.Children.Add(DynamicGrid);
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.Show();
        }

        private void Button_Right_Click(object sender, MouseButtonEventArgs e)
        {
            Button b = (Button)sender;
            int col = Grid.GetColumn(b);
            int row = Grid.GetRow(b);
            Minesweeper.MarkClick(col, row);
            Render();
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

                Set_Style(child, col, row);
            }
        }

        private void Set_Style(Button button, int col, int row)
        {
            if (Minesweeper.StatusArr[col, row] == BombStatus.notClicked)
            {
                button.Content = ""; // CONTENT
                button.Background = (Brush)BC.ConvertFrom("#EEEEEE"); // BRUSH
            }
            else if (Minesweeper.StatusArr[col, row] == BombStatus.Clicked)
            {
                button.Content = ""; // CONTENT
                button.Background = (Brush)BC.ConvertFrom("#BDBDBD"); // BRUSH
            }
            else if (Minesweeper.StatusArr[col, row] == BombStatus.Marked)
            {
                Image image = new Image();
                image.Source = new BitmapImage(new Uri(@"../../imgs/flag.png", UriKind.Relative));
                button.Content = image; // CONTENT
            }
            else if (Minesweeper.StatusArr[col, row] == BombStatus.Boomed)
            {
                Image image = new Image();
                image.Source = new BitmapImage(new Uri(@"../../imgs/bomb.png", UriKind.Relative));
                button.Content = image; // CONTENT
            }
            else
            {
                button.Content = (int)Minesweeper.StatusArr[col, row]; // CONTENT
                button.Foreground = (Brush)BC.ConvertFrom(Colors[(int)Minesweeper.StatusArr[col, row]]);
            }
        }
    }
}
