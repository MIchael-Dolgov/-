using System.Threading;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia;
using Avalonia.Rendering;
using CrissCross.Models;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;


namespace CrissCross.Views
{
    public partial class MainWindow : Window
    {
        private const uint CHAR_BORDER_SIZE = 35;

        public MainWindow()
        {
            InitializeComponent();

            //char[,] matrix = new char[,] { { 'b', 'i', ' ', 'a' }, { 'b', ' ', 'b', 'a' }, { 'a', ' ', ' ', ' ' } };
            //var viewModel = new MainWindowViewModel(matrix);
            //DataContext = viewModel;
            CrossBoard board =
                new CrossBoard(
                    "/Users/michael/Documents/University (original)/2 course/casd/casd-labs/CrissCross/CrissCross/Models/Words.txt");
            if (board.SolveCrissCross())
            {
                char[,] matrix = board.matr._matrix;

                // Инициализация Grid для отображения матрицы
                Grid? grid = this.FindControl<Grid>("MatrixGrid");
                SetupGrid(grid!, matrix);

                // Заполнение Grid элементами матрицы
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        var textBlock = new TextBlock
                        {
                            Text = matrix[i, j].ToString(),
                            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
                        };
                        if (matrix[i, j] != '\0')
                        {
                            var border = new Border
                            {
                                BorderBrush = Brushes.DarkBlue,
                                BorderThickness = new Thickness(2),
                                Child = textBlock,
                                Width = CHAR_BORDER_SIZE, // Фиксированная ширина клетки
                                Height = CHAR_BORDER_SIZE // Фиксированная высота клетки
                            };


                            grid!.Children.Add(border);
                            Grid.SetRow(border, i);
                            Grid.SetColumn(border, j);
                        }
                    }
                }
            }
            else
            {
                var messageBox = MessageBoxManager.GetMessageBoxStandard(
                    "Error",
                    "Решение не найдено!",
                    ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Error
                );
                //Thread.Sleep(1);
                messageBox.ShowAsync();
            }
        }

        private void SetupGrid(Grid grid, char[,] matrix)
        {
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();

            // Устанавливаем количество строк и столбцов с фиксированными размерами
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(35) }); // 35 пикселей
            }

            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(35) }); // 35 пикселей
            }
        }
    }
}