using System;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using sdf_asp_net.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using sdf_asp_net.ViewModels;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;


namespace sdf_asp_net.Controllers
{

    [Authorize]

    public class ProjectController : Controller
    {
        ApplicationDbContext _context;

        public ProjectController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult _Chat()
        {
            var liste = _context.ChatModels.ToList();
            return View(liste);
        }

        string connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=aspnet-sdf_asp_net-20181114091107;Integrated Security=True";
        private ApplicationUserManager _userManager;

        

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

            for(int i = 0; i < dtblProjects.Rows.Count; i++)
            {
                DataTable dtblProjectUser = new DataTable();
                string id = dtblProjects.Rows[i][0].ToString();
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query = "SELECT * FROM ProjectUser WHERE ProjectId = @Id";
                    SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                    sqlDa.SelectCommand.Parameters.AddWithValue("@Id", id);
                    sqlDa.Fill(dtblProjectUser);
                }
                bool contains = false;
                for(int j = 0; j < dtblProjectUser.Rows.Count; j++)
                {
                    if (dtblProjectUser.Rows[j][1].ToString().Trim().ToLower().Equals(userName.ToString().Trim().ToLower()) || dtblProjects.Rows[i][4].ToString().Trim().ToLower().Equals(userName.ToString().Trim().ToLower()))
                    {
                        contains = true;
                    }
                }
                
                if (!contains)
                {
                    dtblProjects.Rows.RemoveAt(i);
                    i--;
                }


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
            DataTable dtblProject = new DataTable();
            DataTable dtblMessageboard = new DataTable();
            DataTable dtblMessageboardReplies = new DataTable();
            DataTable dtblProjectUser = new DataTable();
            DataTable img = new DataTable();

            byte[] profileImage = null;
            string contentType = "";
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT * FROM Projects WHERE Id = @Id";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@Id", id);
                sqlDa.Fill(dtblProject);

                query = "SELECT * FROM Messageboards WHERE ProjectId = @Id";
                sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@Id", id);
                sqlDa.Fill(dtblMessageboard);

                query = "SELECT * FROM ProjectUser WHERE ProjectId = @Id";
                sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@Id", id);
                sqlDa.Fill(dtblProjectUser);

                string selectQuery = "SELECT AspNetUsers.ProfileImage, AspNetUsers.FileContentType, AspNetUsers.UserName FROM AspNetUsers, ProjectUser WHERE ProjectUser.ProjectId = " + id + " AND AspNetUsers.UserName = ProjectUser.Name";

                // Read Byte [] Value from Sql Table 
                SqlCommand selectCommand = new SqlCommand(selectQuery, sqlCon);

