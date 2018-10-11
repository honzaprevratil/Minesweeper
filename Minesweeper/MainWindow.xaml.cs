using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
        public int Bombs = 55;

        public int SizePx = 25;
        public MinesweeperClass Minesweeper { get; set; }

        private List<Mode> Modes { get; set; } = new List<Mode>();
        private Grid DynamicGrid { get; set; }

        private BrushConverter BC = new BrushConverter();
        private string[] Colors = new string[] { "#F5F5F5", "#1565C0", "#558B2F", "#c62828", "#311B92", "#3E2723", "#004D40", "#263238", "#212121" };

        public Timer timer1;

        public MainWindow()
        {
            InitializeComponent();

            Modes.Add(new Mode("Beginner", 9, 9, 5));
            Modes.Add(new Mode("Intermediate", 16, 16, 40));
            Modes.Add(new Mode("Expert", 30, 16, 99));
            Modes.Add(new Mode("New", 20, 20, 55));

            ModeCombobox.ItemsSource = Modes;
            ModeCombobox.SelectedIndex = 3;

            GenerateGrid();
        }

        private void Mode_Selected(object sender, RoutedEventArgs e)
        {
            Cols = (ModeCombobox.SelectedItem as Mode).Cols;
            Rows = (ModeCombobox.SelectedItem as Mode).Rows;
            Bombs = (ModeCombobox.SelectedItem as Mode).Bombs;
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            DynamicGrid = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5, 0, 5, 5)
            };
            Grid.SetRow(DynamicGrid, 1);
            Minesweeper = new MinesweeperClass(Cols, Rows, Bombs);

            // Create Columns
            for (int i = 0; i < Cols; i++)
            {
                ColumnDefinition gridCol = new ColumnDefinition
                {
                    Width = new GridLength(SizePx)
                };
                DynamicGrid.ColumnDefinitions.Add(gridCol);
            }

            // Create Rows
            for (int i = 0; i < Rows; i++)
            {
                RowDefinition gridRow = new RowDefinition
                {
                    Height = new GridLength(SizePx)
                };
                DynamicGrid.RowDefinitions.Add(gridRow);

            }

            int count = 1;

            for (int i = 0; i < Cols; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    Button MyControl1 = new Button
                    {
                        Name = "Button" + count.ToString(),
                        FontWeight = FontWeights.Bold,
                        FontSize = 16.5
                    };
                    MyControl1.Click += Mouse_Left_Click;
                    MyControl1.MouseRightButtonDown += Mouse_Right_Click;

                    Grid.SetColumn(MyControl1, i);
                    Grid.SetRow(MyControl1, j);
                    DynamicGrid.Children.Add(MyControl1);

                    count++;
                }
            }

            if (MainGrid.Children.Count > 1)
            {
                MainGrid.Children.RemoveAt(1);
            }
            // Add the Grid as the Content of the Parent Window Object
            MainGrid.Children.Add(DynamicGrid);

            InitTimer();

            // bind time to display
            timeLabel.SetBinding(ContentProperty, new Binding("Time") { Source = Minesweeper });
            statusLabel.SetBinding(ContentProperty, new Binding("GameStatus") { Source = Minesweeper });

            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.Show();
        }

        private void Mouse_Right_Click(object sender, MouseButtonEventArgs e)
        {
            Button b = (Button)sender;
            int col = Grid.GetColumn(b);
            int row = Grid.GetRow(b);
            Minesweeper.RightClick(col, row);
            Render();
        }

        private void Mouse_Left_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            int col = Grid.GetColumn(b);
            int row = Grid.GetRow(b);
            Minesweeper.LeftClick(col, row);
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

            maxMinesLabel.Content = "/ " + Bombs.ToString("000");
            minesLabel.Content = (Bombs - Minesweeper.Flags).ToString("000");
        }

        private void Set_Style(Button button, int col, int row)
        {
            if (Minesweeper.StatusArr[col, row] == BombStatus.notClicked)
            {
                button.Content = ""; // CONTENT
                button.Background = (Brush)BC.ConvertFrom("#F5F5F5"); // BRUSH
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
            else if (Minesweeper.StatusArr[col, row] == BombStatus.Defused)
            {
                Image image = new Image();
                image.Source = new BitmapImage(new Uri(@"../../imgs/bomb-x.png", UriKind.Relative));
                button.Content = image; // CONTENT
            }
            else
            {
                button.Content = (int)Minesweeper.StatusArr[col, row]; // CONTENT
                button.Foreground = (Brush)BC.ConvertFrom(Colors[(int)Minesweeper.StatusArr[col, row]]);
                button.Background = (Brush)BC.ConvertFrom("#DADADA"); // BRUSH
            }
        }

        public void InitTimer()
        {
            if (timer1 != null)
                timer1.Stop();

            timer1 = new Timer
            {
                Interval = 1000, // in miliseconds
                Enabled = true,
                AutoReset = true
            };
            timer1.Elapsed += Timer1_Elapsed;
            timer1.Start();
        }

        public void Timer1_Elapsed(object sender, EventArgs e)
        {
            if (!Minesweeper.firstClick)
            {
                Minesweeper.DecreaseTime(-1);
            }
        }
    }
}
