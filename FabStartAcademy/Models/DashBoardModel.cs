using FBAData;
using System.Collections.Generic;
using System.Linq;

namespace FabStartAcademy.Models
{
    public class DashBoardModel
    {
       public List<DashBoardItem> ItemList = new List<DashBoardItem>();
        public List<ProgramItem> groups = new List<ProgramItem>();
        public DashBoardModel()
        {

            
            var groupDB = FBAData.Program.GetPrograms(3);

            groups = groupDB.Select(x => new ProgramItem { Title = x.Name, Process = x.Process is null?"": x.Process.Name,Image= "/imgs/thumb_351_image_big.png" ,ID=x.ID}).ToList();

            DashBoardBO dashBoardBO = new DashBoardBO();


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