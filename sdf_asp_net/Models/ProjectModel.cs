using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace sdf_asp_net.Models
{
    public class ProjectModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayName("Beschreibung")]
        public string Description { get; set; }

    }
}