using System.ComponentModel.DataAnnotations;
namespace ComponentDocs.Components.Pages;
public partial class Introduction
{

    private const string ProjectReference = """
        <ProjectReference Include="../DesignSystem/DesignSystem.csproj" />
        """;
    private const string Styles = """
        <link rel="stylesheet" href="_content/DesignSystem/css/bootstrap/css/bootstrap.min.css" />
        <link rel="stylesheet" href="_content/DesignSystem/css/bootstrap-icons/font/bootstrap-icons.min.css" />
        <link rel="stylesheet" href="NomeDaSuaAplicacao.styles.css" />
        """;
    private const string Imports = """
        @using Microsoft.AspNetCore.Components.Routing
        @using DesignSystem.Enums
        @using DesignSystem.Components.Buttons
        @using DesignSystem.Components.Alerts
        @using DesignSystem.Components.Cards
        @using DesignSystem.Components.Icons
        @using DesignSystem.Components.Feedback
        @using DesignSystem.Components.Forms
        @using DesignSystem.Components.Layout
        @using DesignSystem.Components.Tables
        @using DesignSystem.Components.Navigation
        @using DesignSystem.Components.Accordions
        """;

}
