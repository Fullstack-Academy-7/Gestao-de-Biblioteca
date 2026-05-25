using System;

using Gestao_de_Biblioteca.Enums;

namespace Gestao_de_Biblioteca.Models
{
    public class Emprestimo
    {
        private static int _proximoId = 1;

        public int Id { get; }
        public Livro Livro { get; }
        public Leitor Leitor { get; }
        public DateTime DataEmprestimo { get; }
        public DateTime DataPrevistaDevolucao { get; }
        public DateTime? DataDevolucaoEfetiva { get; private set; }
        public EstadoEmprestimo Estado { get; private set; }

        public Emprestimo(Livro livro, Leitor leitor, int diasEmprestimo = 14)
        {
            Id = _proximoId++;
            Livro = livro ?? throw new ArgumentNullException(nameof(livro));
            Leitor = leitor ?? throw new ArgumentNullException(nameof(leitor));
            DataEmprestimo = DateTime.Now;
            DataPrevistaDevolucao = DataEmprestimo.AddDays(diasEmprestimo);
            Estado = EstadoEmprestimo.EmCurso;
        }

        public decimal CalcularMulta()
        {
            if (Estado == EstadoEmprestimo.Devolvido && DataDevolucaoEfetiva.HasValue)
            {
                if (DataDevolucaoEfetiva.Value > DataPrevistaDevolucao)
                {
                    int diasAtraso = (DataDevolucaoEfetiva.Value - DataPrevistaDevolucao).Days;
                    return diasAtraso * 50; // 50,00Kz por dia de atraso
                }
            }
            else if (Estado == EstadoEmprestimo.EmCurso && DateTime.Now > DataPrevistaDevolucao)
            {
                Estado = EstadoEmprestimo.EmAtraso;
            }
            return 0;
        }

        public void RegistrarDevolucao()
        {
            if (Estado == EstadoEmprestimo.Devolvido)
                throw new InvalidOperationException("Empréstimo já foi devolvido.");

            DataDevolucaoEfetiva = DateTime.Now;
            Estado = EstadoEmprestimo.Devolvido;
            Livro.Devolver();
            Leitor.DecrementarEmprestimos();
        }

        public override string ToString()
        {
            string status = Estado switch
            {
                EstadoEmprestimo.EmCurso => "Em curso",
                EstadoEmprestimo.EmAtraso => "Atrasado",
                _ => "Devolvido"
            };
            return $"Empréstimo #{Id} | Livro: {Livro.Titulo} | Leitor: {Leitor.Nome} | Estado: {status} | Prevista: {DataPrevistaDevolucao:dd/MM/yyyy}";
        }
    }
}
