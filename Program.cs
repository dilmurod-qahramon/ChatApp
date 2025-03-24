using ChatApp.DataAccess;
using Microsoft.EntityFrameworkCore;
using ChatApp.Repositories.interfaces;
using ChatApp.Repositories;
using ChatApp.Services.interfaces;
using ChatApp.Services;
using ChatApp.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ChatDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddHttpClient<IUserService, UserService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    options.ClientTimeoutInterval = TimeSpan.FromMinutes(5);
    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:4200")  
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); 
    });
});
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
    var seeder = new DataSeeder(context);
    seeder.SeedData();
}
app.UseRouting();
app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapHub<ChatHub>("/chatHub");
app.MapControllers();
app.Run();
