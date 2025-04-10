using Ambev.DeveloperEvaluation.Application.Sales.ListSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;
using System;
using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sale.TestData;

/// <summary>
/// Provides test data for <see cref="ListSalesHandler"/> tests.
/// </summary>
public static class ListSalesHandlerTestData
{
    public static ListSaleCommand GenerateValidCommand()
    {
        return new ListSaleCommand
        {
            BranchId = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            StartDate = DateTime.UtcNow.AddDays(-7),
            EndDate = DateTime.UtcNow,
            Page = 1,
            PageSize = 10
        };
    }

    public static List<Ambev.DeveloperEvaluation.Domain.Entities.Sale> GenerateSales(int count)
    {
        var faker = new Faker<Ambev.DeveloperEvaluation.Domain.Entities.Sale>()
            .RuleFor(s => s.Id, f => Guid.NewGuid())
            .RuleFor(s => s.SaleNumber, f => f.Random.AlphaNumeric(6).ToUpper())
            .RuleFor(s => s.SaleDate, f => f.Date.Past())
            .RuleFor(s => s.CustomerName, f => f.Company.CompanyName())
            .RuleFor(s => s.BranchName, f => f.Company.CompanySuffix());

        return faker.Generate(count);
    }

    public static List<GetSaleResult> GenerateGetSaleResults(int count)
    {
        var faker = new Faker<GetSaleResult>()
            .RuleFor(r => r.Id, f => Guid.NewGuid())
            .RuleFor(r => r.SaleNumber, f => f.Random.AlphaNumeric(6).ToUpper());

        return faker.Generate(count);
    }
}
