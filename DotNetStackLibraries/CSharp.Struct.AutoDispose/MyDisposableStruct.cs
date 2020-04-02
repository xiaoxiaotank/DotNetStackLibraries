using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp.Struct.AutoDispose
{
    ref struct MyDisposableStruct
    {
        public void Dispose() => throw new NotImplementedException();
    }
}
