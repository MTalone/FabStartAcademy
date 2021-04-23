﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBAData
{
    public class TeamMember
    {
        /*	[Id] INT NOT NULL PRIMARY KEY,
	    [TeamID] INT NOT NULL, 
        [MemberID] INT NOT NULL,
	    [RoleID] INT NOT NULL,
        [IsConfirmed] BIT NOT NULL,*/
        public int ID { get; set; }
        public int TeamID { get; set; }
        public int MemberID { get; set; }
        public bool IsConfirmed { get; set; }

        public Member Member { get; set; }

        //public static List<TeamMember> GetList() {
        //    using (var a = new UserContext())
        //    {
        //        try
        //        {

        //            var query = a.TeamMember.Include(x => x.Member).Where(x => x.TeamID == teamID).Select(x => x.Member).ToList();



        //            return query.ToList();

        //        }
        //        catch (Exception ex)
        //        {

        //            return null;
        //        }

        //    }
        //}

        public static TeamMember Get(int ID)
        {
            using (var a = new UserContext())
            {
                return a.TeamMember.Find(ID);
            }
        }

        public static int Save(TeamMember tm)
        {
            using (var a = new UserContext())
            {
                if (tm.ID == 0)
                {


                    var newgroup = a.TeamMember.Add(tm);

                    a.SaveChanges();
                    return newgroup.Entity.ID;
                }
                else
                {
                    var changes = a.Update(tm);
                    a.SaveChanges();
                    return changes.Entity.ID;

                }

            }

        }

    }
}
