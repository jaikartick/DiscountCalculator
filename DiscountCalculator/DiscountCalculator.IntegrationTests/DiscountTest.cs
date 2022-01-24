
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace DiscountCalculator.IntegrationTests
{
    public class DiscountTest:IClassFixture<TestServerFixture>, IDisposable
    {
        public TestServerFixture TestFixture { get; private set; }

        public DiscountTest()
        {
            TestFixture = new TestServerFixture();
            var dbContext = TestFixture.GetDbContext();

            dbContext.Users.AddAsync(new DomainModel.Models.User() { Id = Guid.NewGuid(), Username = "karthick", Email = "karthick@gmail.com", PasswordHash = Encoding.ASCII.GetBytes("Password"), PasswordSalt = Encoding.ASCII.GetBytes("Password") });
        }

        [Fact]
        public async Task GetTotalPriceTest()
        {
            string content = JsonConvert.SerializeObject(new Gold() { Price = 1000, Weight = 10, Discount = 5 });
            var requestContent = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await TestFixture.HttpClient.PostAsync("discount", requestContent);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            var discount = JsonConvert.DeserializeObject<DiscountResult>(result);
            discount.TotalPrice.Should().Be(9500);
        }

        public void Dispose()
        {
            TestFixture.Dispose();
        }
    }
}
