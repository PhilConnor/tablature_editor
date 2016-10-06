namespace PFE.Interfaces
{
    public interface IObserverable
    {
        void Subscribe(IObserver observer);
    }
}
