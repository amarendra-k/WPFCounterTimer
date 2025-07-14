using System.Windows;
using System.Windows.Input;
using WPFCounterTimer.Commands;
using WPFCounterTimer.Views;

namespace WPFCounterTimer.ViewModels
{
    public class CountdownViewModel : BaseViewModel, IDisposable
    {
        #region Fields
        private bool _isDisposed = false;
        private int _timeLeft = 10;
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private ManualResetEvent _manualResetEvent = new ManualResetEvent(true);
        #endregion

        #region Properties
        public int TimeLeft
        {
            get => _timeLeft;
            set { _timeLeft = value; OnPropertyChanged(nameof(TimeLeft)); }
        }
        #endregion

        #region Commands
        public ICommand CancelCommand { get; }
        #endregion

        #region Events
        public event EventHandler? TimerCompletedEvent;
        #endregion

        #region Constructor
        public CountdownViewModel()
        {
            CancelCommand = new RelayCommand(_ => ShowCancelDialog());
            StartCountdown();
        }
        #endregion

        #region Private methods
        private async void StartCountdown()
        {
            try
            {
                for (int i = 10; i > 0; i--)
                {
                    TimeLeft = i;
                    await Task.Delay(1000, _cts.Token).ConfigureAwait(false);
                    _manualResetEvent.WaitOne();
                }
            }
            catch (TaskCanceledException)
            {

            }
            TimerCompletedEvent?.Invoke(this, EventArgs.Empty);
        }

        private void ShowCancelDialog()
        {
            _manualResetEvent.Reset();
            var vm = new CancelDialogViewModel(
                onConfirm: () =>
                {
                    _cts.Cancel();
                    _manualResetEvent.Set();
                },
                onAbort: () => CloseWindow(typeof(CancelDialogViewModel)));

            var dialog = new CancelDialog { DataContext = vm, Owner = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive) };
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dialog.Width = dialog.Owner.Width - 50;
            dialog.Height = dialog.Owner.Height - 50;
            dialog.ShowDialog();
            _manualResetEvent.Set();
        }

        #endregion

        #region Protected methods
        protected virtual void Dispose(bool isDisposing)
        {
            if (_isDisposed) return;

            _manualResetEvent.Dispose();
            _cts.Dispose();
            _isDisposed = true;
        }
        #endregion

        #region Public methods
        public void Dispose()
        {
            _cts.Cancel();
            Dispose(true);
            GC.SuppressFinalize(true);

        }
        #endregion

        #region Destructor
        ~CountdownViewModel()
        {
            Dispose(false);
        }
        #endregion
    }
}
