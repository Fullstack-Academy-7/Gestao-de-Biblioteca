using Gestao_de_Biblioteca.Exceptions;
using Gestao_de_Biblioteca.Models;
using Gestao_de_Biblioteca.Services;

namespace Gestao_de_Biblioteca
{
    class Program
    {
        static GestorBiblioteca gestor = new();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            InicializarDadosExemplo();

            bool sair = false;
            while (!sair)
            {
                Console.Clear();
                Console.WriteLine("===== SISTEMA DE GESTÃO DE BIBLIOTECA =====");
                Console.WriteLine("1 - Cadastrar Livro");
                Console.WriteLine("2 - Cadastrar Leitor");
                Console.WriteLine("3 - Realizar Empréstimo");
                Console.WriteLine("4 - Registrar Devolução");
                Console.WriteLine("5 - Pesquisar Livros");
                Console.WriteLine("6 - Listar Empréstimos Abertos");
                Console.WriteLine("7 - Ver Histórico de um Leitor");
                Console.WriteLine("8 - Listar Todos os Livros");
                Console.WriteLine("0 - Sair");
                Console.Write("Escolha: ");

                string opcao = Console.ReadLine()?.Trim() ?? "-";
                try
                {
                    switch (opcao)
                    {
                        case "1": CadastrarLivro(); break;
                        case "2": CadastrarLeitor(); break;
                        case "3": RealizarEmprestimo(); break;
                        case "4": RegistrarDevolucao(); break;
                        case "5": PesquisarLivros(); break;
                        case "6": ListarEmprestimosAbertos(); break;
                        case "7": HistoricoLeitor(); break;
                        case "8": ListarTodosLivros(); break;
                        case "0": sair = true; break;
                        default: Console.WriteLine("Opção inválida."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nERRO: {ex.Message}");
                }

                if (!sair)
                {
                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        static void InicializarDadosExemplo()
        {
            try
            {
                gestor.RegistrarLeitor(new Leitor { Id = "L001", Nome = "João Silva", Email = "joao@email.com", Telefone = "99999-1111", DataRegistro = DateTime.Now });
                gestor.RegistrarLeitor(new Leitor { Id = "L002", Nome = "Maria Santos", Email = "maria@email.com", Telefone = "99999-2222", DataRegistro = DateTime.Now });

                var livro1 = new Livro { ISBN = "978-3-16-148410-0", Titulo = "C# Avançado", Autor = "Robert Martin", AnoPublicacao = 2020, TotalExemplares = 3, ExemplaresDisponiveis = 3 };
                var livro2 = new Livro { ISBN = "978-0-13-110362-7", Titulo = "Clean Code", Autor = "Robert Martin", AnoPublicacao = 2008, TotalExemplares = 2, ExemplaresDisponiveis = 2 };
                gestor.ObterCatalogo().AdicionarLivro(livro1);
                gestor.ObterCatalogo().AdicionarLivro(livro2);
            }
            catch { }
        }

        static void CadastrarLivro()
        {
            Console.WriteLine("\n--- Cadastrar Livro ---");
            var livro = new Livro();
            Console.Write("ISBN: "); livro.ISBN = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Título: "); livro.Titulo = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Autor: "); livro.Autor = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Ano de Publicação: "); livro.AnoPublicacao = int.Parse(Console.ReadLine() ?? "0");
            Console.Write("Total de Exemplares: "); livro.TotalExemplares = int.Parse(Console.ReadLine() ?? "0");
            livro.ExemplaresDisponiveis = livro.TotalExemplares;
            gestor.ObterCatalogo().AdicionarLivro(livro);
            Console.WriteLine("Livro cadastrado com sucesso!");
        }

        static void CadastrarLeitor()
        {
            Console.WriteLine("\n--- Cadastrar Leitor ---");
            var leitor = new Leitor();
            Console.Write("ID: "); leitor.Id = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Nome: "); leitor.Nome = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Email: "); leitor.Email = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Telefone: "); leitor.Telefone = Console.ReadLine()?.Trim() ?? "";
            leitor.DataRegistro = DateTime.Now;
            leitor.EmprestimosActivos = 0;
            gestor.RegistrarLeitor(leitor);
            Console.WriteLine("Leitor cadastrado com sucesso!");
        }

        static void RealizarEmprestimo()
        {
            Console.WriteLine("\n--- Realizar Empréstimo ---");
            Console.Write("ISBN do livro: "); string isbn = Console.ReadLine()?.Trim() ?? "";
            Console.Write("ID do leitor: "); string idLeitor = Console.ReadLine()?.Trim() ?? "";
            gestor.RealizarEmprestimo(isbn, idLeitor);
            Console.WriteLine("Empréstimo realizado com sucesso!");
        }

        static void RegistrarDevolucao()
        {
            Console.WriteLine("\n--- Registrar Devolução ---");
            Console.Write("ID do empréstimo: ");
            if (!int.TryParse(Console.ReadLine(), out int idEmp))
                throw new ArgumentException("ID inválido.");
            decimal multa = gestor.ProcessarDevolucao(idEmp);
            if (multa > 0)
                Console.WriteLine($"Devolução registrada. Multa: R$ {multa:F2}");
            else
                Console.WriteLine("Devolução registrada. Sem multa.");
        }

        static void PesquisarLivros()
        {
            Console.WriteLine("\n--- Pesquisar Livros ---");
            Console.Write("Digite o termo (título, autor ou ISBN): ");
            string termo = Console.ReadLine()?.Trim() ?? "";
            var resultados = gestor.PesquisarLivro(termo);
            if (resultados.Count == 0)
                Console.WriteLine("Nenhum livro encontrado.");
            else
                foreach (var l in resultados) Console.WriteLine(l);
        }

        static void ListarEmprestimosAbertos()
        {
            Console.WriteLine("\n--- Empréstimos em Aberto ---");
            var emprestimos = gestor.ListarEmprestimosAbertos();
            if (emprestimos.Count == 0)
                Console.WriteLine("Nenhum empréstimo ativo.");
            else
                foreach (var e in emprestimos) Console.WriteLine(e);
        }

        static void HistoricoLeitor()
        {
            Console.WriteLine("\n--- Histórico de Empréstimos por Leitor ---");
            Console.Write("ID do leitor: ");
            string id = Console.ReadLine()?.Trim() ?? "";
            var historico = gestor.HistoricoPorLeitor(id);
            if (historico.Count == 0)
                Console.WriteLine("Nenhum empréstimo encontrado para este leitor.");
            else
                foreach (var e in historico) Console.WriteLine(e);
        }

        static void ListarTodosLivros()
        {
            Console.WriteLine("\n--- Todos os Livros do Catálogo ---");
            var livros = gestor.ObterCatalogo().ListarTodos();
            if (livros.Count == 0)
                Console.WriteLine("Catálogo vazio.");
            else
                foreach (var l in livros) Console.WriteLine(l);
        }
    }
}