using System;
using Avalonia.Controls;
using ScottPlot.Avalonia;
using Algs;

namespace Task3;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        // Это инициализирует компоненты, после данного метода можно тянуть элементы, иначе - null
        InitializeComponent();
        
        double[] dataX = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
        double[] dataY = {2, 4, 1, 1, 10, 26, 22, 3, 21, 12 };

        AvaPlot? MainPlot = this.FindControl<AvaPlot>("MainPlot");
        if (MainPlot!=null)
        {
            MainPlot.Plot.Add.Scatter(dataX, dataY);
            MainPlot.Refresh();
        }
        else
        {
            Console.WriteLine("Plot name was not found");
        }
    }
}