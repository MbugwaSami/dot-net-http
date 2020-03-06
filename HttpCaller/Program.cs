using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpCaller
{
    public static class Program
    {
        static readonly HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {

            string[] adresses = new string[] { "nairobi", "nakuru", "kericho" };
            string apiUrl = "http://localhost:7071/api/Adress-call";
            Dictionary<string, string[]> postData1 = new Dictionary<string, string[]>()
            {
                { "adresses", adresses },

            };
            var postData = JsonConvert.SerializeObject(postData1);
            var data = new StringContent(postData, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(apiUrl, data);

            var result = await response.Content.ReadAsStringAsync();
            var models = JsonConvert.DeserializeObject<List<LocationModel>>(result);
            DataTable dataTable = models.ToDataTable();
            
            Console.WriteLine(dataTable);


            //var postData = JsonConvert.SerializeObject(postData1);
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
            //request.Method = "POST";
            //request.ContentType = "application/json";
            //request.ContentLength = postData.Length;
            //using (Stream webStream = request.GetRequestStream())
            //using (StreamWriter requestWriter = new StreamWriter(webStream, Encoding.ASCII))
            //{
            //    requestWriter.Write(postData);
            //}
            //try
            //{
            //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //    using (Stream responseStream = response.GetResponseStream())
            //    {
            //        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
            //        DataTable ds = new DataTable();
            //        var pageViewModel = JsonConvert.DeserializeObject<List<LocationModel>>(reader.ReadToEnd());

            //    }
            //}
            //catch (WebException ex)
            //{
            //    WebResponse errorResponse = ex.Response;
            //    using (Stream responseStream = errorResponse.GetResponseStream())
            //    {
            //        StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
            //        String errorText = reader.ReadToEnd();
            //        // log errorText
            //    }
            //    throw;
            //}


        }

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

    }
}
