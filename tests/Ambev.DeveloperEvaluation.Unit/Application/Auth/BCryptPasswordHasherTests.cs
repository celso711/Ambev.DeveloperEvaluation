using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Common.Security;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Auth
{
    public class BCryptPasswordHasherTests
    {
        private readonly BCryptPasswordHasher _hasher;

        public BCryptPasswordHasherTests()
        {
            _hasher = new BCryptPasswordHasher();
        }

        [Fact(DisplayName = "Should generate a valid hash and verify correctly")]
        public void HashPassword_ShouldCreateValidHash_And_VerifyIt()
        {
            // Arrange
            var password = "Secure@123";

            // Act
            var hash = _hasher.HashPassword(password);
            var isValid = _hasher.VerifyPassword(password, hash);

            // Assert
            hash.Should().NotBeNullOrWhiteSpace();
            isValid.Should().BeTrue();
        }

        [Fact(DisplayName = "Should return false when verifying with wrong password")]
        public void VerifyPassword_WithWrongPassword_ShouldReturnFalse()
        {
            // Arrange
            var originalPassword = "Secure@123";
            var wrongPassword = "WrongPass123";
            var hash = _hasher.HashPassword(originalPassword);

            // Act
            var isValid = _hasher.VerifyPassword(wrongPassword, hash);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact(DisplayName = "Should generate different hashes for the same password")]
        public void HashPassword_SamePassword_ShouldProduceDifferentHashes()
        {
            // Arrange
            var password = "Secure@123";

            // Act
            var hash1 = _hasher.HashPassword(password);
            var hash2 = _hasher.HashPassword(password);

            // Assert
            hash1.Should().NotBe(hash2); // Salting ensures uniqueness
        }
    }
}
