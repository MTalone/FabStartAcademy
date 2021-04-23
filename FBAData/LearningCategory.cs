using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;

namespace FBAData
{
    public class LearningCategory
    {
        public int ID { get; set; }
        public string Name { get; set; }    

        public static List<LearningCategory> GetList()
        {
            using (var i = new KnowledgeBaseContext() )
            {
                try
                {

                    var query = i.LearningCategory.ToList();



                    return query;

                }
                catch (Exception ex)
                {

                    return null;
                }

            }
        }

        public static LearningCategory Get(int id)
        {
            using (var i = new KnowledgeBaseContext())
            {
                try
                {

                  

                    LearningCategory query = i.LearningCategory.Where(i => i.ID == id).First();



                    return query; 

                }
                catch (Exception ex)
                {

                    return null;
                }
            }

        }

        public static int Save(LearningCategory lc)
        {
            using (var a = new KnowledgeBaseContext())
            {
                if (lc.ID == 0)
                {
                    var newgroup = a.LearningCategory.Add(lc);

                    a.SaveChanges();
                    return newgroup.Entity.ID;
                }
                else
                {

                    a.LearningCategory.Update(lc);

                    a.SaveChanges();
                    return lc.ID;

                }

            }
        }

        public static void Delete(int id)
        {
            using (var a = new KnowledgeBaseContext())
            {
                var lc = Get(id);
                
                a.LearningCategory.Remove(lc);
                a.SaveChanges();
            }
        }

    }
}