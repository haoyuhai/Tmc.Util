using System.Collections;

namespace Tmc.Util.Excel
{
    /// <summary>
    /// Excel导出时的列信息
    /// </summary>
    public class ExportColumn
    {
        public ExportColumn(string property, string title)
        {
            Property = property;
            Title = title;
        }

        public string Property { get; set; }

        public string Title { get; set; }
    }

    /// <summary>
    /// Excel模板中的变量信息
    /// </summary>
    public class Variable
    {
        public Variable(string name, object data)
        {
            Name = name;
            Data = data;
        }

        public string Name { get; set; }

        public object Data { get; set; }
    }

    /// <summary>
    /// Excel模板中的数组变量信息
    /// </summary>
    public class ArrayVariable
    {
        public ArrayVariable(string name, IEnumerable data)
        {
            Name = name;
            Data = data;
        }

        public string Name { get; set; }

        public IEnumerable Data { get; set; }
    }
}
