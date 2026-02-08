namespace TaskManager.Validations.Common;

public class WarningServiceProvider : IServiceProvider
{
    public object? GetService(Type serviceType)
    {
        // IWarningValidationContext の型を要求されたら、ダミーのインスタンスを返す
        if (serviceType == typeof(IWarningValidationContext))
        {
            return new object();
        }

        // それ以外のサービスは知らないので null を返す
        return null;
    }
}