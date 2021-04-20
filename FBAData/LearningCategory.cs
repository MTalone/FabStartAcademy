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

        public static List<LearningCategory> GetLearningCategories()
        {
            using (var i = new KnowledgeBaseContext() )
            {
                try
                {

                    //var query = i.LearningCategory.Where(i => i.SessionID == sessionID).OrderBy(i => i.Order).ToList();



                    return null; //query.ToList();

                }
                catch (Exception ex)
                {

                    return null;
                }

            }
        }

    }
}