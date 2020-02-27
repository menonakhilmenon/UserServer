using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
        public static IActionResult RespondWithBadRequestOnException(this Controller controller,Exception e)
        {
            Console.WriteLine(e.ToString());
            return controller.BadRequest();
        }


    }
    public class JsonTypeHandler : SqlMapper.ITypeHandler
    {
        public void SetValue(IDbDataParameter parameter, object value)
        {
            parameter.Value = JsonConvert.SerializeObject(value);
        }

        public object Parse(Type destinationType, object value)
        {
            return JsonConvert.DeserializeObject(value as string, destinationType);
        }
    }
    public class JsonObjectHandler : SqlMapper.ITypeHandler
    {
        public object Parse(Type destinationType, object value)
        {
            return JObject.Parse(value.ToString());
        }

        public void SetValue(IDbDataParameter parameter, object value)
        {
            throw new NotImplementedException();
        }
    }
}
