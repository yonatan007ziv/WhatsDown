namespace WhatsDown.WPF.Interfaces;

interface IResultCommunicator<T>
{
    void SetResult(T result);
}