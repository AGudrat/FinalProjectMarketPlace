﻿using AutoMapper;
using MarketPlace.Catalog.DTOs;
using MarketPlace.Catalog.Models;
using MarketPlace.Catalog.Settings;
using MarketPlace.Shared.Dtos;
using MongoDB.Driver;

namespace MarketPlace.Catalog.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }


        public async Task<Response<List<Category>>> GetAllAsync()
        {
            var categories = await _categoryCollection.Find(categories => true).ToListAsync();

            return Response<List<Category>>.Success(_mapper.Map<List<Category>>(categories), 200);
        }

        public async Task<Response<CategoryDto>> CreateAsync(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _categoryCollection.InsertOneAsync(category);
            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), 200);
        }
        public async Task<Response<CategoryDto>> GetByIdAsync(string id)
        {
            var category = await _categoryCollection.Find<Category>(x => x.Id == id).FirstOrDefaultAsync();
            if (category is null)
                return Response<CategoryDto>.Failed("Category Not Found", 404);
            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), 200);
        }

        public async Task<Response<CategoryDto>> DeleteAsync(string id)
        {
            var category = await GetByIdAsync(id);
            if (category.Data is not null)
            {
                await _categoryCollection.DeleteOneAsync(x => x.Id == id);
            }
            else
                return Response<CategoryDto>.Failed("Category not found", 404);
            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category.Data), 200);
        }

        public async Task<Response<CategoryDto>> UpdateAsync(CategoryUpdateDto categoryUpdateDto)
        {
            var updateCategory = _mapper.Map<Category>(categoryUpdateDto);

            var category = await GetByIdAsync(updateCategory.Id);
            if (category is not null)
            {
                var result = await _categoryCollection.FindOneAndReplaceAsync(x => x.Id == updateCategory.Id, updateCategory);
            }
            else
                return Response<CategoryDto>.Failed("Category not found", 404);
            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category.Data), 200);
        }

    }
}
