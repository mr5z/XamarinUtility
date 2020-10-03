using System;
using Xamarin.Forms;

namespace XamarinUtility.Test
{
    class Program
    {
        static BindableProperty SubmitCommandProperty = BindableHelper.CreateProperty<string>();

        static void Main()
        {
            var obj = new
            {
                a = "apple",
                b = "ball",
                c = "cat"
            };
            var result = QueryStringHelper.ToQueryString(obj);
            var d = result;
        }
    }
}
