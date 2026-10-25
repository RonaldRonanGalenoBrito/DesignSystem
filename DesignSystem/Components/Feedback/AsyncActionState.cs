using DesignSystem.Enums;

namespace DesignSystem.Components.Feedback;

/// <summary>Representa o carregamento e o feedback visual de uma ação assíncrona.</summary>
public class AsyncActionState
{
    public bool IsLoading { get; private set; }
    public bool IsAlertVisible { get; set; }
    public string Message { get; private set; } = string.Empty;
    public AlertVariant Variant { get; private set; } = AlertVariant.Success;

    public void Begin()
    {
        IsLoading = true;
        IsAlertVisible = false;
        Message = string.Empty;
    }

    public void Succeed(string message) => Complete(message, AlertVariant.Success);

    public void Fail(string message) => Complete(message, AlertVariant.Danger);

    public virtual void Reset()
    {
        IsLoading = false;
        IsAlertVisible = false;
        Message = string.Empty;
        Variant = AlertVariant.Success;
    }

    private void Complete(string message, AlertVariant variant)
    {
        IsLoading = false;
        Message = message;
        Variant = variant;
        IsAlertVisible = true;
    }
}

/// <summary>Estado de ação assíncrona associado a um item específico.</summary>
public sealed class AsyncActionState<TKey> : AsyncActionState where TKey : notnull
{
    public TKey? ActiveKey { get; private set; }

    public void Begin(TKey key)
    {
        ActiveKey = key;
        base.Begin();
    }

    public override void Reset()
    {
        ActiveKey = default;
        base.Reset();
    }
}
