using System;

namespace Shoon
{
    public class TableNameCalculator
    {
        public string GetTheTableName(Type type)
        {
            var name = type.Name;
            if (name.EndsWith("Denormalizer"))
                name = name.Substring(0, type.Name.Length - "Denormalizer".Length);
            return name;
        }
    }
}