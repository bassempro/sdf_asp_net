/*using sdf_asp_net.Models;



namespace sdf_asp_net.Services
{
    public sealed class MockUpService
    {
        private ProjectsViewModel projectsViewModel;

        private static MockUpService instance = null;
        private static readonly object padlock = new object();
        private int currentID = 0;

        MockUpService()
        {
            projectsViewModel = new ProjectsViewModel();
            Project dummyProject01 = new Project
            {
                Name = "Pujans Nasenverkleinerung"
            };
            Project dummyProject02 = new Project
            {
                Name = "Cihans Barmitzwa"
            };
            Project dummyProject03 = new Project
            {
                Name = "Project X"
            };
            Project dummyProject04 = new Project
            {
                Name = "Sarouch party"
            };
            this.AddProject(dummyProject01);
            this.AddProject(dummyProject02);
            this.AddProject(dummyProject03);
            this.AddProject(dummyProject04);
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


        public ProjectsViewModel GetProjects()
        {
            return this.projectsViewModel;
        }

        public void AddProject(Project newProject)
        {
            newProject.Id = this.currentID;
            this.currentID++;
            this.projectsViewModel.projects.Add(newProject);
        }

        public void DeleteProject(int id)
        {
            for (int i = 0; i < this.projectsViewModel.projects.Count; i++)
            {
                if (this.projectsViewModel.projects[i].Id == id)
                {
                    this.projectsViewModel.projects.RemoveAt(i);
                }
            }
        }

    }


}*/