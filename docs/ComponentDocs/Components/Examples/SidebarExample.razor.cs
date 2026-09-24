namespace ComponentDocs.Components.Examples;
public partial class SidebarExample
{
private string background = "#1b6098", foreground = "#ffffff", active = "#ffffff";
    private string Code => $$"""
        <Sidebar BackgroundColor="{{background}}" TextColor="{{foreground}}"
                 ItemTextColor="{{foreground}}" ItemHoverBackgroundColor="#164d7a" ItemHoverTextColor="#ffffff"
                 ItemActiveBackgroundColor="{{active}}" ItemActiveTextColor="#1b6098">
            <HeaderContent>
                <div class="d-flex align-items-center gap-3">
                    <Icon Name="buildings" SizeClass="fs-2" />
                    <div><strong>Meu projeto</strong><div class="small">Área de trabalho</div></div>
                </div>
            </HeaderContent>
            <ChildContent>
                <SidebarItem Href="componentes/sidebar" Text="Visão geral" Icon="grid" />
                <SidebarItem Href="componentes/button" Text="Ações" Icon="cursor" TextColor="#ffe69c" />
            </ChildContent>
            <FooterContent>Versão 1.0</FooterContent>
        </Sidebar>
        """;

}
