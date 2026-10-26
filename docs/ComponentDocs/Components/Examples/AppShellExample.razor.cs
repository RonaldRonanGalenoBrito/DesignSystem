namespace ComponentDocs.Components.Examples;
public partial class AppShellExample
{
    private const string Code = """
        <AppShell>
            <HeaderContent>
                <Header>
                    <BrandContent><strong>Meu sistema</strong></BrandContent>
                    <Actions><Button Theme="ThemeColor.Primary" Appearance="ButtonStyle.Outline">Perfil</Button></Actions>
                </Header>
            </HeaderContent>
            <SidebarContent>
                <Sidebar BackgroundColor="#1b6098" TextColor="#ffffff"
                         ItemActiveBackgroundColor="#ffffff" ItemActiveTextColor="#1b6098">
                    <HeaderContent>
                        <div class="d-flex align-items-center gap-3">
                            <Icon Name="buildings" SizeClass="fs-2xl" />
                            <div><strong>Meu projeto</strong><div class="small">Área de trabalho</div></div>
                        </div>
                    </HeaderContent>
                    <ChildContent>
                        <SidebarItem Href="componentes/app-shell" Text="Visão geral" Icon="grid" Match="NavLinkMatch.All" />
                        <SidebarItem Href="componentes/sidebar" Text="Projetos" Icon="folder2-open" />
                        <SidebarItem Href="componentes/table" Text="Relatórios" Icon="graph-up" />
                        <SidebarGroup Title="Administração" Icon="gear">
                            <SidebarItem Href="componentes/button" Text="Preferências" Icon="sliders" />
                        </SidebarGroup>
                    </ChildContent>
                    <FooterContent><span class="small">Workspace · Versão 1.0</span></FooterContent>
                </Sidebar>
            </SidebarContent>
            <ChildContent>
                <h1 class="h4">Visão geral</h1>
                <p>Acompanhe os principais indicadores do seu projeto.</p>
                <div class="row g-3">
                    <div class="col-12 col-md-6"><Card Title="Projetos ativos"><ChildContent>12 em andamento</ChildContent></Card></div>
                    <div class="col-12 col-md-6"><Card Title="Tarefas concluídas"><ChildContent>84 neste mês</ChildContent></Card></div>
                </div>
            </ChildContent>
            <FooterContent>Meu sistema · 2026</FooterContent>
        </AppShell>
        """;

}
