using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using proyecto_v7.Models;

namespace proyecto_v7.Controllers
{
    public class HomeController : Controller
    {
        private string connectionString = "Data Source=imssur.database.windows.net;Initial Catalog=db;User ID=dbUserS;Password=Spas$w0rd;";
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var data = new List<Clases>();
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT [Clave], [Profesor], [Cupo] FROM [Clases]", con);
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    data.Add(new Clases
                    {
                        Clave = (string)dr["Clave"],
                        Profesor = (string)dr["Profesor"],
                        Cupo = (int)dr["Cupo"]
                    });
                }
                return View(data);
            }
            catch (Exception) { throw; }
            finally
            {
                con.Close();
            }
        }

        public IActionResult Details(Guid id)
        {
            var data = new Clases();
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT [Clave], [Profesor], [Cupo] FROM [Clases] WHERE [Id] = @i", con);
            cmd.Parameters.Add("@i", SqlDbType.UniqueIdentifier).Value = id;
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    data.Id = (Guid)dr["Id"];
                    data.Clave = (string)dr["Clave"];
                    data.Profesor = (string)dr["Profesor"];
                    data.Cupo = (int)dr["Cupo"];
                }
                return View(data);
            }
            catch (Exception) { throw; }
            finally
            {
                con.Close();
            }
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Clases data)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(@"INSERT INTO [Clases] ([Id],[Clave],[Profesor],[Cupo]) VALUES (NEWID(), @c, @p, @d);", con);
            cmd.Parameters.Add("@c", SqlDbType.NVarChar, 100).Value = data.Clave;
            cmd.Parameters.Add("@p", SqlDbType.NVarChar, 100).Value = data.Profesor;
            cmd.Parameters.Add("@d", SqlDbType.Int).Value = data.Cupo;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }


        public IActionResult Edit(Guid id)
        {
            var data = new Clases();
            var con = new SqlConnection(connectionString);
            var cmd = new SqlCommand("SELECT [Id], [Clave], [Profesor], [Cupo] FROM [Clases] WHERE [Id] = @i", con);
            cmd.Parameters.Add("@i", SqlDbType.UniqueIdentifier).Value = id;

            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    data.Id = (Guid)dr["Id"];
                    data.Clave = (string)dr["Clave"];
                    data.Profesor = (string)dr["Profesor"];
                    data.Cupo = (int)dr["Cupo"];
                }
                return View(data);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Clases data)
        {
            var con = new SqlConnection(connectionString);
            var cmd = new SqlCommand(@"UPDATE [Clases] SET [Clave] = @c, [Profesor] = @p, [Cupo] = @d	WHERE [Id] = @i;", con);

            cmd.Parameters.Add("@i", SqlDbType.UniqueIdentifier).Value = data.Id;
            cmd.Parameters.Add("@c", SqlDbType.NVarChar, 100).Value = data.Clave;
            cmd.Parameters.Add("@p", SqlDbType.NVarChar, 100).Value = data.Profesor;
            cmd.Parameters.Add("@d", SqlDbType.Int).Value = data.Cupo;

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public IActionResult Delete(Guid id)
        {
            var con = new SqlConnection(connectionString);
            var cmd = new SqlCommand("DELETE FROM [Clases] WHERE [Id] = @i", con);

            cmd.Parameters.Add("@i", SqlDbType.UniqueIdentifier).Value = id;

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
