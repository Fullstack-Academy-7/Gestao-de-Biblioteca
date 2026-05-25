using System;

using Gestao_de_Biblioteca.Enums;
using Gestao_de_Biblioteca.Exceptions;
using Gestao_de_Biblioteca.Interfaces;
using Gestao_de_Biblioteca.Models;

namespace Gestao_de_Biblioteca.Services
{
    public class GestorBiblioteca
    {
        private readonly Catalogo _catalogo = new();
        private readonly IPesquisavel _pesquisador;
        private readonly List<Leitor> _leitores = new();
        private readonly List<Emprestimo> _emprestimos = new();

        public GestorBiblioteca()
        {
            _pesquisador = _catalogo;
        }

        public void RegistrarLivro(Livro livro)
        {
            _catalogo.AdicionarLivro(livro);
        }

        public void EditarLivro(string isbnActual, string? titulo, string? autor, int? anoPublicacao, int? totalExemplares)
        {
            _catalogo.EditarLivro(isbnActual, titulo, autor, anoPublicacao, totalExemplares);
        }

        public void RemoverLivro(string isbn)
        {
            if (_emprestimos.Any(e => e.Livro.ISBN == isbn && e.Estado != EstadoEmprestimo.Devolvido))
                throw new InvalidOperationException("Não é possível remover um livro com empréstimos em aberto.");

            _catalogo.RemoverLivro(isbn);
        }

        public void RegistrarLeitor(Leitor leitor)
        {
            if (leitor == null) throw new ArgumentNullException(nameof(leitor));
            if (_leitores.Any(l => l.Id == leitor.Id))
                throw new InvalidOperationException($"Leitor com ID {leitor.Id} já está registrado.");
            _leitores.Add(leitor);
        }

        public void EditarLeitor(string id, string? nome, string? email, string? telefone)
        {
            var leitor = _leitores.FirstOrDefault(l => l.Id == id);
            if (leitor == null)
                throw new ArgumentException($"Leitor com ID {id} não encontrado.");

            if (!string.IsNullOrWhiteSpace(nome))
                leitor.Nome = nome;

            if (!string.IsNullOrWhiteSpace(email))
                leitor.Email = email;

            if (!string.IsNullOrWhiteSpace(telefone))
                leitor.Telefone = telefone;
        }

        public void RemoverLeitor(string id)
        {
            var leitor = _leitores.FirstOrDefault(l => l.Id == id);
            if (leitor == null)
                throw new ArgumentException($"Leitor com ID {id} não encontrado.");

            if (_emprestimos.Any(e => e.Leitor.Id == id && e.Estado != EstadoEmprestimo.Devolvido))
                throw new InvalidOperationException("Não é possível remover um leitor com empréstimos em aberto.");

            _leitores.Remove(leitor);
        }

        public void RealizarEmprestimo(string isbn, string idLeitor)
        {
            var livro = _catalogo.PesquisarPorISBN(isbn);
            if (livro == null)
                throw new LivroNaoEncontradoException($"Livro com ISBN {isbn} não encontrado.");

            if (!livro.EstaDisponivel())
                throw new LivroIndisponivelException($"O livro '{livro.Titulo}' não está disponível.");

            var leitor = _leitores.FirstOrDefault(l => l.Id == idLeitor);
            if (leitor == null)
                throw new ArgumentException($"Leitor com ID {idLeitor} não encontrado.");

            if (!leitor.PodeEmprestar())
                throw new LimiteEmprestimoExcedidoException($"Leitor {leitor.Nome} já atingiu o limite de {Leitor.LimiteMaximoEmprestimos} empréstimos ativos.");

            livro.Emprestar();
            leitor.IncrementarEmprestimos();

            var emprestimo = new Emprestimo(livro, leitor);
            _emprestimos.Add(emprestimo);
        }

        public decimal ProcessarDevolucao(int idEmprestimo)
        {
            var emprestimo = _emprestimos.FirstOrDefault(e => e.Id == idEmprestimo);
            if (emprestimo == null)
                throw new ArgumentException($"Empréstimo com ID {idEmprestimo} não encontrado.");

            if (emprestimo.Estado == EstadoEmprestimo.Devolvido)
                throw new InvalidOperationException("Este empréstimo já foi devolvido.");

            decimal multa = emprestimo.CalcularMulta();
            emprestimo.RegistrarDevolucao(); 
            return multa;
        }

        public List<Emprestimo> ListarEmprestimosAbertos()
        {
            return _emprestimos.Where(e => e.Estado != EstadoEmprestimo.Devolvido).ToList();
        }

        public List<Emprestimo> HistoricoPorLeitor(string idLeitor)
        {
            if (!_leitores.Any(l => l.Id == idLeitor))
                throw new ArgumentException($"Leitor com ID {idLeitor} não encontrado.");

            return _emprestimos.Where(e => e.Leitor.Id == idLeitor).ToList();
        }

        public List<Livro> PesquisarLivro(string termo)
        {
            var resultado = new List<Livro>();
            resultado.AddRange(_pesquisador.PesquisarPorTítulo(termo));
            resultado.AddRange(_pesquisador.PesquisarPorAutor(termo));
            var porIsbn = _pesquisador.PesquisarPorISBN(termo);
            if (porIsbn != null) resultado.Add(porIsbn);
            return resultado.Distinct().ToList();
        }

        public Catalogo ObterCatalogo() => _catalogo;
        public List<Leitor> ObterLeitores() => _leitores.ToList();
    }
}
