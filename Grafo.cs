using System;
using System.Collections.Generic;
using System.Windows.Forms;

public class Grafo
{
    const int Max_Vertices = 80;    // tamanho físico máximo
    Vertice[] vertices;
    int quantosVertices;
    int[,] matrizDeAjacencias;

    /// DIJKSTRA
    DistOriginal[] percurso;
    int infinity = int.MaxValue;
    int verticeAtual; // global que indica o vértice atualmente sendo visitado 
    int doInicioAteAtual; // global usada para ajustar menor caminho com Djikstra
    int nTree;

    public Grafo()
    {
        vertices = new Vertice[Max_Vertices];
        matrizDeAjacencias = new int[Max_Vertices, Max_Vertices];
        quantosVertices = 0;    // tamanho lógico
        nTree = 0;
        for (int i = 0; i < Max_Vertices; i++)
            for (int j = 0; j < Max_Vertices; j++)
                matrizDeAjacencias[i, j] = infinity; // distância tão grande que não existe

        percurso = new DistOriginal[Max_Vertices];
    }

    public void NovoVertice(string nome)
    {
        vertices[quantosVertices++] = new Vertice(nome);

        //if (dgv != null) // se foi passado como parâmetro um dataGridView para exibição
        //{                // suas dimensões são ajustadas para a quantidade de vértices
        //    dgv.RowCount = quantosVertices + 1;
        //    dgv.ColumnCount = quantosVertices + 1;
        //    dgv.Columns[quantosVertices].Width = 45;
        //}
    }

    public void NovaAresta(int origem, int destino, int custo)
    {
        if (origem < 0 || origem >= quantosVertices || destino < 0 || destino >= quantosVertices)
            throw new Exception("Índice de origem e/ou destino inválido!");

        matrizDeAjacencias[origem, destino] = custo;
        // matrizDeAjacencias[destino, origem] = custo; // gera ciclos
    }

    public void ExibirVertice(int v)
    {
        Console.Write(vertices[v].Rotulo + " ");
    }

    public void ExibirVertice(int v, TextBox txt)
    {
        txt.Text += vertices[v].Rotulo + " ";
    }

    public int SemSucessores() // encontra e retorna a linha de um vértice sem sucessores
    {
        bool temSucessor;
        for (int linha = 0; linha < quantosVertices; linha++)
        {
            temSucessor = false;
            for (int col = 0; col < quantosVertices; col++)
                if (matrizDeAjacencias[linha, col] > 0)
                {
                    temSucessor = true;
                    break;
                }
            if (!temSucessor)
                return linha;
        }
        return -1;
    }

    //public void RemoverVertice(int vert)
    //{
    //    if (dgv != null)
    //    {
    //        MessageBox.Show($"Matriz de Adjacências antes de remover vértice {vert}");
    //        ExibirAdjacencias();
    //    }
    //    if (vert != quantosVertices - 1)
    //    {
    //        for (int j = vert; j < quantosVertices - 1; j++)// remove vértice do vetor
    //            vertices[j] = vertices[j + 1];
    //        // remove vértice da matriz
    //        for (int row = vert; row < quantosVertices; row++)
    //            MoverLinhas(row, quantosVertices - 1);
    //        for (int col = vert; col < quantosVertices; col++)
    //            MoverColunas(col, quantosVertices - 1);
    //    }
    //    quantosVertices--;
    //    if (dgv != null)
    //    {
    //        MessageBox.Show($"Matriz de Adjacências após remover vértice {vert}");
    //        ExibirAdjacencias();
    //        MessageBox.Show("Retornando à ordenação");
    //    }
    //}

    private void MoverLinhas(int row, int length)
    {
        if (row != quantosVertices - 1)
            for (int col = 0; col < length; col++)
                matrizDeAjacencias[row, col] = matrizDeAjacencias[row + 1, col]; // desloca para excluir
    }

    private void MoverColunas(int col, int length)
    {
        if (col != quantosVertices - 1)
            for (int row = 0; row < length; row++)
                matrizDeAjacencias[row, col] = matrizDeAjacencias[row, col + 1]; // desloca para excluir
    }

    //public void ExibirAdjacencias()
    //{
    //    dgv.RowCount = quantosVertices + 1;
    //    dgv.ColumnCount = quantosVertices + 1;
    //    for (int j = 0; j < quantosVertices; j++)
    //    {
    //        dgv.Rows[j + 1].Cells[0].Value = vertices[j].Rotulo;
    //        dgv.Rows[0].Cells[j + 1].Value = vertices[j].Rotulo;
    //        for (int k = 0; k < quantosVertices; k++)
    //            dgv.Rows[j + 1].Cells[k + 1].Value = Convert.ToString(matrizDeAjacencias[j, k]);
    //    }
    //}

    //public String OrdenacaoTopologica()
    //{
    //    Stack<String> gPilha = new Stack<String>(); //guarda a sequência de vértices
    //    int origVerts = quantosVertices;
    //    while (quantosVertices > 0)
    //    {
    //        int indiceDeVerticeSemSucessores = SemSucessores();
    //        if (indiceDeVerticeSemSucessores == -1)
    //            return "Erro: grafo possui ciclos.";
    //        gPilha.Push(vertices[indiceDeVerticeSemSucessores].Rotulo); // empilha vértice
    //        RemoverVertice(indiceDeVerticeSemSucessores);
    //    }
    //    String resultado = "Sequência da Ordenação Topológica: ";
    //    while (gPilha.Count > 0)
    //        resultado += gPilha.Pop() + " "; // desempilha para exibir
    //    return resultado;
    //}

