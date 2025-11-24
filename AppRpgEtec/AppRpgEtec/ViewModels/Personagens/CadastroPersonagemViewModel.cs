using AppRpgEtec.Models;
using AppRpgEtec.Models.Enuns;
using AppRpgEtec.Services.Personagens;
using Microsoft.Maui.Graphics.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Personagens
{
    [QueryProperty("PersonagemSelecionadoId", "pId")]
    public class CadastroPersonagemViewModel : BaseViewModel
    {
        private PersonagemService pService;
        public ICommand SalvarCommand { get; }
        public ICommand CancelarCommand { get; set; }
        

        public CadastroPersonagemViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            pService = new PersonagemService(token);

            _ = ObterClasses();
            SalvarCommand = new Command(async () => { await SalvarPersonagem(); }, () => ValidarCampos());
            CancelarCommand = new Command(async => CancelarCadastro());
        }

        private async void CancelarCadastro()
        {
            await Shell.Current.GoToAsync("..");
        }

        private int id;
        private string nome;
        private int pontosVida;
        private int forca;
        private int defesa;
        private int inteligencia;
        private int disputas;
        private int vitorias;
        private int derrotas;

        public bool CadastroHabilitado
        {
            get
            {
                return PontosVida > 0;
            }
        }

        public int PontosVida
        {
            get => pontosVida;
            set
            {
                pontosVida = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CadastroHabilitado));
            }
        }

        public bool ValidarCampos()
        {
            return !string.IsNullOrEmpty(Nome)
                && CadastroHabilitado
                && Forca != 0
                && Defesa != 0;
        }

        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged(nameof(Id));//Informa mundaça de estado para a View para ViewModel ou vice-versa de acordo com a herança da BaseViewModel
            }
        }

        public string Nome
        {
            get => nome;
            set
            {
                nome = value;
                OnPropertyChanged();
                ((Command)SalvarCommand).ChangeCanExecute();
            }
        }

        public int Forca
        {
            get => forca;
            set
            {
                forca = value;
                OnPropertyChanged(nameof(Forca));
            }
        }

        public int Defesa
        {
            get => defesa;
            set
            {
                defesa = value;
                OnPropertyChanged(nameof(Defesa));
            }
        }

        public int Inteligencia
        {
            get => inteligencia;
            set
            {
                inteligencia = value;
                OnPropertyChanged(nameof(Inteligencia));
            }
        }

        public int Disputas
        {
            get => disputas;
            set
            {
                disputas = value;
                OnPropertyChanged(nameof(Disputas));
            }
        }

        public int Vitorias
        {
            get => vitorias;
            set
            {
                vitorias = value;
                OnPropertyChanged(nameof(Vitorias));
            }
        }

        public int Derrotas
        {
            get => derrotas;
            set
            {
                derrotas = value;
                OnPropertyChanged(nameof(Derrotas));
            }
        }

        public class PontosVidaConverer : IValueConverter
        {
            public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            {
                ColorTypeConverter converter = new ColorTypeConverter();

                int pontosVida = (int)value;
                if (pontosVida == 100)
                    return (Color)converter.ConvertFromInvariantString("SeaGreen");
                else if (pontosVida >= 75)
                    return (Color)converter.ConvertFromInvariantString("YellowGreen");
                else if (pontosVida >= 25)
                    return (Color)converter.ConvertFromInvariantString("Yellow");
                else if (pontosVida >= 1)
                    return (Color)converter.ConvertFromInvariantString("OrangeRed");
                else
                    return (Color)converter.ConvertFromInvariantString("Red");
            }

            public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        private ObservableCollection<TipoClasse> listaTiposClasse;
        public ObservableCollection<TipoClasse> ListaTiposClasse
        {
            get { return listaTiposClasse; }
            set
            {
                if (value != null)
                {
                    listaTiposClasse = value;
                    OnPropertyChanged(nameof(ListaTiposClasse));
                }
            }
        }

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
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
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
                    OnPropertyChanged(nameof(TipoClasseSelecionado));
                }
            }
        }

        public async Task SalvarPersonagem()
        {
            try
            {
                Personagem model = new Personagem()
                {
                    Nome = this.nome,
                    PontosVida = this.pontosVida,
                    Defesa = this.defesa,
                    Derrotas = this.derrotas,
                    Disputas = this.disputas,
                    Forca = this.forca,
                    Inteligencia = this.inteligencia,
                    Vitorias = this.vitorias,
                    Id = this.id,
                    Classe = (ClasseEnum)tipoClasseSelecionado.Id
                };
                if (model.Id == 0)
                    await pService.PostPersonagemAsync(model);               
                else
                    await pService.PutPersonagemAsync(model);

                await Application.Current.MainPage
                       .DisplayAlert("Mensagem", "Dados salvos com sucesso!", "Ok");

                await Shell.Current.GoToAsync(".."); //Remove a página atual da pilha de páginas                 
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }
        public async void CarregarPersonagem()
        {
            try
            {
                Personagem p = await pService.GetPersonagemAsync(int.Parse(personagemSelecionadoId));

                this.Nome = p.Nome;
                this.PontosVida = p.PontosVida;
                this.Defesa = p.Defesa;
                this.Derrotas = p.Derrotas;
                this.Disputas = p.Disputas;
                this.Forca = p.Forca;
                this.Inteligencia = p.Inteligencia;
                this.Vitorias = p.Vitorias;
                this.Id = p.Id;

                TipoClasseSelecionado = this.ListaTiposClasse
                .FirstOrDefault(tClasse => tClasse.Id == (int)p.Classe);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        private string personagemSelecionadoId;//CTRL + R,E
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





    }
}
