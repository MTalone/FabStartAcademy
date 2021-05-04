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

        public bool IsUser { get; set; }

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

        public static Member GetByEmain(string email)
        {
            using (var a = new UserContext())
            {
                return a.Member.Where(x=>x.Email==email).FirstOrDefault();
            }
        }
    }
}
