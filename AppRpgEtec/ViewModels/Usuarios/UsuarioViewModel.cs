using AppRpgEtec.Models;
using AppRpgEtec.Services.Usuarios;
using AppRpgEtec.Views.Personagens;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace AppRpgEtec.ViewModels.Usuarios
{
    public class UsuarioViewModel : BaseViewModel
    {
        private readonly UsuarioService _uService;

        public ICommand AutenticarCommand { get; private set; }
        public ICommand RegistrarCommand { get; private set; }
        public ICommand DirecionarCadastroCommand { get; private set; }

        public UsuarioViewModel()
        {
            _uService = new UsuarioService();
            InicializarCommands();
        }

        private void InicializarCommands()
        {
            AutenticarCommand = new Command(async () => await AutenticarUsuario());
            RegistrarCommand = new Command(async () => await RegistrarUsuario());
            DirecionarCadastroCommand = new Command(async () => await DirecionarParaCadastro());
        }

        #region Atributos e Propriedades

        private string login = string.Empty;
        public string Login
        {
            get => login;
            set
            {
                login = value;
                OnPropertyChanged();
            }
        }

        private string senha = string.Empty;
        public string Senha
        {
            get => senha;
            set
            {
                senha = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Métodos

        public async Task AutenticarUsuario()
        {
            try
            {
                var u = new Usuario
                {
                    Username = Login,
                    PasswordString = Senha
                };

                Usuario uAutenticado = await _uService.PostAutenticarUsuarioAsync(u);

                if (uAutenticado.Id != 0)
                {
                    Preferences.Set("UsuarioToken", uAutenticado.Token);
                    Preferences.Set("UsuarioId", uAutenticado.Id);
                    Preferences.Set("UsuarioUsername", uAutenticado.Username);
                    Preferences.Set("UsuarioPerfil", uAutenticado.Perfil);

                    await Application.Current.MainPage
                        .DisplayAlert("Bem‑vindo", $"Olá, {uAutenticado.Username}!", "OK");

                    // Após login bem‑sucedido, aí sim redefinimos a MainPage:
                    Application.Current.MainPage = new AppShell();
                }
                else
                {
                    await Application.Current.MainPage
                        .DisplayAlert("Informação", "Usuário ou senha incorretos.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro",
                    ex.Message + "\n" + ex.InnerException, "OK");
            }
        }

        public async Task RegistrarUsuario()
        {
            try
            {
                var u = new Usuario
                {
                    Username = Login,
                    PasswordString = Senha
                };

                Usuario uRegistrado = await _uService.PostRegistrarUsuarioAsync(u);

                if (uRegistrado.Id != 0)
                {
                    await Application.Current.MainPage
                        .DisplayAlert("Sucesso",
                            $"Usuário (Id {uRegistrado.Id}) registrado com sucesso.", "OK");

                    // volta para a página de login
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Erro",
                        ex.Message + "\n" + ex.InnerException, "OK");
            }
        }

        public async Task DirecionarParaCadastro()
        {
            try
            {
                await Application.Current.MainPage
                    .Navigation.PushAsync(new Views.Usuarios.CadastroView());
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Erro",
                        ex.Message + "\n" + ex.InnerException, "OK");
            }
        }

        #endregion
    }
}
