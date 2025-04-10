using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.ListSale;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Sale.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sale;

/// <summary>
/// Contains unit tests for the <see cref="ListSalesHandler"/> class.
/// </summary>
public class ListSalesHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ListSalesHandler _handler;

    public ListSalesHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new ListSalesHandler(_saleRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid ListSaleCommand When handler is called Then returns expected sales list")]
    public async Task Handle_ValidCommand_ReturnsSalesList()
    {
        var command = ListSalesHandlerTestData.GenerateValidCommand();
        var filter = new ListSaleFilter(); 
        var domainSales = ListSalesHandlerTestData.GenerateSales(3);
        var expectedResults = ListSalesHandlerTestData.GenerateGetSaleResults(3);

        _mapper.Map<ListSaleFilter>(command).Returns(filter);
        _saleRepository.ListAllAsync(filter, Arg.Any<CancellationToken>()).Returns(domainSales);
        _mapper.Map<List<GetSaleResult>>(domainSales).Returns(expectedResults);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().HaveCount(3);
    }

    [Fact(DisplayName = "Given invalid ListSaleCommand When handler is called Then throws ValidationException")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        var command = new ListSaleCommand();

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>();
    }
}
