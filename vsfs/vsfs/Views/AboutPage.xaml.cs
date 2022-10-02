using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text;
using System.Net;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace vsfs.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();

            if(Application.Current.Properties.ContainsKey("name"))
            {
                name.Text = Application.Current.Properties["name"].ToString();
                pass.Text = Application.Current.Properties["pass"].ToString();
                save.IsChecked = Boolean.Parse(Application.Current.Properties["sign"].ToString());

                loginAuto();
            }
        }

        private async Task loginAuto()
        {
            loading.IsVisible = true;
            loading.IsRunning = true;
            try
            {
                // Ignore certificate issues
                System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate (object sender2,
                    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };

                string username = name.Text;
                string password = pass.Text;
                bool stay = save.IsChecked;

                string encode = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://is.vsfs.cz/auth/rozvrh/zobraz/muj");
                request.PreAuthenticate = true;
                Console.WriteLine(encode);
                request.Headers.Set("Authorization", "Basic " + encode);


                var response = await request.GetResponseAsync();
                await ResponseParser.parseDataIntoListAsync((HttpWebResponse)response);


                loading.IsVisible = false;
                loading.IsRunning = false;

                if (save.IsChecked)
                {
                    Application.Current.Properties["name"] = name.Text;
                    Application.Current.Properties["pass"] = pass.Text;
                    Application.Current.Properties["sign"] = stay;
                    await Application.Current.SavePropertiesAsync();
                }
                else
                {
                    Application.Current.Properties.Remove("name");
                    Application.Current.Properties.Remove("pass");
                    Application.Current.Properties.Remove("sign");
                }

                await Shell.Current.GoToAsync($"//{nameof(Timetable)}");
            }
            catch (Exception err)
            {
                await DisplayAlert("Error", err.Message, "Ok :(");
                Console.WriteLine("ERROR \n" + err.Message);
                Console.WriteLine("Stack Trace \n" + err.StackTrace);
            }
        }

        private async Task login(object sender, EventArgs e)
        {
            loading.IsVisible = true;
            loading.IsRunning = true;
            try
            {
                // Ignore certificate issues
                System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate (object sender2,
                    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };

                Button btn = sender as Button;
                string username = name.Text;
                string password = pass.Text;
                bool stay = save.IsChecked;

                string encode = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://is.vsfs.cz/auth/rozvrh/zobraz/muj");
                request.PreAuthenticate = true;
                Console.WriteLine(encode);
                request.Headers.Set("Authorization", "Basic " + encode);


                var response = await request.GetResponseAsync();
                await ResponseParser.parseDataIntoListAsync((HttpWebResponse)response);
                

                loading.IsVisible = false;
                loading.IsRunning = false;
                //await Shell.Current.GoToAsync("Timetable");

                if (save.IsChecked)
                {
                    Application.Current.Properties["name"] = name.Text;
                    Application.Current.Properties["pass"] = pass.Text;
                    Application.Current.Properties["sign"] = stay;
                    await Application.Current.SavePropertiesAsync();
                } else
                {
                    Application.Current.Properties.Remove("name");
                    Application.Current.Properties.Remove("pass");
                    Application.Current.Properties.Remove("sign");
                }

                //if (debug.IsChecked)
                //{
                //    await Shell.Current.GoToAsync("DebugOutput");
                //}
                //else 
                await Shell.Current.GoToAsync("Timetable");
            }
            catch (Exception err)
            {
                await DisplayAlert("Error", err.Message, "Ok :(");
                Console.WriteLine("ERROR \n" + err.Message);
                Console.WriteLine("Stack Trace \n" + err.StackTrace);
            }
            
        }

        private async void loginBtn(object sender, EventArgs e)
        {
            await login(sender, e);
        }
    }
}