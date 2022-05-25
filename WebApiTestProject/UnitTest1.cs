using BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SquaresAPI.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiTestProject
{
    [TestClass]
    public class UnitTest1
    {
        private Uri baseaddress = new System.Uri("https://localhost:44728/");
        private HttpClient httpclient;
        private TokenResponse Token;
        public UnitTest1()
        {
            var application = new WebApplicationFactory<Program>()
           .WithWebHostBuilder(builder => { });
            httpclient = application.CreateClient();
            httpclient.BaseAddress = baseaddress;
        }
        private async Task GenerateAccessToken()
        {
            var cred = new { username = "admin", password = "admin" };
            var data = new StringContent(JsonConvert.SerializeObject(cred), Encoding.UTF8, "application/json");
            httpclient.DefaultRequestHeaders.Accept.Clear();
            httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response=  await httpclient.PostAsync("/token", data);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Token = JsonConvert.DeserializeObject<TokenResponse>(content);
            }           
        }
        private async Task GenerateRefreshToken()
        {
            var cred = new TokenResponse(){ username = Token.username, token = Token.token, refresh_token = Token.refresh_token };
            var data = new StringContent(JsonConvert.SerializeObject(cred), Encoding.UTF8, "application/json");
            httpclient.DefaultRequestHeaders.Accept.Clear();
            httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await httpclient.PostAsync("/refreshtoken", data);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Token = JsonConvert.DeserializeObject<TokenResponse>(content);
            }            
        }

        private async Task InitializeToken()
        {
            if (Token == null)
            {
                await GenerateAccessToken();
            }
            else if (Token.expiration <= DateTime.Now)
            {
                await GenerateRefreshToken();
            }            
        }
        [TestMethod]
        public async Task TestAddPoint()
        {
            await InitializeToken();
            var point = new PointModel(-19200, 19200 );
            var data = new StringContent(JsonConvert.SerializeObject(point), Encoding.UTF8, "application/json");
            httpclient.DefaultRequestHeaders.Accept.Clear();
            httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token.token);
            var response = await httpclient.PostAsync("Point/Add", data);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }
        [TestMethod]
        public async Task TestAddList()
        {
            await InitializeToken();
            var point = new PointModel[] {
                new PointModel(){ X = 1, Y = 1 },
                new PointModel(){ X = 1, Y = -1 },
                new PointModel(){ X = -1, Y = -1 },
                new PointModel(){ X = -1, Y = 1 },
                new PointModel(){ X = 1, Y = 3 },
                new PointModel(){ X = 3, Y = 3 },
                new PointModel(){ X = 3, Y = 1 },
                new PointModel(){ X = 10, Y = 10 },
            };
            var data = new StringContent(JsonConvert.SerializeObject(point), Encoding.UTF8, "application/json");
            httpclient.DefaultRequestHeaders.Accept.Clear();
            httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token.token);
            var response = await httpclient.PostAsync("Point/Import", data);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [TestMethod]
        public async Task TestDeletePoint()
        {
            await InitializeToken();
            var point = new PointModel(){ X = -19200, Y = 19200 };
            var data = new StringContent(JsonConvert.SerializeObject(point), Encoding.UTF8, "application/json");
            httpclient.DefaultRequestHeaders.Accept.Clear();
            httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token.token);
            var response = await httpclient.PostAsync("Point/Delete", data);
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }
        [TestMethod]
        public async Task TestGetSquares()
        {
            await InitializeToken();
            httpclient.DefaultRequestHeaders.Accept.Clear();
            httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token.token);
            Stopwatch sw = new Stopwatch();
            sw.Start(); 
            var response = await httpclient.GetAsync("GetSquares");
            sw.Stop();
            Console.WriteLine($"total seconds in getting response:{sw.Elapsed.TotalSeconds}"); 
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                SquareResponse squares = JsonConvert.DeserializeObject<SquareResponse>(content);
                Console.WriteLine("Square count:" + squares.count);
                Assert.AreEqual(2, squares.count);
            }
        }

        //[TestMethod]
        //public async Task TestImportList()
        //{
        //    await InitializeToken();
        //    List<PointModel> points = new List<PointModel>();
        //    for(int i=100;i<=400;i++)
        //    {
        //        points.Add(new PointModel() { X = i, Y = i });
        //        points.Add(new PointModel() { X = -i, Y = i });
        //        points.Add(new PointModel() { X = i, Y = -i });
        //        points.Add(new PointModel() { X = -i, Y = -i });
        //    }          
        //    var data = new StringContent(JsonConvert.SerializeObject(points), Encoding.UTF8, "application/json");
        //    httpclient.DefaultRequestHeaders.Accept.Clear();
        //    httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token.token);
        //    Stopwatch sw = new Stopwatch();
        //    sw.Start();
        //    var response = await httpclient.PostAsync("Point/Import", data);
        //    sw.Stop();
        //    Console.WriteLine($"total seconds in getting response:{sw.Elapsed.TotalSeconds}");
        //    Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        //}
    }
}