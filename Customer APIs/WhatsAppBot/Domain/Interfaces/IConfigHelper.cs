using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IConfigHelper
    {
        public string CnnVal();
        public string CnnVal(string name);
        public T GetVal<T>(string name);
    }
}
