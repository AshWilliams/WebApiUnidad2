using WebApiUnidad2.Utilidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace WebApiUnidad2.Models
{
    public class CapaDatos : IDisposable
    {
        private static IConfiguration configuration;
        /*
         * HMACSHA256 hmac = new HMACSHA256();
         * string Secret = Convert.ToBase64String(hmac.Key);
         */
        private static string Secret = "qW/8zPiKxQenLqd3ULJBVkjIQ/JoWbdEa83+CkmEQZ4PeSTRKoSw/zyvKesGXSQT9JfXox/SMwJBDLUeoz44FA==";

        #region "Dispose"
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                GC.ReRegisterForFinalize(this);
            }

        }

        /// <summary>
        /// Realiza tareas definidas por la aplicación asociadas a la liberación o al restablecimiento de recursos no administrados.
        /// </summary>
        public void Dispose()
        {
            //  Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        public CapaDatos(IConfiguration iconfiguration)
        {
            configuration = iconfiguration;
        }
        public async Task<DataTable> getIngredientes()
        {
            try
            {
                DataTable dt = new DataTable();
                var qry = "select * from Ingredientes";
                Dictionary<string, string> pars = new Dictionary<string, string>();
//pars.Add("@StatId", statid);
                using (var db = new SQLServer(configuration))
                {
                    dt = await db.getQueryResultAsync(qry, pars, "text");
                }
                return dt;
            }
            catch (Exception)
            {

                throw;
            }

        }


        public async Task<DataTable> getPlatillos()
        {
            try
            {
                DataTable dt = new DataTable();
                var qry = "select * from Platillos";
                Dictionary<string, string> pars = new Dictionary<string, string>();
                //pars.Add("@StatId", statid);
                using (var db = new SQLServer(configuration))
                {
                    dt = await db.getQueryResultAsync(qry, pars, "text");
                }
                return dt;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<DataTable> getPlatilloPorId(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                var qry = "select * from Platillos where idPlatillo = @id";
                Dictionary<string, string> pars = new Dictionary<string, string>();
                pars.Add("@id", id.ToString());
                using (var db = new SQLServer(configuration))
                {
                    dt = await db.getQueryResultAsync(qry, pars, "text");
                }
                return dt;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<int> savePedido(Pedido item)
        {
            try
            {
                DataTable dt = new DataTable();
                var qry = @"insert into Pedido(idPlatillo,nombreCompleto,direccion,comuna,email,telefono,idTipoPago)
                            values(@idPlatillo,@nombreCompleto,@direccion,@comuna,@email,@telefono,@idTipoPago)";
                Dictionary<string, string> pars = new Dictionary<string, string>();
                pars.Add("@idPlatillo", item.idPlatillo.ToString());
                pars.Add("@nombreCompleto",item.nombreCompleto);
                pars.Add("@direccion",item.direccion);
                pars.Add("@comuna",item.comuna);
                pars.Add("@email",item.email);
                pars.Add("@telefono",item.telefono);
                pars.Add("@idTipoPago",item.idTipoPago.ToString());
                using (var db = new SQLServer(configuration))
                {
                    dt = await db.getQueryResultAsync(qry, pars, "text");
                }
          
                return 1;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }

        }

        public async Task<bool> updateStock(string ingredientes)
        {
            try
            {
                DataTable dt = new DataTable();
                string[] idIngredientes = ingredientes.Split(',');
                foreach (string idIngrediente in idIngredientes)
                {
                    var qry = "update Ingredientes set cantidad = cantidad - 1 where idIngrediente = @idIngrediente";
                    Dictionary<string, string> pars = new Dictionary<string, string>();
                    pars.Add("@idIngrediente", idIngrediente);
                    using (var db = new SQLServer(configuration))
                    {
                        dt = await db.getQueryResultAsync(qry, pars, "text");
                    }
                }                
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }

        }

        public string GenerateToken(string username)
        {
            byte[] key = Convert.FromBase64String(Secret);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                      new Claim(ClaimTypes.Name, username)}),
                Expires = DateTime.UtcNow.AddDays(120),
                Issuer = "Integración 3",
                SigningCredentials = new SigningCredentials(securityKey,
                SecurityAlgorithms.HmacSha256Signature)
            };


            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }

        public string ValidateToken(string token)
        {
            string username = null;
            ClaimsPrincipal principal = GetPrincipal(token);
            if (principal == null)
                return null;
            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }
            Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim.Value;
            return username;
        }

        public ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                    return null;
                byte[] key = Convert.FromBase64String(Secret);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
                      parameters, out securityToken);
                return principal;
            }
            catch (Exception e)
            {
                return null;
            }
        }


    }
}