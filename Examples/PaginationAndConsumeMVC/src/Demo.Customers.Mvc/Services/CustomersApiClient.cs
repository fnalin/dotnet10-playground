using System.Net.Http.Json;
using Demo.Customers.Mvc.Models;

namespace Demo.Customers.Mvc.Services;

public sealed class CustomersApiClient(
    HttpClient httpClient,
    ILogger<CustomersApiClient> logger) : ICustomersApiClient
{
    public async Task<CustomersPageViewModel> GetCustomersAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.GetAsync(
                $"api/customers?page={page}&pageSize={pageSize}",
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<PagedResultViewModel<CustomerViewModel>>(
                cancellationToken: cancellationToken);

            if (data is null)
            {
                logger.LogWarning("A resposta da API veio nula.");
                return BuildFallback("Não foi possível carregar os clientes.");
            }

            return new CustomersPageViewModel
            {
                Data = data,
                HasFallback = false
            };
        }
        catch (OperationCanceledException ex) when (!cancellationToken.IsCancellationRequested)
        {
            logger.LogWarning(ex, "Timeout ao consultar a API.");
            return BuildFallback("A API demorou mais do que o esperado.");
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Falha HTTP ao consultar a API.");
            return BuildFallback("A API está indisponível no momento.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro inesperado ao consultar a API.");
            return BuildFallback("Ocorreu um erro ao buscar os clientes.");
        }
    }

    private static CustomersPageViewModel BuildFallback(string message)
    {
        return new CustomersPageViewModel
        {
            HasFallback = true,
            ErrorMessage = message,
            Data = new PagedResultViewModel<CustomerViewModel>
            {
                Page = 1,
                PageSize = 10,
                TotalItems = 0,
                TotalPages = 0,
                Items = []
            }
        };
    }
}