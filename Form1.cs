using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Proj4
{
    public partial class Form1 : Form
    {
        ArvoreAVL<Cidade>  arvore = new ArvoreAVL<Cidade>();
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
                pbMapa.CreateGraphics().FillEllipse(Brushes.Red, (float)novaCidade.X, (float)novaCidade.Y, 10, 10);
                arvore.InserirBalanceado(novaCidade);
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
                pbMapa.CreateGraphics().FillEllipse(Brushes.Red, (float)arvore.Atual.Info.X, (float)arvore.Atual.Info.Y, 10, 10);
            }
            else
                MessageBox.Show("Cidade não encontrada!");
        }

        private void btnAlterarCidade_Click(object sender, EventArgs e)
        {
            Cidade alteraCidade = new Cidade(txtNomeCidade.Text, 0, 0);
            if (arvore.Existe(alteraCidade))
            {
                arvore.Atual.Info.X = Convert.ToDouble(udX.Text);
                arvore.Atual.Info.Y = Convert.ToDouble(udY.Text);
                pbMapa.Refresh();
                pbMapa.CreateGraphics().FillEllipse(Brushes.Red, (float)arvore.Atual.Info.X, (float)arvore.Atual.Info.Y, 10, 10);
            }
            else
                MessageBox.Show("Cidade não encontrada!");
        }
    }
}
