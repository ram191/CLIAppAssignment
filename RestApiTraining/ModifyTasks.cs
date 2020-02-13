using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RestApiTraining
{
    public class ModifyTasks
    {
        public static HttpClient client = new HttpClient();
        public static string database = "http://localhost:5000/tasks";

        public static async Task<string> Get()
        {
            return await client.GetStringAsync(database);
        }

        public static async void Delete()
        {
            await client.DeleteAsync(database);
        }

        public static void Post(HttpContent data)
        {
            client.PostAsync(database, data);
        }

        public static async void Put(HttpContent data)
        {
            await client.PostAsync(database, data);
        }

        public static async void Patch(HttpContent data)
        {
            await client.PostAsync(database, data);
        }
    }
}
