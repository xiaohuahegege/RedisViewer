using Newtonsoft.Json;
using Prism.Mvvm;

namespace RedisViewer.Core
{
    public class InfoBase : BindableBase
    {
        private string _name;

        [JsonProperty("name", Required = Required.Always)]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private bool _isExpanded;

        [JsonIgnore]
        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value);
        }

        private bool _isSelected;

        [JsonIgnore]
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        private int _level = -1;

        [JsonIgnore]
        public int Level
        {
            get => _level;
            set => SetProperty(ref _level, value);
        }

        public bool IsInitialized { get; set; }
    }
}