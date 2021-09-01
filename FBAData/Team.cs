using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FBAData
{
    public class Team
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public int? ProgramID { get; set; }

        public int? LogoID { get; set; }

        public Program Program { get; set; }

        public Document Logo { get; set; }

        public string Code { get; set; }

        public int PartnerID { get; set; }

        public bool IsMain { get; set; }

        public string HypothesisValidationUrl { get; set; }

        public static List<Team> GetTeams(int programID)
        {
            using (var a = new ProgramContext())
            {
                try
                {

                    var query = a.Team.Include(x => x.Program).Where(x => x.ProgramID == programID).ToList();

                    return query;

                }
                catch (Exception ex)
                {

                    throw;
                }

            }

        }

        public static Team Get(int ID, bool includeProgram)
        {
            using (var a = new ProgramContext())
            {
                try
                {
                    if (ID > 0)
                    {
                        if (includeProgram) { return a.Team.Include(x => x.Program).Where(i => i.ID == ID).ToList().First(); }
                        else
                        {
                            return a.Team.Where(i => i.ID == ID).ToList().First();
                        }

                    }
                    else
                    {
                        return new Team();
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public static Team GetByToken(string groupToken)
        {
            using (var a = new ProgramContext())
            {
                return a.Team.Where(i => i.Code == groupToken).ToList().FirstOrDefault();
            }
        }

        public static int Save(Team team)
        {
            using (var a = new ProgramContext())
            {
                if (team.ID == 0)
                {
                    if (string.IsNullOrEmpty(team.Code)) 
                    {
                        Guid g = Guid.NewGuid();
                        string GuidString = Convert.ToBase64String(g.ToByteArray());
                        GuidString = GuidString.Replace("=", "");
                        GuidString = GuidString.Replace("+", "");
                        team.Code = "T-" + GuidString.Substring(0, 8);

                    }
                   

                    var newgroup = a.Team.Add(team);
                    a.SaveChanges();
                    return newgroup.Entity.ID;
                }
                else
                {
                    var changes = a.Update(team);
                    a.SaveChanges();
                    return changes.Entity.ID;

                }

            }
        }

        public static Team GetMain(int partnerID)
        {
            using (var a = new ProgramContext())
            {
                return a.Team.Where(i => i.PartnerID == partnerID & i.IsMain).ToList().FirstOrDefault();
            }
        }
    }
}
