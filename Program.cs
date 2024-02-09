
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;

namespace AzureKeyVault
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            string keyVaultURL = builder.Configuration.GetSection("KeyVault")["KeyVaultURL"].ToString();
            string clientId = builder.Configuration.GetSection("KeyVault")["ClientId"].ToString();
            string clientSecret = builder.Configuration.GetSection("KeyVault")["ClientSecret"].ToString();
            string directoryId = builder.Configuration.GetSection("KeyVault")["TenantId"].ToString();
            var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions
            {
                AdditionallyAllowedTenants = { "*" }
            });
            var credentials= new ClientSecretCredential(directoryId, clientId, clientSecret);
            var client = new SecretClient(new Uri(keyVaultURL), credentials);

            builder.Configuration.AddAzureKeyVault(client,new AzureKeyVaultConfigurationOptions());
            var str=client.GetSecret("ConnectionString").Value.Value.ToString();
          

            

            builder.Services.AddControllers();
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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
