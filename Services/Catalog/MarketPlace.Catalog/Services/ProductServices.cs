using AutoMapper;
using MarketPlace.Catalog.DTOs;
using MarketPlace.Catalog.Models;
using MarketPlace.Catalog.Settings;
using MarketPlace.Shared.Dtos;
using MarketPlace.Shared.Messages;
using MongoDB.Driver;

namespace MarketPlace.Catalog.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IMongoCollection<Product> _productCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;
        private readonly MassTransit.IPublishEndpoint _publishEndpoint;
        public ProductServices(IMapper mapper, IDatabaseSettings databaseSettings, MassTransit.IPublishEndpoint publishEndpoint)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _productCollection = database.GetCollection<Product>(databaseSettings.ProductCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName); ;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;

        }

        public async Task<Response<List<ProductDto>>> GetAllAsync()
        {
            var products = await _productCollection.Find(product => true).ToListAsync();
            if (products.Any())
                foreach (var cource in products)
                {
                    cource.Category = await _categoryCollection.Find<Category>(x => x.Id == cource.CategoryId).FirstAsync();
                }
            else
                products = new List<Product>();

            return Response<List<ProductDto>>.Success(_mapper.Map<List<ProductDto>>(products), 200);
        }

        public async Task<Response<ProductDto>> GetByIdAsync(string id)
        {
            var product = await _productCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (product is null)
                return Response<ProductDto>.Failed("Cource Not Found", 404);

            product.Category = await _categoryCollection.Find(x => x.Id == product.CategoryId).FirstAsync();

            return Response<ProductDto>.Success(_mapper.Map<ProductDto>(product), 200);
        }

        public async Task<Response<List<ProductDto>>> GetAllByUserId(string userId)
        {
            var products = await _productCollection.Find<Product>(x => x.UserId == userId).ToListAsync();
            if (products.Any())
                foreach (var product in products)
                {
                    product.Category = await _categoryCollection.Find<Category>(x => x.Id == product.CategoryId).FirstAsync();
                }
            else
                products = new List<Product>();

            return Response<List<ProductDto>>.Success(_mapper.Map<List<ProductDto>>(products), 200);
        }

        public async Task<Response<Product>> CreateAsync(ProductCreateDto productCreateDto)
        {
            var newProduct = _mapper.Map<Product>(productCreateDto);
            newProduct.CreatedTime = DateTime.UtcNow.AddHours(4);
            await _productCollection.InsertOneAsync(newProduct);
            return Response<Product>.Success(_mapper.Map<Product>(newProduct), 200);
        }

        public async Task<Response<NoContent>> UpdateAsync(ProductUpdateDto productUpdateDto)
        {
            var updateProduct = _mapper.Map<Product>(productUpdateDto);
            var result = await _productCollection.FindOneAndReplaceAsync(x => x.Id == productUpdateDto.Id, updateProduct);
            if (result is null)
                return Response<NoContent>.Failed("Product Not Found", 404);

            await _publishEndpoint.Publish<ProductNameChangedEvent>(new ProductNameChangedEvent { ProductId = productUpdateDto.Id, UpdatedName = productUpdateDto.Name, UserId = productUpdateDto.UserId });
            return Response<NoContent>.Success(204);
        }

        public async Task<Response<NoContent>> Delete(string id)
        {
            var result = await _productCollection.DeleteOneAsync(x => x.Id == id);
            if (result.DeletedCount > 0)
                return Response<NoContent>.Success(204);
            else
                return Response<NoContent>.Failed("Product Not Found", 404);
        }
    }
}
