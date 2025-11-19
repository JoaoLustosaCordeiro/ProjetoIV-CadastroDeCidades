using System;
using System.Collections.Generic;
using System.Drawing;
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
        }

        private void pnlArvore_Paint(object sender, PaintEventArgs e)
        {
            arvore.Desenhar(pnlArvore);
        }

        private void btnIncluirCidade_Click(object sender, EventArgs e)
        {
            Cidade novaCidade = new Cidade(txtNomeCidade.Text, Convert.ToDouble(udX.Text), Convert.ToDouble(udY.Text));
            if (!arvore.Existe(novaCidade))
            {
                pbMapa.Refresh();
                incluir = true;
                // pbMapa.CreateGraphics().FillEllipse(Brushes.Red, ((float)novaCidade.X)*pbMapa.Width, ((float)novaCidade.Y)*pbMapa.Height, 10, 10);
            }
            else
                MessageBox.Show("Cidade já existe!");
        }

        private void btnExcluirCidade_Click(object sender, EventArgs e)
        {
            Cidade cidadeProcurada = new Cidade(txtNomeCidade.Text, 0, 0);
            if (arvore.Existe(cidadeProcurada))
            {
                if (arvore.Atual.Esq == null && arvore.Atual.Dir == null)
                {
                    arvore.ExcluirNaoBalanceado(cidadeProcurada);
                    pbMapa.Refresh();
                }
                else
                    MessageBox.Show("Cidade tem conexões!");
            }
            else
                MessageBox.Show("Cidade não encontrada!");
        }

        private void btnBuscarCidade_Click(object sender, EventArgs e)
        {
            Cidade cidadeProcurada = new Cidade(txtNomeCidade.Text, 0, 0);

            if (arvore.Existe(cidadeProcurada))
            {
                pbMapa.Refresh();
                udX.Value = (decimal)arvore.Atual.Info.X;
                udY.Value = (decimal)arvore.Atual.Info.Y;
                pbMapa.CreateGraphics().FillEllipse(Brushes.Red, ((float)arvore.Atual.Info.X)*pbMapa.Width, ((float)arvore.Atual.Info.Y)*pbMapa.Height, 10, 10);
                List<Ligacao> lista =  cidadeProcurada.ListarLigacaoCidade();
                for(int i = 0; i <= lista.Count - 1; i++)
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

        private void pbMapa_MouseDown(object sender, MouseEventArgs e)
        {
            if (incluir)
            {
                Cidade novaCidade = new Cidade(txtNomeCidade.Text, (double)e.X / pbMapa.Width, (double)e.Y / pbMapa.Height);
                arvore.InserirBalanceado(novaCidade);
                // MessageBox.Show(novaCidade.X.ToString(), novaCidade.Y.ToString());
                pbMapa.Refresh();
                pbMapa.CreateGraphics().FillEllipse(Brushes.Red, ((float)novaCidade.X) * pbMapa.Width, ((float)novaCidade.Y) * pbMapa.Height, 10, 10);
                incluir = false;
            }
            if (alterar)
            {
                // MessageBox.Show(novaCidade.X.ToString(), novaCidade.Y.ToString());
                arvore.Atual.Info.X = (double)e.X / pbMapa.Width;
                arvore.Atual.Info.Y = (double)e.Y / pbMapa.Height;
                pbMapa.Refresh();
                pbMapa.CreateGraphics().FillEllipse(Brushes.Red, ((float)arvore.Atual.Info.X) * pbMapa.Width, ((float)arvore.Atual.Info.Y) * pbMapa.Height, 10, 10);
                alterar = false;
            }
        }
    }
}
