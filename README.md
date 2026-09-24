# Biblioteca de componentes Blazor

Razor Class Library em .NET 8, com Bootstrap **5.3.3** e Bootstrap Icons **1.11.3** incluídos. Os componentes têm nomes genéricos. O nome do projeto e os namespaces existentes foram preservados para manter compatibilidade.

## Instalação no projeto consumidor

Adicione uma referência ao projeto:

```xml
<ProjectReference Include="../SIDEB.DesignSystem/SIDEB.DesignSystem.csproj" />
```

Inclua no documento HTML principal (por exemplo, `Components/App.razor` em um Blazor Web App):

```html
<link rel="stylesheet" href="_content/SIDEB.DesignSystem/css/bootstrap/css/bootstrap.min.css" />
<link rel="stylesheet" href="_content/SIDEB.DesignSystem/css/bootstrap-icons/font/bootstrap-icons.min.css" />
```

Se a aplicação já carrega Bootstrap 5.3, mantenha uma única cópia do CSS. Os componentes desta biblioteca não precisam do JavaScript do Bootstrap. Eventos, binding e fechamento de alertas precisam de um modo interativo do Blazor; configure-o na aplicação consumidora. A biblioteca não impõe um render mode.

No `_Imports.razor` da aplicação:

```razor
@using SIDEB.DesignSystem.Enums
@using SIDEB.DesignSystem.Components.Alerts
@using SIDEB.DesignSystem.Components.Buttons
@using SIDEB.DesignSystem.Components.Cards
@using SIDEB.DesignSystem.Components.Icons
@using SIDEB.DesignSystem.Components.Forms
@using SIDEB.DesignSystem.Components.Feedback
@using SIDEB.DesignSystem.Components.Tables
@using SIDEB.DesignSystem.Components.Navigation
@using SIDEB.DesignSystem.Components.Accordions
```

## Componentes

