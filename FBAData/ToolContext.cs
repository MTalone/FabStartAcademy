using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FBAData
{
    public class ToolContext:FBADataContext
    {
        public DbSet<Tool> Tool { get; set; }

    }

    public class Tool {
        
        public int ID { get; set; }
        public string Name { get; set; }
        
        public string URL { get; set; }

        public static List<Tool> GetTools()
        {
            using (var a = new ToolContext())
            {
                try
                {

                    return a.Tool.ToList(); ;

                }
                catch (Exception ex)
                {

                    return null;
                }

            }
        }

        public enum Tools
        {
            HypothesisValidation = 1,
            BusinessCanvas=2,
            PitchDeck = 3
        }

    }
}
