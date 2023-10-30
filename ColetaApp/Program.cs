using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

interface IRepositorDeProdutos
{
	void AdicionarProduto(string empresa, string codigo, string nome, int quantidade, string comprador, string fornecedor, decimal preco);
	List<Produto> ObterProdutos(string empresa, string comprador = null);
	List<string> ObterCompradores();
	int ObterProdutosPorComprador(string empresa, string comprador);
}
class Produto
{
	public string Codigo { get; set; }
	public string Nome { get; set; }
	public int Quantidade { get; set; }
	public string Empresa { get; set; }
	public string Comprador { get; set; }
	public string Fornecedor { get; set; }
	public decimal Preco { get; set; }

}
class RepositorioDeProdutos :IRepositorDeProdutos
{
	private List<Produto> produtos = new List<Produto>();

	public void AdicionarProduto(string empresa, string codigo, string nome, int quantidade, string comprador, string fornecedor, decimal preco)
	{
		Produto produto = new Produto
		{
			Empresa = empresa,
			Codigo = codigo,
			Nome = nome,
			Quantidade = quantidade,
			Comprador = comprador,
			Fornecedor = fornecedor,
			Preco = preco
		};
		produtos.Add(produto);
	}
	public List <Produto> ObterProdutos(string empresa,string comprador=null)
	{
		if (comprador == null)
			return produtos.FindAll(p => p.Empresa == empresa);
		else
			return produtos.FindAll(p => p.Empresa == empresa && string.Equals(p.Comprador, comprador, StringComparison.OrdinalIgnoreCase));
	}
	public List <string> ObterCompradores()
	{
		return produtos.Select(p => p.Comprador).Distinct().ToList();
	}
	public int ObterProdutosPorComprador(string empresa,string comprador)
	{
		return produtos.Count(p => p.Empresa == empresa && string.Equals(p.Comprador, comprador, StringComparison.OrdinalIgnoreCase));
	}

}

class Usuario
{
	public string NomeDeUsuario { get; set; }
	public string Senha { get; set; }
	public string Empresa { get; set; }
}
class Program
{
	static List<Usuario> usuarios = new List<Usuario>
	{
		new Usuario { NomeDeUsuario = "dalton.a", Senha = "coleta.ana", Empresa = "705 - CRF FLOR" },
		new Usuario { NomeDeUsuario = "dalton.b", Senha = "coleta.ana", Empresa = "706 - CRF CORE" }
	};

