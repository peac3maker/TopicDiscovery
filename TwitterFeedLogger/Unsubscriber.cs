using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterFeedLogger
{
    class Unsubscriber : IDisposable
    {
        private List<IObserver<TweetItem>> observerList;
        private IObserver<TweetItem> observer;

        public Unsubscriber(IObserver<TweetItem> observer, 
                            List<IObserver<TweetItem>> observerList)
        {
            this.observer = observer;
            this.observerList = observerList;
        }

        public void Dispose()
        {
            if (this.observerList != null)
            {
                if (observerList.Contains(observer))
                {
                    observerList.Remove(observer);
                }
                observer = null;
            }
        }
    }
}
