using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Collections.ObjectModel;

namespace WorkTimeTracker
{
    public class SaveSystem()
    {
        #region Variables
        private readonly string savePath = Path.Combine(FileSystem.AppDataDirectory, "worktimes.json");

        private ObservableCollection<WorkTimeData> timeList = [];

        public event EventHandler? DataChanged;
        #endregion

        /// <summary>
        /// Function to save the work time data
        /// </summary>
        /// <param name="data">Current work time data</param>
        public void Save(WorkTimeData data)
        {
            if(!timeList.Contains(data))timeList.Add(data);
            string json = JsonSerializer.Serialize<ObservableCollection<WorkTimeData>>(timeList);
            if(File.Exists(savePath)) File.Delete(savePath);
            File.WriteAllText(savePath, json);
            DataChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Function to load the current unfinished work time data, without EndTime
        /// </summary>
        /// <returns>WorkTimeData</returns>
        public WorkTimeData? Load() 
        {
            if (!File.Exists(savePath)) return null;

            string json = File.ReadAllText(savePath);

            timeList = JsonSerializer.Deserialize<ObservableCollection<WorkTimeData>>(json);

            if(timeList == null || timeList.Count == 0) return null;
            
            return timeList.FirstOrDefault(x => x.EndTime == null);
        }

        /// <summary>
        /// Deletes all the data in the json file
        /// </summary>
        public void DeleteData()
        {
            timeList.Clear();
            string json = JsonSerializer.Serialize<ObservableCollection<WorkTimeData>>(timeList);
            if (File.Exists(savePath)) File.Delete(savePath);
            File.WriteAllText(savePath, json);
            DataChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Getter for the complete tracked time list.
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<WorkTimeData> GetTrackedData()
        {
            return timeList;
        }

    }

   
}
