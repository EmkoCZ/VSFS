using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using vsfs.Models;
using vsfs.Views;
using vsfs.ViewModels;
using System.Globalization;
using System.Threading;

namespace vsfs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Timetable : ContentPage
    {
        List<StackLayout> subjects = new List<StackLayout>();

        public Timetable()
        {
            InitializeComponent();
            setUp();
        }

        async void setUp()
        {
            try
            {
                loading.IsVisible = true;
                loading.IsRunning = true;
                await createGrid();

                if(await ResponseParser.GetWeekType() == 2)
                {
                    tyden.Text = tyden.Text.Split(' ')[0] + " Sudý";
                } else
                {
                    tyden.Text = tyden.Text.Split(' ')[0] + " Lichý";
                }

                den.Text = den.Text.Split(' ')[0] + " " + await getDayName();
                datum.Text = datum.Text.Split(' ')[0] + " " + DateTime.Now.ToString("dd.MM.");

                loading.IsVisible = false;
                loading.IsRunning = false;
            }
            catch (Exception err)
            {
                Console.WriteLine("Error " + err.Message);
                Console.WriteLine("Error Stack " + err.StackTrace);
            }
        }

        public async Task createGrid()
        {
            var grid = new Grid
            {
                ColumnSpacing = 0,
                Margin = new Thickness(0, 50, 0, 0),
                BackgroundColor = Color.FromHex("#03c6fc"),
                RowSpacing = 0
            };

            // Set width and height of rows
            grid.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(110, GridUnitType.Absolute)
            });

            grid.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(110, GridUnitType.Absolute)
            });

            grid.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(110, GridUnitType.Absolute)
            });

            grid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(35, GridUnitType.Absolute)
            });



            // Grid days
            List<DaysModel> days = new List<DaysModel>
            {
                new DaysModel {dayName = "Po"},
                new DaysModel {dayName = "Út"},
                new DaysModel {dayName = "St"}
            };

            for (int i = 0; i < days.Count; i++)
            {
                grid.Children.Add(new Label { Text = days[i].dayName, FontSize = 20, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, TextColor = Color.White }, 0, i);
            }

            for (int i = 0; i < 12; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(120, GridUnitType.Absolute)
                });
            }

            scView.Content = grid;
            await setInfo(grid);
        }

        public async Task setInfo(Grid grid)
        {
            try
            {
                await ResponseParser.PopulateTimetable();

                for (int i = 0; i < ResponseParser.timetableModels.Count; i++)
                {
                    TimetableModel model = ResponseParser.timetableModels[i];

                    var stackLayout = new StackLayout
                    {
                        BackgroundColor = await getSubjectColor(model),
                        Padding = new Thickness(5, 0, 0, 0),
                        Spacing = 0
                    };

                    subjects.Add(stackLayout);

                    var subType = new Label
                    {
                        Text = model.type,
                        FontSize = 12
                    };

                    stackLayout.Children.Add(subType);

                    var subName = new Label
                    {
                        Text = model.name,
                        FontAttributes = FontAttributes.Bold
                    };

                    stackLayout.Children.Add(subName);

                    var subTeacher = new Label
                    {
                        Text = model.teacher,
                        FontSize = 12
                    };

                    stackLayout.Children.Add(subTeacher);

                    var subRoom = new Label
                    {
                        Text = model.room,
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 13
                    };

                    stackLayout.Children.Add(subRoom);

                    var subTime = new Label
                    {
                        Text = model.time,
                        FontSize = 13,
                        HorizontalOptions = LayoutOptions.Center
                    };

                    stackLayout.Children.Add(subTime);

                    grid.Children.Add(stackLayout, model.colum, model.row);
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.StackTrace);
                Console.WriteLine(err.Message);
            }

        }

        public async Task<Color> getSubjectColor(TimetableModel model)
        {
            DateTime startTime = DateTime.ParseExact(model.time.Split('-')[0], "HH:mm",
                                        CultureInfo.InvariantCulture);
            DateTime endTime = DateTime.ParseExact(model.time.Split('-')[1], "HH:mm",
                                        CultureInfo.InvariantCulture);

            //Console.WriteLine("Day: " + await getDay());

            if ((DateTime.Now.TimeOfDay >= startTime.TimeOfDay) && (DateTime.Now.TimeOfDay <= endTime.TimeOfDay) && (model.row == await getDay()))
            {
                return Color.FromHex("#ffae57");
            }
            else return Color.FromHex("#f7f7f7");
        }

        public async Task<int> getDay()
        {
            string day = DateTime.Now.DayOfWeek.ToString();

            switch (day)
            {
                case "Monday":
                    return 0;
                case "Tuesday":
                    return 1;
                case "Wednesday":
                    return 2;
                default:
                    return 3;
            }
        }

        public async Task<string> getDayName()
        {
            string day = DateTime.Now.DayOfWeek.ToString();

            switch (day)
            {
                case "Monday":
                    return "Pondělí";
                case "Tuesday":
                    return "Úterý";
                case "Wednesday":
                    return "Středa";
                default:
                    return "Mimo výuku";
            }
        }

        private async void refresh(object sender, EventArgs e)
        {
            for (int i = 0; i < subjects.Count; i++)
            {
                TimetableModel model = ResponseParser.timetableModels[i];
                subjects[i].BackgroundColor = await getSubjectColor(model);
            }
            //await DisplayAlert("Timetable Refresh", "Refreshed the timetable", "Great");
            //Console.WriteLine("Refresh");
        }
    }
}