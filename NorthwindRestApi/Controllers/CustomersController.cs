using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models; 


namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        //Alustetaan tietokantayhteys
        Northwind1Context db = new Northwind1Context();

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
    }
}
