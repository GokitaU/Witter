using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Witter.Models
{
    public class League
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Prize { get; set; }
        public User Admin { get; set; }
        public ICollection<UserInLeague> Users { get; set; }
    }
}
