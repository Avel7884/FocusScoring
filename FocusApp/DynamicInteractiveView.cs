namespace FocusApp
{
    public interface IDynamicInteractiveView<in T> : IInteractiveView
    {
        void Add(T obj);
        void Delete(T obj);
    }
    

    public interface IDynamicInteractiveView<in TAdd,in TDel> : IInteractiveView
    {
        void Add(TAdd obj);
        void Delete(TDel obj);
    }
}