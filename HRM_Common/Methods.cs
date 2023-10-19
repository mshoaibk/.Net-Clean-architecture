using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Common
{
    public static class Methods
    {
        /// <summary>
        /// This method will decode the base64 encoded string to JSON model of the given type and return
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="encodedKey"></param>
        /// <returns></returns>
        public static T GetDecodedModel<T>(string encodedKey)
        {
            try
            {
                byte[] data = Convert.FromBase64String(encodedKey);
                string decodedString = Encoding.UTF8.GetString(data);
                return JsonConvert.DeserializeObject<T>(decodedString);
            }
            catch(Exception ex)
            {
                return JsonConvert.DeserializeObject<T>(ex.Message);
            }
        }
    }
}
