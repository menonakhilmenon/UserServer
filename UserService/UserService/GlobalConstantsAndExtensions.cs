using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService
{
    public static class GlobalConstantsAndExtensions
    {
        public const string CERTIFICATE_AUTHENTICATION_POLICY = "CertificateAuthPolicy";
        public static DynamicParameters AddParameter(this DynamicParameters parameter,string name,object data) 
        {
            parameter.Add(name, data);
            return parameter;
        }
    }
}
