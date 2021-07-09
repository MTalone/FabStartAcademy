using FBAData;
using System.Collections.Generic;
using System.Linq;

namespace FabStartAcademy.Models
{
    public class DashBoardModel:IAccount
    {
       public List<DashBoardItem> ItemList = new List<DashBoardItem>();
        public List<ProgramItem> Programs = new List<ProgramItem>();
        public Account.Account CurrentAccount { get; set; }
        public DashBoardModel(string WebRootPath, Microsoft.AspNetCore.Http.ISession session,string username)
        {
            string programDefaultPath = "/imgs/placeholder-group.png";
            CurrentAccount = Account.Account.GetAccountSession(session, username);

            var groupDB = FBAData.Program.GetPrograms(3,CurrentAccount.IsSuperAdmin,CurrentAccount.PartnerID);

            Programs = new List<ProgramItem>();

            foreach (var x in groupDB)
            {
                ProgramItem pi = new ProgramItem
                {

                    Title = x.Name,

                    Process = x.Process != null ? x.Process.Name : string.Empty,
                    ID = x.ID
                };
                if (!(x.Logo is null))
                {
                    pi.Image = x.Logo.Path;
                    if (!(pi.Image is null))
                    {
                        pi.Image = pi.Image.Replace(WebRootPath, "");
                    }
                }
                if (pi.Image is null | pi.Image == string.Empty)
                {
                    pi.Image = programDefaultPath;
                }
                Programs.Add(pi);
            }

            DashBoardBO dashBoardBO = new DashBoardBO(CurrentAccount.IsSuperAdmin,CurrentAccount.PartnerID);


            ItemList.Add(new DashBoardItem { Title = Resources.FabStartAcademy.Programs, Class = "bi-people-fill", Count = dashBoardBO.ProgramsCount });
            ItemList.Add(new DashBoardItem { Title = Resources.FabStartAcademy.Methods, Class = "bi-diagram-3-fill", Count = dashBoardBO.ProcessesCount });
            ItemList.Add(new DashBoardItem { Title = Resources.FabStartAcademy.Mentors, Class = "bi-person-square", Count = dashBoardBO.MentorsCount }); //MentorsCount not Implemented
            ItemList.Add(new DashBoardItem { Title = Resources.FabStartAcademy.Members, Class = "bi-people", Count = dashBoardBO.MembersCount }); //MembersCount not Implemented

        }
    }
    public class DashBoardItem
    {
        public string Title { get; set; }
        public int Count { get; set; }

        public string Class { get; set; }
    }
}