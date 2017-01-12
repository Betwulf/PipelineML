using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestOwin.Hubs
{
    //[Authorize]
    public class CounterHub : Hub
    {
        static int _counter = 0;

        public void RecordHit()
        {
            _counter++;
            Clients.All.OnHit(_counter);
        }
    }
}
