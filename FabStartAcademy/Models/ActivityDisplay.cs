using FBAData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FabStartAcademy.Models
{
    public class ActivityDisplay
    {
        public string Image { get; set; }
       
        public DateTime Date { get; set; }

        public string TaskTitle { get; set; }

        public string SessionName { get; set; }
        public string TeamName { get; set; }

        public string MemberName { get; set; }

        public int? TaskID { get; set; }
        public int TeamID { get; set; }
        public string Message { get {
                switch (Type)
                {
                    case ActivityType.Types.Document:
                        return string.Format(Resources.FabStartAcademy.ActivityDocument, MemberName, TaskTitle, SessionName);
                        
                    case ActivityType.Types.Submission:
                        return string.Format(Resources.FabStartAcademy.ActivitySubmitted, MemberName, TaskTitle, SessionName);
                    case ActivityType.Types.Message:
                        break;
                    case ActivityType.Types.Comment:
                        return string.Format(Resources.FabStartAcademy.ActivityComment, MemberName, TaskTitle, SessionName);
                    case ActivityType.Types.Evaluation:
                        return string.Format(Resources.FabStartAcademy.ActivityEvaluation, MemberName, TaskTitle, SessionName);
                    default:
                        break;
                }

                return string.Empty; } }

        public ActivityType.Types Type { get; set; }

        public string DisplayDate { get {
                if (Date.Date == DateTime.Today) 
                {
                    return Date.ToString("HH:mm");
                }
                else
                {
                    return Date.ToString("yyyy/MM/dd");

                }
                
                } }
    }
}
