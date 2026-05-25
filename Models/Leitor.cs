using System;

namespace Gestao_de_Biblioteca.Models
{
    public class Leitor
    {
        private string _id = string.Empty;
        private string _nome = string.Empty;
        private string _email = string.Empty;
        private string _telefone = string.Empty;
        private int _emprestimosActivos;
        public const int LimiteMaximoEmprestimos = 3;

        public string Id
        {
            get => _id;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("ID do leitor não pode ser vazio.");
                _id = value.Trim();
            }
        }

        public string Nome
        {
            get => _nome;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Nome não pode ser vazio.");
                _nome = value.Trim();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
                    throw new ArgumentException("Email inválido.");
                _email = value.Trim();
            }
        }

        public string Telefone
        {
            get => _telefone;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Telefone não pode ser vazio.");
                _telefone = value.Trim();
            }
        }

        public DateTime DataRegistro { get; set; }

        public int EmprestimosActivos
        {
            get => _emprestimosActivos;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Empréstimos ativos não podem ser negativos.");
                _emprestimosActivos = value;
            }
        }

        public bool PodeEmprestar() => EmprestimosActivos < LimiteMaximoEmprestimos;

        public void IncrementarEmprestimos() => EmprestimosActivos++;

        public void DecrementarEmprestimos()
        {
            if (EmprestimosActivos == 0)
                throw new InvalidOperationException("O leitor não possui empréstimos ativos.");
            EmprestimosActivos--;
        }

        public string ObterResumo()
        {
            return $"{Nome} (ID: {Id})";
        }

        public override string ToString()
        {
            return $"{Nome} (ID: {Id}) - Email: {Email} | - Telefone: {Telefone} | Empréstimos ativos: {EmprestimosActivos}";
        }
    }
}
