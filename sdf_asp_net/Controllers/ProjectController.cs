using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using sdf_asp_net.Models;

namespace sdf_asp_net.Controllers
{
    [Authorize]

    public class ProjectController : Controller
    {
        string connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=aspnet-sdf_asp_net-20181114091107;Integrated Security=True";
        //Lists all Projects 
        // GET: Project
        [HttpGet]
        public ActionResult Index()
        {
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
            return View(new ProjectModel());
        }

        public ActionResult DetailView(int id)
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

        // POST: Project/Create
        [HttpPost]
        public ActionResult Create(ProjectModel projectModel)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "INSERT INTO Projects VALUES(@Name, @Description)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@Name", projectModel.Name);
                sqlCmd.Parameters.AddWithValue("@Description", projectModel.Description);
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
