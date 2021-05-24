using System;
using Xamarin.Forms;

namespace XamarinApp.Models
{
    public class ComputerWithImage
    {
        public Computer Computer { get; set; }
        public ImageSource ComputerImage { get; set; }

        public ComputerWithImage()
        {
            var uri = Computer.Images.Count > 0 ? new Uri(Computer.Images[0]) : null;
            const string emptyImage = "https://bh.by/upload/iblock/112/1127994722e9b9e78c1f69861320a45d.jpg";
            ComputerImage = uri is null ? ImageSource.FromUri(new Uri(emptyImage)) : ImageSource.FromUri(uri);
        }
    }
}