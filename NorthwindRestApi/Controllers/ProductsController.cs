using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models;

namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private Northwind1Context db;

        public ProductsController(Northwind1Context dbparametri)
        {
            db = dbparametri;
        }

        //Hakee kaikki tuotteet
        [HttpGet]
        public ActionResult GetAllProducts()
        {
            try
            {
                var tuotteet = db.Products.ToList();
                return Ok(tuotteet);
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + e.InnerException);
            }
        }

        //Hakee yhden tuotteen pääavaimella
        [HttpGet("{id}")]
        public ActionResult GetOneProductById(int id)
        {
            try
            {
                var tuote = db.Products.Find(id);
                if (tuote != null)
                {
                    return Ok(tuote);
                }
                else
                {
                    return NotFound($"Tuotetta id:llä {id} ei löydy."); // string interpolation -tapa
                }
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + e);
            }
        }

        //Hakee nimen osalla: 
        [HttpGet("productname/{pname}")]
        public ActionResult GetByProductName(string pname)
        {
            try
            {
                var prod = db.Products.Where(p => p.ProductName.Contains(pname));
                return Ok(prod);
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Hakee tuotteita jolla tietty varastosaldo
        [HttpGet("stock/{amount}")]
        public ActionResult GetProductsByStockAmount(short amount)
        {
            try
            {
                var tuote = db.Products
                    .Where(p => p.UnitsInStock == amount)
                    .ToList();

                if (tuote.Count == 0)
                {
                    return NotFound($"Ei löytynyt tuotteita, joilla on varastossa {amount} kpl.");
                }

                return Ok(tuote);
            }
            catch (Exception ex)
            {
                return BadRequest("Virhe haettaessa tuotteita: " + ex.Message);
            }
        }

        //Uuden lisääminen
        [HttpPost]
        public ActionResult AddNewProduct([FromBody] Product prod)
        {
            try
            {
                db.Products.Add(prod);
                db.SaveChanges();
                return Ok($"Lisättiin uusi tuote {prod.ProductName}");
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + e.InnerException);
            }
        }
        // Tuoteen muokkaaminen
        [HttpPut("{id}")]
        public ActionResult EditProduct(int id, [FromBody] Product product)
        {
            var tuote = db.Products.Find(id);
            if (tuote != null)
            {

                db.Entry(tuote).CurrentValues.SetValues(product);
                

                db.SaveChanges();
                return Ok("Muokattu tuotetta " + tuote.ProductName);
            }

            return NotFound("Asikasta ei löytynyt id:llä " + id);
        }

        //Tuotteen poistaminen
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var tuote = db.Products.Find(id);

                if (tuote != null) //Jos id:llä löytyy tuote
                {
                    db.Products.Remove(tuote);
                    db.SaveChanges();
                    return Ok("Tuote " + tuote.ProductName + " poistettiin.");
                }

                return NotFound("Tuote id:llä " + id + " ei löytynyt.");
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }

    }
}
