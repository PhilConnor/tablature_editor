namespace PFE.Interfaces
{
    public interface IObservable
    {
        void Subscribe(IObserver observer);
    }
}
