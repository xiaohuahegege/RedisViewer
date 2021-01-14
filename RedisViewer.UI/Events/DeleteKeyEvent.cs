using Prism.Events;
using RedisViewer.Core;

namespace RedisViewer.UI.Events
{
    internal class DeleteKeyEvent : PubSubEvent<KeyInfo>
    {

    }
}