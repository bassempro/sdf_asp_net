using sdf_asp_net.Models;
using sdf_asp_net.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sdf_asp_net.Services
{
    public sealed class MockUpService
    {
        private ProjectsViewModel projectsViewModel;

        private static MockUpService instance = null;
        private static readonly object padlock = new object();

        MockUpService()
        {
            projectsViewModel = new ProjectsViewModel();
            Project dummyProject01 = new Project();
            dummyProject01.Name = "Pujans Nasenverkleinerung";
            Project dummyProject02 = new Project();
            dummyProject02.Name = "Cihans Barmitzwa";
            projectsViewModel.projects.Add(dummyProject01);
            projectsViewModel.projects.Add(dummyProject02);
        }

        public static MockUpService Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new MockUpService();
                    }
                    return instance;
                }
            }
        }


        public ProjectsViewModel getProjects()
        {
            return this.projectsViewModel;
        }

        public void addProject(Project newProject)
        {
            this.projectsViewModel.projects.Add(newProject);
        }

    }


}