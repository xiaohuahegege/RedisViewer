using Prism.Events;
using Prism.Navigation;
using RedisViewer.Core;
using System;

namespace Prism.Mvvm
{
    public abstract class ViewModelBase : BindableBase, IDestructible
    {
        private readonly IEventAggregator _eventAggregator;

        protected ViewModelBase()
        {
            _eventAggregator = UnityResolver.Resolve<IEventAggregator>();
        }

        public virtual void Destroy()
        {

        }

        public void PublishEvent<T>() where T : PubSubEvent, new()
        {
            _eventAggregator.GetEvent<T>().Publish();
        }

        public void PublishEvent<T>(T payload) where T : PubSubEvent<T>, new()
        {
            _eventAggregator.GetEvent<T>().Publish(payload);
        }

        //public void SubscribeEvent<T>(Action action) where T : PubSubEvent, new()
        //{
        //    _eventAggregator.GetEvent<T>().Subscribe(action);
        //}

        public void SubscribeEvent<T>(Action<T> action) where T : PubSubEvent<T>, new()
        {
            _eventAggregator.GetEvent<T>().Subscribe(action);
        }

        public void ShowLoading(bool isShow)
        {
            IsShowLoading = isShow;
        }

        private bool _isShowLoading;
        public bool IsShowLoading
        {
            get => _isShowLoading;
            set => SetProperty(ref _isShowLoading, value);
        }
    }
}