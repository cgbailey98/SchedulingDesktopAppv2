using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingDesktopAppv2.Classes
{
    public class AppointmentModel
    {
        public int AppointmentId { get; set; }
        public string CustomerName { get; set; }
        public DateTime Start { get; set; }
        public int UserId { get; set; }
    }
}
