using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Net;
using System.Web.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};
string connectionString = "server=localhost;port=3306;database=test_db;uid=root;password=Mysql@98153;";

app.MapGet("/country", GetCountries)
    .WithName("GetCountries");

// Method to handle the GET request
Dictionary<string, string>[] GetCountries()
{
    List<Dictionary<string, string>> countriesList = new List<Dictionary<string, string>>();

    using (MySqlConnection connection = new MySqlConnection(connectionString))
    {
        connection.Open();

        string query = "SELECT country_name, capital_city FROM country_table";

        using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Dictionary<string, string> country = new Dictionary<string, string>();
                    country["country_name"] = reader["country_name"].ToString();
                    country["capital_city"] = reader["capital_city"].ToString();
                    countriesList.Add(country);
                }
            }
        }
    }

    Console.WriteLine(countriesList.Count);

    return countriesList.ToArray();
}


app.Run();

