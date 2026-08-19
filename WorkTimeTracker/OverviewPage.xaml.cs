namespace WorkTimeTracker;

public partial class OverviewPage : ContentPage
{

	private readonly SaveSystem _saveSystem;
	
	public OverviewPage(SaveSystem saveSystem)
	{
		InitializeComponent();
		_saveSystem = saveSystem;
		BindingContext = _saveSystem.GetTrackedData();
	}

	private void OnDeleteClicked(object sender, EventArgs e)
	{
		_saveSystem.DeleteData();

		//OverviewList.ItemsSource = null;
		//OverviewList.ItemsSource = _saveSystem.GetTrackedData();
	}
}