using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models;

namespace NorthwindRestApi.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly Northwind1Context db;

        public EmployeesController(Northwind1Context dbparametri)
        {
            db = dbparametri;
        }


        //Hakee kaikki työntekijät
        [HttpGet]
        public ActionResult GetAllEmployees()
        {
            try
            {
                var tyontekijat = db.Employees.ToList();
                return Ok(tyontekijat);
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + e.InnerException);
            }
        }


        //Hakee yhden työntekijän pääavaimella
        [HttpGet("{id}")]
        public ActionResult GetOneEmployeeById(int id)
        {
            try
            {
                var tyontekija = db.Employees.Find(id);
                if (tyontekija != null)
                {
                    return Ok(tyontekija);
                }
                else
                {
                    return NotFound($"Työntekijää id:llä {id} ei löydy."); // string interpolation -tapa
                }
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + e);
            }
        }


        //Hakee nimen osalla: 
        [HttpGet("employeename/{ename}")]
        public ActionResult GetByEmployeeName(string ename)
        {
            try
            {
                var employ = db.Employees.Where(e => e.FirstName.Contains(ename));
                return Ok(employ);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Hakee työntekijää kaupungin nimellä
        [HttpGet("hometown/{city}")]
        public ActionResult GetEmployeesByCity(string city)
        {
            try
            {
                var tyontekija = db.Employees
                    .Where(e => e.City != null && e.City.Contains(city))
                    .ToList();

                if (tyontekija.Count == 0)
                    return NotFound($"Työntekijöitä ei löytynyt kaupungista '{city}'");

                return Ok(tyontekija);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Uuden lisääminen
        [HttpPost]
        public ActionResult AddNewEmployee([FromBody] Employee empl)
        {
            try
            {
                db.Employees.Add(empl);
                db.SaveChanges();
                return Ok($"Lisättiin uusi työntekijä {empl.FirstName}  {empl.LastName}");
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + e.InnerException);
            }
        }

        // Työntekijän muokkaaminen
        [HttpPut("{id}")]
        public ActionResult EditEmployee(int id, [FromBody] Employee employee)
        {
            var tyontekija = db.Employees.Find(id);
            if (tyontekija != null)
            {
                db.Entry(tyontekija).CurrentValues.SetValues(employee);
                db.SaveChanges();
                return Ok($"Muokattu työntekijä {employee.FirstName} {employee.LastName}");
            }

            return NotFound("Työntekijä ei löytynyt id:llä " + id);
        }

        //Työntekijän poistaminen
        [HttpDelete("{id}")]

        public ActionResult Delete(int id)
        {
            try
            {
                var tyontekija = db.Employees.Find(id);

                if (tyontekija != null) //Jos id:llä löytyy työntekijä
                {
                    db.Employees.Remove(tyontekija);
                    db.SaveChanges();
                    return Ok($"Työntekijä {tyontekija.FirstName} {tyontekija.LastName} poistettiin");
                }

                return NotFound("Työntekijä id:llä " + id + " ei löytynyt.");
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }
    }
}