	static void Main(string[] args)
	{
		Console.WriteLine("Escolha a empresa: ");
		Console.WriteLine("1. 705 - CRF FLOR");
		Console.WriteLine("2. 706 - CRF CORE");
		Console.Write("Digite o número da empresa: ");
		int escolhaEmpresa = int.Parse(Console.ReadLine());

		string empresaSelecionada = (escolhaEmpresa == 1) ? "705 - CRF FLOR" : "706 - CRF CORE";

		Usuario usuarioAtual = usuarios.Find(u => u.Empresa == empresaSelecionada);

		if (usuarioAtual == null)
		{
			Console.WriteLine("Empresa não encontrada.");
			return;
		}
		Console.WriteLine("Digite o nome do usuario: ");
		string nomeDeUsuario = Console.ReadLine();
		Console.Write("Digite a senha: ");
		string senha = Console.ReadLine();


		if (usuarioAtual.NomeDeUsuario == nomeDeUsuario && usuarioAtual.Senha == senha)
		{
			Console.WriteLine("Bem-vindo à empresa " + usuarioAtual.Empresa);

			IRepositorDeProdutos repositorioDeProdutos = new RepositorioDeProdutos();

			while (true)
			{
				Console.WriteLine("Opções de estoque para a empresa " + usuarioAtual.Empresa);
				Console.WriteLine("1. Cadastrar produto");
				Console.WriteLine("2. Procurar produto");
				Console.WriteLine("3. Ver estoque");
				Console.WriteLine("4. Sair");
				Console.Write("Escolha uma opção: ");

				int escolha = int.Parse(Console.ReadLine());

				switch (escolha)
				{
					case 1:
						Console.Write("Digite o código do produto: ");
						string codigo = Console.ReadLine();

						
						if (!int.TryParse(codigo, out _))
						{
							Console.WriteLine("Código do produto deve conter apenas números.");
							break;
						}

						Console.Write("Digite o nome do produto (apenas letras): ");
						string nome = Console.ReadLine();

						
						if (!SaoTodasLetras(nome))
						{
							Console.WriteLine("Nome do produto deve conter apenas letras.");
							break;
						}

						Console.Write("Digite a quantidade em estoque (apenas números): ");
						string quantidadeInput = Console.ReadLine();

						if (!int.TryParse(quantidadeInput, out int quantidade))
						{
							Console.WriteLine("Quantidade em estoque deve conter apenas números.");
							break;
						}

						Console.Write("Digite o comprador (apenas letras): ");
						string comprador = Console.ReadLine();

						if (!SaoTodasLetras(comprador))
						{
							Console.WriteLine("Nome do comprador deve conter apenas letras.");
							break;
						}

						Console.Write("Digite o fornecedor (apenas letras): ");
						string fornecedor = Console.ReadLine();

						if (!SaoTodasLetras(fornecedor))
						{
							Console.WriteLine("Nome do fornecedor deve conter apenas letras.");
							break;
						}

						Console.Write("Digite o preço de venda (em formato decimal): ");
						if (!decimal.TryParse(Console.ReadLine(), out decimal preco))
						{
							Console.WriteLine("Preço de venda deve ser um número decimal.");
							break;
						}

						repositorioDeProdutos.AdicionarProduto(usuarioAtual.Empresa, codigo, nome, quantidade, comprador, fornecedor, preco);
						Console.WriteLine("Produto cadastrado com sucesso!");
						break;

					case 2:
						Console.Write("Digite o código do produto a ser procurado: ");
						string codigoDeBusca = Console.ReadLine();

						Produto produtoEncontrado = repositorioDeProdutos.ObterProdutos(usuarioAtual.Empresa)
						.FirstOrDefault(p => p.Codigo == codigoDeBusca);

						if (produtoEncontrado != null)
						{
							Console.WriteLine("Informações do Produto:");
							Console.WriteLine($"Código: {produtoEncontrado.Codigo}");
							Console.WriteLine($"Nome: {produtoEncontrado.Nome}");
							Console.WriteLine($"Quantidade em estoque: {produtoEncontrado.Quantidade}");
							Console.WriteLine($"Comprador: {produtoEncontrado.Comprador}");
							Console.WriteLine($"Fornecedor: {produtoEncontrado.Fornecedor}");
							Console.WriteLine($"Preço de Venda: R$ {produtoEncontrado.Preco:F2}");

							Console.WriteLine("\nPressione Enter para continuar...");
							Console.ReadLine();
						}
						else
						{
							Console.WriteLine("Produto não encontrado.");
						}
						break;

					case 3:
						List<string> compradores = repositorioDeProdutos.ObterCompradores();
						Console.WriteLine("Compradores disponíveis:");

						foreach (var nomeComprador in compradores)
						{
							Console.WriteLine(nomeComprador);
						}

						Console.Write("Escolha um comprador para ver a quantidade de itens cadastrados: ");
						string compradorSelecionado = Console.ReadLine();

						int quantidadeDeProdutos = repositorioDeProdutos.ObterProdutosPorComprador(usuarioAtual.Empresa, compradorSelecionado);
						Console.WriteLine($"Quantidade de itens cadastrados para o comprador {compradorSelecionado}: {quantidadeDeProdutos}");
						break;

					case 4:
						Environment.Exit(0);
						break;

					default:
						Console.WriteLine("Opção inválida. Tente novamente.");
						break;
				}
			}
		}
		else
		{
			Console.WriteLine("Usuário ou senha incorretos. Saindo.");
		}
		static bool SaoTodasLetras(string str)
		{
			foreach (char c in str)
			{
				if (!char.IsLetter(c) && c != ' ')
				{
					return false;
				}
			}
			return true;
		}
	}
}
		