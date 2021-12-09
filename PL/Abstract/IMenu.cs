using System;

namespace PL.Abstract
{
    public interface IMenu<T>
    {
        void RenderOptions();

        T ReadOption();
    }
}
