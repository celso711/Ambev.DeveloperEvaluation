using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sale
{
    /// <summary>
    /// Contains unit tests for the <see cref="CreateSaleHandler"/> class.
    /// </summary>
    public class CreateSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly CreateSaleHandler _handler;

        public CreateSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new CreateSaleHandler(_saleRepository, _mapper);
        }

        [Fact(DisplayName = "Given valid sale data When creating sale Then returns success response")]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            var command = SaleHandlerTestData.GenerateValidCreateCommand();
            var sale = SaleHandlerTestData.GenerateSale();
            var result = new CreateSaleResult { Id = sale.Id };

            _mapper.Map<Ambev.DeveloperEvaluation.Domain.Entities.Sale>(command).Returns(sale);
            _mapper.Map<CreateSaleResult>(sale).Returns(result);
            _saleRepository.CreateAsync(Arg.Any<Ambev.DeveloperEvaluation.Domain.Entities.Sale>(), Arg.Any<CancellationToken>()).Returns(sale);

            var createSaleResult = await _handler.Handle(command, CancellationToken.None);

            createSaleResult.Should().NotBeNull();
            createSaleResult.Id.Should().Be(sale.Id);
            await _saleRepository.Received(1).CreateAsync(Arg.Any<Ambev.DeveloperEvaluation.Domain.Entities.Sale>(), Arg.Any<CancellationToken>());
        }

        [Fact(DisplayName = "Given invalid sale data When creating sale Then throws validation exception")]
        public async Task Handle_InvalidRequest_ThrowsValidationException()
        {
            var command = new CreateSaleCommand();
            var act = () => _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ValidationException>();
        }
    }
}
