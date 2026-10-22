using System;
using System.Collections.Generic;
using System.Linq;

class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public int Quantidade { get; set; }
}

class Program
{
    static readonly List<Produto> produtos = new();
    static int proximoId = 1;

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n=== CONTROLE DE ESTOQUE ===");
            Console.WriteLine("1 - Cadastrar produto");
            Console.WriteLine("2 - Listar produtos");
            Console.WriteLine("3 - Registrar entrada");
            Console.WriteLine("4 - Registrar saída");
            Console.WriteLine("5 - Ver produtos com estoque baixo");
            Console.WriteLine("0 - Sair");
            Console.Write("Escolha uma opção: ");

            string? opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1": CadastrarProduto(); break;
                case "2": ListarProdutos(); break;
                case "3": AlterarEstoque(true); break;
                case "4": AlterarEstoque(false); break;
                case "5": ListarEstoqueBaixo(); break;
                case "0": return;
                default: Console.WriteLine("Opção inválida."); break;
            }
        }
    }

    static void CadastrarProduto()
    {
        Console.Write("Nome do produto: ");
        string nome = (Console.ReadLine() ?? "").Trim();

        if (string.IsNullOrWhiteSpace(nome))
        {
            Console.WriteLine("O nome não pode ficar vazio.");
            return;
        }

        int quantidade = LerQuantidade("Quantidade inicial: ");
        produtos.Add(new Produto { Id = proximoId++, Nome = nome, Quantidade = quantidade });
        Console.WriteLine("Produto cadastrado com sucesso!");
    }

    static void ListarProdutos()
    {
        if (produtos.Count == 0)
        {
            Console.WriteLine("Nenhum produto cadastrado.");
            return;
        }

        Console.WriteLine("\nID | PRODUTO | QUANTIDADE");
        foreach (var produto in produtos.OrderBy(p => p.Nome))
            Console.WriteLine($"{produto.Id} | {produto.Nome} | {produto.Quantidade}");
    }

    static void AlterarEstoque(bool entrada)
    {
        ListarProdutos();
        if (produtos.Count == 0) return;

        int id = LerQuantidade("Digite o ID do produto: ");
        Produto? produto = produtos.FirstOrDefault(p => p.Id == id);

        if (produto is null)
        {
            Console.WriteLine("Produto não encontrado.");
            return;
        }

        int quantidade = LerQuantidade("Quantidade: ");
        if (!entrada && quantidade > produto.Quantidade)
        {
            Console.WriteLine("Saída não permitida: quantidade maior que o estoque disponível.");
            return;
        }

        produto.Quantidade += entrada ? quantidade : -quantidade;
        Console.WriteLine($"Estoque atualizado. Quantidade atual: {produto.Quantidade}");
    }

    static void ListarEstoqueBaixo()
    {
        var baixos = produtos.Where(p => p.Quantidade <= 5).OrderBy(p => p.Quantidade).ToList();

        if (baixos.Count == 0)
        {
            Console.WriteLine("Nenhum produto com estoque baixo.");
            return;
        }

        Console.WriteLine("\nProdutos com 5 unidades ou menos:");
        foreach (var produto in baixos)
            Console.WriteLine($"{produto.Nome}: {produto.Quantidade} unidade(s)");
    }

    static int LerQuantidade(string mensagem)
    {
        while (true)
        {
            Console.Write(mensagem);
            if (int.TryParse(Console.ReadLine(), out int valor) && valor >= 0)
                return valor;

            Console.WriteLine("Digite um número inteiro igual ou maior que zero.");
        }
    }
}
