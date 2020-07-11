using System.Collections;
using System.Collections.Generic;

namespace FocusApp
{
    public interface IProvidableInteractiveView<in T> : IInteractiveView where T : IEnumerable
    {
        void Provide(T data);//TODO naming
    }
}