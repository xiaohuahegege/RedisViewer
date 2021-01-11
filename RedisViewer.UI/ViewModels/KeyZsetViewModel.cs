using RedisViewer.Core;
using System.Threading.Tasks;

namespace RedisViewer.UI.ViewModels
{
    internal class KeyZsetViewModel : KeyTypeViewModelBase, IKeyZsetViewModel
    {
        public KeyZsetViewModel()
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
                var result = await Key.GetValueByZsetAsync(PageSize, PageIndex);

                DispatcherService.BeginInvoke(() =>
                {
                    Values = new KeyZsetValueCollection(result.values);
                    PageIndex = result.pageIndex;
                    PageCount = result.pageCount;
                });
            });
        }

        private KeyZsetValueCollection _values;
        public KeyZsetValueCollection Values
        {
            get => _values;
            set => SetProperty(ref _values, value);
        }
    }
}