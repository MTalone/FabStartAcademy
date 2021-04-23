using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;

namespace FBAData
{
    public class LearningUnit
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public bool Active{get;set;}
        public string Teaser { get; set; }
        public string Content { get; set; }
        public int LogoID { get; set; }
        public Document Logo { get; set; }
        public int LearningCategoryID { get; set; }
        public  LearningCategory LearningCategory { get; private set; }

        public static List<LearningUnit> GetList()
        {
            using (var i = new KnowledgeBaseContext())
            {
                try
                {
                    var query = i.LearningUnit.Include(x => x.LearningCategory).Include(y => y.Logo).ToList();

                   

                    return query;

                }
                catch (Exception ex)
                {

                    return null;
                }

            }
        }

        public static LearningUnit Get(int id)
        {
            using (var i = new KnowledgeBaseContext())
            {
                try
                {

                    LearningUnit query = i.LearningUnit.Where(i => i.ID == id).First();

                   

                    return query;

                }
                catch (Exception ex)
                {

                    return null;
                }
            }

        }

        public static LearningCategory GetCategory(int id)
        {
            using (var i = new KnowledgeBaseContext())
            {
                try
                {

                    
                    LearningUnit lu = i.LearningUnit.Where(i => i.ID == id).First();

                    LearningCategory query = i.LearningCategory.Where(i => i.ID == id).First();
                    


                    return query;

                }
                catch (Exception ex)
                {

                    return null;
                }
            }

        }

        public static int Save(LearningUnit lu)
        {
            using (var a = new KnowledgeBaseContext())
            {
                if (lu.ID == 0)
                {
                    var newgroup = a.LearningUnit.Add(lu);

                    a.SaveChanges();
                    return newgroup.Entity.ID;
                }
                else
                {

                    a.LearningUnit.Update(lu);

                    a.SaveChanges();
                    return lu.ID;

                }

            }
        }

        public static void Delete(int id)
        {
            using (var a = new KnowledgeBaseContext())
            {
                var lc = Get(id);

                a.LearningUnit.Remove(lc);
                a.SaveChanges();
            }
        }


    }
}