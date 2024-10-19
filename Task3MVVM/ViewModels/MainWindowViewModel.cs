using System;
using System.Runtime.CompilerServices;
using ReactiveUI;
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

    public bool IsTestCompleted() => this._TestsCompleted;
    public string ArrStatus
    {
        get => _arrStatus;
        set
        {
            _arrStatus = value;
            OnPropertyChanged(nameof(ArrStatus));  // Уведомляем об изменении
        }
    }
    
    public int ArrsIndx
    {
        get => _arrsindx;
        set
        {
            _arrsindx = value;
            Model.ArrgroupStatus(value);
            OnPropertyChanged(nameof(ArrsIndx));  // Уведомляем об изменении
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

    public MainWindowViewModel()
    {
        this._TestsCompleted = false;
        this.NotReady();
        this.ArrsIndx = 0;
        this.AlgsIndx = 0;
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
        Model.GenTests(MAGICK_NUM_DEFAULT_ARRAY_SIZE+_algsindx);
    }
    
    public long[][] StartTest()
    {
        _TestsCompleted = true;
        return Model.StartTest();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}