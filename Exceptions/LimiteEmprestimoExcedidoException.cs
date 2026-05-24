using System;

namespace Gestao_de_Biblioteca.Exceptions
{
    public class LimiteEmprestimoExcedidoException : Exception
    {
        public LimiteEmprestimoExcedidoException(string message) : base(message) { }
    }
}