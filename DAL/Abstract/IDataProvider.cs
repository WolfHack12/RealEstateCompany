using System;

namespace DAL.Abstract
{
    public interface IDataProvider
    {
        object Read(string path, Type type, Type[] extratype);

        void Write(string path, object data, Type type, Type[] extratype);

        void Delete(string path);
    }
}
