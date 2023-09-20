namespace WhatsDown.Server.Interfaces.Services.Factories;

internal interface IFactory<TParam, TResult>
{
    TResult Create(TParam param);
}