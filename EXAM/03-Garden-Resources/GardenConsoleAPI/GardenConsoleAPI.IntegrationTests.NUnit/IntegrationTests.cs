﻿using GardenConsoleAPI.Business;
using GardenConsoleAPI.Business.Contracts;
using GardenConsoleAPI.Data.Models;
using GardenConsoleAPI.DataAccess;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GardenConsoleAPI.IntegrationTests.NUnit
{
    public class IntegrationTests
    {
        private TestPlantsDbContext dbContext;
        private IPlantsManager plantsManager;

        [SetUp]
        public void SetUp()
        {
            this.dbContext = new TestPlantsDbContext();
            this.plantsManager = new PlantsManager(new PlantsRepository(this.dbContext));
        }


        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }


        //positive test
        [Test]
        public async Task AddPlantAsync_ShouldAddNewPlant()
        {
            // Arrange
            var newPlant = new Plant
            {
                Name = "Bulgarian Rose",
                PlantType = "Flower",
                FoodType = "Edible Flower",
                Quantity = 800,
                IsEdible = true,
                CatalogNumber = "01HP01PRFHHW",
            };

            // Act
            await plantsManager.AddAsync(newPlant);

            // Assert
            var plantInDb = await dbContext.Plants.FirstOrDefaultAsync(p => p.CatalogNumber == newPlant.CatalogNumber);
            Assert.That(plantInDb, Is.Not.Null);  
            Assert.That(plantInDb.Name, Is.EqualTo(newPlant.Name));
            Assert.That(plantInDb.PlantType, Is.EqualTo(newPlant.PlantType));
            Assert.That(plantInDb.FoodType, Is.EqualTo(newPlant.FoodType));
            Assert.That(plantInDb.Quantity, Is.EqualTo(newPlant.Quantity));
            Assert.That(plantInDb.IsEdible, Is.EqualTo(newPlant.IsEdible));
        
        }

        //Negative test
        [Test]
        public async Task AddPlantAsync_TryToAddPlantWithInvalidCredentials_ShouldThrowException()
        {
            // Arrange
            var newPlant = new Plant
            {
                Name = null,
                PlantType = "Flower",
                FoodType = "",
                Quantity = 800,
                IsEdible = true,
                CatalogNumber = "01HP01PRFH",
            };
            // Act and Assert

            var exception =  Assert.ThrowsAsync<ValidationException>(async () => await plantsManager.AddAsync(newPlant));
            var plantInDb = await dbContext.Plants.FirstOrDefaultAsync(p => p.CatalogNumber == newPlant.CatalogNumber);
            Assert.IsNull(plantInDb);
            Assert.That(exception.Message, Is.EqualTo("Invalid plant!"));



        }

        [Test]
        public async Task DeletePlantAsync_WithValidCatalogNumber_ShouldRemovePlantFromDb()
        {
            // Arrange
            var newPlant = new Plant
            {
                Name = "Bulgarian Rose",
                PlantType = "Flower",
                FoodType = "Edible Flower",
                Quantity = 800,
                IsEdible = true,
                CatalogNumber = "01HP01PRFHHW",
            };

            await plantsManager.AddAsync(newPlant);

            // Act
            await plantsManager.DeleteAsync(newPlant.CatalogNumber);

            // Assert
            var plantInDb = await dbContext.Plants.FirstOrDefaultAsync(p => p.CatalogNumber == newPlant.CatalogNumber);
            Assert.IsNull(plantInDb);
        }

        [TestCase("")]
        [TestCase("     ")]
        [TestCase(null)]
        public async Task DeletePlantAsync_TryToDeleteWithNullOrWhiteSpaceCatalogNumber_ShouldThrowException(string invalidCatalogNumber)
        {

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await plantsManager.DeleteAsync(invalidCatalogNumber));
            Assert.That(exception.Message, Is.EqualTo("Catalog number cannot be empty."));

        }

        [Test]
        public async Task GetAllAsync_WhenPlantsExist_ShouldReturnAllPlants()
        {
            // Arrange
            var newPlant = new Plant
            {
                Name = "Bulgarian Rose",
                PlantType = "Flower",
                FoodType = "Edible Flower",
                Quantity = 800,
                IsEdible = true,
                CatalogNumber = "01HP01PRFHHW",
            };

            var newPlant2 = new Plant
            {
                Name = "Cucumber",
                PlantType = "Creepers",
                FoodType = "Vegetable",
                Quantity = 234,
                IsEdible = true,
                CatalogNumber = "01HP01PRFDBK",
            };
            await plantsManager.AddAsync(newPlant);
            await plantsManager.AddAsync(newPlant2);
            // Act
           var result =  await plantsManager.GetAllAsync();
            // Assert
            Assert.That(result.Count, Is.EqualTo(2));

            var firstPlantInResult = result.First();
            Assert.That(firstPlantInResult.Name, Is.EqualTo(newPlant.Name));
            Assert.That(firstPlantInResult.PlantType, Is.EqualTo(newPlant.PlantType));
            Assert.That(firstPlantInResult.FoodType, Is.EqualTo(newPlant.FoodType));
            Assert.That(firstPlantInResult.Quantity, Is.EqualTo(newPlant.Quantity));
            Assert.That(firstPlantInResult.IsEdible, Is.EqualTo(newPlant.IsEdible));
            Assert.That(firstPlantInResult.CatalogNumber, Is.EqualTo(newPlant.CatalogNumber));

            var secondPlantInResult = result.ElementAt(1);
            Assert.That(secondPlantInResult.Name, Is.EqualTo(newPlant2.Name));
            Assert.That(secondPlantInResult.PlantType, Is.EqualTo(newPlant2.PlantType));
            Assert.That(secondPlantInResult.FoodType, Is.EqualTo(newPlant2.FoodType));
            Assert.That(secondPlantInResult.Quantity, Is.EqualTo(newPlant2.Quantity));
            Assert.That(secondPlantInResult.IsEdible, Is.EqualTo(newPlant2.IsEdible));
            Assert.That(secondPlantInResult.CatalogNumber, Is.EqualTo(newPlant2.CatalogNumber));
        }

        [Test]
        public async Task GetAllAsync_WhenNoPlantsExist_ShouldThrowKeyNotFoundException()
        {

            // Act & Assert 
            var exception = Assert.ThrowsAsync<KeyNotFoundException>(async () => await plantsManager.GetAllAsync());
            Assert.That(exception.Message, Is.EqualTo("No plant found."));

            
        }

        [Test]
        public async Task SearchByFoodTypeAsync_WithExistingFoodType_ShouldReturnMatchingPlants()
        {
            // Arrange
            var newPlant = new Plant
            {
                Name = "Bulgarian Rose",
                PlantType = "Flower",
                FoodType = "Edible Flower",
                Quantity = 800,
                IsEdible = true,
                CatalogNumber = "01HP01PRFHHW",
            };

            var newPlant2 = new Plant
            {
                Name = "Cucumber",
                PlantType = "Creepers",
                FoodType = "Vegetable",
                Quantity = 234,
                IsEdible = true,
                CatalogNumber = "01HP01PRFDBK",
            };
            await plantsManager.AddAsync(newPlant);
            await plantsManager.AddAsync(newPlant2);
            // Act
            var result = await plantsManager.SearchByFoodTypeAsync(newPlant2.FoodType);
            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
            var matchingPlant = result.FirstOrDefault();

            Assert.That(matchingPlant.Name, Is.EqualTo(newPlant2.Name));
            Assert.That(matchingPlant.PlantType, Is.EqualTo(newPlant2.PlantType));
            Assert.That(matchingPlant.FoodType, Is.EqualTo(newPlant2.FoodType));
            Assert.That(matchingPlant.Quantity, Is.EqualTo(newPlant2.Quantity));
            Assert.That(matchingPlant.IsEdible, Is.EqualTo(newPlant2.IsEdible));
            Assert.That(matchingPlant.CatalogNumber, Is.EqualTo(newPlant2.CatalogNumber));

        }

        [Test]
        public async Task SearchByFoodTypeAsync_WithNonExistingFoodType_ShouldThrowKeyNotFoundException()
        {
            // Act & Assert
            var exception = Assert.ThrowsAsync<KeyNotFoundException>( async () => await plantsManager.SearchByFoodTypeAsync("Fruit"));

            Assert.That(exception.Message, Is.EqualTo("No plant found with the given food type."));
        }

        // ДОПЪЛНИТЕЛ ТЕСТ 
        [TestCase("")]
        [TestCase("     ")]
        [TestCase(null)]
        public async Task SearchByFoodTypeAsync_WithNullorWhiteSpaceFoodType_ShouldThrowArgumentException(string invalidFoodType)
        {
            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await plantsManager.SearchByFoodTypeAsync(invalidFoodType));

            Assert.That(exception.Message, Is.EqualTo("Food type cannot be empty."));
        }

        [Test]
        public async Task GetSpecificAsync_WithValidCatalogNumber_ShouldReturnPlant()
        {
            // Arrange
            var newPlant = new Plant
            {
                Name = "Bulgarian Rose",
                PlantType = "Flower",
                FoodType = "Edible Flower",
                Quantity = 800,
                IsEdible = true,
                CatalogNumber = "01HP01PRFHHW",
            };

            var newPlant2 = new Plant
            {
                Name = "Cucumber",
                PlantType = "Creepers",
                FoodType = "Vegetable",
                Quantity = 234,
                IsEdible = true,
                CatalogNumber = "01HP01PRFDBK",
            };
            await plantsManager.AddAsync(newPlant);
            await plantsManager.AddAsync(newPlant2);
            // Act
            var result = await plantsManager.GetSpecificAsync(newPlant2.CatalogNumber);
            // Assert
            Assert.NotNull(result);
            Assert.That(result.Name, Is.EqualTo(newPlant2.Name));
            Assert.That(result.PlantType, Is.EqualTo(newPlant2.PlantType));
            Assert.That(result.FoodType, Is.EqualTo(newPlant2.FoodType));
            Assert.That(result.Quantity, Is.EqualTo(newPlant2.Quantity));
            Assert.That(result.IsEdible, Is.EqualTo(newPlant2.IsEdible));
            Assert.That(result.CatalogNumber, Is.EqualTo(newPlant2.CatalogNumber));
        }

        [Test]
        public async Task GetSpecificAsync_WithInvalidCatalogNumber_ShouldThrowKeyNotFoundException()
        {

            // Act & Assert
            var exception = Assert.ThrowsAsync<KeyNotFoundException>(async () => await plantsManager.GetSpecificAsync("123456"));
            Assert.That(exception.Message, Is.EqualTo("No plant found with catalog number: 123456"));
        }

        // ДОПЪЛНИТЕЛ ТЕСТ 
        [TestCase("")]
        [TestCase("     ")]
        [TestCase(null)]
        public async Task GetSpecificAsync_WithNullorWhiteSpaceCatalogNumber_ShouldThrowArgumentException(string invalidCatalogNumber)
        {
            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await plantsManager.GetSpecificAsync(invalidCatalogNumber));

            Assert.That(exception.Message, Is.EqualTo("Catalog number cannot be empty."));
        }
        [Test]
        public async Task UpdateAsync_WithValidPlant_ShouldUpdatePlant()
        {
            // Arrange
            var newPlant = new Plant
            {
                Name = "Bulgarian Rose",
                PlantType = "Flower",
                FoodType = "Edible Flower",
                Quantity = 800,
                IsEdible = true,
                CatalogNumber = "01HP01PRFHHW",
            };

            await plantsManager.AddAsync(newPlant);
            var updatedPlant = newPlant;
            updatedPlant.Name = "Pink Rose";
            // Act
            await plantsManager.UpdateAsync(updatedPlant);
            // Assert
           var plantInDb = await dbContext.Plants.FirstOrDefaultAsync(p => p.CatalogNumber == newPlant.CatalogNumber);
            Assert.NotNull(plantInDb);
            Assert.That(plantInDb.Name, Is.EqualTo(updatedPlant.Name));
            Assert.That(plantInDb.PlantType, Is.EqualTo(updatedPlant.PlantType));
            Assert.That(plantInDb.FoodType, Is.EqualTo(updatedPlant.FoodType));
            Assert.That(plantInDb.Quantity, Is.EqualTo(updatedPlant.Quantity));
            Assert.That(plantInDb.IsEdible, Is.EqualTo(updatedPlant.IsEdible));
        }

        [Test]
        public async Task UpdateAsync_WithInvalidPlant_ShouldThrowValidationException()
        {

            // Act & Assert 
            var exception = Assert.ThrowsAsync<ValidationException>(async () => await plantsManager.UpdateAsync(new Plant()));
            Assert.That(exception.Message, Is.EqualTo("Invalid plant!"));
        }
    }
}