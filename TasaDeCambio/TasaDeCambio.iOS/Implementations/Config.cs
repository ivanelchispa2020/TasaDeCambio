﻿using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Text;
using TasaDeCambio.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(TasaDeCambio.iOS.Implementations.Config))]

namespace TasaDeCambio.iOS.Implementations
{
    public class Config : Iconfig
    {
        private string directoryDB;
        private ISQLitePlatform platform;

        public string DirectoryDB
        {
            get
            {
                if (string.IsNullOrEmpty(directoryDB))
                {
                    var directory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    directoryDB = System.IO.Path.Combine(directory, "..", "Library");
                }

                return directoryDB;
            }
        }

        public ISQLitePlatform Platform
        {
            get
            {
                if (platform == null)
                {
                    platform = new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS();
                }

                return platform;
            }
        }
    }


}
