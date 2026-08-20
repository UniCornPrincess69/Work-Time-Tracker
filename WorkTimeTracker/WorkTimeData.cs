using System;
using System.Collections.Generic;
using System.Text;

namespace WorkTimeTracker
{
    public enum WorkStatus
    {
        NotWorking,
        Working
    }
  
    public class WorkTimeData()
    {
        public DateTime StartTime { get; set ; }
        public DateTime? EndTime {  get; set; }
        public WorkStatus Status { get; set; }
        public TimeSpan? TimeSpan => EndTime.HasValue ? EndTime.Value - StartTime : null;

               
    }
}
