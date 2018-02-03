using System;
using System.Collections.Generic;
using System.Text;
using TasaDeCambio.ViewModels;

namespace TasaDeCambio.Infrastructure
{
    public class InstanceLocator
    {
        public MainViewModel Main { get; set; }

        public InstanceLocator()
        {
            Main = new MainViewModel();
        }

    }
}
