namespace SampleMAUI
{
    public partial class MainPage : ContentPage
    {
        private MainViewModel _viewModel;

        public MainViewModel ViewModel
        {
            get => _viewModel;
            set { _viewModel = value; BindingContext = _viewModel; }
        }

        public MainPage()
        {
            InitializeComponent();

            ViewModel = new();
        }

    }
}
