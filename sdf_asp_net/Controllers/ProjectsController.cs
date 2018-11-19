using sdf_asp_net.Models;
using sdf_asp_net.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sdf_asp_net.Controllers
{
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
                Project newProject = new Project();
                newProject.Name = project.Name;
                mockUpService.addProject(newProject);
            }

            return new RedirectResult("~/Home/Index");
        }

    }
}