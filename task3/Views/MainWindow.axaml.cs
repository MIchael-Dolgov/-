using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ScottPlot.Avalonia;
using Task3MVVM.Models;
using Task3MVVM.ViewModels;

namespace Task3MVVM.Views;

public partial class MainWindow : Window
{
    public static MainWindowViewModel viewModel = new MainWindowViewModel();
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    private void AddToPlot(long[] dataX, long[] dataY)
    {
        AvaPlot? MainPlot = this.FindControl<AvaPlot>("MainPlot");
        if (MainPlot != null)
        {
            MainPlot.Plot.Add.Scatter(dataX, dataY);
            MainPlot.Plot.Axes.Bottom.Label.Text = "Размер массива(кол-во эл)";
            MainPlot.Plot.Axes.Left.Label.Text = "Время выполнения(мс)";
        }
        else
        {
            Console.WriteLine("Plot name was not found");
        }
    }
    private void AddToPlot(long[] dataX, long[] dataY, int graphName)
    {
        
        AvaPlot? MainPlot = this.FindControl<AvaPlot>("MainPlot");
        if (MainPlot!=null)
        {
            var scatt = MainPlot.Plot.Add.Scatter(dataX, dataY);
            scatt.LegendText = $"Function: {graphName}";
            MainPlot.Plot.Axes.Bottom.Label.Text = "Размер массива(кол-во эл)";
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
        if (MainPlot!=null)
        {
            MainPlot.Plot.Clear();
        }
        else
        {
            Console.WriteLine("Plot name was not found");
        }
    }

    public MainWindow()
    {
        DataContext = viewModel;
        // Этот объект инициализирует компоненты, после данного метода можно тянуть элементы, иначе - null
        InitializeComponent();
        AddToPlot([],[]);
    }

    private void ArrGenButton(object? sender, RoutedEventArgs e)
    {
        viewModel.InProgressTest();  // Обновляем текст через ViewModel
        DataContext = viewModel;
        viewModel.GenArrs();
        DataContext = viewModel;
        viewModel.ReadyTest();
        DataContext = viewModel;
    }
    private void StartTestButton(object? sender, RoutedEventArgs e)
    {
        if (viewModel.ArrStatus != "Готовы")
        {
            viewModel.Alert();
            DataContext = viewModel;
            return;
        }
        ClearPlot();
        long[][] results;
        results = viewModel.StartTest();
        for (int i = 0; i < results.Length; i++)
        {
            var variable = new long[results[i].Length];
            for (int j = 1; j <= results[i].Length; j++)
                variable[j - 1] = j;
            AddToPlot(variable,results[i], i);
        }
    }
    private void SaveResultsButton(object? sender, RoutedEventArgs e)
    {
        if(viewModel.IsTestCompleted() == true)
            Model.SaveCurrentTestToFile("/Users/michael/Documents/University (original)/2 course/casd/casd-labs/Task3/Task3MVVM/Task3MVVM/Models/arrays.txt");
        else 
        {
            viewModel.Alert();
            DataContext = viewModel;
        }
    }
    private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var comboBox = sender as ComboBox;
        if (comboBox != null & comboBox.SelectedIndex != null)
        {
            if (viewModel != null)
            {
                viewModel.AlgsIndx = comboBox.SelectedIndex;
                viewModel.Alert();
                DataContext = viewModel;
            }
        }
    }

    private void ArrSelection_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var comboBox = sender as ComboBox;
        if (comboBox != null & comboBox.SelectedIndex != null)
        {
            if (viewModel != null)
            {
                viewModel.ArrsIndx = comboBox.SelectedIndex;
                viewModel.Alert();
                DataContext = viewModel;
            }
        }
    }
}