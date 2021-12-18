using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataSource.Converter
{
    public class ListStringToStringConverter : ValueConverter<IEnumerable<string>, string>
    {
        public ListStringToStringConverter() : base(le => ListToString(le), s=> StringToList(s))
        {

        }

        public static IEnumerable<string> StringToList(string s)
        {
            return s != null ? s.Split(',') : new List<string>();
        }

        public static string ListToString(IEnumerable<string> value)
        {
            if (value == null || value.Count() == 0)
            {
                return null;
            }
            return string.Join(',', value);
        }
    }
}
