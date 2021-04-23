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
        public int CategoryID { get; set; }
        public int LogoID { get; set; }
        public Document Document { get; set; }
        public LearningCategory LearningCategory { get; set; }

        public static List<LearningUnit> GetList()
        {
            using (var i = new KnowledgeBaseContext())
            {
                try
                {
                    var query = i.LearningUnit.ToList();

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

        public static int Save(LearningUnit lc)
        {
            using (var a = new KnowledgeBaseContext())
            {
                if (lc.ID == 0)
                {
                    var newgroup = a.LearningUnit.Add(lc);

                    a.SaveChanges();
                    return newgroup.Entity.ID;
                }
                else
                {

                    a.LearningUnit.Update(lc);

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

                a.LearningUnit.Remove(lc);
                a.SaveChanges();
            }
        }


    }
}