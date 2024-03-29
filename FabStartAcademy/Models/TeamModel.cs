﻿using FBAData;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FabStartAcademy.Models
{
    public class TeamItem : Item
    {


        

        public bool IsReadOnly { get; set; }

        public string ProgramTitle { get; set; }

        public int ProgramID { get; set; }

        public string Image { get; set; }

        public int? LogoID { get; set; }
        public string Code { get; set; }

        public string MethodName { get; set; }
        public int Rate { get; set; }
        public int Progress { get; set; }

        //HypothesisValidationUrl
        [Display(Name = "HypothesisValidationUrl", ResourceType = typeof(Resources.FabStartAcademy))]
        public string HypothesisValidationUrl { get; set; }

    }
    public class TeamModel
    {
        public List<TeamItem> Teams { get; set; }

        public string ProgramTitle { get; set; }

        public int ProgramID { get; set; }

        public int TeamID { get; set; }

        public List<MemberItem> Members{get;set;}

        public MemberItem Member { get; set; }

        public List<SelectListItem> Roles { get; set; }

        public Team Team { get; set; }

        public TeamModel() { }

        public TeamModel(int programID,string WebRootPath, string defaultPath)
        {
            ProgramTitle = FBAData.Program.GetProgram(programID).Name;
            Teams = Team.GetTeams(programID).Select(x => new TeamItem
            {
                Title = x.Name,
                ID = x.ID,
                ProgramID =x.ProgramID??0,
                Description = x.Description,
                ProgramTitle = ProgramTitle,
                Image = GetImageFromDocument(x.Logo, WebRootPath, defaultPath)
            }).ToList();
            ProgramID = programID;
        }

        public static string GetImageFromDocument(Document document,string WebRootPath,string defaultPath) 
        {
            string Image = string.Empty;
            if (!(document is null))
            {
                Image = document.Path;
                if (!(Image is null))
                {
                    Image = Image.Replace(WebRootPath, "");
                }
            }
            if (Image is null | Image == string.Empty)
            {
                Image = defaultPath;
            }

            return Image;
        }
    }
}
