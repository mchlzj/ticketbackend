using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ticketsystem_backend.Models
{
    public class Module
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public User Responsible { get; set; }
    }

    // Module-Model for API
    public class CreateModuleVM
    {
        public string Name { get; set; }
        public int ResponsibleUserId { get; set; }
    }
}
