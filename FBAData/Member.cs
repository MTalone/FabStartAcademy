using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FBAData
{
    public class Member
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsUser { get; set; }

        public string UserID { get; set; }

        public int PartnerID { get; set; }

        public Partner Partner { get; set; }

        public TeamMember TeamMember { get; set; }

        public static List<Member> GetList(int teamID)
        {
            using (var a = new UserContext())
            {
                try
                {

                    var query = a.TeamMember.Include(x => x.Member).Where(x => x.TeamID == teamID).Select(x => x.Member).ToList();



                    return query.ToList();

                }
                catch (Exception ex)
                {

                    return null;
                }

            }
        }

        public static Member Get(int ID)
        {
            using (var a = new UserContext())
            {
                return a.Member.Find(ID);
            }
        }

        public static int Save(Member member)
        {

            using (var a = new UserContext())
            {
                if (member.ID == 0)
                {


                    var newgroup = a.Member.Add(member);

                    a.SaveChanges();
                    return newgroup.Entity.ID;
                }
                else
                {
                    var changes = a.Update(member);
                    a.SaveChanges();
                    return changes.Entity.ID;

                }

            }
        }

        public static Member GetByEmail(string email)
        {
            using (var a = new UserContext())
            {
                return a.Member.Include(x=>x.Partner).Where(x=>x.Email==email).FirstOrDefault();
            }
        }
        
        public static List<TeamMember> GetTeams(int MemberID)
        {
            using (var a = new UserContext())
            {
                return a.TeamMember.Where(x => x.MemberID == MemberID).ToList();
            }
        }
    }
}
