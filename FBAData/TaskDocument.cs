using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FBAData
{
    public class TaskDocument
    {
        public int ID { get; set; }
        public int TaskID { get; set; }

        public int DocumentID { get; set; }

        public Document Document{ get; set; }

        public static int Save(TaskDocument taskDocument)
        {
            using (var a = new TaskContext())
            {
                
                    
                    var newgroup = a.TaskDocument.Add(taskDocument);

                    a.SaveChanges();
                    return newgroup.Entity.ID;
               

            }
        }

        public static bool Delete(int taskDocumentID)
        {
            try
            {
                using (var a = new TaskContext())
                {
                    var td = a.TaskDocument.Where(x => x.ID == taskDocumentID).First();
                    var doc = a.Document.Where(x => x.ID == td.DocumentID).First();
                    a.TaskDocument.Remove(td);
                    a.Document.Remove(doc);
                    a.SaveChanges();
                    return true;

                }

            }
            catch (Exception)
            {

                return false;
            }
            
        }
    }
}
