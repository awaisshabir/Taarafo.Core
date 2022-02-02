﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Moq;
using Taarafo.Core.Models.Profiles;
using Taarafo.Core.Models.Profiles.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Profiles
{
    public partial class ProfileServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfProfileIsNullAndLogItAsync()
        {
            // given
            Profile nullProfile = null;

            var nullProfileException =
                new NullProfileException();

            var expectedProfileValidationException
                = new ProfileValidationException(nullProfileException);

            // when
            ValueTask<Profile> addProfileTask =
                this.profileService.AddProfileAsync(nullProfile);

            // then
            await Assert.ThrowsAsync<ProfileValidationException>(() =>
                addProfileTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProfileValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProfileAsync(nullProfile),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}