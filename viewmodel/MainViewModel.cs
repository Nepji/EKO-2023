using EKO.Core;


namespace EKO.viewmodel
{
    class MainViewModel : ObservableObj
    {
        public RelayCommand bdTables { get; set; }
        public RelayCommand calc { get; set; }


        public BDTablesVM bdTablesVM { get; set; }
        public CalculatorVM calculatorVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        public MainViewModel()
        {
            bdTablesVM = new BDTablesVM();
            calculatorVM = new CalculatorVM();

            bdTables = new RelayCommand(o =>
            {
                CurrentView = bdTablesVM;
            });

            calc = new RelayCommand(o =>
            {
                CurrentView = calculatorVM;
            });
        }
    }
}
