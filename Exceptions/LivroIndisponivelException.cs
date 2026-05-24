using System;

namespace Gestao_de_Biblioteca.Exceptions
{
    public class LivroIndisponivelException : Exception
    {
        public LivroIndisponivelException(string message) : base(message) { }
    }
}