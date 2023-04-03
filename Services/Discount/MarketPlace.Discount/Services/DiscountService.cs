using Dapper;
using MarketPlace.Shared.Dtos;
using Npgsql;
using System.Data;

namespace MarketPlace.Discount.Services;

public class DiscountService : IDiscountService
{
    private readonly IConfiguration _configuration;
    private readonly IDbConnection _connection;

    public DiscountService(IConfiguration configuration)
    {
        _configuration = configuration;

        _connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
    }

    public async Task<Response<NoContent>> DeleteById(int id)
    {
        var status = await _connection.ExecuteAsync("Delete from discount where id = @Id", id);

        if (status > 0)
            return Response<NoContent>.Success(204);
        return Response<NoContent>.Failed("Discount not found", 404);
    }

    public async Task<Response<List<Models.Discount>>> GetAll()
    {
        var discounts = await _connection.QueryAsync<Models.Discount>("Select * from discount");

        return Response<List<Models.Discount>>.Success(discounts.ToList(), 200);
    }

    public async Task<Response<Models.Discount>> GetByCode(string code, string userId)
    {
        var discount = await _connection.QueryAsync<Models.Discount>("select*from discount where userid=@UserId AND code=@Code", new { UserId = userId, Code = code });

        var hasDiscount = discount.FirstOrDefault();

        if (hasDiscount is null)
            return Response<Models.Discount>.Failed("Discount not found", 404);
        return Response<Models.Discount>.Success(hasDiscount, 200);
    }

    public async Task<Response<Models.Discount>> GetById(int id)
    {
        var discount = (await _connection.QueryAsync<Models.Discount>("Select * from discount where id=@Id", new { id })).SingleOrDefault();
        if (discount is null)
            return Response<Models.Discount>.Failed("Discount Not Found", 404);

        return Response<Models.Discount>.Success(discount, 200);
    }


    public async Task<Response<NoContent>> Save(Models.Discount discount)
    {
        var saveStatus = await _connection.ExecuteAsync("Insert Into discount (userid,rate,code) Values(@UserId, @Rate, @Code)", discount);
        if (saveStatus > 0)
            return Response<NoContent>.Success(204);
        return Response<NoContent>.Failed("Bir hata meydana geldi", 500);
    }

    public async Task<Response<NoContent>> Update(Models.Discount discount)
    {
        var status = await _connection.ExecuteAsync("update discount set userId = @UserId, rate = @Rate, code = @Code where id = @Id", discount);

        if (status > 0)
            return Response<NoContent>.Success(204);
        return Response<NoContent>.Failed("Discount not found", 404);

    }
}
