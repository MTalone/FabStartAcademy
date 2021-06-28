using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FBAData
{
    public class Role
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public static List<Role> GetList() {
            using (var a = new UserContext())
            {
                try
                {

                    var query = a.Role.ToList();



                    return query.ToList();

                }
                catch (Exception ex)
                {

                    return null;
                }

            }

        }

        public static Role Get(int ID)
        {
            using (var a = new UserContext())
            {
                return a.Role.Find(ID);
            }
        }

        public enum Roles
        {
            Admin = 1,
            Mentor = 2,
            User = 3,
            SuperAdmin=4

        }

       // public static int Save(Role role) { return 0; }
    }
}
