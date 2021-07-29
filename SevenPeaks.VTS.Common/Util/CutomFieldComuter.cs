using System;
using SevenPeaks.VTS.Common.Models;

namespace SevenPeaks.VTS.Common.Util
{
    public class CutomFieldComuter
    {
        public CutomFieldComuter()
        {
        }
         
        static Type GetCustomType(CustomField customField)
        {
            switch (customField.Type)
            {
                case "String":
                case "string":
                    return typeof(string);
                case "Int":
                case "int":
                    return typeof(int);
                case "Decimal":
                case "decimal":
                    return typeof(decimal);
                case "bool":
                case "Boolean":
                    return typeof(bool);
                case "float":
                case "Float":
                    return typeof(float);
                case "chat":
                case "Char":
                    return typeof(char);

                default:return typeof(string);
            }
        }
    }
}
