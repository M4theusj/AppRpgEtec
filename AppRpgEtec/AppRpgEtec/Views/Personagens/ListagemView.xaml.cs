namespace AppRpgEtec.Views.Personagens;
using AppRpgEtec.ViewModels;
using AppRpgEtec.ViewModels.Personagens;

public partial class ListagemView : ContentPage
{
	ListagemPersonagemViewModel viewModel;
	public ListagemView()
	{
		InitializeComponent();

		viewModel = new ListagemPersonagemViewModel();
		BindingContext = viewModel;
		Title = "Personagens - App Rpg Etec";
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		_ = viewModel.ObterPesonagens();
    }
}