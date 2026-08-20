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
        private IDispatcherTimer? elapsedTimer;

        private readonly string startButtonName = "StartBtn";
        private readonly string stopButtonName = "StopBtn";
        private readonly string overviewButtonName = "OverviewBtn";
        private readonly string statusWork = "Beschäftigt";
        private readonly string statusIdle = "Nicht beschäftigt";
        private readonly string timerReset = "00:00:00";

        private static readonly Color startColor = Colors.Green;
        private static readonly Color stopColor = Colors.Red;
        private static readonly Color overviewColor = Colors.DeepSkyBlue;
        private static readonly Color inactiveColor = Colors.DarkGray;

        private WorkStatus currentStatus =>
            currentWorkData is not null && currentWorkData.EndTime is null
            ? WorkStatus.Working
            : WorkStatus.NotWorking;
        #endregion

        public MainPage(SaveSystem saveSystem)
        {
            InitializeComponent();
            elapsedTimer = Dispatcher.CreateTimer();
            elapsedTimer.Interval = TimeSpan.FromSeconds(1);
            elapsedTimer.Tick += OnElapsedTimerTick;
            this.saveSystem = saveSystem;
            currentWorkData = saveSystem.Load();
            ButtonInitialization();
            UpdateStatus();
            saveSystem.DataChanged += OnDataChanged;
            if(currentStatus == WorkStatus.Working) elapsedTimer?.Start();
        }

        private void OnStartClicked(object? sender, EventArgs e)
        {
            //Debug.WriteLine("Start button clicked");
            if (sender != null)
            {
                startButton.IsEnabled = false; // Disable the Start button
                startButton.BackgroundColor = inactiveColor;
                currentWorkData = new WorkTimeData { StartTime = GetCurrentTime() };
                saveSystem.Save(currentWorkData);
                elapsedTimer?.Start();
                stopButton.IsEnabled = true; // Enable the Stop button
                stopButton.BackgroundColor = stopColor;
                UpdateStatus();
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
                elapsedTimer?.Stop();
                ElapsedTimeLabel.Text = timerReset;
                currentWorkData = null;
                startButton.IsEnabled = true; // Enable the Start button
                startButton.BackgroundColor = startColor;
                overviewButton.IsEnabled = true;
                overviewButton.BackgroundColor = overviewColor;
                UpdateStatus();
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
                stopButton.IsEnabled = true;
                stopButton.BackgroundColor = stopColor;
            }
            if (startButton.IsEnabled)
            {
                stopButton.IsEnabled = false;
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
                now.Second);
        }

        private void OnDataChanged(object? sender, EventArgs e)
        {
            overviewButton.IsEnabled = true;
            overviewButton.BackgroundColor = overviewColor;
        }

        private void UpdateStatus()
        {
            switch (currentStatus)
            {
                case WorkStatus.Working:
                    StatusLabel.Text = statusWork;
                    break;

                case WorkStatus.NotWorking:
                    StatusLabel.Text = statusIdle;
                    break;
            }
        }

        private void OnElapsedTimerTick(object? sender, EventArgs e)
        {
            if (currentWorkData is null) return;

            TimeSpan elapsed = GetCurrentTime() - currentWorkData.StartTime;

            ElapsedTimeLabel.Text = elapsed.ToString(@"hh\:mm\:ss");

            Debug.WriteLine(elapsed.ToString());
        }

    }
}
