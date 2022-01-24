using System;
using System.IO;
using System.Net.Http;
using DiscountCalculator.DomainModel.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.PlatformAbstractions;

namespace DiscountCalculator.IntegrationTests
{
    public class TestServerFixture : IDisposable
    {
        private TestServer m_TestServer;
        public HttpClient HttpClient;
        public TestServerFixture()
        {
            var builder = new WebHostBuilder()
                .UseContentRoot(GetContentRootPath())
                   .UseEnvironment("Test")
                   .UseStartup<Startup>();
            m_TestServer = new TestServer(builder);
            HttpClient = m_TestServer.CreateClient();
        }


        private string GetContentRootPath()
        {
            var testProjectPath = PlatformServices.Default.Application.ApplicationBasePath;
            var relativeHostProjectPath = $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}DiscountCalculator";
            return Path.Combine(testProjectPath, relativeHostProjectPath);
        }

        public void Dispose()
        {
            HttpClient.Dispose();
            m_TestServer.Dispose();
        }

        public IAppDbContext GetDbContext()
        {
            var context = m_TestServer.Host.Services.GetService(typeof(AppDbContext)) as AppDbContext;
            context.Database.EnsureCreated();
            return context;
        }
    }
}
