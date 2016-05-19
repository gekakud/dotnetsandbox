using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignatureProvider
{
    interface IDigitalSignatureBase
    {
        bool SignDocument(string p_docPath, string p_algType);

    }
}
