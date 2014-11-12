namespace System
{
    [Serializable()]
    public delegate void EventHandler(object sender, EventArgs e);

    [Serializable()]
    public delegate void EventHandler<TEventArgs>(object sender, TEventArgs e) where TEventArgs : EventArgs;
}
