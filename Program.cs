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
                Console.WriteLine("9 - Editar Livro");
                Console.WriteLine("10 - Remover Livro");
                Console.WriteLine("11 - Listar Leitores");
                Console.WriteLine("12 - Editar Leitor");
                Console.WriteLine("13 - Remover Leitor");
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
                        case "9": EditarLivro(); break;
                        case "10": RemoverLivro(); break;
                        case "11": ListarLeitores(); break;
                        case "12": EditarLeitor(); break;
                        case "13": RemoverLeitor(); break;
                        case "0": sair = true; break;
                        default: Console.WriteLine("Opção inválida."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nERRO: {ex.Message}");
                }
                finally
                {
                    Console.ResetColor();
                }

                if (!sair)
                {
                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                    if (Console.IsInputRedirected)
                        Console.ReadLine();
                    else
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
                gestor.RegistrarLivro(livro1);
                gestor.RegistrarLivro(livro2);
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
            livro.AnoPublicacao = LerInteiro("Ano de Publicação: ");
            livro.TotalExemplares = LerInteiro("Total de Exemplares: ");
            livro.ExemplaresDisponiveis = livro.TotalExemplares;
            gestor.RegistrarLivro(livro);
            Console.WriteLine("Livro cadastrado com sucesso!");
        }

        static void EditarLivro()
        {
            Console.WriteLine("\n--- Editar Livro ---");
            Console.Write("ISBN do livro a editar: ");
            string isbn = Console.ReadLine()?.Trim() ?? "";
            string? titulo = LerTextoOpcional("Novo título (Enter para manter): ");
            string? autor = LerTextoOpcional("Novo autor (Enter para manter): ");
            int? ano = LerInteiroOpcional("Novo ano de publicação (Enter para manter): ");
            int? total = LerInteiroOpcional("Novo total de exemplares (Enter para manter): ");
            gestor.EditarLivro(isbn, titulo, autor, ano, total);
            Console.WriteLine("Livro editado com sucesso!");
        }

        static void RemoverLivro()
        {
            Console.WriteLine("\n--- Remover Livro ---");
            Console.Write("ISBN do livro: ");
            string isbn = Console.ReadLine()?.Trim() ?? "";
            gestor.RemoverLivro(isbn);
            Console.WriteLine("Livro removido com sucesso!");
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

        static void EditarLeitor()
        {
            Console.WriteLine("\n--- Editar Leitor ---");
            Console.Write("ID do leitor: ");
            string id = Console.ReadLine()?.Trim() ?? "";
            string? nome = LerTextoOpcional("Novo nome (Enter para manter): ");
            string? email = LerTextoOpcional("Novo email (Enter para manter): ");
            string? telefone = LerTextoOpcional("Novo telefone (Enter para manter): ");
            gestor.EditarLeitor(id, nome, email, telefone);
            Console.WriteLine("Leitor editado com sucesso!");
        }

        static void RemoverLeitor()
        {
            Console.WriteLine("\n--- Remover Leitor ---");
            Console.Write("ID do leitor: ");
            string id = Console.ReadLine()?.Trim() ?? "";
            gestor.RemoverLeitor(id);
            Console.WriteLine("Leitor removido com sucesso!");
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
                Console.WriteLine($"Devolução registrada. Multa: {multa:F2} Kz");
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

        static void ListarLeitores()
        {
            Console.WriteLine("\n--- Leitores Registados ---");
            var leitores = gestor.ObterLeitores();
            if (leitores.Count == 0)
                Console.WriteLine("Nenhum leitor registado.");
            else
                foreach (var leitor in leitores) Console.WriteLine(leitor.ObterResumo());
        }

        static int LerInteiro(string mensagem)
        {
            Console.Write(mensagem);
            if (!int.TryParse(Console.ReadLine(), out int valor))
                throw new ArgumentException("Valor numérico inválido.");
            return valor;
        }

        static string? LerTextoOpcional(string mensagem)
        {
            Console.Write(mensagem);
            string? valor = Console.ReadLine()?.Trim();
            return string.IsNullOrWhiteSpace(valor) ? null : valor;
        }

        static int? LerInteiroOpcional(string mensagem)
        {
            Console.Write(mensagem);
            string? valor = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(valor))
                return null;

            if (!int.TryParse(valor, out int numero))
                throw new ArgumentException("Valor numérico inválido.");

            return numero;
        }
    }
}
