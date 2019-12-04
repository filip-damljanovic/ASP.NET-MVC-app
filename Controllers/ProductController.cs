using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using MVC_app.Models;

namespace MVC_app.Controllers
{
    public class ProductController : Controller
    {
        string connectionString = @"Data Source = DESKTOP-5MF7LI8; Initial Catalog = MVC_app_database; Integrated Security=True";
        // GET: List of products
        [HttpGet]
        public ActionResult Index()
        {
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM Proizvodi", sqlCon);
                sqlDa.Fill(dtblProduct);
            }

            return View(dtblProduct);
        }

        // GET: Product/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View(new ProductModel());
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(ProductModel productModel)
        {
            using(SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "INSERT INTO Proizvodi VALUES(@Naziv, @Opis, @Kategorija, @Proizvodjac, @Dobavljac, @Cena)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@Naziv", productModel.Naziv);
                sqlCmd.Parameters.AddWithValue("@Opis", productModel.Opis);
                sqlCmd.Parameters.AddWithValue("@Kategorija", productModel.Kategorija);
                sqlCmd.Parameters.AddWithValue("@Proizvodjac", productModel.Proizvodjac);
                sqlCmd.Parameters.AddWithValue("@Dobavljac", productModel.Dobavljac);
                sqlCmd.Parameters.AddWithValue("@Cena", productModel.Cena);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
