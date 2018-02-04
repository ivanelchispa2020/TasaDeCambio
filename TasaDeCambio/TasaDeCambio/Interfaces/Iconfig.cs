using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace TasaDeCambio.Interfaces
{
    public interface Iconfig
    {

         string DirectoryDB { get; }

        ISQLitePlatform Platform { get; }

    }
}
