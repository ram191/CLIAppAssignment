using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using RestApiTraining.Models;

namespace RestApiTraining
{
    class Program
    {
        static int Main(string[] args)
        {
            var app = new CommandLineApplication
            {
                Name = "Todo CLI App",
                Description = "An app for your tasks"
            };

            app.Command("list", cmd =>
            {
                cmd.OnExecuteAsync(async cancellationToken =>
                {
                    var data = await ModifyTasks.Get();
                    var jObj = JsonConvert.DeserializeObject<List<Task>>(data);

                    foreach (var task in jObj)
                    {
                        string done = null;
                        if (task.done)
                        {
                            done = "DONE";
                        }
                        Console.WriteLine($"{task.id}. {task.task} {done}");
                    }
                });
            });

            app.Command("add", cmd =>
            {
                var taskName = cmd.Argument("task", "task to add", multipleValues: false);
                cmd.OnExecuteAsync(async cancellationToken =>
                {
                    string jsondata = @"{""task"":""" + taskName.Value + @""",""done"":false}";

                    var buffer = System.Text.Encoding.UTF8.GetBytes(jsondata);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    HttpClient client = new HttpClient();
                    string database = "http://localhost:5000/tasks";
                    await client.PostAsync(database, byteContent);
                });
            });

            app.Command("update", cmd =>
            {
                var taskNumber = cmd.Argument("task", "task to add", multipleValues: false);
                var taskName = cmd.Argument("task", "task to add", multipleValues: false);
                cmd.OnExecuteAsync(async cancellationToken =>
                {
                    string jsondata = @"{""task"":""" + taskName.Value + @"""}";


                    var buffer = System.Text.Encoding.UTF8.GetBytes(jsondata);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    HttpClient client = new HttpClient();
                    string database = $"http://localhost:5000/tasks/{taskNumber.Value}";
                    await client.PatchAsync(database, byteContent);
                });
            });

            app.Command("del", cmd =>
            {
                var taskNumber = cmd.Argument("task", "task to delete", multipleValues: false);
                cmd.OnExecuteAsync(async cancellationToken =>
                {
                    HttpClient client = new HttpClient();
                    string database = $"http://localhost:5000/tasks/{taskNumber.Value}";
                    await client.DeleteAsync(database);
                });
            });

            app.Command("clear", cmd =>
            {
                cmd.OnExecuteAsync(async cancellationToken =>
                {
                    var prompt = Prompt.GetYesNo("You are about to clear all lists. Are you sure?", false, ConsoleColor.Red);
                    HttpClient client = new HttpClient();

                    if (prompt)
                    {
                        var data = await ModifyTasks.Get();
                        var jObj = JsonConvert.DeserializeObject<List<Task>>(data);
                        List<int> ids = new List<int>();
                        foreach (var task in jObj)
                        {
                            ids.Add(task.id);
                        }
                        foreach (var num in ids)
                        {
                            string database = $"http://localhost:5000/tasks/{num}";
                            await client.DeleteAsync(database);
                        }
                    }
                });
            });

            app.Command("done", cmd =>
            {
                var taskNumber = cmd.Argument("task", "task to delete", multipleValues: false);
                cmd.OnExecuteAsync(async cancellationToken =>
                {
                    string jsondata = @"{""done"":true}";

                    var buffer = System.Text.Encoding.UTF8.GetBytes(jsondata);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    HttpClient client = new HttpClient();
                    string database = $"http://localhost:5000/tasks/{taskNumber.Value}";
                    await client.PatchAsync(database, byteContent);
                });
            });

            app.Command("undone", cmd =>
            {
                var taskNumber = cmd.Argument("task", "task to delete", multipleValues: false);
                cmd.OnExecuteAsync(async cancellationToken =>
                {
                    string jsondata = @"{""done"":false}";

                    var buffer = System.Text.Encoding.UTF8.GetBytes(jsondata);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    HttpClient client = new HttpClient();
                    string database = $"http://localhost:5000/tasks/{taskNumber.Value}";
                    await client.PatchAsync(database, byteContent);
                });
            });

            return app.Execute(args);
        }
    }
}