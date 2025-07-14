using System.Windows.Input;
using WPFCounterTimer.Commands;

namespace WPFCounterTimer.ViewModels
{
    public class CancelDialogViewModel : BaseViewModel
    {
        public ICommand ConfirmCommand { get; }
        public ICommand AbortCommand { get; }

        public CancelDialogViewModel(Action onConfirm, Action onAbort)
        {
            ConfirmCommand = new RelayCommand(_ =>
            {
                onConfirm();
                CloseWindow(this.GetType());
            });

            AbortCommand = new RelayCommand(_ =>
            {
                onAbort();
            });
        }
    }
}
