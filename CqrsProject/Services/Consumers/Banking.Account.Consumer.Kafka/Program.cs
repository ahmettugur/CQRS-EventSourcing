using Banking.Account.Consumer.Kafka;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddServices(builder.Configuration);

var app = builder.Build();
app.ConfigureApplication();

app.Run();