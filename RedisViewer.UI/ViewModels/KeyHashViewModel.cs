namespace RedisViewer.UI.ViewModels
{
    internal class KeyHashViewModel : KeyTypeViewModelBase, IKeyHashViewModel
    {
        public KeyHashViewModel()
        {

        }

        public override void PageLoad()
        {
            PageQuery(PageIndex);
        }

        public override void PageQuery(int pageIndex)
        {

        }
    }
}