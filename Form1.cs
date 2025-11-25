using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Proj4
{
    public partial class Form1 : Form
    {
        ArvoreAVL<Cidade> arvore;
        bool incluir, alterar = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void tpCadastro_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            arvore = new ArvoreAVL<Cidade>();
            arvore.LerArquivoDeRegistros("../../Dados/cidades.dat");
            StreamReader arquivo = new StreamReader("../../Dados/GrafoOnibusSaoPaulo.txt");
            LerArquivo(arquivo);
            pbMapa.Invalidate();
        }

        private void pnlArvore_Paint(object sender, PaintEventArgs e)
        {
            arvore.Desenhar(pnlArvore);
        }

        private void btnIncluirCidade_Click(object sender, EventArgs e)
        {
            if (txtNomeCidade.Text == "")
            {
                MessageBox.Show("Informe o nome da cidade!");
                return;
            }
            Cidade novaCidade = new Cidade(txtNomeCidade.Text, Convert.ToDouble(udX.Text), Convert.ToDouble(udY.Text));
            if (!arvore.Existe(novaCidade))
            {
                incluir = true;
                // pbMapa.CreateGraphics().FillEllipse(Brushes.Red, ((float)novaCidade.X)*pbMapa.Width, ((float)novaCidade.Y)*pbMapa.Height, 10, 10);
            }
            else
                MessageBox.Show("Cidade já existe!");
        }

        public void LerArquivo(StreamReader arquivo)
        {
            string linhaDeDados = "";

            while (!arquivo.EndOfStream)
            {
                linhaDeDados = arquivo.ReadLine();
                string[] info = linhaDeDados.Split(';');
                string cidade = info[0];
                Cidade cidadeProcurada = new Cidade(cidade, 0, 0);
                if (arvore.Existe(cidadeProcurada))
                {
                    string destino = info[1];
                    string distancia = info[2];

                    Ligacao ligacao = new Ligacao() { Origem = cidade.TrimEnd(), Destino = destino, Distancia = int.Parse(distancia) };
                    arvore.Atual.Info.ligacoes.InserirAposFim(ligacao);
                }
            }

            arquivo.Close();
        }

        private void btnExcluirCidade_Click(object sender, EventArgs e)
        {
            Cidade cidadeProcurada = new Cidade(txtNomeCidade.Text, 0, 0);
            if (arvore.Existe(cidadeProcurada))
            {
                if (arvore.Atual.Esq == null && arvore.Atual.Dir == null)
                {
                    arvore.ExcluirNaoBalanceado(cidadeProcurada);
                    pbMapa.Invalidate();
                }
                else
                    MessageBox.Show("Cidade tem conexões!");
            }
            else
                MessageBox.Show("Cidade não encontrada!");
            pbMapa.Invalidate();
        }

        private void btnBuscarCidade_Click(object sender, EventArgs e)
        {
            dgvLigacoes.Rows.Clear();
            Cidade cidadeProcurada = new Cidade(txtNomeCidade.Text, 0, 0);

            if (arvore.Existe(cidadeProcurada))
            {
                pbMapa.Invalidate();
                udX.Value = (decimal)arvore.Atual.Info.X;
                udY.Value = (decimal)arvore.Atual.Info.Y;
                pbMapa.CreateGraphics().FillEllipse(Brushes.Red, ((float)arvore.Atual.Info.X) * pbMapa.Width, ((float)arvore.Atual.Info.Y) * pbMapa.Height, 10, 10);
                pbMapa.CreateGraphics().DrawString(arvore.Atual.Info.Nome, SystemFonts.DefaultFont, Brushes.Black, ((float)arvore.Atual.Info.X) * pbMapa.Width, ((float)arvore.Atual.Info.Y) * pbMapa.Height);
                List<Ligacao> lista = arvore.Atual.Info.ListarLigacaoCidade();
                for (int i = 0; i <= lista.Count - 1; i++)
                {
                    dgvLigacoes.Rows.Add();
                    dgvLigacoes[0, i].Value = lista[i].Destino.ToString();
                    dgvLigacoes[1, i].Value = lista[i].Distancia.ToString();

                }
            }
            else
                MessageBox.Show("Cidade não encontrada!");
        }

        private void btnAlterarCidade_Click(object sender, EventArgs e)
        {
            Cidade alteraCidade = new Cidade(txtNomeCidade.Text, 0, 0);
            if (arvore.Existe(alteraCidade))
            {
                alterar = true;
            }
            else
                MessageBox.Show("Cidade não encontrada!");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //arvore.GravarArquivoDeRegistros("../../Dados/cidades.dat");
        }

        private void btnIncluirCaminho_Click(object sender, EventArgs e)
        {
            string nomeCidade = txtNomeCidade.Text;
            Cidade cidadeProcurada = new Cidade(nomeCidade, 0, 0);

            string destino = txtNovoDestino.Text;
            Cidade cidadeDestino = new Cidade(destino, 0, 0);

            string distancia = numericUpDown1.Text;

            if (arvore.Existe(cidadeDestino) && arvore.Existe(cidadeProcurada))
            {
                Ligacao ligacao = new Ligacao() { Origem = nomeCidade.TrimEnd(), Destino = destino, Distancia = int.Parse(distancia) };
                arvore.Atual.Info.ligacoes.InserirAposFim(ligacao);
                arvore.Existe(cidadeDestino);
                ligacao = new Ligacao() { Origem = destino.TrimEnd(), Destino = nomeCidade, Distancia = int.Parse(distancia) };
                arvore.Atual.Info.ligacoes.InserirAposFim(ligacao);
                MessageBox.Show("Ligação incluída!" + destino + distancia);
            }
            else
                MessageBox.Show("Cidade não encontrada!");
        }

        private void btnExcluirCaminho_Click(object sender, EventArgs e)
        {
            string nomeCidade = txtNomeCidade.Text;
            Cidade cidadeProcurada = new Cidade(nomeCidade, 0, 0);

            string destino = txtNovoDestino.Text;
            Cidade cidadeDestino = new Cidade(destino, 0, 0);

            if (arvore.Existe(cidadeDestino) && arvore.Existe(cidadeProcurada))
            {
                Ligacao ligacao = new Ligacao() { Origem = nomeCidade, Destino = destino.TrimEnd(), Distancia = 0 };
                arvore.Atual.Info.ligacoes.RemoverDado(ligacao);
                arvore.Existe(cidadeDestino);
                Ligacao ligacao2 = new Ligacao() { Origem = destino.TrimEnd(), Destino = nomeCidade, Distancia = 0 };
                arvore.Atual.Info.ligacoes.RemoverDado(ligacao2);
                MessageBox.Show("Ligação incluída!" + destino);
            }
            else
                MessageBox.Show("Cidade não encontrada!");
        }

        private void pbMapa_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            List<Cidade> listaCidades = new List<Cidade>();
            float x, y;
            string nome;
            arvore.VisitarEmOrdem(ref listaCidades);
            for (int i = 0; i < listaCidades.Count; i++)
            {
                x = ((float)listaCidades[i].X) * pbMapa.Width;
                y = ((float)listaCidades[i].Y) * pbMapa.Height;
                nome = listaCidades[i].Nome;
                g.FillEllipse(Brushes.Green, (x), (y), 10, 10);
                g.DrawString(nome, SystemFonts.DefaultFont, Brushes.Black, (x), (y) - 15);
            }
        }

        private void pbMapa_MouseDown(object sender, MouseEventArgs e)
        {
            if (incluir)
            {
                Cidade novaCidade = new Cidade(txtNomeCidade.Text, (double)e.X / pbMapa.Width, (double)e.Y / pbMapa.Height);
                arvore.InserirBalanceado(novaCidade);
                // MessageBox.Show(novaCidade.X.ToString(), novaCidade.Y.ToString());
                pbMapa.Invalidate();  
                pbMapa.CreateGraphics().FillEllipse(Brushes.Red, ((float)novaCidade.X) * pbMapa.Width, ((float)novaCidade.Y) * pbMapa.Height, 10, 10);
                incluir = false;
            }
            if (alterar)
            {
                // MessageBox.Show(novaCidade.X.ToString(), novaCidade.Y.ToString());
                arvore.Atual.Info.X = (double)e.X / pbMapa.Width;
                arvore.Atual.Info.Y = (double)e.Y / pbMapa.Height;
                pbMapa.Invalidate();
                pbMapa.CreateGraphics().FillEllipse(Brushes.Red, ((float)arvore.Atual.Info.X) * pbMapa.Width, ((float)arvore.Atual.Info.Y) * pbMapa.Height, 10, 10);
                alterar = false;
            }
        }
    }
}
