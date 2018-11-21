using sdf_asp_net.Models;
using sdf_asp_net.Services;
using System;
using System.Web.Mvc;

namespace sdf_asp_net.Controllers
{

    [Authorize]
    public class ProjectsController : Controller
    {
        // GET: Projects
        public ViewResult New()
        {
            return new ViewResult();
        }

        public RedirectResult Create(Project project)
        {
            if (ModelState.IsValid && project != null)
            {
                MockUpService mockUpService = MockUpService.Instance;
                Project newProject = new Project
                {
                    Name = project.Name
            };
                mockUpService.AddProject(newProject);
            }

            return new RedirectResult("~/Home/Index");
        }

        public RedirectResult Delete(string id)
        {
            if (ModelState.IsValid && id != null)
            {
                MockUpService mockUpService = MockUpService.Instance;
                int indize = 0;
                Int32.TryParse(id, out indize);
                mockUpService.DeleteProject(indize);

            }

            return new RedirectResult("~/Home/Index");
        }

    }
}