    private int ObterVerticeAdjacenteNaoVisitado(int v)
    {
        for (int j = 0; j <= quantosVertices - 1; j++)
            if ((matrizDeAjacencias[v, j] == 1) && (!vertices[j].FoiVisitado))
                return j;
        return -1;
    }

    public void PercursoEmProfundidade(TextBox txt)
    {
        txt.Clear();
        Stack<int> gPilha = new Stack<int>(); // para guardar a sequência de vértices
        vertices[0].FoiVisitado = true;
        ExibirVertice(0, txt);
        gPilha.Push(0);
        int v;
        while (gPilha.Count > 0)
        {
            v = ObterVerticeAdjacenteNaoVisitado(gPilha.Peek());
            if (v == -1)
                gPilha.Pop();   // desempilhar
            else
            {
                vertices[v].FoiVisitado = true;
                ExibirVertice(v, txt);
                gPilha.Push(v);         // empilhar
            }
        }
        for (int j = 0; j <= quantosVertices - 1; j++)
            vertices[j].FoiVisitado = false;
    }

    public void percursoPorLargura(TextBox txt)
    {
        txt.Clear();
        Queue<int> gQueue = new Queue<int>();
        vertices[0].FoiVisitado = true;
        ExibirVertice(0, txt);
        gQueue.Enqueue(0);
        int vert1, vert2;
        while (gQueue.Count > 0)
        {
            vert1 = gQueue.Dequeue();       // desenfileirar / retirar
            vert2 = ObterVerticeAdjacenteNaoVisitado(vert1);
            while (vert2 != -1)
            {
                vertices[vert2].FoiVisitado = true;
                ExibirVertice(vert2, txt);
                gQueue.Enqueue(vert2);        // enfileirar
                vert2 = ObterVerticeAdjacenteNaoVisitado(vert1);
            }
        }
        for (int i = 0; i < quantosVertices; i++)
            vertices[i].FoiVisitado = false;
    }

    public int ObterMenor()
    {
        int distanciaMinima = infinity;
        int indiceDaMinima = 0;
        for (int j = 0; j < quantosVertices; j++)
            if (!(vertices[j].FoiVisitado) && (percurso[j].distancia < distanciaMinima))
            {
                distanciaMinima = percurso[j].distancia;
                indiceDaMinima = j;
            }
        return indiceDaMinima;
    }

    public void AjustarMenorCaminho()
    {
        for (int coluna = 0; coluna < quantosVertices; coluna++)
        {
            if (!vertices[coluna].FoiVisitado) // para cada vértice ainda não visitado
            {
                // acessamos a distância desde o vértice atual (pode ser infinity)
                int atualAteMargem = matrizDeAjacencias[verticeAtual, coluna];
                // calculamos a distância desde inicioDoPercurso passando por vertice atual 
                // até esta saída
                int doInicioAteMargem = doInicioAteAtual + atualAteMargem;
                // quando encontra uma distância menor, marca o vértice a partir do
                // qual chegamos no vértice de índice coluna, e a soma da distância
                // percorrida para nele chegar
                int distanciaDoCaminho = percurso[coluna].distancia;
                if (doInicioAteMargem < distanciaDoCaminho)
                {
                    percurso[coluna].verticePai = verticeAtual;
                    percurso[coluna].distancia = doInicioAteMargem;
                }
            }
        }

    }

    public List<(string Rotulo, int Distancia)> Caminho(string rotuloInicio, string rotuloFim)
    {
        int indiceInicio = ObterIndiceVertice(rotuloInicio.Trim());
        int indiceFim = ObterIndiceVertice(rotuloFim.Trim());

        if (indiceInicio == -1 || indiceFim == -1)
            return null; 

        foreach (var vertice in vertices)
        {
            if (vertice == null) break;
            vertice.FoiVisitado = false;
        }

        vertices[indiceInicio].FoiVisitado = true; 

        for (int j = 0; j < quantosVertices; j++)
        {
            int distTemp = matrizDeAjacencias[indiceInicio, j];
            percurso[j] = new DistOriginal(indiceInicio, distTemp);
        }

        for (int i = 0; i < quantosVertices; i++)
        {
            int menorIndice = ObterMenor(); 

            if (menorIndice == -1)
                break; 

            verticeAtual = menorIndice;
            doInicioAteAtual = percurso[menorIndice].distancia;
            vertices[menorIndice].FoiVisitado = true;

            AjustarMenorCaminho();
        }

        if (percurso[indiceFim].distancia == infinity)
        {
            return null; 
        }

        List<(string Rotulo, int Distancia)> caminhos = new List<(string Rotulo, int Distancia)>();
        int atual = indiceFim;

        while (atual != indiceInicio)
        {
            caminhos.Add((vertices[atual].Rotulo, percurso[atual].distancia));
            atual = percurso[atual].verticePai; 
        }

        caminhos.Add((vertices[indiceInicio].Rotulo, 0));

        caminhos.Reverse();

        return caminhos;
    }

    public int ObterIndiceVertice(string rotulo)
    {
        if (rotulo == null)
            return -1;
        for (int i = 0; i < quantosVertices; i++)
        {
            if (vertices[i].Rotulo == rotulo)
                return i;
        }

        return -1; 
    }
}
