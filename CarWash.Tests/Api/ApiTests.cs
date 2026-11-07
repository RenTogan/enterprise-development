using CarWash.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace CarWash.Tests.Api;

/// <summary>
/// Интеграционные тесты контроллеров через WebApplicationFactory
/// </summary>
public class ApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllClients_ReturnsOk()
    {
        HttpResponseMessage response = await _client.GetAsync("/api/clients");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        List<ClientDto>? clients = await response.Content.ReadFromJsonAsync<List<ClientDto>>();
        Assert.NotNull(clients);
        Assert.NotEmpty(clients);
    }

    [Fact]
    public async Task Client_CreateGetDelete_WorksCorrectly()
    {
        var request = new ClientRequest
        {
            FullName = "Иванов Иван",
            Phone = "+79990001122"
        };

        HttpResponseMessage postResponse = await _client.PostAsJsonAsync("/api/clients", request);
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

        ClientDto? created = await postResponse.Content.ReadFromJsonAsync<ClientDto>();
        Assert.NotNull(created);
        Assert.Equal(request.FullName, created.FullName);

        HttpResponseMessage getResponse = await _client.GetAsync($"/api/clients/{created.Id}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        HttpResponseMessage deleteResponse = await _client.DeleteAsync($"/api/clients/{created.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }

    [Fact]
    public async Task GetAnalytics_TopClients_ReturnsFiveClients()
    {
        HttpResponseMessage response = await _client.GetAsync("/api/analytics/top-clients");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        List<TopClientDto>? result = await response.Content.ReadFromJsonAsync<List<TopClientDto>>();
        Assert.NotNull(result);
        Assert.Equal(5, result.Count);
        Assert.Equal("Яцентюк Данила", result[0].FullName);
    }

    [Fact]
    public async Task GetAnalytics_BoxFreeTime_ReturnsCalculatedTime()
    {
        var time = new DateTime(2026, 5, 10, 10, 10, 0);
        HttpResponseMessage response = await _client.GetAsync($"/api/analytics/box-free-time?boxNumber=1&fromTime={time:O}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        BoxFreeTimeDto? result = await response.Content.ReadFromJsonAsync<BoxFreeTimeDto>();
        Assert.NotNull(result);
        Assert.Equal(new DateTime(2026, 5, 10, 10, 20, 0), result.FreeTime);
    }
}