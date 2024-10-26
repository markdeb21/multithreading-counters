namespace MultithreadingCounters;
using System.Threading;

public class Counter
{
    private int _value;
    private int _interval;
    private bool _isRunning;
    private Thread _counterThread;

    public Counter(int interval)
    {
        _counterThread = new Thread(() => { });
        _interval = interval * 1000;
    }

    public void Start()
    {
        if (_isRunning) return;

        _isRunning = true;
        _counterThread = new Thread(() =>
        {
            while (_isRunning)
            {
                _value++;
                Thread.Sleep(_interval);
            }
        });
        _counterThread.Start();
    }

    public void Stop()
    {
        _isRunning = false;
        _counterThread?.Join();
    }

    public int GetValue() => _value;
}
