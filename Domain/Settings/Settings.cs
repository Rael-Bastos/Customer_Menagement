using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Settings
{
    public static class Settings
    {
        public static string _apiName = "Client.Menagement.API";
        public static string _swaggerVersion = "v1";
        public static string _securityDefinition = "Bearer";
        public static string _securityKey = "Trade@Key@Token@ClientMenagement";
        public static string _culture = "pt-BR";
        public static string _securitySchemeId = "TradeClientAuth";
        public static SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_securityKey));
    }
}
