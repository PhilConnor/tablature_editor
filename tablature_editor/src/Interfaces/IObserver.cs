namespace PFE.Interfaces
{
    public interface IObserver
    {
        void NotifyRedraw();
        void NotifyNewStaffAdded();
        void NotifyScrollToCursor();
    }
}
