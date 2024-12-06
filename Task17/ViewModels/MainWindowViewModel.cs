using System;
using System.ComponentModel;
using Task17.Models;

namespace Task17.ViewModels;

public class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
{
    private bool _testCompleted;
    public int operationIndx { get; set; }
    
    public MainWindowViewModel()
    { 
        this._testCompleted = false;
    }

    public long[] Start(ITestable<int> dataStruct, int minTenPower, int maxTenPower)
    {
        switch (operationIndx)
        {
            case 0:
                return Model.StartGetTest(dataStruct, minTenPower, maxTenPower);
            case 1:
                return Model.StartSetTest(dataStruct, minTenPower, maxTenPower);
            case 2:
                return Model.StartAddTest(dataStruct, minTenPower, maxTenPower);
            case 3:
                return Model.StartInsertionTest(dataStruct, minTenPower, maxTenPower);
            case 4:
                return Model.StartRemoveTest(dataStruct, minTenPower, maxTenPower);
            default:
                throw new Exception("Test type not found");
        }
    }
}