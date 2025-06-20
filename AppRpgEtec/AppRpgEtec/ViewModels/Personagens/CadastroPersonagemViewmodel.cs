﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AppRpgEtec.Models;
using AppRpgEtec.Models.Enuns;
using AppRpgEtec.Services.Personagens;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Personagens
{
    public ICommand SalvarCommand { get; }
    public ICommand CancelarCommand { get; set;  }

    [QueryProperty("PersonagemSelecionadoId", "pId")]
    public class CadastroPersonagemViewmodel : BaseViewModel
    {
        private int id;
        private string nome;
        private int pontosVide;
        private int forca;
        private int defesa;
        private int inteligencia;
        private int disputas;
        private int vitorias;
        private int derrotas;

        private PersonagemService pService;

        private string personagemSelecionadoId;
        public string PersonagemSelecionadoId
        {
            set
            {
                if (value != null)
                {
                    personagemSelecionadoId = Uri.UnescapeDataString(value);
                    CarregarPersonagem();

                }
            }
        }

        public CadastroPersonagemViewmodel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            pService = new PersonagemService(token);
            _ = ObterClasses();

            SalvarCommand = new Command(async () => { await SalvarPersonagem(); });
            CancelarCommand = new Command(async () => { await CancelarCadastro(); });
        }

        public async void CarregarPersonagem()
        {
            try
            {
                Personagem p = await.pService.GetPersonagemAsync(int.Parse(personagemSelecionadoId);

                this.Nome = p.Nome;
                this.PontosVida = p.PontosVida;
                this.Defesa = p.Defesa;
                this.Derrotas = p.Derrotas;
                this.Disputas = p.Disputas;
                this.Forca = p.Forca;
                this.Inteligencia = p.Inteligencia;
                this.Vitorias = p.Vitorias;
                this.Id = p.Id;

                TipoClasseSelecionado = this.ListaTiposClasse.FirstOrDefault(tClasse => tClasse.Id == (int)p.Classe);
            }
            catch (Exception ex) {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "ok");
            }
        }

        
        private async Task CancelarCadastro()
        {
            await Shell.Current.GoToAsync("..");
        }

        private TipoClasse tipoClasseSelecionado;
        public TipoClasse TipoClasseSelecionado
        {
            get { return tipoClasseSelecionado; }
            set
            {
                if (value != null)
                {
                    tipoClasseSelecionado = value;
                    OnPropertyChanged();
                }
            }
        }

        public async Task SalvarPersonagem()
        {
            try
            {
                Personagem model = new Personagem()
                {
                    Nome = this.Nome,
                    PontosVida = this.PontosVide,
                    Defesa = this.Defesa,
                    Derrotas = this.Derrotas,
                    Disputas = this.Disputas,
                    Forca = this.Forca,
                    Inteligencia = this.Inteligencia,
                    Vitorias = this.Vitorias,
                    Id = this.Id,
                    Classe = (ClasseEnum)tipoClasseSelecionado.Id
                };
                if (model.Id == 0)
                    await pService.PostPersonagemAsync(model);
                else
                    await pService.PutPersonagemAsync(model);

                await Application.Current.MainPage.DisplayAlert("Mensagem", "Dados salvos com sucesso!", "ok");
                await Shell.Current.GoToAsync(".."); // Remove a página atual da pilha de páginas
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
            }



        public int Id { get => id; set { id = value; OnPropertyChanged(); } }
        public string Nome { get => nome; set { nome = value; OnPropertyChanged(); } }
        public int PontosVida { get => pontosVide; set { pontosVide = value; OnPropertyChanged(); } }
        public int Forca { get => forca; set { forca = value; OnPropertyChanged(); } }
        public int Defesa { get => defesa; set { defesa = value; OnPropertyChanged(); } }
        public int Inteligencia { get => inteligencia; set { inteligencia = value; OnPropertyChanged(); } }
        public int Disputas { get => disputas; set { disputas = value; OnPropertyChanged(); } }
        public int Vitorias { get => vitorias; set { vitorias = value; OnPropertyChanged(); } }
        public int Derrotas { get => derrotas; set { derrotas = value; OnPropertyChanged(); } }

        public ObservableCollection<TipoClasse> ListaTiposClasse { get => listaTiposClasse; set { if (value != null) { listaTiposClasse = value;  OnPropertyChanged();  } } }

        private ObservableCollection<TipoClasse> listaTiposClasse;

        public async Task ObterClasses()
        {
            try
            {
                ListaTiposClasse = new ObservableCollection<TipoClasse>();
                ListaTiposClasse.Add(new TipoClasse() { Id = 1, Descricao = "Cavaleiro" });
                ListaTiposClasse.Add(new TipoClasse() { Id = 2, Descricao = "Mago" });
                ListaTiposClasse.Add(new TipoClasse() { Id = 3, Descricao = "Clerigo" });
                OnPropertyChanged(nameof(ListaTiposClasse));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }
        
    }
}
