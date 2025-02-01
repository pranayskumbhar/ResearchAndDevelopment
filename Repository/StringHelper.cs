using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Repository
{

    public class Respond
    {
        public int Type { get; set; }
        public object? Data { get; set; }
    }

    public class StringHelper
    {
        public static Respond ParseString(string input)
        {
            // Check for null or empty string
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            // Unescape the string (to handle escape sequences like \n, \t, etc.)
            input = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(input))!.Trim();

            // Attempt to parse as JSON
            if (input.StartsWith("{") && input.EndsWith("}"))
            {
                try
                {
                    return new Respond()
                    {
                        Data = JObject.Parse(input),
                        Type = 1
                    };
                }
                catch
                {
                    throw new Exception("Invalid JSON Object format.");
                }
            }
            else if (input.StartsWith("[") && input.EndsWith("]"))
            {
                try
                {

                    return new Respond()
                    {
                        Data = JArray.Parse(input),
                        Type = 2
                    };
                }
                catch
                {
                    throw new Exception("Invalid JSON Array format.");
                }
            }

            // Attempt to parse as XML
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(input);

                return new Respond()
                {
                    Data = xmlDocument,
                    Type = 3
                };
            }
            catch
            {
                throw new Exception("Input is not a valid JSON or XML string.");
            }
        }
    }

}
