using System;

namespace Gestao_de_Biblioteca.Exceptions
{
    public class LivroNaoEncontradoException : Exception
    {
        public LivroNaoEncontradoException(string message) : base(message) { }
    }
}