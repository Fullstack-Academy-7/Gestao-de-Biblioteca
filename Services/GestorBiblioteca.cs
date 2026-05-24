using System;

using Gestao_de_Biblioteca.Enums;
using Gestao_de_Biblioteca.Exceptions;
using Gestao_de_Biblioteca.Models;

namespace Gestao_de_Biblioteca.Services
{
    public class GestorBiblioteca
    {
        private Catalogo _catalogo = new();
        private List<Leitor> _leitores = new();
        private List<Emprestimo> _emprestimos = new();
        private readonly int _limiteEmprestimos = 3;

        public void RegistrarLeitor(Leitor leitor)
        {
            if (leitor == null) throw new ArgumentNullException(nameof(leitor));
            if (_leitores.Any(l => l.Id == leitor.Id))
                throw new InvalidOperationException($"Leitor com ID {leitor.Id} já está registrado.");
            _leitores.Add(leitor);
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
                throw new LimiteEmprestimoExcedidoException($"Leitor {leitor.Nome} já atingiu o limite de {_limiteEmprestimos} empréstimos ativos.");

            livro.Emprestar();
            leitor.EmprestimosActivos++;

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
            return _emprestimos.Where(e => e.Leitor.Id == idLeitor).ToList();
        }

        public List<Livro> PesquisarLivro(string termo)
        {
            var resultado = new List<Livro>();
            resultado.AddRange(_catalogo.PesquisarPorTítulo(termo));
            resultado.AddRange(_catalogo.PesquisarPorAutor(termo));
            var porIsbn = _catalogo.PesquisarPorISBN(termo);
            if (porIsbn != null) resultado.Add(porIsbn);
            return resultado.Distinct().ToList();
        }

        public Catalogo ObterCatalogo() => _catalogo;
        public List<Leitor> ObterLeitores() => _leitores.ToList();
    }
}