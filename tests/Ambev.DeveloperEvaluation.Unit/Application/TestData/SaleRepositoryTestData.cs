using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;
using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Unit.Infrastructure.Repositories.TestData;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation for <see cref="Sale"/>
/// entities to ensure consistency and validity across unit tests.
/// </summary>
public static class SaleRepositoryTestData
{
    /// <summary>
    /// Faker configured to generate valid <see cref="Sale"/> instances.
    /// Includes:
    /// - SaleNumber: random code with prefix
    /// - Date: current UTC date
    /// - Items: one or more valid <see cref="SaleItem"/>
    /// </summary>
    private static readonly Faker<Sale> saleFaker = new Faker<Sale>()
        .RuleFor(s => s.Id, f => Guid.NewGuid())
        .RuleFor(s => s.SaleNumber, f => $"SALE-{f.Random.AlphaNumeric(6).ToUpper()}")
        .RuleFor(s => s.Items, f => new List<SaleItem>
        {
            new SaleItem
            {
                ProductName = f.Commerce.ProductName(),
                Quantity = f.Random.Int(1, 10),
                UnitPrice = f.Finance.Amount(5, 100)
            }
        });

    /// <summary>
    /// Generates a valid <see cref="Sale"/> with populated properties
    /// and a single valid item.
    /// </summary>
    /// <returns>A valid <see cref="Sale"/> instance.</returns>
    public static Sale GenerateValidSale()
    {
        return saleFaker.Generate();
    }
}
