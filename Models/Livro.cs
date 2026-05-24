using System;

namespace Gestao_de_Biblioteca.Models
{
    public class Livro
    {
        private string _isbn = string.Empty;
        private string _titulo = string.Empty;
        private string _autor = string.Empty;
        private int _anoPublicacao;
        private int _totalExemplares;
        private int _exemplaresDisponiveis;

        public string ISBN
        {
            get => _isbn;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("ISBN não pode ser vazio.");
                _isbn = value;
            }
        }

        public string Titulo
        {
            get => _titulo;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Título não pode ser vazio.");
                _titulo = value;
            }
        }

        public string Autor
        {
            get => _autor;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Autor não pode ser vazio.");
                _autor = value;
            }
        }

        public int AnoPublicacao
        {
            get => _anoPublicacao;
            set
            {
                if (value < 0 || value > DateTime.Now.Year)
                    throw new ArgumentException("Ano de publicação inválido.");
                _anoPublicacao = value;
            }
        }

        public int TotalExemplares
        {
            get => _totalExemplares;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Total de exemplares não pode ser negativo.");
                _totalExemplares = value;
            }
        }

        public int ExemplaresDisponiveis
        {
            get => _exemplaresDisponiveis;
            set
            {
                if (value < 0 || value > TotalExemplares)
                    throw new ArgumentException("Exemplares disponíveis inválidos.");
                _exemplaresDisponiveis = value;
            }
        }

        public bool EstaDisponivel() => ExemplaresDisponiveis > 0;

        public void Emprestar()
        {
            if (!EstaDisponivel())
                throw new InvalidOperationException("Livro não disponível para empréstimo.");
            ExemplaresDisponiveis--;
        }

        public void Devolver()
        {
            if (ExemplaresDisponiveis >= TotalExemplares)
                throw new InvalidOperationException("Todos os exemplares já foram devolvidos.");
            ExemplaresDisponiveis++;
        }

        public override string ToString()
        {
            return $"{Titulo} - {Autor}, {AnoPublicacao} (ISBN: {ISBN}) | Disponível: {ExemplaresDisponiveis}/{TotalExemplares}";
        }
    }
}
