# Manual de componentes

Aplicação Blazor .NET 8 da mesma solução. Usa os componentes reais da biblioteca; o projeto consumidor não precisa referenciar este manual.

## Executar

Na raiz:

```shell
dotnet run --project docs/ComponentDocs
```

Abra [localhost:5190](http://localhost:5190). O exemplo de aplicação em `samples/LayoutDemo` continua independente, na porta 5186.

## Documentar um componente

Decore o próprio componente Razor com `ComponentDoc`:

```razor
@attribute [ComponentDoc(
    Slug = "button",
    Name = "Button",
    Group = "Ações",
    Description = "Botões com variantes, ícones e estados.",
    Code = "<Button Variant=\"ButtonVariant.Primary\">Salvar</Button>",
    Guidance = "Use Type=Submit dentro de um EditForm.",
    Order = 10
)]
```

O catálogo encontra automaticamente os tipos públicos com esse atributo na assembly da biblioteca. Não adicione entradas ao Catalog.

- **Slug:** obrigatório, único, com letras minúsculas, números e hífens.
- **Name:** opcional; quando vazio, usa o nome da classe sem a aridade genérica.
- **Group e Description:** obrigatórios.
- **Code e Guidance:** opcionais; exemplos e orientações editoriais.
- **Order:** posição dentro do grupo. Empates são ordenados por nome; grupos têm ordenação ordinal determinística.
- **TypeArguments:** especialização representativa para componentes genéricos.

Para FormInputNumber, por exemplo:

```razor
@attribute [ComponentDoc(
    Slug = "form-input-number",
    Group = "Formulários",
    Description = "Entrada numérica tipada.",
    TypeArguments = new[] { typeof(decimal?) }
)]
```

Informe um tipo fechado para cada parâmetro genérico, respeitando suas restrições. O manual usa essa especialização para obter valores padrão, mas preserva nomes como TValue na tabela da API. Tipos abertos não podem ser instanciados diretamente.

Metadados inválidos e slugs duplicados falham na inicialização do manual, com mensagem que identifica o componente. Componentes internos de apresentação, como FieldLayout, não precisam receber ComponentDoc.

Para exemplos longos, Code pode referenciar uma constante string declarada em um arquivo C# próximo ao componente, inclusive escrita como raw string literal. Isso evita muitas aspas escapadas no arquivo Razor; o valor do atributo ainda precisa ser uma constante de compilação.

## Documentar parâmetros

Coloque a descrição na propriedade:

```csharp
[ParameterDoc("Exibe o indicador de carregamento e bloqueia o clique.")]
[Parameter]
public bool IsLoading { get; set; }
```

O manual descobre ParameterAttribute, tipos anuláveis, opções dos enums, EditorRequired e descrições herdadas. As propriedades de FormFieldBase são documentadas uma vez e reaproveitadas por todos os campos. CascadingParameter não é listado como parâmetro configurável.

O padrão normalmente vem da propriedade inicializada. Para valores gerados em tempo de execução:

```csharp
[ParameterDoc("Identificador HTML do controle.", Default = "Gerado automaticamente")]
[Parameter]
public string Id { get; set; } = $"field-{Guid.NewGuid():N}";
```

ParameterDoc.Default é uma explicação para o manual; não altera o comportamento da propriedade. Também há suporte a System.ComponentModel.DescriptionAttribute quando ParameterDoc não estiver presente. Parâmetros sem descrição recebem um aviso explícito na tabela.

As tabelas ficam em cache por tipo. Componentes com construtor público sem argumentos são instanciados para ler os padrões; seus construtores devem ser livres de efeitos externos. Se o construtor exige serviços, a tabela continua funcionando e usa Default explícito ou “Não disponível”.

## Prévias interativas independentes

**Code é texto para exibir e copiar, não código compilado em tempo de execução.** Uma prévia com eventos e binding precisa de um componente Razor compilado.

Cada prévia fica em um arquivo próprio em `Components/Examples`, com registro automático:

```razor
@inherits ExampleBase
@attribute [ComponentExample(typeof(Button))]

<CodeExample Code="@Entry.Code">
    <Button Variant="ButtonVariant.Primary">Salvar</Button>
</CodeExample>
```

Para genéricos, registre a definição aberta: `typeof(FormSelect<>)`. A prévia pode usar `<FormSelect TValue="int?" ...>`.

Não é necessário editar Showcase nem manter um switch de slugs. ComponentExample relaciona o exemplo ao **tipo** do componente, por isso mudar um slug não quebra esse vínculo. Prévias duplicadas ou sem o parâmetro Entry são rejeitadas.

Os exemplos existentes, inclusive controles de cor, tamanho e estado, foram preservados em arquivos separados. Eles podem gerar seu próprio código correspondente ao estado atual. O campo Code do ComponentDoc continua disponível na seção “Exemplo declarado no componente”, sem ser sobrescrito pela prévia.

Um componente novo sem prévia já ganha página, orientações, código e referência de parâmetros. O manual informa que ainda não há prévia interativa; não tenta executar a string Code. Mantenha o texto do exemplo e sua prévia coerentes ao alterar a API.

## Verificação

```shell
dotnet build DesignSystem.slnx -c Release
dotnet run --project tests/ComponentTests -c Release
dotnet run --project tests/DocumentationTests -c Release
powershell -File docs/ComponentDocs/Verify-Manual.ps1
```

DocumentationTests verifica descoberta por atributos, genéricos, descrições herdadas, padrões, tipos anuláveis, opções de enums, ordenação, registros de prévias e rejeição de metadados inválidos.

Verify-Manual pressupõe o manual em execução e descobre as páginas pelos links do catálogo renderizado. Não depende de analisar código C# nem de uma contagem fixa de componentes. Para outra porta, use `-BaseUrl http://localhost:5191`.

A escolha de tema dura a sessão atual, sem persistência local. A cópia de código usa a Clipboard API e apresenta uma alternativa quando não está disponível.
