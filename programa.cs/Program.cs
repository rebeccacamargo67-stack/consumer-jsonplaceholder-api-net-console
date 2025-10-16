using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

// Classes de modelo (Coloque elas no mesmo arquivo ou em um arquivo separado, como User.cs, Company.cs, etc.)

public class Geo
{
    public string lat { get; set; }
    public string lng { get; set; }
}

public class Address
{
    public string street { get; set; }
    public string suite { get; set; }
    public string city { get; set; }
    public string zipcode { get; set; }
    public Geo geo { get; set; }
}

public class Company
{
    public string name { get; set; }
    public string catchPhrase { get; set; }
    public string bs { get; set; }
}

public class User
{
    public int id { get; set; }
    public string name { get; set; }
    public string username { get; set; }
    public string email { get; set; }
    public Address address { get; set; }
    public string phone { get; set; }
    public string website { get; set; }
    public Company company { get; set; }
}


class Program
{
    // O HttpClient é thread-safe e deve ser reutilizado
    private static readonly HttpClient client = new HttpClient();
    private const string ApiUrl = "https://jsonplaceholder.typicode.com/users";

    static async Task Main(string[] args)
    {
        Console.WriteLine("Iniciando consumo da API de usuários...");
        
        // Chamamos o método assíncrono para obter e processar os dados
        await GetAndDisplayFirstUser();

        Console.WriteLine("\nProcessamento concluído. Pressione qualquer tecla para sair...");
        Console.ReadKey();
    }

    static async Task GetAndDisplayFirstUser()
    {
        try
        {
            // 1. Faz a requisição HTTP GET e obtém a resposta
            HttpResponseMessage response = await client.GetAsync(ApiUrl);
            response.EnsureSuccessStatusCode(); // Lança uma exceção se o código HTTP não for de sucesso (2xx)

            // 2. Lê o conteúdo da resposta como uma string JSON
            string jsonContent = await response.Content.ReadAsStringAsync();

            // 3. Deserializa o JSON em uma lista de objetos User
            var users = JsonSerializer.Deserialize<List<User>>(jsonContent);

            // 4. Verifica se a lista não está vazia e obtém o primeiro usuário
            if (users != null && users.Count > 0)
            {
                User firstUser = users[0];

                // 5. Imprime os dados solicitados
                Console.WriteLine("------------------------------------------");
                Console.WriteLine("Dados da PRIMEIRA pessoa retornada pela API:");
                Console.WriteLine($"Nome: {firstUser.name}");
                Console.WriteLine($"Cidade: {firstUser.address.city}");
                Console.WriteLine($"Empresa: {firstUser.company.name}");
                Console.WriteLine("------------------------------------------");
            }
            else
            {
                Console.WriteLine("A API retornou uma lista de usuários vazia.");
            }
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"\nExceção na Requisição: {e.Message}");
            Console.WriteLine("Verifique a URL da API ou sua conexão com a internet.");
        }
        catch (JsonException e)
        {
            Console.WriteLine($"\nErro ao Deserializar JSON: {e.Message}");
            Console.WriteLine("O formato do JSON pode ter mudado ou as classes de modelo estão incorretas.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"\nOcorreu um erro inesperado: {e.Message}");
        }
    }
}