using System;
using Gestao_de_Biblioteca.Models;

namespace Gestao_de_Biblioteca.Interfaces
{
    public interface IPesquisavel
    {
        List<Livro> PesquisarPorTítulo(string titulo);
        List<Livro> PesquisarPorAutor(string autor);
        Livro PesquisarPorISBN(string isbn);
    }
}