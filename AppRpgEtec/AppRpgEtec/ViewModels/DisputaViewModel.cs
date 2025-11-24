using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppRpgEtec.Models;
using AppRpgEtec.Services.Disputas;
using AppRpgEtec.Services.PersonagemHabilidades;
using AuthenticationServices;

namespace AppRpgEtec.ViewModels
{
    public class DisputaViewModel
    {
        public Personagem Oponente { get; set; }
        private DisputaService dService;
        public DisputaService DisputaPersonagens {  get; set; }
        private PersonagemHabilidadeService phService;
        public ObservableCollection<PersonagemHabilidade> Habilidades { get; set; }

        public DisputaViewModel()
        {
            string token = Application.Current.Properties["UsuarioToken"].ToString();
            dService = new DisputaService(token);

            Atacante = new Personagem();
            Oponente = new Personagem();
            DisputaPersonagens = new DisputaService();
            phService = new PersonagemHabilidadeService(token);
        }

        public async Task ObterHabilidadesAsync(int personagemId)
        {
            try
            {
                Habilidades = await phService.GetPersonagemHabilidadesAsync(personagemId);
                OnPropertyChanged(nameof(Habilidades));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async Task ExecutarDisputaArmada()
        {
            try
            {
                DisputaPersonagens.AtacanteId = Atacante.Id;
                DisputaPersonagens.OponenteId = Oponente.Id;
                DisputaPersonagens = await dService.PostDisputaGeralAsync(DisputaPersonagens);

                await Application.Current.MainPage
                    .DisplayAlert("Result", DisputaPersonagens.Narracao, "ok");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "ok");
            }
        }
    }
}
