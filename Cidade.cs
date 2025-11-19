using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using AgendaAlfabetica;

namespace Proj4
{
    public class Cidade : IComparable<Cidade>, IRegistro
    {
        string nome;
        double x, y;
        public ListaSimples<Ligacao> ligacoes = new ListaSimples<Ligacao>();

        const int tamanhoNome = 25;
        const int tamanhoRegistro = tamanhoNome + (2 * sizeof(double));

        public string Nome
        {
            get => nome;
            set => nome = value.PadRight(tamanhoNome, ' ').Substring(0, tamanhoNome);
        }

        public Cidade(string nome, double x, double y)
        {
            this.Nome = nome;
            this.x = x;
            this.y = y;
        }
        public override string ToString()
        {
            return Nome.TrimEnd() + " (" + ligacoes.QuantosNos + ")";
        }

        public Cidade()
        {
            this.Nome = "";
            this.x = 0;
            this.y = 0;
        }

        public Cidade(string nome)
        {
            this.Nome = nome;
        }

        public ListaSimples<Ligacao> GetLigacoes()
        {
            return ligacoes;
        }

        public List<Ligacao> ListarLigacaoCidade()
        {
            return ligacoes.Listar();
        }

        public int CompareTo(Cidade outraCid)
        {
            return Nome.CompareTo(outraCid.Nome);
        }

        public int TamanhoRegistro { get => tamanhoRegistro; }
        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }

        public void LerRegistro(BinaryReader arquivo, long qualRegistro)
        {
            if (arquivo != null)  // arquivo foi aberto pela aplicação
                try
                {
                    long qtosBytesAPular = qualRegistro * TamanhoRegistro;

                    arquivo.BaseStream.Seek(qtosBytesAPular, SeekOrigin.Begin);

                    char[] umNome = new char[tamanhoNome];

                    umNome = arquivo.ReadChars(tamanhoNome);
                    string nomeLido = "";
                    for (int i = 0; i < tamanhoNome; i++)
                        nomeLido += umNome[i];
                    Nome = nomeLido;

                    X = arquivo.ReadDouble();
                    Y = arquivo.ReadDouble();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }

        }

        public void PreencherLigacao(string destino, int distancia)
        {

        }

        public void GravarRegistro(BinaryWriter arquivo)
        {

        }
    }

}
