using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 *Subject interface of observer pattern.
 */
abstract class SignalSource
{
    private List<SignalListener> listeners = new List<SignalListener>();

    public void Attach(SignalListener listener)
    {
        listeners.Add(listener);
    }

    public void Detach(SignalListener listener)
    {
        listeners.Remove(listener);
    }

    public void feedAll(short dotum)
    {
        foreach (SignalListener listener in listeners)
            listener.recieveData(dotum);
    }

    public abstract void start();
    public abstract void stop();
}
