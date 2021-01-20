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

        [Fact]
        public async Task AddReviewToTestGame()
        {
            LoginDTO login = new LoginDTO()
            {
                UserName = "Regular",
                Password = "haslo123"
            };
            var serializedLogin = JsonConvert.SerializeObject(login);
            var loginContent = new StringContent(serializedLogin, Encoding.UTF8, "application/json");
            var result = await _client.PostAsync("/api/v1/users/login", loginContent);
            var responseString = await result.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<TokenDTO>(responseString);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

            ReviewDTO review = new ReviewDTO()
            {
                Title = "Great!",
                Description = "Gr8 game",
                Rating = 5
            };
            var serializedReview = JsonConvert.SerializeObject(review);
            var addReviewHttpContent = new StringContent(serializedReview, Encoding.UTF8, "application/json");

            var addReview = await _client.PostAsync("/api/v1/games/1/reviews", addReviewHttpContent);
            var addReviewResponseString = await addReview.Content.ReadAsStringAsync();
            var reviewId = JsonConvert.DeserializeObject<long>(addReviewResponseString);

            var getReview = await _client.GetAsync("/api/v1/games/1/reviews/" + reviewId);
            var getReviewResponseString = await getReview.Content.ReadAsStringAsync();
            var reviewResult = JsonConvert.DeserializeObject<Review>(getReviewResponseString);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(review.Title, reviewResult.Title);
            Assert.Equal(review.Description, reviewResult.Description);
            Assert.Equal(review.Rating, reviewResult.Rating);
        }
        [Fact]
        public async Task RegisterNewUser()
        {
            UserSignUpDTO newUser = new UserSignUpDTO(){
                UserName = "New",
                Email = "new@new.com",
                Password = "passw"
            };
            var serializedNewUser = JsonConvert.SerializeObject(newUser);
            var signUpContent = new StringContent(serializedNewUser, Encoding.UTF8, "application/json");

            var addUser = await _client.PostAsync("/api/v1/users/signUp", signUpContent);
            var addUserResponseString = await addUser.Content.ReadAsStringAsync();
            var userId = JsonConvert.DeserializeObject<long>(addUserResponseString);

            var getUser = await _client.GetAsync("/api/v1/users/" + userId);
            var getUserResponseString = await getUser.Content.ReadAsStringAsync();
            var userResult = JsonConvert.DeserializeObject<User>(getUserResponseString);

            Assert.Equal(HttpStatusCode.OK, addUser.StatusCode);
            Assert.Equal(newUser.UserName, userResult.UserName);
            Assert.Equal(newUser.Email, userResult.Email);
        }

        [Fact]
        public async Task DeleteGameAsModerator()
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

            var updateGame = await _client.DeleteAsync("/api/v1/games/1");

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(HttpStatusCode.NoContent, updateGame.StatusCode);
        }
    }
}
