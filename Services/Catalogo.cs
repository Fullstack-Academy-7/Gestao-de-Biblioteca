using System;

using Gestao_de_Biblioteca.Interfaces;
using Gestao_de_Biblioteca.Models;
using Gestao_de_Biblioteca.Exceptions;

namespace Gestao_de_Biblioteca.Services
{
    public class Catalogo : IPesquisavel
    {
        private readonly Dictionary<string, Livro> _livros = new();

        public void AdicionarLivro(Livro livro)
        {
            if (livro == null) throw new ArgumentNullException(nameof(livro));
            if (_livros.ContainsKey(livro.ISBN))
                throw new InvalidOperationException($"Já existe um livro com o ISBN {livro.ISBN}.");
            _livros.Add(livro.ISBN, livro);
        }

        public void EditarLivro(string isbnActual, string? titulo, string? autor, int? anoPublicacao, int? totalExemplares)
        {
            var livro = PesquisarPorISBN(isbnActual);
            if (livro == null)
                throw new LivroNaoEncontradoException($"Livro com ISBN {isbnActual} não encontrado.");

            if (!string.IsNullOrWhiteSpace(titulo))
                livro.Titulo = titulo;

            if (!string.IsNullOrWhiteSpace(autor))
                livro.Autor = autor;

            if (anoPublicacao.HasValue)
                livro.AnoPublicacao = anoPublicacao.Value;

            if (totalExemplares.HasValue)
            {
                int exemplaresEmprestados = livro.TotalExemplares - livro.ExemplaresDisponiveis;
                if (totalExemplares.Value < exemplaresEmprestados)
                    throw new ArgumentException("O total de exemplares não pode ser inferior aos exemplares emprestados.");

                livro.TotalExemplares = totalExemplares.Value;
                livro.ExemplaresDisponiveis = totalExemplares.Value - exemplaresEmprestados;
            }
        }

        public void RemoverLivro(string isbn)
        {
            var livro = PesquisarPorISBN(isbn);
            if (livro == null)
                throw new LivroNaoEncontradoException($"Livro com ISBN {isbn} não encontrado.");
            _livros.Remove(livro.ISBN);
        }

        public List<Livro> PesquisarPorTítulo(string titulo)
        {
            List<Livro> resultado = _livros.Values.Where(l => l.Titulo.Contains(titulo, StringComparison.OrdinalIgnoreCase)).ToList();
            
            return resultado;
        }

        public List<Livro> PesquisarPorAutor(string autor)
        {
            List<Livro> resultado = _livros.Values.Where(l => l.Autor.Contains(autor, StringComparison.OrdinalIgnoreCase)).ToList();

            return resultado;
        }

        public Livro? PesquisarPorISBN(string isbn)
        {
            _livros.TryGetValue(isbn, out Livro? livro);
            return livro;
        }

        public List<Livro> ListarTodos()
        {
            return _livros.Values.ToList();
        }
    }
}
