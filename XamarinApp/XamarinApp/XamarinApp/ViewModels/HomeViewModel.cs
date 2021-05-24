using System;
using System.Collections.Generic;
using System.Text;
using XamarinApp.Models;

namespace XamarinApp.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public List<ComputerWithImage> Computers { get; set; }
    }
}
