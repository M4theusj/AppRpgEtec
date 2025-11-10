using AppRpgEtec.ViewModels.Disputas;

namespace AppRpgEtec.Views.Disputas;
// Matheus Jacob Duarte
// Vitor da Luz
public partial class ListagemView : ContentPage
{
    DisputaViewModel viewModel;
    public ListagemView()
	{
		InitializeComponent();

		viewModel = new DisputaViewModel();
		BindingContext = viewModel;
	}
}