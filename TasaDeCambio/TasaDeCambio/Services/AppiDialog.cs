using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TasaDeCambio.Services
{
    public class AppiDialog
    {

        public async Task ShowMessage(string title, string messsage)
        {
            await Application.Current.MainPage.DisplayAlert(
                title,
                messsage,
                Resources.Resource.Accept);
        }
    }
}
