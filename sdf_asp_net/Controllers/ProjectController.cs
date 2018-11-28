using System;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using sdf_asp_net.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Collections;
using sdf_asp_net.ViewModels;
using Microsoft.AspNet.Identity;

namespace sdf_asp_net.Controllers
{
    [Authorize]

    public class ProjectController : Controller
    {
        string connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=aspnet-sdf_asp_net-20181114091107;Integrated Security=True";
        private ApplicationUserManager _userManager;

        public ProjectController()
        {
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        //Lists all Projects 
        // GET: Project
        [HttpGet]
        public ActionResult Index()
        {
            var userName = User.Identity.GetUserName();
            ViewBag.Username = userName;
            DataTable dtblProjects = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM Projects", sqlCon);
                sqlDa.Fill(dtblProjects);
            }
                return View(dtblProjects);
        }

        // GET: Project/Create
        [HttpGet]
        public ActionResult Create()
        {
            var context = new ApplicationDbContext();
            var allUsers = context.Users.ToList();
            string[] user = new string[allUsers.Count];
            for (int i = 0; i < allUsers.Count; i++)
            {
                user[i] = allUsers[i].UserName;
            }

            return View((object)user);
        }

        public ActionResult DetailView(int id)
        {
            var userName = User.Identity.GetUserName();
            userName.ToString();
            ProjectModel projectModel = new ProjectModel();
            DataTable dtblProject = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT * FROM Projects WHERE Id = @Id";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@Id", id);
                sqlDa.Fill(dtblProject);
            }
            if (dtblProject.Rows.Count == 1)
            {
                projectModel.Id = Convert.ToInt32(dtblProject.Rows[0][0].ToString());
                projectModel.Name = dtblProject.Rows[0][1].ToString();
                projectModel.Description = dtblProject.Rows[0][2].ToString();
                projectModel.Member = dtblProject.Rows[0][3].ToString();
                projectModel.ManagerId = dtblProject.Rows[0][4].ToString();
                projectModel.ManagerName = dtblProject.Rows[0][5].ToString();
                ProjectViewModel pvm = new ProjectViewModel();
                pvm.Id = projectModel.Id;
                pvm.Name = projectModel.Name;
                pvm.Description = projectModel.Description;
                pvm.ConvertStringMemberToArrayListMember(projectModel.Member);
                pvm.ManagerId = projectModel.ManagerId;
                pvm.ManagerName = projectModel.ManagerName;

                ViewBag.IsAuthorized = userName;

                return View(pvm);
            }
            else
                return RedirectToAction("Index");
        }

        // POST: Project/Create
        [HttpPost]
        public ActionResult Create(string name, string description, FormCollection collection)
        {
            var userId = User.Identity.GetUserId();
            var userName = User.Identity.GetUserName();
            string usersToAdd = "";
            var context = new ApplicationDbContext();
            var allUsers = context.Users.ToList();
            for (int i = 0; i < allUsers.Count; i++)
            {
                if (collection["box_" + allUsers[i].UserName] == "True")
                {
                    usersToAdd += allUsers[i].UserName + ";";
                }
            }
            usersToAdd = usersToAdd.Substring(0, (usersToAdd.Length - 1));

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "INSERT INTO Projects VALUES(@Name, @Description, @Member, @ManagerId, @ManagerName)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@Name", name);
                sqlCmd.Parameters.AddWithValue("@Description", description);
                sqlCmd.Parameters.AddWithValue("@Member", usersToAdd);
                sqlCmd.Parameters.AddWithValue("@ManagerId", userId);
                sqlCmd.Parameters.AddWithValue("@ManagerName", userName);

                sqlCmd.ExecuteNonQuery();

            }
            return RedirectToAction("Index");
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int id)
        {
            ProjectModel projectModel = new ProjectModel();
            DataTable dtblProject = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT * FROM Projects WHERE Id = @Id";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@Id", id);
                sqlDa.Fill(dtblProject);
            }
            if (dtblProject.Rows.Count == 1)
            {
                projectModel.Id = Convert.ToInt32(dtblProject.Rows[0][0].ToString());
                projectModel.Name = dtblProject.Rows[0][1].ToString();
                projectModel.Description = dtblProject.Rows[0][2].ToString();

                return View(projectModel);
            }
            else
                return RedirectToAction("Index");
        }

        // POST: Project/Edit/5
        [HttpPost]
        public ActionResult Edit(ProjectModel projectModel)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "UPDATE Projects SET Name = @Name, Description = @Description WHERE Id = @Id";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@Id", projectModel.Id);
                sqlCmd.Parameters.AddWithValue("@Name", projectModel.Name);
                sqlCmd.Parameters.AddWithValue("@Description", projectModel.Description);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "DELETE FROM Projects WHERE Id = @Id";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@Id", id);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}
