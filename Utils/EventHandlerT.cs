using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public delegate void EventHandlerT<TEventArgsValue>(object sender, EventArgs<TEventArgsValue> e);
}
