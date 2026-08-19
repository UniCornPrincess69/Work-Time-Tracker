using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Collections.ObjectModel;

namespace WorkTimeTracker
{
    public class SaveSystem()
    {
        private readonly string savePath = Path.Combine(FileSystem.AppDataDirectory, "worktimes.json");

        //private List<WorkTimeData> timeList = [];

        private ObservableCollection<WorkTimeData> timeList = [];

        public event EventHandler? DataChanged;


        public void Save(WorkTimeData data)
        {
            if(!timeList.Contains(data))timeList.Add(data);
            string json = JsonSerializer.Serialize<ObservableCollection<WorkTimeData>>(timeList);
            if(File.Exists(savePath)) File.Delete(savePath);
            File.WriteAllText(savePath, json);
            DataChanged?.Invoke(this, EventArgs.Empty);
        }

        public WorkTimeData? Load() 
        {
            if (!File.Exists(savePath)) return null;

            string json = File.ReadAllText(savePath);

            timeList = JsonSerializer.Deserialize<ObservableCollection<WorkTimeData>>(json);

            if(timeList == null || timeList.Count == 0) return null;
            
            return timeList.FirstOrDefault(x => x.EndTime == null);
        }

        public void DeleteData()
        {
            timeList.Clear();
            string json = JsonSerializer.Serialize<ObservableCollection<WorkTimeData>>(timeList);
            if (File.Exists(savePath)) File.Delete(savePath);
            File.WriteAllText(savePath, json);
            DataChanged?.Invoke(this, EventArgs.Empty);
        }

        public ObservableCollection<WorkTimeData> GetTrackedData()
        {
            return timeList;
        }

    }

   
}
