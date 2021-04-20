using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FBAData
{
    public class DocumentContext:FBADataContext
    {
        public DbSet<Document> Document { get; set; }


    }

    public class Document
    {
        public int ID { get; set; }
        public string FileName { get; set; }

        public string FileType { get; set; }

        public byte[] Content { get; set; }

        public string Path { get; set; }


        public static int Upload(Document document) 
        {
            using (var a = new DocumentContext())
            {
                if (document.ID == 0)
                {
                    var newgroup = a.Document.Add(document);

                    a.SaveChanges();
                    return newgroup.Entity.ID;
                }
                else
                {

                    a.Document.Update(document);

                    a.SaveChanges();
                    return document.ID;

                }
            }

        }

        public static Document Download(int documentID)
        {
            using (var a = new DocumentContext())
            {
                return a.Document.Where(x => x.ID == documentID).First();
            }
        }

    }

}
