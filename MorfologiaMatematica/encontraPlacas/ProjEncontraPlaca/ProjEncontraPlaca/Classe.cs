namespace ProjEncontraPlaca
{
    internal class Classe
    {
        public char caractere { get; set; }
        public int n_dim { get; set; }
        public int n_restricoes { get; set; }
        public NaoOcorre[] NOC { get; set; }

        public Classe(char caractere, int n_dim)
        {
            this.caractere = caractere;
            this.n_dim = n_dim;
            n_restricoes = 0;
            NOC = new NaoOcorre[n_dim];
            for (int i = 0; i < n_dim; i++)
                NOC[i] = new NaoOcorre();
        }
    }
}