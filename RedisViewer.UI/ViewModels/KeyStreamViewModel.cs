namespace RedisViewer.UI.ViewModels
{
    internal class KeyStreamViewModel : KeyTypeViewModelBase, IKeyStreamViewModel
    {
        public KeyStreamViewModel()
        {

        }

        public override void PageLoad()
        {
            PageQuery(PageIndex);
        }

        public override void PageQuery(int pageIndex)
        {
           // Key.GetValueByStreamAsync();
        }

        //public KeyStreamViewModel()
        //{

        //}

        //public override void Navigated()
        //{
        //    LoadAsync();
        //}

        //public async void LoadAsync()
        //{
        //    Values = await Key.GetValueByStreamAsync(1000);
        //}

        //private StreamEntry[] _values;
        //public StreamEntry[] Values
        //{
        //    get => _values;
        //    set => SetProperty(ref _values, value);
        //}
    }
}