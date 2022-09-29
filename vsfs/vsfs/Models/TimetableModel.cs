using System;
using System.Collections.Generic;
using System.Text;

namespace vsfs.Models
{
    public class TimetableModel
    {
        public string type { get; set; }
        public string name { get; set; }
        public string teacher { get; set; }
        public string room { get; set; }
        public string time { get; set; }
        public int colum { get; set; }
        public int row { get; set; }
    }
}