                sqlDa = new SqlDataAdapter(selectQuery, sqlCon);
                sqlDa.Fill(img);

/*
                SqlDataReader reader;
                reader = selectCommand.ExecuteReader();
                Dictionary<string, string> images = new Dictionary<string, string>();
                if (reader != null)
                {
                    if (reader.Read() && !System.Convert.IsDBNull(reader.GetValue(0)))
                    {
                        profileImage = (byte[])reader.GetValue(0);
                        contentType = Convert.ToString(reader.GetValue(1));
                        userName = Convert.ToString(reader.GetValue(2));

                        images.Add("testo", "test");

                        images.Add(userName, string.Format("data:{0};base64,{1}",
                            contentType, Convert.ToBase64String(profileImage)));
                        Debug.WriteLine(userName);
                        Debug.WriteLine("++++++++++++++++++ " + userName +" "+ string.Format("data:{0};base64,{1}",
                            contentType, Convert.ToBase64String(profileImage)));

                    }
                }
                */
                //ViewBag.Images = images;
            }

            Dictionary<string, string> images = new Dictionary<string, string>();
            Debug.WriteLine("++++++++++++++++++ " + img.Rows.Count);
            images.Add("testo","test");
            if (img.Rows.Count >= 1)
            {
                for (int i = 0; i < img.Rows.Count; i++)
                {
                    if (img.Rows[i][0] != System.DBNull.Value && img.Rows[i][1] != System.DBNull.Value && img.Rows[i][2] != System.DBNull.Value)
                    {
                        profileImage = (byte[])img.Rows[i][0];
                        contentType = img.Rows[i][1].ToString();
                        userName = img.Rows[i][2].ToString();

                        Debug.WriteLine("++++++++++++++++++ " + img.Rows.Count + " " + userName);

                        images.Add(userName, string.Format("data:{0};base64,{1}", contentType, Convert.ToBase64String(profileImage)));
                    }

                }
            }
            ViewBag.Images = images;

            if (dtblProject.Rows.Count == 1)
            {
                ProjectViewModel pvm = new ProjectViewModel();
                pvm.Id = Convert.ToInt32(dtblProject.Rows[0][0].ToString());
                pvm.Name = dtblProject.Rows[0][1].ToString();
                pvm.Description = dtblProject.Rows[0][2].ToString();
                pvm.ManagerId = dtblProject.Rows[0][3].ToString();
                pvm.ManagerName = dtblProject.Rows[0][4].ToString();

                for (int i = 0; i < dtblProjectUser.Rows.Count; i++)
                {
                    pvm.Member.Add(dtblProjectUser.Rows[i][1].ToString());
                }
                for (int i = 0; i < dtblMessageboard.Rows.Count; i++)
                {
                    MessageViewModel mvm = new MessageViewModel();
                    mvm.Id = Convert.ToInt32(dtblMessageboard.Rows[i][0].ToString());
                    mvm.Message = dtblMessageboard.Rows[i][1].ToString();
                    mvm.Author = dtblMessageboard.Rows[i][3].ToString();
                    mvm.Date = dtblMessageboard.Rows[i][4].ToString();
                    mvm.FileName = dtblMessageboard.Rows[i][5].ToString();
                    mvm.FileContentType = dtblMessageboard.Rows[i][6].ToString();
                    using (SqlConnection sqlCon = new SqlConnection(connectionString))
                    {
                        sqlCon.Open();
                        string query = "SELECT * FROM MessageboardReplies WHERE MessageboardId = @mId";
                        SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                        sqlDa.SelectCommand.Parameters.AddWithValue("@mId", dtblMessageboard.Rows[i][0].ToString());
                        sqlDa.Fill(dtblMessageboardReplies);
                    }
                    for (int j = 0; j < dtblMessageboardReplies.Rows.Count; j++)
                    {
                        string info1 = dtblMessageboardReplies.Rows[j][3].ToString();
                        string info2 = dtblMessageboard.Rows[i][0].ToString();
                        System.Diagnostics.Debug.WriteLine(info1 + " compared to: " + info2);
                        if (dtblMessageboardReplies.Rows[j][3].ToString() == dtblMessageboard.Rows[i][0].ToString())
                        {
                            MessageReplyModel mrm = new MessageReplyModel();
                            mrm.Id = Convert.ToInt32(dtblMessageboardReplies.Rows[j][0]);
                            mrm.Message = dtblMessageboardReplies.Rows[j][1].ToString();
                            mrm.Author = dtblMessageboardReplies.Rows[j][2].ToString();
                            mrm.Date = dtblMessageboardReplies.Rows[j][4].ToString();
                            mvm.MessageReplies.Add(mrm);
                            System.Diagnostics.Debug.WriteLine(mvm.MessageReplies.Count.ToString());
                        }
                    }
                    pvm.Messages.Add(mvm);
                }

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
            List<string> usersToAdd = new List<string>();
            var context = new ApplicationDbContext();
            var allUsers = context.Users.ToList();
            for (int i = 0; i < allUsers.Count; i++)
            {
                if (collection["box_" + allUsers[i].UserName] == "True")
                {
                    usersToAdd.Add(allUsers[i].UserName);
                }
            }

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "INSERT INTO Projects VALUES(@Name, @Description, @ManagerId, @ManagerName)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@Name", name);
                sqlCmd.Parameters.AddWithValue("@Description", description);
                sqlCmd.Parameters.AddWithValue("@ManagerId", userId);
                sqlCmd.Parameters.AddWithValue("@ManagerName", userName);
                sqlCmd.ExecuteNonQuery();

                DataTable dtblProject = new DataTable();
                query = "SELECT * FROM Projects WHERE Name = @Name";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@Name", name);
                sqlDa.Fill(dtblProject);

                if (dtblProject.Rows.Count == 1)
                {
                    int createdProjectId = Convert.ToInt32(dtblProject.Rows[0][0].ToString());
                    for (int i = 0; i < usersToAdd.Count; i++)
                    {
                        query = "INSERT INTO ProjectUser VALUES(@Name, @ProjectId)";
                        sqlCmd = new SqlCommand(query, sqlCon);
                        sqlCmd.Parameters.AddWithValue("@Name", usersToAdd[i]);
                        sqlCmd.Parameters.AddWithValue("@ProjectId", createdProjectId);
                        sqlCmd.ExecuteNonQuery();
                    }

                    for (int i = 0; i < usersToAdd.Count; i++)
                    {
                        System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                            new System.Net.Mail.MailAddress("sdf.asp.donet@gmail.com", "Projekteinladung"),
                            new System.Net.Mail.MailAddress(usersToAdd[i]));
                        m.Subject = name;
                        m.Body = string.Format("Dear {0}, Sie wurden zu dem Projekt " + name + " eingeladen. Unter dieser Addresse: " + HttpContext.Request.Url.Authority + "/Project/DetailView/" + Convert.ToInt32(dtblProject.Rows[0][0].ToString()) + " können sie das Projekt einsehen",
                         usersToAdd[i], Request.Url.Scheme);
                        m.IsBodyHtml = true;
                        System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                        smtp.Credentials = new System.Net.NetworkCredential("sdf.asp.donet@gmail.com", "test-123123");
                        m.IsBodyHtml = true;
                        // smtp.ServerCertificateValidationCallback = () => true;
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                        client.EnableSsl = true;
                        smtp.Send(m);
                    }


                }

            }
            return RedirectToAction("Index");
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int id)
        {
            ProjectViewModel projectModel = new ProjectViewModel();
            DataTable dtblProject = new DataTable();
            DataTable dtblProjectUser = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT * FROM Projects WHERE Id = @Id";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@Id", id);
                sqlDa.Fill(dtblProject);

                query = "SELECT * FROM ProjectUser WHERE ProjectId = @Id";
                sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@Id", id);
                sqlDa.Fill(dtblProjectUser);
            }
            if (dtblProject.Rows.Count == 1)
            {
                projectModel.Id = Convert.ToInt32(dtblProject.Rows[0][0].ToString());
                projectModel.Name = dtblProject.Rows[0][1].ToString();
                projectModel.Description = dtblProject.Rows[0][2].ToString();
                var context = new ApplicationDbContext();
                var allUsers = context.Users.ToList();
                string[] user = new string[allUsers.Count];
                for (int i = 0; i < allUsers.Count; i++)
                {
                    projectModel.Member.Add(allUsers[i].UserName);
                }
                return View(projectModel);
            }
            else
                return RedirectToAction("Index");
        }

        // POST: Project/Edit/5
        [HttpPost]
        public ActionResult Edit(ProjectViewModel projectViewModel, FormCollection collection)
        {
            List<string> usersToAdd = new List<string>();
            var context = new ApplicationDbContext();
            var allUsers = context.Users.ToList();
            for (int i = 0; i < allUsers.Count; i++)
            {
                if (collection["box_" + allUsers[i].UserName] == "True")
                {
                    usersToAdd.Add(allUsers[i].UserName);
                }
            }

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "UPDATE Projects SET Name = @Name, Description = @Description WHERE Id = @Id";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@Id", projectViewModel.Id);
                sqlCmd.Parameters.AddWithValue("@Name", projectViewModel.Name);
                sqlCmd.Parameters.AddWithValue("@Description", projectViewModel.Description);
                sqlCmd.ExecuteNonQuery();

                query = "DELETE FROM ProjectUser WHERE ProjectId = @Id";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@Id", projectViewModel.Id);
                sqlCmd.ExecuteNonQuery();

                for (int i = 0; i < usersToAdd.Count; i++)
                {
                    query = "INSERT INTO ProjectUser VALUES(@Name, @ProjectId)";
                    sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@Name", usersToAdd[i]);
                    sqlCmd.Parameters.AddWithValue("@ProjectId", projectViewModel.Id);
                    sqlCmd.ExecuteNonQuery();
                    System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                        new System.Net.Mail.MailAddress("sdf.asp.donet@gmail.com", "Projekteinladung"),
                        new System.Net.Mail.MailAddress(usersToAdd[i]));
                    m.Subject = projectViewModel.Name;
         
                    m.Body = string.Format("Dear {0}, Sie wurden zu dem Projekt " + projectViewModel.Name + " eingeladen. Unter dieser Addresse: http://"+ HttpContext.Request.Url.Authority + "/Project/DetailView/" + projectViewModel.Id.ToString() + " können sie das Projekt einsehen",
                         usersToAdd[i], Request.Url.Scheme);
                    m.IsBodyHtml = true;
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                    smtp.Credentials = new System.Net.NetworkCredential("sdf.asp.donet@gmail.com", "test-123123");
                    m.IsBodyHtml = true;
                    // smtp.ServerCertificateValidationCallback = () => true;
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                    client.EnableSsl = true;
                    smtp.Send(m);
                }


            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult Reply(FormCollection collection)
        {
            string message = collection["message"];
            string projectId = collection["projectId"];
            int projectIdConverted = int.Parse(projectId);
            if (message != null && message.Length > 2)
            {
                DateTime timeStamp = System.DateTime.Now;
                int messageboardId = int.Parse(collection["messageboardId"]);
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query = "INSERT INTO MessageboardReplies VALUES(@Message, @Author, @MessageboardId, @Time, @ProjectId)";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@Message", message);
                    sqlCmd.Parameters.AddWithValue("@Author", User.Identity.GetUserName());
                    sqlCmd.Parameters.AddWithValue("@MessageboardId", messageboardId);
                    sqlCmd.Parameters.AddWithValue("@Time", timeStamp);
                    sqlCmd.Parameters.AddWithValue("@ProjectId", projectIdConverted);

                    sqlCmd.ExecuteNonQuery();
                }
            }

            return RedirectToAction("DetailView/" + projectId);
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
                query = "DELETE FROM Messageboards WHERE ProjectId = @Id";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@Id", id);
                sqlCmd.ExecuteNonQuery();
                query = "DELETE FROM ProjectUser WHERE ProjectId = @Id";
                sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@Id", id);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Message(string message, string projectId, HttpPostedFileBase postedFile)
        {
            int projectIdConverted = int.Parse(projectId);
            DateTime timeStamp = System.DateTime.Now;

            if (postedFile != null && message != null && message.Length > 5)
            {
                byte[] bytes;
                using (BinaryReader br = new BinaryReader(postedFile.InputStream))
                {
                    bytes = br.ReadBytes(postedFile.ContentLength);
                }
                if (message != null && message.Length > 5)
                {
                    using (SqlConnection sqlCon = new SqlConnection(connectionString))
                    {
                        sqlCon.Open();
                        string query = "INSERT INTO Messageboards VALUES(@Message, @ProjectId, @Author, @Time, @FileName, @FileContentType, @FileData)";
                        SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                        sqlCmd.Parameters.AddWithValue("@Message", message);
                        sqlCmd.Parameters.AddWithValue("@ProjectId", projectIdConverted);
                        sqlCmd.Parameters.AddWithValue("@Author", User.Identity.GetUserName());
                        sqlCmd.Parameters.AddWithValue("@Time", timeStamp);
                        sqlCmd.Parameters.AddWithValue("@FileName", Path.GetFileName(postedFile.FileName));
                        sqlCmd.Parameters.AddWithValue("@FileContentType", postedFile.ContentType);
                        sqlCmd.Parameters.AddWithValue("@FileData", bytes);

                        sqlCmd.ExecuteNonQuery();
                    }
                }

            }
            else
            {
                if (message != null && message.Length > 5)
                {
                    using (SqlConnection sqlCon = new SqlConnection(connectionString))
                    {
                        sqlCon.Open();
                        string query = "INSERT INTO Messageboards VALUES(@Message, @ProjectId, @Author, @Time, NULL, NULL, NULL)";
                        SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                        sqlCmd.Parameters.AddWithValue("@Message", message);
                        sqlCmd.Parameters.AddWithValue("@ProjectId", projectIdConverted);
                        sqlCmd.Parameters.AddWithValue("@Author", User.Identity.GetUserName());
                        sqlCmd.Parameters.AddWithValue("@Time", timeStamp);
                        sqlCmd.Parameters.AddWithValue("@FileName", DBNull.Value);
                        sqlCmd.Parameters.AddWithValue("@FileContentType", DBNull.Value);
                        sqlCmd.Parameters.AddWithValue("@FileData", DBNull.Value);

                        sqlCmd.ExecuteNonQuery();
                    }
                }
            }
            return RedirectToAction("DetailView/" + projectId);
        }
        [HttpPost]
        public FileResult DownloadFile(int? fileId)
        {
            byte[] bytes;
            string fileName, contentType;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT FileName, FileData, FileContentType FROM Messageboards WHERE Id=@Id";
                    cmd.Parameters.AddWithValue("@Id", fileId);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        sdr.Read();
                        bytes = (byte[])sdr["FileData"];
                        contentType = sdr["FileContentType"].ToString();
                        fileName = sdr["FileName"].ToString();
                    }
                    con.Close();
                }
            }
            return File(bytes, contentType, fileName);
        }
    }
}
