using AppRpgEtec.ViewModels.Personagens;

namespace AppRpgEtec.Views.Personagens;

public partial class CadastroPersonagemView : ContentPage
{
	private CadastroPersonagemViewmodel cadViewModel;
	public CadastroPersonagemView()
	{
		InitializeComponent();

		cadViewModel = new CadastroPersonagemViewmodel();
		BindingContext = cadViewModel;
		Title = "novo personagem";
	}
}