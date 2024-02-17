using Api;
using Api.CompiledModels;
using Api.Models;
using Api.ResponseStructs;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

string conString = "Host=db;Port=5432;Database=rzanc;Username=rzanc;Password=1234;";
builder.Services.AddDbContextPool<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString: conString, db =>
    {
        db.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);

    });

    options.UseModel(AppDbContextModel.Instance);

#if DEBUG
    options.EnableDetailedErrors();
#endif

}, poolSize: 1024);

builder.Services.AddRequestTimeouts(options => options.DefaultPolicy = new RequestTimeoutPolicy { Timeout = TimeSpan.FromSeconds(10)});

var app = builder.Build();

var clientEndpoints = app.MapGroup("/clientes");

clientEndpoints.MapPost("/{id}/transacoes", async(int id, [FromBody] Transaction data, [FromServices] AppDbContext context, CancellationToken token) =>
{
    if (id < 1 || id > 5) return Results.NotFound();

    if(data.Tipo != 'c' && data.Tipo != 'd') return Results.NotFound();
    
    if(string.IsNullOrWhiteSpace(data.Descricao) || data.Descricao.Length > 10) return Results.NotFound();


    var client = await context.Clients.FindAsync([id], cancellationToken: token);
    if(client is null) return Results.NotFound();

    if (data.Tipo == 'd' && (client.Saldo - data.Valor < client.Limite * -1))
        return Results.UnprocessableEntity();

    client.Saldo = data.Tipo switch
    {
        'c' => client.Saldo += data.Valor,
        'd' => client.Saldo -= data.Valor,
        _ => throw new Exception("Unhandled tipo de operacao")
    };

    data.IdCliente = id;
    data.Realizada_em = DateTime.UtcNow.AddHours(-3);
    await context.Transactions.AddAsync(data, token);

    await context.SaveChangesAsync(token);
    return Results.Ok(new TransactionOutput { limite = client.Limite, saldo = client.Saldo }); 
});

clientEndpoints.MapGet("/", async ([FromServices] AppDbContext context, CancellationToken token) =>
{
    return Results.Ok(await context.Clients.AsNoTrackingWithIdentityResolution().ToListAsync(token));
});

clientEndpoints.MapGet("/{id}/extrato", async (int id, [FromServices] AppDbContext context, CancellationToken token) =>
{
    var client = context.GetClientStatementQuery(id);
    if(client is null) return Results.NotFound();

     return Results.Ok(new StatementOutput
     {
         data_extrato = DateTime.UtcNow.AddHours(-3),
         limite = client.Limite,
         total = client.Saldo,
         ultimas_transacoes = client.Transactions 
     });
});

app.Run();

