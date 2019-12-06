using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using MVC_app.Models;
using System.IO;
using Newtonsoft.Json;
using System.Collections;

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

        // GET: List of products JSON
        [HttpGet]
        public ActionResult IndexJSON()
        {
            StreamReader streamReader = new StreamReader(HttpContext.Server.MapPath("~/App_Data/proizvodi.json"));
            string data = streamReader.ReadToEnd();
            streamReader.Close();

            List<ProductModel> products = new List<ProductModel>();
            products = JsonConvert.DeserializeObject<List<ProductModel>>(data);

            return View(products);
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

        // GET: Product/Create JSON
        [HttpGet]
        public ActionResult CreateJSON()
        {
            return View(new ProductModel());
        }

        // POST: Product/Create JSON
        [HttpPost]
        public ActionResult CreateJSON(ProductModel productModel)
        {
            StreamReader streamReader = new StreamReader(HttpContext.Server.MapPath("~/App_Data/proizvodi.json"));
            string data = streamReader.ReadToEnd();
            streamReader.Close();

            List<ProductModel> products = new List<ProductModel>();
            products = JsonConvert.DeserializeObject<List<ProductModel>>(data);

            productModel.ID = products[products.Count - 1].ID + 1;
            products.Add(productModel);

            string jSONString = JsonConvert.SerializeObject(products);
            StreamWriter streamWriter = System.IO.File.CreateText(HttpContext.Server.MapPath("~/App_Data/proizvodi.json"));
            streamWriter.Write(jSONString);
            streamWriter.Close();

            return RedirectToAction("IndexJSON");
        }

        // GET: Product/Edit/{id}
        [HttpGet]
        public ActionResult Edit(int id)
        {
            ProductModel productModel = new ProductModel();
            DataTable dtblProduct = new DataTable();

            using(SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT * FROM Proizvodi WHERE ID = @ID";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@ID", id);
                sqlDa.Fill(dtblProduct);
            }

            if(dtblProduct.Rows.Count == 1)
            {
                productModel.ID = Convert.ToInt32(dtblProduct.Rows[0][0].ToString());
                productModel.Naziv = dtblProduct.Rows[0][1].ToString();
                productModel.Opis = dtblProduct.Rows[0][2].ToString();
                productModel.Kategorija = dtblProduct.Rows[0][3].ToString();
                productModel.Proizvodjac = dtblProduct.Rows[0][4].ToString();
                productModel.Dobavljac = dtblProduct.Rows[0][5].ToString();
                productModel.Cena = Convert.ToDecimal(dtblProduct.Rows[0][6].ToString());

                return View(productModel);
            }

            else
            {
                return RedirectToAction("Index");
            }
            
        }

        // POST: Product/Edit/{id}
        [HttpPost]
        public ActionResult Edit(ProductModel productModel)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "UPDATE Proizvodi SET Naziv = @Naziv, Opis = @Opis, Kategorija = @Kategorija, Proizvodjac = @Proizvodjac, Dobavljac = @Dobavljac, Cena = @Cena WHERE ID = @ID";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@ID", productModel.ID);
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

        // GET: Product/Edit/{id} JSON
        [HttpGet]
        public ActionResult EditJSON(int id)
        {
            ProductModel productModel = new ProductModel();
            StreamReader streamReader = new StreamReader(HttpContext.Server.MapPath("~/App_Data/proizvodi.json"));
            string data = streamReader.ReadToEnd();
            streamReader.Close();

            List<ProductModel> products = new List<ProductModel>();
            products = JsonConvert.DeserializeObject<List<ProductModel>>(data);

            productModel.ID = products[id-1].ID;
            productModel.Naziv = products[id-1].Naziv;
            productModel.Opis = products[id-1].Opis;
            productModel.Kategorija = products[id-1].Kategorija;
            productModel.Proizvodjac = products[id-1].Proizvodjac;
            productModel.Dobavljac = products[id-1].Dobavljac;
            productModel.Cena = products[id-1].Cena;

            return View(productModel);
        }

        // POST: Product/Edit/{id} JSON
        [HttpPost]
        public ActionResult EditJSON(ProductModel productModel)
        {
            StreamReader streamReader = new StreamReader(HttpContext.Server.MapPath("~/App_Data/proizvodi.json"));
            string data = streamReader.ReadToEnd();
            streamReader.Close();

            List<ProductModel> products = new List<ProductModel>();
            products = JsonConvert.DeserializeObject<List<ProductModel>>(data);

            int index = products.FindIndex(x => x.ID == productModel.ID);
            products[index] = productModel;

            string jSONString = JsonConvert.SerializeObject(products);
            StreamWriter streamWriter = System.IO.File.CreateText(HttpContext.Server.MapPath("~/App_Data/proizvodi.json"));
            streamWriter.Write(jSONString);
            streamWriter.Close();

            return RedirectToAction("IndexJSON");
        }

        // GET: Product/Delete/{id}
        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "DELETE FROM Proizvodi WHERE ID = @ID";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@ID", id);
                sqlCmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        // GET: Product/Delete/{id} JSON
        [HttpGet]
        public ActionResult DeleteJSON(int id)
        {
            StreamReader streamReader = new StreamReader(HttpContext.Server.MapPath("~/App_Data/proizvodi.json"));
            string data = streamReader.ReadToEnd();
            streamReader.Close();

            List<ProductModel> products = new List<ProductModel>();
            products = JsonConvert.DeserializeObject<List<ProductModel>>(data);

            int index = products.FindIndex(x => x.ID == id);
            products.RemoveAt(index);

            string jSONString = JsonConvert.SerializeObject(products);
            StreamWriter streamWriter = System.IO.File.CreateText(HttpContext.Server.MapPath("~/App_Data/proizvodi.json"));
            streamWriter.Write(jSONString);
            streamWriter.Close();

            return RedirectToAction("IndexJSON");
        }
    }
}