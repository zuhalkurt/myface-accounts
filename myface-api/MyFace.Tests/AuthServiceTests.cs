using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MyFace.Models.Database;
using MyFace.Repositories;
using MyFace.Services;
using NUnit.Framework;

namespace MyFace.Tests
{
    public class AuthServiceTests
    {
        private AuthService _authService;
        private IUsersRepo _fakeUsersRepo;

        [SetUp]
        public void SetUp()
        {
            _fakeUsersRepo = A.Fake<IUsersRepo>();
            _authService = new AuthService(_fakeUsersRepo);
        }
        
        [Test]
        public void ReturnsTrueIfRequestIsValid()
        {
            var request = A.Fake<HttpRequest>();
            A.CallTo(() => request.Headers).Returns(new HeaderDictionary
            {
                { "Authorization", new [] { "Basic dGVzdC11c2VyOnNlY3JldA==" } }
            });

            A.CallTo(() => _fakeUsersRepo.GetByUsername("test-user"))
                .Returns(A.Dummy<User>());
            
            var isValid = _authService.HasValidAuthorization(request);

            isValid.Should().BeTrue();
        }

        [Test]
        public void ReturnsFalseIfAuthorizationHeaderIsMissing()
        {
            var request = A.Fake<HttpRequest>();
            A.CallTo(() => request.Headers).Returns(new HeaderDictionary());
            
            var isValid = _authService.HasValidAuthorization(request);

            isValid.Should().BeFalse();
        }

        [Test]
        public void ReturnsFalseIfAuthorizationIsNotBasicAuth()
        {
            var request = A.Fake<HttpRequest>();
            A.CallTo(() => request.Headers).Returns(new HeaderDictionary
            {
                { "Authorization", new [] { "Other dGVzdC11c2VyOnNlY3JldA==" } }
            });
            
            var isValid = _authService.HasValidAuthorization(request);

            isValid.Should().BeFalse();
        }

        [Test]
        public void ReturnsFalseIfUserDoesNotExist()
        {
            var request = A.Fake<HttpRequest>();
            A.CallTo(() => request.Headers).Returns(new HeaderDictionary
            {
                { "Authorization", new [] { "Basic dGVzdC11c2VyOnNlY3JldA==" } }
            });

            A.CallTo(() => _fakeUsersRepo.GetByUsername("test-user"))
                .Returns(null);
            
            var isValid = _authService.HasValidAuthorization(request);

            isValid.Should().BeFalse();
        }
    }
}