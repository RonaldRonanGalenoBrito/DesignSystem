using DesignSystem.Attributes;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace DesignSystem.Components.Forms;

/// <summary>Shared presentation and validation for Bootstrap form controls.</summary>
public abstract class FormFieldBase<TValue> : ComponentBase, IDisposable
{
    [CascadingParameter] private EditContext? CascadedEditContext { get; set; }
    [ParameterDoc("Identificador HTML para associar controle, label e mensagens.", Default = "Gerado automaticamente")]
    [Parameter] public string Id { get; set; } = $"field-{Guid.NewGuid():N}";
    [ParameterDoc("Texto do label associado ao controle.")]
    [Parameter] public string? Label { get; set; }
    [ParameterDoc("Valor atual. Nos campos, aceita @bind-Value.")]
    [Parameter] public TValue Value { get; set; } = default!;
    [ParameterDoc("Notifica alteração do valor do campo.")]
    [Parameter] public EventCallback<TValue> ValueChanged { get; set; }
    [ParameterDoc("Expressão do campo no modelo; gerada por @bind-Value.")]
    [Parameter] public Expression<Func<TValue>>? ValueExpression { get; set; }
    [ParameterDoc("Orientação associada ao campo.")]
    [Parameter] public string? HelpText { get; set; }
    [ParameterDoc("Erro externo, com prioridade visual sobre a validação.")]
    [Parameter] public string? ErrorMessage { get; set; }
    [ParameterDoc("Desabilita interação.")]
    [Parameter] public bool Disabled { get; set; }
    [ParameterDoc("Aplica required ao HTML; valide também no modelo.")]
    [Parameter] public bool Required { get; set; }
    [ParameterDoc("Classes do contêiner do campo. Use class para estilizar o controle.")]
    [Parameter] public string? ClassExtra { get; set; }
    [ParameterDoc("Atributos HTML adicionais; class é combinada às classes do componente.")]
    [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private EditContext? subscribedContext;
    private Expression<Func<TValue>>? fallbackExpression;
    protected Expression<Func<TValue>> EffectiveExpression => ValueExpression ?? (fallbackExpression ??= () => Value);
    protected string? ValidationError => ErrorMessage ?? CascadedEditContext?
        .GetValidationMessages(FieldIdentifier.Create(EffectiveExpression)).FirstOrDefault();
    protected string? Invalid => ValidationError is not null ? "true" : null;
    protected string DescribedBy => string.Join(" ", new[]
    {
        ComponentAttributes.Get(AdditionalAttributes, "aria-describedby"),
        string.IsNullOrWhiteSpace(HelpText) ? null : $"{Id}-help",
        ValidationError is null ? null : $"{Id}-error"
    }.Where(value => !string.IsNullOrWhiteSpace(value)));
    protected string ControlClass(string baseClass) =>
        ComponentAttributes.Classes(AdditionalAttributes, baseClass, Invalid is null ? null : "is-invalid");
    protected Task ChangeValue(TValue value) => Disabled ? Task.CompletedTask : ValueChanged.InvokeAsync(value);

    protected override void OnParametersSet()
    {
        if (CascadedEditContext is not null && ValueExpression is null)
            throw new InvalidOperationException($"{GetType().Name} requires @bind-Value or ValueExpression inside EditForm.");
        if (subscribedContext == CascadedEditContext) return;
        if (subscribedContext is not null) subscribedContext.OnValidationStateChanged -= ValidationChanged;
        subscribedContext = CascadedEditContext;
        if (subscribedContext is not null) subscribedContext.OnValidationStateChanged += ValidationChanged;
    }

    private void ValidationChanged(object? sender, ValidationStateChangedEventArgs args) => _ = InvokeAsync(StateHasChanged);
    public void Dispose()
    {
        if (subscribedContext is not null) subscribedContext.OnValidationStateChanged -= ValidationChanged;
        GC.SuppressFinalize(this);
    }
}



