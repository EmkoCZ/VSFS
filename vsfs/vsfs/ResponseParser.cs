using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using vsfs.Models;
using System.Globalization;

namespace vsfs
{
    public static class ResponseParser
    {
        public static List<string> response { get; set; }
        public static List<TimetableModel> timetableModels { get; set; }

        public static async Task parseDataIntoListAsync(HttpWebResponse webResponse)
        {
            StreamReader sr = new StreamReader(webResponse.GetResponseStream());

            List<string> lines = new List<string>();

            while (!sr.EndOfStream)
            {
                string ln = await sr.ReadLineAsync();
                lines.Add(ln);
            }

            sr.Close();
            response = lines;
        }

        public static async Task<List<string>> GetSubjectNamesAsync()
        {
            List<string> names = new List<string>();

            foreach (var line in response)
            {
                if (line.Contains("Název předmětu:"))
                {
                    string code = line.Split('>')[1].Split('<')[0];
                    names.Add(code);
                }
            }
            return names;
        }

        public static async Task<List<string>> GetSubjectTeachersAsync()
        {
            List<string> teachers = new List<string>();

            foreach (var line in response)
            {
                if (line.Contains("Vyučující"))
                {
                    string code = line.Split('>')[2].Split('<')[0];
                    teachers.Add(code);
                }
            }
            return teachers;
        }

        public static async Task<List<string>> GetSubjectRoomsAsync()
        {
            List<string> rooms = new List<string>();

            foreach (var line in response)
            {
                if (line.Contains("Místnost:"))
                {
                    string code = line.Split('>')[2].Split('<')[0];
                    rooms.Add(code);
                }
            }
            return rooms;
        }

        public static async Task<List<string>> GetSubjectCodesAsync()
        {
            List<string> codes = new List<string>();

            foreach (var line in response)
            {
                if (line.Contains("Kód předmětu"))
                {
                    string code = line.Split('>')[2].Split('<')[0];
                    codes.Add(code);
                }
            }
            return codes;
        }

        public static async Task<List<string>> GetSubjectTimesAsync()
        {
            List<string> times = new List<string>();

            foreach (var line in response)
            {
                if (line.Contains("cas_od_do hide-for-large"))
                {
                    string start = line.Split('>')[1].Split('<')[0].Split(';')[0];
                    start = start.Replace("&ndash", "");
                    string end = line.Split('>')[1].Split('<')[0].Split(';')[1];
                    end = end.Replace("&ndash", "");
                    times.Add(start + "-" + end);
                }
            }
            return times;
        }

        public static async Task<int> GetSubjectColumAsync(string time)
        {
            switch (time)
            {
                case "08:45-09:29":
                    return 1;
                case "8:45-9:29":
                    return 1;
                case "9:30-10:15":
                    return 2;
                case "09:30-10:15":
                    return 2;
                case "10:30-11:14":
                    return 3;
                case "11:15-12:00":
                    return 4;
                case "12:15-12:59":
                    return 5;
                case "13:00-13:45":
                    return 6;
                case "14:00-14:44":
                    return 7;
                case "14:45-15:30":
                    return 8;
                case "15:45-16:29":
                    return 9;
                case "16:30-17:15":
                    return 10;
                case "17:30-18:14":
                    return 11;
                case "18:15-19:00":
                    return 12;
                default:
                    return 0;
            }
        }

        public static async Task<List<int>> GetSubjectDaysAsync()
        {
            List<int> days = new List<int>();
            int day = 0;

            foreach (var line in response)
            {
                if (line.Contains("Po</TH>")) day = 0;
                if (line.Contains("Út</TH>")) day = 1;
                if (line.Contains("St</TH>")) day = 2;
                if (line.Contains("seminář"))
                {
                    days.Add(day);
                }
            }
            return days;
        }

        public static async Task PopulateTimetable()
        {
            try
            {
                if (timetableModels == null)
                    timetableModels = new List<TimetableModel>();

                List<string> codes = await GetSubjectCodesAsync();
                List<string> names = await GetSubjectNamesAsync();
                List<string> teachers = await GetSubjectTeachersAsync();
                List<string> rooms = await GetSubjectRoomsAsync();
                List<string> times = await GetSubjectTimesAsync();
                List<int> days = await GetSubjectDaysAsync();
                

                CultureInfo cultureInfo = new CultureInfo("cs-CZ");
                Calendar calendar = cultureInfo.Calendar;

                if(calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday) % 2 == 0)
                {
                    codes.RemoveRange(8, 2);
                    codes.RemoveRange(8, 10);
                    names.RemoveRange(8, 2);
                    names.RemoveRange(8, 10);
                    teachers.RemoveRange(8, 2);
                    teachers.RemoveRange(8, 10);
                    rooms.RemoveRange(8, 2);
                    rooms.RemoveRange(8, 10);
                    times.RemoveRange(8, 2);
                    times.RemoveRange(8, 10);
                    days.RemoveRange(8, 2);
                    days.RemoveRange(8, 10);
                } else
                {
                    codes.RemoveRange(4, 4);
                    codes.RemoveRange(6, 10);
                    names.RemoveRange(4, 4);
                    names.RemoveRange(6, 10);
                    teachers.RemoveRange(4, 4);
                    teachers.RemoveRange(6, 10);
                    rooms.RemoveRange(4, 4);
                    rooms.RemoveRange(6, 10);
                    times.RemoveRange(4, 4);
                    times.RemoveRange(6, 10);
                    days.RemoveRange(4, 4);
                    days.RemoveRange(6, 10);
                }

                for (int i = 0; i < codes.Count-1; i++)
                {
                    int col = await GetSubjectColumAsync(times[i]);
                    string type = codes[i];
                    string name = names[i];
                    string teacher = teachers[i];
                    string room = rooms[i];
                    string time = times[i];
                    int row = days[i];

                    timetableModels.Add(new TimetableModel { type = type, name = name, teacher = teacher, room = room, time = time, colum = col, row = row });
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("ERROR " + err.Message);
                Console.WriteLine("ERR STACK " + err.StackTrace);
            }
            
        }
    }
}
