using MinimalApiSample.Components;
using MinimalApiSample.RouterClasses;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();


builder.Services.AddScoped<RouterBase, BookRouter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();


//*************************************
// Add Routes from all "Router Classes" folder 
//*************************************
using (var scope = app.Services.CreateScope())
{
    // Instance of services where you build all RouterBase classes
    var services = scope.ServiceProvider.GetServices<RouterBase>();

    // Loop through each RouterBase class
    foreach (var item in services)
    {
        // Invoke the AddRoutes() method for each RouterBase class
        item.AddRoutes(app);
    }

    // Make sure this is called within the application scope
    app.Run();
}