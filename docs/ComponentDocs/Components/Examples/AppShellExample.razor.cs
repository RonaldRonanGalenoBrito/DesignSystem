namespace ComponentDocs.Components.Examples;
public partial class AppShellExample
{
private const string Code = """
        <AppShell Title="Meu sistema">
            <SidebarContent>
                <Sidebar BackgroundColor="#1b6098" TextColor="#ffffff" ItemTextColor="#ffffff"
                         ItemActiveBackgroundColor="#ffffff" ItemActiveTextColor="#1b6098">
                    <HeaderContent><strong><Icon Name="buildings" /> Meu projeto</strong></HeaderContent>
                    <ChildContent><SidebarItem Href="" Text="Início" Icon="house" Match="NavLinkMatch.All" /></ChildContent>
                </Sidebar>
            </SidebarContent>
            <ChildContent><h2>Bem-vindo</h2></ChildContent>
            <FooterContent>Minha aplicação</FooterContent>
        </AppShell>
        """;

}
