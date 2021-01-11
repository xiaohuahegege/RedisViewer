using RedisViewer.Core;
using System.Threading.Tasks;

namespace RedisViewer.UI.ViewModels
{
    /// <summary>
    /// Key view model (List)
    /// </summary>
    internal class KeyListViewModel : KeyTypeViewModelBase, IKeyListViewModel
    {
        public KeyListViewModel()
        {

        }

        public override void PageLoad()
        {
            PageQuery(PageIndex);
        }

        public override void PageQuery(int pageIndex)
        {
            Task.Run(async () =>
            {
                var result = await Key.GetValueByListAsync(PageSize, PageIndex);

                DispatcherService.BeginInvoke(() =>
                {
                    Values = new KeyListValueCollection(result.values);
                    PageIndex = result.pageIndex;
                    PageCount = result.pageCount;
                });
            });
        }

        private KeyListValueCollection _values;
        public KeyListValueCollection Values
        {
            get => _values;
            set => SetProperty(ref _values, value);
        }
    }
}