using E_Commerce_Application.Interfaces.Presistences;
using E_Commerce_Application.Interfaces.Services;
using E_Commerce_Application.Services;
using E_commerce_DataModeling.Context;
using E_commerce_Task.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped< ICustomerRepository, CustomerRepository >();
builder.Services.AddScoped< ICustomerService, CustomerService >();
builder.Services.AddScoped< IProductRepository, ProductRepository >();
builder.Services.AddScoped< IProductService, ProductService >();
builder.Services.AddScoped< IOrderRepository, OrderRepository >();
builder.Services.AddScoped< IOrderService, OrderService >();

builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddDbContext<ApplicationDbContext>(option =>
        option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        );
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
