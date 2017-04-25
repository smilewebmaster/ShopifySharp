using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ShopifySharp.Tests
{
    [Trait("Category", "ScriptTag")]
    public class ScriptTag_Tests : IClassFixture<ScriptTag_Tests_Fixture>
    {
        private ScriptTag_Tests_Fixture Fixture { get; }

        public ScriptTag_Tests(ScriptTag_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public async Task Counts_ScriptTags()
        {
            var count = await Fixture.Service.CountAsync();

            Assert.True(count > 0);
        }

        [Fact]
        public async Task Lists_ScriptTags()
        {
            var list = await Fixture.Service.ListAsync();

            Assert.True(list.Count() > 0);
        }

        [Fact]
        public async Task Deletes_ScriptTags()
        {
            var created = await Fixture.Create(true);
            bool threw = false;

            try
            {
                await Fixture.Service.DeleteAsync(created.Id.Value);
            }
            catch (ShopifyException ex)
            {
                Console.WriteLine($"{nameof(Deletes_ScriptTags)} failed. {ex.Message}");

                threw = true;
            }

            Assert.False(threw);
        }

        [Fact]
        public async Task Gets_ScriptTags()
        {
            var obj = await Fixture.Service.GetAsync(Fixture.Created.First().Id.Value);

            Assert.NotNull(obj);
            Assert.True(obj.Id.HasValue);
            Assert.Equal(Fixture.Src, obj.Src);
            Assert.Equal(Fixture.Event, obj.Event);
            Assert.Equal(Fixture.Scope, obj.DisplayScope);
        }

        [Fact]
        public async Task Creates_ScriptTags()
        {
            var obj = await Fixture.Create();

            Assert.NotNull(obj);
            Assert.True(obj.Id.HasValue);
            Assert.Equal(Fixture.Src, obj.Src);
            Assert.Equal(Fixture.Event, obj.Event);
            Assert.Equal(Fixture.Scope, obj.DisplayScope);
        }

        [Fact]
        public async Task Updates_ScriptTags()
        {
            string newValue = "all";
            var original = Fixture.Created.First();
            original.DisplayScope = newValue;
            
            var updated = await Fixture.Service.UpdateAsync(original);

            Assert.Equal(newValue, updated.DisplayScope);   
        }
    }

    public class ScriptTag_Tests_Fixture : IAsyncLifetime
    {
        public ScriptTagService Service => new ScriptTagService(Utils.MyShopifyUrl, Utils.AccessToken);

        public List<ScriptTag> Created { get; } = new List<ScriptTag>();

        public string Event => "onload";

        public string Src => "https://unpkg.com/davenport@2.1.0/bin/browser.js";

        public string Scope => "online_store";

        public async Task InitializeAsync()
        {
            // Create one collection for use with count, list, get, etc. tests.
            await Create();
        }

        public async Task DisposeAsync()
        {
            foreach (var obj in Created)
            {
                try
                {
                    await Service.DeleteAsync(obj.Id.Value);
                }
                catch (ShopifyException ex)
                {
                    if (ex.HttpStatusCode != HttpStatusCode.NotFound)
                    {
                        Console.WriteLine($"Failed to delete created ScriptTag with id {obj.Id.Value}. {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Convenience function for running tests. Creates an object and automatically adds it to the queue for deleting after tests finish.
        /// </summary>
        public async Task<ScriptTag> Create(bool skipAddToCreatedList = false)
        {
            var obj = await Service.CreateAsync(new ScriptTag()
            {
                Event = Event,
                Src = Src,
                DisplayScope = Scope,
            });

            if (! skipAddToCreatedList)
            {
                Created.Add(obj);
            }

            return obj;
        }
    }
}