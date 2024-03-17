using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;

namespace ODataStorage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var modelBuilder = new ODataConventionModelBuilder();
            var customerSet = modelBuilder.EntitySet<Customer>("Customers");
            var customerType = customerSet.EntityType;
            customerType.HasKey(e => new { e.PartitionKey, e.RowKey });

            builder.Services.AddControllers().AddOData(
                options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null).AddRouteComponents(
                    "odata",
                    modelBuilder.GetEdmModel()));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.UseODataRouteDebug();

            app.Run();
        }
    }
}