using DAL.Abstract;
using System;
using System.IO;
using System.Runtime.Serialization.Json;

namespace DAL
{
    public class JSONDataProvider : IDataProvider
    {
        public object Read(string path, Type type, Type[] extratype)
        {
            DataContractJsonSerializer formatter = new DataContractJsonSerializer(type, extratype);
            using (FileStream file = new FileStream(path, FileMode.OpenOrCreate))
            {
                return formatter.ReadObject(file);
            }
        }

        public void Write(string path, object obj, Type type, Type[] extratype)
        {
            DataContractJsonSerializer formatter = new DataContractJsonSerializer(type, extratype);
            using (FileStream file = new FileStream(path, FileMode.OpenOrCreate))
            {
                formatter.WriteObject(file, obj);
            }
        }

        public void Delete(string path)
        {
            File.Delete(path);
        }
    }
}
