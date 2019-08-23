using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace WebApiUnidad2.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private static IConfiguration configuration;
        public ValuesController(IConfiguration iconfiguration)
        {
            configuration = iconfiguration;
        }

        

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            try
            {
                DataTable platillos = new DataTable();
                Models.CapaDatos misDatos = new Models.CapaDatos(configuration);
                var grupo = "grupouniacc";
                var headers = Request.Headers;
                Microsoft.Extensions.Primitives.StringValues headerValues;
                headers.TryGetValue("tokenBearer", out headerValues);
                var tokenBearer = headerValues.First();
                headers.TryGetValue("username", out headerValues);
                var username = headerValues.First();
                string tokenUsername = misDatos.ValidateToken(tokenBearer);

                if (tokenUsername.Equals(grupo))
                {
                    using (misDatos = new Models.CapaDatos(configuration))
                    {
                        platillos = await misDatos.getPlatillos();
                    }
                    string JSONString = string.Empty;
                    JSONString = JsonConvert.SerializeObject(platillos);
                    return JSONString;
                }
                else
                {
                    throw new Exception();
                }
                
            }
            catch (Exception)
            {
                return StatusCode(500, "Oooops, Acceso No Autorizado!!!");
                //return Unauthorized();
            }
            
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> Get(int id)
        {
            try
            {
                DataTable platillos = new DataTable();
                Models.CapaDatos misDatos = new Models.CapaDatos(configuration);
                var grupo = "grupouniacc";
                var headers = Request.Headers;
                Microsoft.Extensions.Primitives.StringValues headerValues;
                headers.TryGetValue("tokenBearer", out headerValues);
                var tokenBearer = headerValues.First();
                headers.TryGetValue("username", out headerValues);
                var username = headerValues.First();
                string tokenUsername = misDatos.ValidateToken(tokenBearer);

                if (tokenUsername.Equals(grupo))
                {
                    using (misDatos = new Models.CapaDatos(configuration))
                    {
                        platillos = await misDatos.getPlatilloPorId(id);
                    }
                    string JSONString = string.Empty;
                    JSONString = JsonConvert.SerializeObject(platillos);
                    return JSONString;
                }
                else
                {
                    throw new Exception();
                }
                
            }
            catch (Exception)
            {
                return StatusCode(500, "Oooops, Acceso No Autorizado!!!");
                //return Unauthorized();
            }
           
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
