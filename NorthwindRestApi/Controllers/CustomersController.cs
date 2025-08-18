using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models;
using System.Net;


namespace NorthwindRestApi.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        //Alustetaan tietokantayhteys

        //Perinteinen tapa
        //Northwind1Context db = new Northwind1Context();

        //Dependency Injection -tapa
        private readonly Northwind1Context db;

        public CustomersController(Northwind1Context dbparametri)
        { 
            db = dbparametri;
        }

        //Hakee kaikki asiakkaat
        [HttpGet]
        public ActionResult GetAllCustomers()
        {
            try
            {
                var asiakkaat = db.Customers.ToList();
                return Ok(asiakkaat);
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + e.InnerException);
            }
        }


        //Hakee yhden asiakkaan pääavaimella
        [HttpGet("{id}")]
        public ActionResult GetOneCustomerById(string id)
        {
            try
            {
                var asiakas = db.Customers.Find(id);
                if (asiakas != null)
                {
                    return Ok(asiakas);
                }
                else
                {
                    //return BadRequest("Asiakasta id:llä " + id + " ei löydy.");//perinteinen tapa liittää muuttuja
                    return NotFound($"Asiakasta id:llä {id} ei löydy."); // string interpolation -tapa
                }
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + e);
            }
        }

        //Uuden lisääminen
        [HttpPost]
        public ActionResult AddNew([FromBody] Customer cust)
        {
            try
            {
                db.Customers.Add(cust);
                db.SaveChanges();
                return Ok($"Lisättiin uusi asiakas {cust.CompanyName} from {cust.City}");
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + e.InnerException);
            }
        }

        //Asiakkaan poistaminen
        [HttpDelete("{id}")]

        public ActionResult Delete(string id)
        {
            try {
                var asiakas = db.Customers.Find(id);

                if (asiakas != null) //Jos id:llä löytyy asiakas
                {
                    db.Customers.Remove(asiakas);
                    db.SaveChanges();
                    return Ok("Asiakas " + asiakas.CompanyName + " poistettiin.");
                }

                return NotFound("Asiakas id:llä " + id + " ei löytynyt.");
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }

        //Asiakkaan muokkaaminen
        [HttpPut("{id}")]

        public ActionResult EditCustomer(string id, [FromBody]Customer customer)
        {
            var asiakas = db.Customers.Find(id);
            if (asiakas != null)
            {
                asiakas.CompanyName = customer.CompanyName;
                asiakas.ContactName = customer.ContactName;
                asiakas.Address = customer.Address;
                asiakas.City= customer.City;
                asiakas.Region = customer.Region;
                asiakas.PostalCode = customer.PostalCode;
                asiakas.Country = customer.Country;
                asiakas.Phone = customer.Phone;
                asiakas.Fax = customer.Fax;

                db.SaveChanges();
                return Ok("Muokattu asiakasta " + asiakas.CompanyName);
            }

            return NotFound("Asiakas ei löytynyt id:llä " + id);
        }

        //Hakee nimen osalla: /api/companyname/hakusana
        [HttpGet("companyname/{cname}")]
        public ActionResult GetByCompanyName(string cname)
        {
            try
            {
                var cust = db.Customers.Where(c => c.CompanyName.Contains(cname));
                return Ok(cust);
                //var cust = from c in db.Customers where c.CompanyName.Contains(cname) select c; <--sama mutta traditional
                //var cust = db.Customers.Where(c => c.CompanyName == cname); <---perfect match kun nimen pitää olla täysin sama

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
