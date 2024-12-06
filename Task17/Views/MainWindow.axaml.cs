using System;
using ScottPlot.Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Task17.Models;
using Task17.ViewModels;

namespace Task17.Views
{

    public partial class MainWindow : Window
    {
        public static MainWindowViewModel viewModel = new MainWindowViewModel();

        public MainWindow()
        {
            DataContext = viewModel;
            // Этот объект инициализирует компоненты, после данного метода можно тянуть элементы, иначе - null
            InitializeComponent();
            AddToPlot([], []);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void TestType(object? sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null & comboBox.SelectedIndex != null)
            {
                if (viewModel != null)
                {
                    viewModel.operationIndx = comboBox.SelectedIndex;
                    DataContext = viewModel;
                }
            }
        }

        private void StartTestButton(object? sender, RoutedEventArgs e)
        {
            ClearPlot();
            MyArrayDeque<int> arrayDeque = new MyArrayDeque<int>();
            MyLinkedList<int> linkedList = new MyLinkedList<int>();
            long[] arrayResults = viewModel.Start(arrayDeque, 5, 8);
            long[] listResults = viewModel.Start(linkedList, 5, 8);
            AddToPlot([5,6,7,8], arrayResults, "MyArrayDeque");
            AddToPlot([5,6,7,8], listResults, "MyLinkedList");
        }

        private void AddToPlot(long[] dataX, long[] dataY)
        {
            AvaPlot? MainPlot = this.FindControl<AvaPlot>("MainPlot");
            if (MainPlot != null)
            {
                MainPlot.Plot.Add.Scatter(dataX, dataY);
                MainPlot.Plot.Axes.Bottom.Label.Text = "Количество элементов (степени 10-ти)";
                MainPlot.Plot.Axes.Left.Label.Text = "Время выполнения(мс)";
            }
            else
            {
                Console.WriteLine("Plot name was not found");
            }
        }

        private void AddToPlot(long[] dataX, long[] dataY, string graphName)
        {

            AvaPlot? MainPlot = this.FindControl<AvaPlot>("MainPlot");
            if (MainPlot != null)
            {
                var scatt = MainPlot.Plot.Add.Scatter(dataX, dataY);
                scatt.LegendText = $"Function: {graphName}";
                MainPlot.Plot.Axes.Bottom.Label.Text = "Количество элементов (степени 10-ти)";
                MainPlot.Plot.Axes.Left.Label.Text = "Время выполнения(мс)";
                MainPlot.Plot.ShowLegend();
                MainPlot.Refresh();
            }
            else
            {
                Console.WriteLine("Plot name was not found");
            }
        }

        private void ClearPlot()
        {

            AvaPlot? MainPlot = this.FindControl<AvaPlot>("MainPlot");
            if (MainPlot != null)
            {
                MainPlot.Plot.Clear();
            }
            else
            {
                Console.WriteLine("Plot name was not found");
            }
        }
    }
}