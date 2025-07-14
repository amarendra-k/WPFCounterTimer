using System.Windows;
using System.Windows.Input;
using WPFCounterTimer.Commands;
using WPFCounterTimer.Views;

namespace WPFCounterTimer.ViewModels
{
    public class MainViewModel : BaseViewModel
    {

        public ICommand RunCommand { get; }

        public MainViewModel()
        {
            RunCommand = new RelayCommand(_ => RunOperation());
        }

        private void RunOperation()
        {
            var vm = new CountdownViewModel();
            WeakEventManager<CountdownViewModel, EventArgs>.AddHandler(vm, nameof(CountdownViewModel.TimerCompletedEvent), OnTimerFinished);
            var dialog = new CountdownDialog { DataContext = vm };
            dialog.Owner = Application.Current.MainWindow;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dialog.Width = dialog.Owner.Width - 30;
            dialog.Height = dialog.Owner.Height - 30;
            dialog.ShowDialog();
            vm.Dispose();
            WeakEventManager<CountdownViewModel, EventArgs>.RemoveHandler(vm, nameof(CountdownViewModel.TimerCompletedEvent), OnTimerFinished);
        }

        private void OnTimerFinished(object? sender, EventArgs e)
        {
            CloseWindow(sender?.GetType());
        }
    }
}
