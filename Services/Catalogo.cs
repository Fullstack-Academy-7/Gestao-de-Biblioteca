using System;

using Gestao_de_Biblioteca.Interfaces;
using Gestao_de_Biblioteca.Models;
using Gestao_de_Biblioteca.Exceptions;

namespace Gestao_de_Biblioteca.Services
{
    public class Catalogo : IPesquisavel
    {
        private List<Livro> livros = new();

        public void AdicionarLivro(Livro livro)
        {
            if (livro == null) throw new ArgumentNullException(nameof(livro));
            if (livros.Any(l => l.ISBN == livro.ISBN))
                throw new InvalidOperationException($"Já existe um livro com o ISBN {livro.ISBN}.");
            livros.Add(livro);
        }

        public void RemoverLivro(string isbn)
        {
            var livro = PesquisarPorISBN(isbn);
            if (livro == null)
                throw new LivroNaoEncontradoException($"Livro com ISBN {isbn} não encontrado.");
            livros.Remove(livro);
        }

        public List<Livro> PesquisarPorTítulo(string titulo)
        {
            List<Livro> resultado = livros.Where(l => l.Titulo.Contains(titulo, StringComparison.OrdinalIgnoreCase)).ToList();
            
            return resultado;
        }

        public List<Livro> PesquisarPorAutor(string autor)
        {
            List<Livro> resultado = livros.Where(l => l.Autor.Contains(autor, StringComparison.OrdinalIgnoreCase)).ToList();

            return resultado;
        }

        public Livro? PesquisarPorISBN(string isbn)
        {
            Livro livro = livros.FirstOrDefault(l => l.ISBN == isbn);
            
            return livro;
        }

        public List<Livro> ListarTodos()
        {
            return livros.ToList();
        }
    }
}