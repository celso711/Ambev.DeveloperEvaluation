using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Sale.TestData;
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
    /// Contains unit tests for the <see cref="DeleteSaleHandler"/> class.
    /// </summary>
    public class DeleteSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly DeleteSaleHandler _handler;

        public DeleteSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _handler = new DeleteSaleHandler(_saleRepository);
        }

        [Fact(DisplayName = "Given existing sale ID When deleting sale Then returns success")]
        public async Task Handle_ExistingSaleId_DeletesSuccessfully()
        {
            var saleId = Guid.NewGuid();
            var command = new DeleteSaleCommand(saleId);
            var sale = SaleHandlerTestData.GenerateSale(saleId);

            await _saleRepository.CreateAsync(sale, Arg.Any<CancellationToken>());

            _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns(sale);
            _saleRepository.DeleteAsync(saleId, Arg.Any<CancellationToken>()).Returns(true);

            await _handler.Handle(command, CancellationToken.None);

            _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns(new Ambev.DeveloperEvaluation.Domain.Entities.Sale { Id = saleId });
            //await _saleRepository.Received(1).GetByIdAsync(saleId, Arg.Any<CancellationToken>());
            await _saleRepository.Received(1).DeleteAsync(saleId, Arg.Any<CancellationToken>());
        }

        [Fact(DisplayName = "Given non-existing sale ID When deleting sale Then throws KeyNotFoundException")]
        public async Task Handle_NonExistingSaleId_ThrowsException()
        {
            var command = new DeleteSaleCommand(Guid.NewGuid());
            _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
                .Returns((Ambev.DeveloperEvaluation.Domain.Entities.Sale)null);

            var act = () => _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<KeyNotFoundException>();
        }
    }
}
