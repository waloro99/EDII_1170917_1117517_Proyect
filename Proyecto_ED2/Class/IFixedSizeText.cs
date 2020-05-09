using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_ED2.Class
{

    public interface IFixedSizeText
    {
        int FixedSizeText { get; set; }
        string ToFixedSizeString();
        string ToNullFormat();
    }
}