O [manual interativo](docs/ComponentDocs/README.md) está no projeto `docs/ComponentDocs`. Execute `dotnet run --project docs/ComponentDocs` e acesse [localhost:5190](http://localhost:5190) para explorar exemplos, estilos e a referência de parâmetros.

Para header, menu lateral e estrutura de páginas, consulte o [guia de layouts](docs/layouts.md). Um exemplo executável está em `samples/LayoutDemo`.

| Componente | Recursos principais |
| --- | --- |
| Button | Variantes sólidas e outline, tamanho, ícone, carregamento, submit/reset |
| Alert | Variante, ícone, fechamento, `@bind-Visible`, `OnDismissed` |
| Card | Título, subtítulo, slots de cabeçalho, corpo e rodapé |
| Icon | Bootstrap Icons; decorativo por padrão, `Label` para significado acessível |
| FormInputText | Texto, email, senha, placeholder, readonly |
| FormSelect&lt;TValue&gt; | Opções por ChildContent, enums, tipos anuláveis e validação de conversão |
| FormCheckbox | Checkbox ou switch com `IsSwitch` |
| FormTextArea | Texto multilinha, `Rows`, readonly |
| FormInputNumber&lt;TValue&gt; | Tipos numéricos aceitos pelo InputNumber do Blazor |
| FormInputDate&lt;TValue&gt; | Tipos de data aceitos pelo InputDate do Blazor, incluindo DateOnly |
| Badge | Cores contextuais e formato pill |
| Spinner | Border/grow, tamanho pequeno e texto acessível |
| ProgressBar | Valor/máximo, percentual, listras e animação |
| Table&lt;TItem&gt; | Linhas por template, cabeçalho, rodapé, responsividade, estilos, carregamento e estado vazio |
| Pagination | Binding de página, janela de páginas, reticências, tamanhos e estado desabilitado |
| Accordion / AccordionItem | Painéis únicos ou múltiplos, binding da expansão e estilo flush |

A tabela recebe os itens que devem aparecer e um `RowTemplate` com as células (`td`). O exemplo do manual combina `Table<TItem>` com `Pagination` usando `Skip` e `Take`; a aplicação controla a consulta, ordenação e filtragem dos dados. Informe `ColumnCount` para os estados vazio e de carregamento ocuparem todas as colunas.

No acordeão, use IDs estáveis e únicos nos itens e `@bind-ExpandedIds` quando precisar controlar a expansão. `AllowMultiple` permite abrir vários painéis. O conteúdo permanece montado ao fechar um painel, preservando o estado dos controles.

## Formulário com validação

```razor
@using System.ComponentModel.DataAnnotations
@using Microsoft.AspNetCore.Components.Forms

<EditForm Model="@model" OnValidSubmit="Save">
    <DataAnnotationsValidator />
    <FormInputText Label="Nome" @bind-Value="model.Name" Required />
    <FormSelect TValue="Department?" Label="Departamento"
                @bind-Value="model.Department" Placeholder="Selecione">
        <option value="@Department.Sales">Comercial</option>
        <option value="@Department.Support">Suporte</option>
    </FormSelect>
    <FormInputNumber TValue="decimal?" Label="Valor" @bind-Value="model.Amount" min="0" step="0.01" />
    <FormInputDate TValue="DateOnly?" Label="Data" @bind-Value="model.Date" />
    <FormTextArea Label="Observações" @bind-Value="model.Notes" Rows="4" />
    <FormCheckbox Label="Ativo" @bind-Value="model.Active" IsSwitch />
    <Button Type="ButtonType.Submit" IsLoading="@saving">Salvar</Button>
</EditForm>

@code {
    private readonly ExampleModel model = new();
    private bool saving;
    private async Task Save()
    {
        saving = true;
        try
        {
            await Task.CompletedTask; // Substitua pela chamada ao serviço da aplicação.
        }
        finally { saving = false; }
    }

    public enum Department { Sales, Support }
    public sealed class ExampleModel
    {
        [Required(ErrorMessage = "Informe o nome.")]
        public string Name { get; set; } = "";
        [Required(ErrorMessage = "Selecione o departamento.")]
        public Department? Department { get; set; }
        public decimal? Amount { get; set; }
        public DateOnly? Date { get; set; }
        public string Notes { get; set; } = "";
        public bool Active { get; set; }
    }
}
```

Os campos aceitam `@bind-Value` dentro e fora de `EditForm`. Dentro de `EditForm`, quem passa `Value` e `ValueChanged` manualmente também deve fornecer `ValueExpression="() => model.Property"`. Os controles nativos do Blazor fazem a conversão, notificam o EditContext e preservam o valor anterior quando a entrada é inválida.

`Required` define o atributo HTML; use também `[Required]` no modelo para validação .NET. Para exigir que um booleano seja verdadeiro, use uma regra específica (o atributo Required sozinho não rejeita false). `ErrorMessage` permite erro externo e tem prioridade visual sobre a validação do formulário. Use `HelpText` para orientações.

O binding textual atualiza no evento `change` (normalmente ao sair do campo). Use FormInputNumber para números e FormInputDate para datas; o parâmetro Type do FormInputText mantém um valor string. No select com placeholder, prefira um tipo anulável para representar ausência de seleção. Seleção múltipla não faz parte da API atual.

## Apresentação e acessibilidade

```razor
<Alert Variant="AlertVariant.Success" Dismissible @bind-Visible="showSuccess">
    Operação concluída.
</Alert>
<Button Icon="download" Variant="ButtonVariant.OutlinePrimary">Exportar</Button>
<Badge Variant="ColorVariant.Success" Pill>Ativo</Badge>
<Spinner Label="Carregando pedidos..." Small />
<ProgressBar Value="42" Max="100" Label="Envio dos arquivos" ShowLabel />

@code {
    private bool showSuccess = true;
}
```

- Atributos adicionais (`data-*`, `aria-*`, `title`, etc.) chegam ao elemento HTML. A classe passada em `class` é combinada com as classes Bootstrap.
- Nos campos, `ClassExtra` estiliza o contêiner e `class` estiliza o controle. Nos demais componentes, ambas são combinadas no elemento principal.
- Use os parâmetros próprios para estados controlados, como `Disabled`, `IsLoading`, `Type` e `Id`. Atributos adicionais não devem substituir eventos internos ou o valor do binding.
- Um Icon sem Label é decorativo. Para um botão que contém somente um ícone, forneça `aria-label` no Button.
- Rótulos auxiliares são configuráveis: `DismissLabel`, `Spinner.Label`, `ProgressBar.Label` e `ParsingErrorMessage` dos campos numérico e de data.
- Para reabrir o mesmo Alert após fechamento, use `@bind-Visible` e volte a variável para true.
- ProgressBar limita Value ao intervalo 0..Max e rejeita números não finitos ou Max menor ou igual a zero.

## Verificação

```shell
dotnet build SIDEB.DesignSystem.slnx
dotnet run --project tests/ComponentTests
```

O projeto de verificações não exige bibliotecas de teste externas. Ele renderiza os componentes, dispara eventos reais do pipeline Blazor e verifica binding, validação, atributos, cultura e estados. Não substitui uma verificação visual e de teclado na aplicação consumidora.

Referências: [inputs do Blazor](https://learn.microsoft.com/aspnet/core/blazor/forms/input-components?view=aspnetcore-8.0), [Bootstrap Progress](https://getbootstrap.com/docs/5.3/components/progress/) e [Bootstrap Spinners](https://getbootstrap.com/docs/5.3/components/spinners/).
