using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVFoundation;

namespace AppRpgEtec.Models
{
    public class Habilidade
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Dano { get; set; }

        public int PersonagemId { get; set; }
        public Personagem Personagem { get; set; }
        public int HabilidadeId { get; set; }
        public Habilidade Habilidade { get; set; }
        public string HabilidadeNome
        {
            get { return Habilidade.Nome; }
        }
    }
}
