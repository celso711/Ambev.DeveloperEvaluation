using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Sale.TestData;
using AutoMapper;
using FluentAssertions;
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
    /// Contains unit tests for the <see cref="UpdateSaleHandler"/> class.
    /// </summary>
    public class UpdateSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly UpdateSaleHandler _handler;

        public UpdateSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new UpdateSaleHandler(_saleRepository, _mapper);
        }

        [Fact(DisplayName = "Given valid update data When updating sale Then returns updated sale")]
        public async Task Handle_ValidRequest_ReturnsUpdatedSale()
        {
            var command = new UpdateSaleCommand
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid().ToString(),
                CustomerName = "Cliente Inexistente",
                BranchId = Guid.NewGuid().ToString(),
                BranchName = "Filial Teste",
                Items = new List<UpdateSaleItemCommand>
                {
                    new() {
                        ProductId = Guid.NewGuid(),
                        ProductName = "Produto",
                        Quantity = 1,
                        UnitPrice = 10
                    }
                }
            };

            var updatedSale = SaleHandlerTestData.GenerateSale(command.Id);
            var result = new UpdateSaleResult { Id = updatedSale.Id };

            _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(updatedSale);
            _saleRepository.UpdateAsync(command.Id, Arg.Any<Ambev.DeveloperEvaluation.Domain.Entities.Sale>(), Arg.Any<CancellationToken>())
                .Returns(updatedSale);

            _mapper.Map<Ambev.DeveloperEvaluation.Domain.Entities.Sale>(command).Returns(updatedSale);
            _mapper.Map<UpdateSaleResult>(updatedSale).Returns(result);

            var updateResult = await _handler.Handle(command, CancellationToken.None);

            updateResult.Should().NotBeNull();
            updateResult.Id.Should().Be(updatedSale.Id);
            await _saleRepository.Received(1).UpdateAsync(command.Id, Arg.Any<Ambev.DeveloperEvaluation.Domain.Entities.Sale>(), Arg.Any<CancellationToken>());
        }

        [Fact(DisplayName = "Given non-existing sale ID When updating sale Then throws KeyNotFoundException")]
        public async Task Handle_NonExistingSaleId_ThrowsException()
        {
            var command = new UpdateSaleCommand
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid().ToString(),
                CustomerName = "Cliente Inexistente",
                BranchId = Guid.NewGuid().ToString(),
                BranchName = "Filial Teste",
                Items = new List<UpdateSaleItemCommand>
                {
                    new() {
                        ProductId = Guid.NewGuid(),
                        ProductName = "Produto",
                        Quantity = 1,
                        UnitPrice = 10
                    }
                }
            };

            _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((Ambev.DeveloperEvaluation.Domain.Entities.Sale)null);

            var act = () => _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<KeyNotFoundException>();
        }
    }
}
