# Layout com header e menu lateral

Os componentes estão em `SIDEB.DesignSystem.Components.Layout`. Eles podem ser combinados ou usados separadamente.

| Componente | Uso |
| --- | --- |
| AppShell | Organiza header, menu, conteúdo principal e rodapé |
| Header | Marca/título, conteúdo livre, ações e botão do menu mobile |
| Sidebar | Região de navegação com cabeçalho e rodapé opcionais |
| SidebarItem | Link com ícone, conteúdo adicional, rota ativa e estado desabilitado |
| SidebarGroup | Grupo recolhível com o elemento HTML nativo details |

## CSS

Além de Bootstrap e Bootstrap Icons, carregue o CSS isolado da aplicação consumidora:

```html
<link rel="stylesheet" href="_content/SIDEB.DesignSystem/css/bootstrap/css/bootstrap.min.css" />
<link rel="stylesheet" href="_content/SIDEB.DesignSystem/css/bootstrap-icons/font/bootstrap-icons.min.css" />
<link rel="stylesheet" href="NomeDaSuaAplicacao.styles.css" />
```

Substitua `NomeDaSuaAplicacao` pelo assembly da aplicação, não pelo nome da biblioteca. O Blazor inclui automaticamente os estilos isolados da biblioteca nesse bundle. Se esse link já existe no host, mantenha apenas uma cópia.

## MainLayout.razor

```razor
@inherits LayoutComponentBase
@using Microsoft.AspNetCore.Components.Routing
@using SIDEB.DesignSystem.Components.Layout

<AppShell>
    <HeaderContent>
        <Header Title="Meu sistema">
            <Actions>
                <span class="text-body-secondary">Olá, usuário</span>
            </Actions>
        </Header>
    </HeaderContent>
    <SidebarContent>
        <Sidebar>
            <ChildContent>
                <SidebarItem Href="" Text="Início" Icon="house" Match="NavLinkMatch.All" />
                <SidebarGroup Title="Cadastros" Icon="folder">
                    <SidebarItem Href="clientes" Text="Clientes" Icon="people" />
                    <SidebarItem Href="produtos" Text="Produtos" Icon="box" />
                </SidebarGroup>
                <SidebarItem Href="configuracoes" Text="Configurações" Icon="gear" />
            </ChildContent>
            <FooterContent>
                <small class="text-body-secondary">Versão 1.0</small>
            </FooterContent>
        </Sidebar>
    </SidebarContent>
    <ChildContent>@Body</ChildContent>
    <FooterContent>Minha aplicação</FooterContent>
</AppShell>
```

Configure esse MainLayout como DefaultLayout do RouteView. Em Blazor Web Apps, configure a interatividade no Routes/Router da aplicação; não aplique `@rendermode` diretamente ao layout que recebe Body/RenderFragment. O projeto de exemplo demonstra `InteractiveServer` no Routes.

## Comportamento

- A partir de **992 px**, o menu ocupa toda a lateral, do topo ao fim da tela, com rolagem própria. Header, conteúdo e footer ficam na coluna à direita. Abaixo desse breakpoint, começa fechado e o botão do Header alterna sua exibição acima do conteúdo. Não é um painel modal ou sobreposto.
- O Header permanece no topo durante a rolagem. O rodapé fica ao fim do conteúdo, preenchendo a altura da tela em páginas curtas.
- Um clique normal em SidebarItem fecha o menu mobile. Ctrl/Cmd/Shift/Alt + clique não o fecha. Use FocusOnNavigate no Router para levar o foco ao título após navegar.
- O item ativo usa NavLink. Use `Match="NavLinkMatch.All"` na página inicial e Prefix para seções com subrotas. Os endereços relativos respeitam o base href da aplicação.
- `Disabled` renderiza um item sem href ou evento de clique. Isso é apenas apresentação; a autorização das rotas deve permanecer na aplicação.
- SidebarGroup usa summary/details, com suporte nativo a teclado. `InitiallyExpanded="false"` inicia o grupo fechado.
- Header e Sidebar aceitam conteúdo personalizado. No Header, `BrandContent` substitui Title, `ChildContent` recebe conteúdo livre e `Actions` recebe ações.
- Fora de AppShell, o Header pode controlar um menu externo com `ShowMenuToggle`, `SidebarId`, `MenuOpen` e `OnMenuToggle`.
- AppShell sem SidebarContent ocupa toda a largura e omite o botão de menu.
- Textos acessíveis são configuráveis por `SkipLinkLabel`, `Header.MenuLabel` e `Sidebar.Label`. O menu usa navegação HTML comum, sem o papel ARIA menu de aplicações.
- Todos aceitam ClassExtra e atributos adicionais. AppShell também tem ContentClass para o conteúdo principal.
- Cores seguem as variáveis Bootstrap, inclusive o tema definido por `data-bs-theme` na aplicação.

A largura do menu pode ser personalizada por uma variável CSS:

```razor
<AppShell style="--app-sidebar-width: 19rem">
    ...
</AppShell>
```

## Marca e cores do menu

`Sidebar.HeaderContent` aceita qualquer conteúdo Razor: texto, imagem, componente ou uma combinação deles. Por exemplo:

```razor
<Sidebar BackgroundColor="#1b6098" TextColor="#ffffff"
         ItemTextColor="#ffffff" ItemHoverBackgroundColor="#164d7a" ItemHoverTextColor="#ffffff"
         ItemActiveBackgroundColor="#ffffff" ItemActiveTextColor="#1b6098">
    <HeaderContent>
        <div class="d-flex align-items-center gap-2">
            <img src="images/logo.svg" alt="Logo da organização" width="40" height="40" />
            <strong>Meu projeto</strong>
        </div>
    </HeaderContent>
    <ChildContent>
        <SidebarItem Href="" Text="Início" Icon="house" Match="NavLinkMatch.All" />
        <SidebarItem Href="pedidos" Text="Pedidos" Icon="bag" TextColor="#ffe69c" />
    </ChildContent>
</Sidebar>
```

Substitua o caminho da imagem pelo seu arquivo. `BackgroundColor` e `TextColor` definem o fundo e o texto do menu. As propriedades `ItemBackgroundColor`, `ItemTextColor`, `ItemHoverBackgroundColor`, `ItemHoverTextColor`, `ItemActiveBackgroundColor` e `ItemActiveTextColor` definem as cores dos links, inclusive dentro de grupos. No `SidebarItem`, use os mesmos nomes sem o prefixo `Item` para sobrescrever apenas aquele link. Os ícones acompanham a cor do texto.

As cores aceitam `#RGB`, `#RGBA`, `#RRGGBB` e `#RRGGBBAA`. Null ou vazio mantém a herança/padrão Bootstrap. Cores explícitas permanecem iguais nos temas claro e escuro; escolha pares de fundo/texto com contraste legível. O foco por teclado usa as cores de hover e mantém o indicador de foco do Bootstrap. Classes como `text-body-secondary` no seu conteúdo podem sobrescrever a cor herdada.

## Exemplo executável

Na raiz do repositório:

```shell
dotnet run --project samples/LayoutDemo
```

Abra [http://localhost:5186](http://localhost:5186). O exemplo contém header, grupos, badges nos links, item desabilitado, rodapé e páginas demonstrativas. Os números são ilustrativos.

Para verificar:

```shell
dotnet run --project tests/ComponentTests
dotnet build SIDEB.DesignSystem.slnx -c Release
```
