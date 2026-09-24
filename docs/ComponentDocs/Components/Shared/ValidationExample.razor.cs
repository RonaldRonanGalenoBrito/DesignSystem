using System.ComponentModel.DataAnnotations;

namespace ComponentDocs.Components.Shared;

public partial class ValidationExample
{
    private readonly ExampleModel model = new();
    private bool saved;
    private sealed class ExampleModel
    {
        [Required(ErrorMessage = "Informe o nome.")]
        public string Name { get; set; } = "";
    }
    private const string Sample = """
        @using System.ComponentModel.DataAnnotations
        @using Microsoft.AspNetCore.Components.Forms

        <EditForm Model="@model">
            <DataAnnotationsValidator />
            <FormInputText Label="Nome" @bind-Value="model.Name" />
            <Button Type="ButtonType.Submit">Validar formulário</Button>
        </EditForm>

        @code {
            private readonly ExampleModel model = new();
            public sealed class ExampleModel
            {
                [Required(ErrorMessage = "Informe o nome.")]
                public string Name { get; set; } = "";
            }
        }
        """;
}
