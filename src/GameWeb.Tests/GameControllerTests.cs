using System;
using Xunit;
using GameWeb;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using System.Collections.Generic;
using GameWeb.Models.Entities;
using Newtonsoft.Json;
using GameWeb.Models.DTO;
using System.Text;
using System.Net.Http.Headers;

namespace GameWeb.Tests
{
    public class GameControllerTests
    : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public GameControllerTests(
            CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
        }

        [Fact]
        public async Task GetGameByGenreTest()
        {
            var result = await _client.GetAsync("/api/v1/genres");
            var responseString = await result.Content.ReadAsStringAsync();
            var genres = JsonConvert.DeserializeObject<List<GameGenre>>(responseString);

            var genreId = genres.Find(g => g.Name.Equals("test")).GameGenreId;
            var gamesResult = await _client.GetAsync("/api/v1/games/genres/"+genreId);

            var gamesResponseString = await gamesResult.Content.ReadAsStringAsync();
            var games = JsonConvert.DeserializeObject<List<Game>>(gamesResponseString);

            var game = games.Find(g => g.Name.Equals("test"));

            Assert.Equal(HttpStatusCode.OK, gamesResult.StatusCode);
            Assert.Equal("test", game.Name);
            Assert.Equal("test", game.Developer.Name);
            Assert.Equal("test", game.Genre.Name);
        }

        [Fact]
        public async Task AddGameAsModerator()
        {
            LoginDTO login = new LoginDTO()
            {
                UserName = "Moderator",
                Password = "haslo123"
            };
            var serializedLogin = JsonConvert.SerializeObject(login);
            var loginContent = new StringContent(serializedLogin, Encoding.UTF8, "application/json");
            var result = await _client.PostAsync("/api/v1/users/login", loginContent);
            var responseString = await result.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<TokenDTO>(responseString);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
            //_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.Token);

            GameDTO newGame = new GameDTO()
            {
                Name = "game1",
                ReleaseDate = DateTime.Now,
                GenreId = 1,
                DeveloperId = 1
            };
            var serializedGame = JsonConvert.SerializeObject(newGame);
            var addGameHttpContent = new StringContent(serializedGame, Encoding.UTF8, "application/json");

            var addGameResult = await _client.PostAsync("/api/v1/games", addGameHttpContent);
            var addGameResponseString = await addGameResult.Content.ReadAsStringAsync();
            var newGameId = JsonConvert.DeserializeObject<long>(addGameResponseString);

            var getGameResult = await _client.GetAsync("/api/v1/games/" + newGameId);
            var getGameResponseString = await getGameResult.Content.ReadAsStringAsync();
            var gameResult = JsonConvert.DeserializeObject<Game>(getGameResponseString);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(newGame.Name, gameResult.Name);
            Assert.Equal(newGame.ReleaseDate, gameResult.ReleaseDate);
            Assert.Equal("test", gameResult.Genre.Name);
            Assert.Equal("test", gameResult.Developer.Name);
        }
    }
}
