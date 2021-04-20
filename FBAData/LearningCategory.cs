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
            using (var a = new KnowledgeBaseContext() )
            {
                try
                {

                    var query = a.LearningCategory.Where(i => i.SessionID == sessionID).OrderBy(i => i.Order).ToList();



                    return query.ToList();

                }
                catch (Exception ex)
                {

                    return null;
                }

            }
        }

    }
}