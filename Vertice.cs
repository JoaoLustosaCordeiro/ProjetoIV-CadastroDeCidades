public class Vertice
{
    string rotulo;
    bool foiVisitado;
    private bool estaAtivo;

    public Vertice(string rotulo)
    {
        this.rotulo = rotulo;
        foiVisitado = false;
        estaAtivo = true;
    }
    public string Rotulo => rotulo;
    public bool FoiVisitado { get => foiVisitado; set => foiVisitado = value; }
}

