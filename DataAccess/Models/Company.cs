using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToysAndGames.Models
{
    public class Company
    {
        //TODO: the naming convension of an ID should be only that, not ObjecID, this second one is used for a FK Backfill property
        public int Id { get; set; }
      
        public string Name { get; set; }
    }
}
