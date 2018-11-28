using System.Collections;
using System.ComponentModel;

namespace sdf_asp_net.Models
{
    public class ProjectModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayName("Beschreibung")]
        public string Description { get; set; }
        [DisplayName("Nutzer")]
        public string Member { get; set; }
        [DisplayName("ProjektleiterID")]
        public string ManagerId { get; set; }
        public string ManagerName { get; set; }

    }
}