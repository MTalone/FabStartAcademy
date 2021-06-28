using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FBAData
{
    public class Partner
    {
        public int ID { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }
        public bool IsMain { get; set; }

        public bool IsSuspended { get; set; }
        public bool IsDeleted { get; set; }

        public static int Save(Partner partner)
        {
            try
            {
                using (var a = new UserContext())
                {
                    if (partner.ID == 0)
                    {
                        Guid g = Guid.NewGuid();
                        string GuidString = Convert.ToBase64String(g.ToByteArray());
                        GuidString = GuidString.Replace("=", "");
                        GuidString = GuidString.Replace("+", "");
                        partner.Code = "P-" + GuidString.Substring(0, 8);

                        var newgroup = a.Partner.Add(partner);

                        a.SaveChanges();
                        return newgroup.Entity.ID;
                    }
                    else
                    {
                        var changes = a.Update(partner);
                        a.SaveChanges();
                        return changes.Entity.ID;

                    }

                }
            }
            catch (Exception ex)
            {

                throw;
            }
           

        }

        public static Partner Get(int ID) 
        {
            using (var a = new UserContext())
            {
                return a.Partner.Find(ID);
            }
        }

        public static List<Partner> List()
        {
            using (var a = new UserContext())
            {
                return a.Partner.ToList();
            }
        }

        public static bool CheckMainCreated() 
        {
            using (var a = new UserContext())
            {
                return a.Partner.Any(x=>x.IsMain);
            }
        }
    }
}
