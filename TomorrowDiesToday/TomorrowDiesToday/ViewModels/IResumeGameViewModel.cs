using System.Windows.Input;

namespace TomorrowDiesToday.ViewModels
{
    public interface IResumeGameViewModel
    {
        string Title { get; }
        string GameId { get; }
        ICommand YesCommand { get; }
        ICommand NoCommand { get; }
    }
}