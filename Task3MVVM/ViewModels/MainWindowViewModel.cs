using System;
using System.Runtime.CompilerServices;
using Task3MVVM.Models;

namespace Task3MVVM.ViewModels;

using System.ComponentModel;

public class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
{
    private const int MAGICK_NUM_DEFAULT_ARRAY_SIZE = 4;
    private bool _TestsCompleted;
    private int _algsindx;
    private int _arrsindx;
    private string _arrStatus;
    private int _selectedDataTypeindx;

    private enum DataType
    {
        Int = 0,
        Double = 1,
        String = 2,
        DateTime = 3
    }
    
    private static Type GetTypeByDataType(DataType dataType)
    {
        switch (dataType)
        {
            case DataType.Int:
                return typeof(int);
            case DataType.Double:
                return typeof(double);
            case DataType.String:
                return typeof(string);
            case DataType.DateTime:
                return typeof(DateTime);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public bool IsTestCompleted() => this._TestsCompleted;

    public string ArrStatus
    {
        get => _arrStatus;
        set
        {
            _arrStatus = value;
            OnPropertyChanged(nameof(ArrStatus)); // Уведомляем об изменении
        }
    }

    public int ArrsIndx
    {
        get => _arrsindx;
        set
        {
            _arrsindx = value;
            Model.ArrgroupStatus(value);
            OnPropertyChanged(nameof(ArrsIndx)); // Уведомляем об изменении
        }
    }

    public int AlgsIndx
    {
        get => _algsindx;
        set
        {
            _algsindx = value;
            Model.AlggroupStatus(value);
            OnPropertyChanged(nameof(AlgsIndx));
        }
    }

    public int SelectedDataIndx
    {
        get => _selectedDataTypeindx;
        set
        {
            _selectedDataTypeindx = value;
            Model.DataTypeStatus(GetTypeByDataType((DataType)_selectedDataTypeindx));
            OnPropertyChanged(nameof(SelectedDataIndx));
        }
    }

public MainWindowViewModel()
    {
        this._TestsCompleted = false;
        this.NotReady();
        this.ArrsIndx = 0;
        this.AlgsIndx = 0;
        this.SelectedDataIndx = 0;
    }

    public void NotReady()
    {
        ArrStatus = "Не готовы.";
        _TestsCompleted = false;
    }
    
    public void InProgressTest()
    {
        ArrStatus = "Создаются";
    }
    
    public void ReadyTest()
    {
        ArrStatus = "Готовы";
    }

    public void Alert()
    {
        ArrStatus = "Нужны массивы!";
        _TestsCompleted = false;
    }

    public void GenArrs()
    {
        if(GetTypeByDataType((DataType)_selectedDataTypeindx) == typeof(int))
            Model.GenTests<int>(MAGICK_NUM_DEFAULT_ARRAY_SIZE+_algsindx);
        else if(GetTypeByDataType((DataType)_selectedDataTypeindx) == typeof(double))
            Model.GenTests<double>(MAGICK_NUM_DEFAULT_ARRAY_SIZE+_algsindx);
        else if(GetTypeByDataType((DataType)_selectedDataTypeindx) == typeof(string))
            Model.GenTests<string>(MAGICK_NUM_DEFAULT_ARRAY_SIZE+_algsindx);
        else if(GetTypeByDataType((DataType)_selectedDataTypeindx) == typeof(DateTime))
            Model.GenTests<DateTime>(MAGICK_NUM_DEFAULT_ARRAY_SIZE+_algsindx);
        else throw new Exception("Datatype not found"); 
    }
    
    public long[][] StartTest()
    {
        _TestsCompleted = true;
        if(GetTypeByDataType((DataType)_selectedDataTypeindx) == typeof(int))
            return Model.StartTest<int>(MAGICK_NUM_DEFAULT_ARRAY_SIZE+_algsindx);
        else if (GetTypeByDataType((DataType)_selectedDataTypeindx) == typeof(double))
            return Model.StartTest<double>(MAGICK_NUM_DEFAULT_ARRAY_SIZE + _algsindx);
        else if (GetTypeByDataType((DataType)_selectedDataTypeindx) == typeof(string))
            return Model.StartTest<string>(MAGICK_NUM_DEFAULT_ARRAY_SIZE + _algsindx);
        else if (GetTypeByDataType((DataType)_selectedDataTypeindx) == typeof(DateTime))
            return Model.StartTest<DateTime>(MAGICK_NUM_DEFAULT_ARRAY_SIZE + _algsindx);
        else throw new Exception("Datatype not found");
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}