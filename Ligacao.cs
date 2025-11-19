using System;

namespace Proj4
{
    public class Ligacao : IComparable<Ligacao>
    {
        string origem, destino;
        int distancia;

        public string Origem { get => origem; set => origem = value; }
        public string Destino { get => destino; set => destino = value; }
        public int Distancia { get => distancia; set => distancia = value; }

        public int CompareTo(Ligacao other)
        {
            return (origem + destino).CompareTo(other.origem + other.destino);
        }
    }
}
