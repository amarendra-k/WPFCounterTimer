using System.ComponentModel;
using System.Windows;

namespace WPFCounterTimer.ViewModels
{

    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public void CloseWindow(Type? dataContext)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var win = Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive && x.DataContext?.GetType()== dataContext);
                win?.Close();
            });
        }
    }
}
