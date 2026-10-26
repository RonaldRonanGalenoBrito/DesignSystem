# Biblioteca de componentes Blazor

Razor Class Library em .NET 8, com Bootstrap **6.0.0-alpha.1** e Bootstrap Icons **1.13.2** incluídos. Os componentes têm nomes genéricos. O nome do projeto e os namespaces existentes foram preservados para manter compatibilidade.

O Bootstrap 6 ainda está em alpha. Requer Chrome/Edge 130+, Firefox 132+ ou Safari 18+. Os componentes usam as classes e estruturas nativas da versão 6, sem folha de compatibilidade com a versão 5. As APIs Razor foram preservadas; classes Bootstrap 5 fornecidas pelo consumidor em `ClassExtra`, `class` ou conteúdo devem ser migradas conforme o [guia oficial](https://getbootstrap.com/docs/6.0/migration/).

`Light` e `Dark` são mantidos nos enums e mapeados para os temas nativos `secondary` e `inverse`. A escala de espaçamento dos parâmetros `Gutter`, `GutterX` e `GutterY` é traduzida para preservar suas medidas anteriores. O visual acompanha o Bootstrap 6.

Os arquivos oficiais do Bootstrap ficam versionados no repositório, na versão `6.0.0-alpha.1`. O LibMan gerencia apenas Bootstrap Icons.

## Instalação no projeto consumidor

Adicione uma referência ao projeto:

```xml
<ProjectReference Include="../DesignSystem/DesignSystem.csproj" />
```

Inclua no documento HTML principal (por exemplo, `Components/App.razor` em um Blazor Web App):

```html
<link rel="stylesheet" href="_content/DesignSystem/css/bootstrap/css/bootstrap.min.css" />
<link rel="stylesheet" href="_content/DesignSystem/css/bootstrap-icons/font/bootstrap-icons.min.css" />
```

Carregue uma única cópia do Bootstrap 6. O Dialog importa automaticamente o módulo ES a partir dos assets da biblioteca; não é necessário adicionar um script global. Eventos, binding e fechamento de alertas precisam de um modo interativo do Blazor; configure-o na aplicação consumidora. A biblioteca não impõe um render mode.

No `_Imports.razor` da aplicação:

```razor
@using DesignSystem.Enums
@using DesignSystem.Components.Alerts
@using DesignSystem.Components.Buttons
@using DesignSystem.Components.Cards
@using DesignSystem.Components.Icons
@using DesignSystem.Components.Forms
@using DesignSystem.Components.Feedback
@using DesignSystem.Components.Tables
@using DesignSystem.Components.Navigation
@using DesignSystem.Components.Accordions
@using DesignSystem.Components.Grid
```

## Componentes

Todos os componentes aceitam `Theme="ThemeColor.Success"`. Os temas disponíveis são `Primary`, `Accent`, `Secondary`, `Success`, `Danger`, `Warning`, `Info` e `Inverse`.

`Theme` define a cor temática e tem prioridade sobre `Variant` nos componentes que ainda oferecem esse parâmetro. Os componentes usam classes e tokens nativos do Bootstrap; nos contêineres de layout, o tema é disponibilizado aos filhos que consomem esses tokens. Ícones e spinners usam a cor de texto do tema; campos preservam os estados de validação. Cores hexadecimais explícitas de Sidebar continuam tendo prioridade.

```razor
<Button Theme="ThemeColor.Success" Appearance="ButtonStyle.Outline">Salvar</Button>
<Alert Theme="ThemeColor.Success">Alterações salvas.</Alert>
<Card Theme="ThemeColor.Accent" Title="Resumo">Conteúdo</Card>
<Badge Theme="ThemeColor.Success" Appearance="BadgeStyle.Subtle" Pill>Ativo</Badge>
<FormCheckbox Theme="ThemeColor.Success" Size="FormControlSize.Large" Label="Aceito os termos" @bind-Value="accepted" />
<FormSwitch Theme="ThemeColor.Success" Label="Receber notificações" @bind-Value="enabled" />
```

No Badge, `Appearance` aceita `Solid`, `Subtle` e `Outline`, usando as classes nativas do Bootstrap 6.

No Button, tema, estilo, tamanho e arredondamento são independentes:

```razor
<Button Theme="ThemeColor.Primary" Appearance="ButtonStyle.Styled"
        Size="ButtonSize.Medium" Rounded="ButtonRounded.Pill">Salvar</Button>
```

`Appearance` aceita `Styled`, `Solid`, `Outline`, `Subtle`, `Text` e `Link`; seu padrão é `Solid`. `Rounded` aceita `Default`, `Pill` e `Square` (cantos retos, sem alterar a proporção). `Size` aceita `ExtraSmall`, `Small`, `Medium` e `Large`. O nome `Appearance` preserva o atributo HTML `style` para CSS inline.

O [manual interativo](docs/ComponentDocs/README.md) está no projeto `docs/ComponentDocs`. Execute `dotnet run --project docs/ComponentDocs` e acesse [localhost:5190](http://localhost:5190) para explorar exemplos, estilos e a referência de parâmetros.

Para header, menu lateral e estrutura de páginas, consulte o [guia de layouts](docs/layouts.md). Um exemplo executável está em `samples/LayoutDemo`.

| Componente | Recursos principais |
| --- | --- |
| Button | Tema, estilo, arredondamento, tamanho, ícone, carregamento, submit/reset |
| Alert | Tema, ícone, fechamento, `@bind-Visible`, `OnDismissed` |
| Card | Título, subtítulo, slots de cabeçalho, corpo e rodapé |
| Icon | Bootstrap Icons; decorativo por padrão, `Label` para significado acessível |
| FormAdornment | Contêiner Bootstrap para ícones internos em campos personalizados |
| FormText | Texto, email, senha, placeholder, readonly e ícones internos |
| FormCombobox&lt;TValue&gt; | Opções tipadas, busca, busca sem acentos, seleção múltipla e navegação por teclado |
| FormCheckbox | Checkbox quadrado com tema e tamanhos |
| FormSwitch | Interruptor com tema e tamanhos |
| FormTextArea | Texto multilinha, `Rows`, readonly e ícones internos |
| FormNumber&lt;TValue&gt; | Tipos numéricos aceitos pelo InputNumber do Blazor e ícones internos |
| FormDatepicker&lt;TValue&gt; | Datas únicas, múltiplas ou intervalos; acionadores input, botão e calendário inline |
| Badge | Cores contextuais e formato pill |
| Spinner | Border/grow, tamanho pequeno e texto acessível |
| Progress | Tema, valor/máximo, percentual, listras e animação |
| Table&lt;TItem&gt; | Linhas por template, cabeçalho, rodapé, responsividade, estilos, carregamento e estado vazio |
| Pagination | Binding de página, janela de páginas, reticências, tamanhos e estado desabilitado |
| Accordion / AccordionItem | Painéis únicos ou múltiplos, binding da expansão e estilo flush |
| Row / Column | Grid responsivo Bootstrap, espaçamentos, larguras e deslocamentos por breakpoint |

A tabela recebe os itens que devem aparecer e um `RowTemplate` com as células (`td`). O exemplo do manual combina `Table<TItem>` com `Pagination` usando `Skip` e `Take`; a aplicação controla a consulta, ordenação e filtragem dos dados. Informe `ColumnCount` para os estados vazio e de carregamento ocuparem todas as colunas.

No acordeão, use IDs estáveis e únicos nos itens e `@bind-ExpandedIds` quando precisar controlar a expansão. `AllowMultiple` permite abrir vários painéis. O conteúdo permanece montado ao fechar um painel, preservando o estado dos controles.

## Formulário com validação

```razor
@using System.ComponentModel.DataAnnotations
@using Microsoft.AspNetCore.Components.Forms

<EditForm Model="@model" OnValidSubmit="Save">
    <DataAnnotationsValidator />
    <FormText Label="Nome" @bind-Value="model.Name" Required />
    <FormCombobox TValue="Department?" Label="Departamento" Items="@departmentOptions"
                  @bind-Value="model.Department" Search SearchNormalize Placeholder="Selecione" />
    <FormNumber TValue="decimal?" Label="Valor" @bind-Value="model.Amount" min="0" step="0.01" />
    <FormDatepicker TValue="DateOnly?" Label="Data" @bind-Value="model.Date" />
    <FormTextArea Label="Observações" @bind-Value="model.Notes" Rows="4" />
    <FormCheckbox Label="Ativo" @bind-Value="model.Active" />
    <Button Type="ButtonType.Submit" IsLoading="@saving">Salvar</Button>
</EditForm>

@code {
    private readonly ExampleModel model = new();
    private readonly FormComboboxOption[] departmentOptions = [
        new(Department.Sales, "Comercial"),
        new(Department.Support, "Suporte")
    ];
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

O binding textual atualiza no evento `change` (normalmente ao sair do campo). Use FormNumber para números e FormDatepicker para datas; o parâmetro Type do FormText mantém um valor string. No select com placeholder, prefira um tipo anulável para representar ausência de seleção. FormDatepicker usa DateOnly? ou DateTime? para uma data e DateOnly[] ou DateTime[] para seleção múltipla e intervalos.

## Apresentação e acessibilidade

```razor
<Alert Theme="ThemeColor.Success" Dismissible @bind-Visible="showSuccess">
    Operação concluída.
</Alert>
<Button Icon="download" Theme="ThemeColor.Primary" Appearance="ButtonStyle.Outline">Exportar</Button>
<Badge Theme="ThemeColor.Success" Pill>Ativo</Badge>
<Spinner Label="Carregando pedidos..." Small />
<Progress Value="42" Max="100" Theme="ThemeColor.Primary" Label="Envio dos arquivos" ShowLabel />

@code {
    private bool showSuccess = true;
}
```

- Atributos adicionais (`data-*`, `aria-*`, `title`, etc.) chegam ao elemento HTML. A classe passada em `class` é combinada com as classes Bootstrap.
- Nos campos, `ClassExtra` estiliza o contêiner e `class` estiliza o controle. Nos demais componentes, ambas são combinadas no elemento principal.
- Use os parâmetros próprios para estados controlados, como `Disabled`, `IsLoading`, `Type` e `Id`. Atributos adicionais não devem substituir eventos internos ou o valor do binding.
- Um Icon sem Label é decorativo. Para um botão que contém somente um ícone, forneça `aria-label` no Button.
- Rótulos auxiliares são configuráveis: `DismissLabel`, `Spinner.Label`, `Progress.Label` e `ParsingErrorMessage` dos campos numérico e de data.
- Para reabrir o mesmo Alert após fechamento, use `@bind-Visible` e volte a variável para true.
- Progress limita Value ao intervalo 0..Max e rejeita números não finitos ou Max menor ou igual a zero.

## Verificação

```shell
dotnet build DesignSystem.slnx
dotnet run --project tests/ComponentTests
```

O projeto de verificações não exige bibliotecas de teste externas. Ele renderiza os componentes, dispara eventos reais do pipeline Blazor e verifica binding, validação, atributos, cultura e estados. Não substitui uma verificação visual e de teclado na aplicação consumidora.

Referências: [inputs do Blazor](https://learn.microsoft.com/aspnet/core/blazor/forms/input-components?view=aspnetcore-8.0), [Bootstrap Progress](https://getbootstrap.com/docs/6.0/components/progress/) e [Bootstrap Spinners](https://getbootstrap.com/docs/6.0/components/spinners/).

