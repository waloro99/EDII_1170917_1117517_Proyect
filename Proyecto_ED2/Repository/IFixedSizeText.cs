using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_ED2.Repository
{
    public interface IFixedSizeText
    {
        int FixedSizeText { get; set; }
        string ToFixedSizeString();
        string ToNullFormat();
    }
}
