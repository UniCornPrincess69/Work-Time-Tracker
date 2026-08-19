using System.Diagnostics;

namespace WorkTimeTracker
{
    public partial class MainPage : ContentPage
    {
        #region Variables
        private Button startButton;
        private Button stopButton;
        private Button overviewButton;

        private readonly SaveSystem saveSystem;
        private WorkTimeData? currentWorkData;

        private readonly string startButtonName = "StartBtn";
        private readonly string stopButtonName = "StopBtn";
        private readonly string overviewButtonName = "OverviewBtn";

        private static readonly Color startColor = Colors.Green;
        private static readonly Color stopColor = Colors.Red;
        private static readonly Color overviewColor = Colors.DeepSkyBlue;
        private static readonly Color inactiveColor = Colors.DarkGray;

        #endregion

        public MainPage(SaveSystem saveSystem)
        {
            InitializeComponent();
            this.saveSystem = saveSystem;
            currentWorkData = saveSystem.Load();
            ButtonInitialization();
            saveSystem.DataChanged += OnDataChanged;
        }

        private void OnStartClicked(object? sender, EventArgs e)
        {
            //Debug.WriteLine("Start button clicked");
            if (sender != null)
            {
                startButton.IsEnabled = false; // Disable the Start button
                startButton.BackgroundColor = inactiveColor;
                currentWorkData = currentWorkData ?? new WorkTimeData { StartTime = GetCurrentTime() };
                saveSystem.Save(currentWorkData);
                stopButton.IsEnabled = true; // Enable the Stop button
                stopButton.BackgroundColor = stopColor;
                //Debug.WriteLine($"stopColor: {stopColor}");
                //Debug.WriteLine($"buttonColor: {stopButton.BackgroundColor}");
            }
        }

        private void OnStopClicked(object? sender, EventArgs e)
        {
           //Debug.WriteLine("Stop button clicked");
            if (sender != null)
            {
                stopButton.IsEnabled = false; // Disable the Stop button
                stopButton.BackgroundColor = inactiveColor;
                currentWorkData.EndTime = GetCurrentTime();
                saveSystem.Save(currentWorkData);
                currentWorkData = null;
                startButton.IsEnabled = true; // Enable the Start button
                startButton.BackgroundColor = startColor;
                overviewButton.IsEnabled = true;
                overviewButton.BackgroundColor = overviewColor;
            }
        }

        private async void OnOverviewClicked(object? sender, EventArgs e)
        {
            
            await Shell.Current.GoToAsync(nameof(OverviewPage));
        }

        private void OnExitClicked(object? sender, EventArgs e)
        {
            Application.Current.Quit();
        }

        private void ButtonInitialization()
        {
            startButton = this.FindByName<Button>(startButtonName);
            stopButton = this.FindByName<Button>(stopButtonName);
            overviewButton = this.FindByName<Button>(overviewButtonName);
            if (currentWorkData != null)
            {
                startButton.IsEnabled = false;
                startButton.BackgroundColor = inactiveColor;
                stopButton.IsEnabled= true;
                stopButton.BackgroundColor = stopColor;
            }
            if (startButton.IsEnabled)
            {
                stopButton.IsEnabled= false;
                stopButton.BackgroundColor = inactiveColor;
            }
            if (saveSystem.GetTrackedData().Count == 0) 
            { 
                overviewButton.IsEnabled = false;
                overviewButton.BackgroundColor = inactiveColor;
            }
            
        }


        private DateTime GetCurrentTime()
        {
            DateTime now = DateTime.Now;

            return new DateTime(
                now.Year,
                now.Month,
                now.Day,
                now.Hour,
                now.Minute,
                0);
        }

        private void OnDataChanged(object? sender, EventArgs e)
        {
            overviewButton.IsEnabled = true;
            overviewButton.BackgroundColor = overviewColor;
        }
    }
}
