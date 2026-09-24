param([string]$BaseUrl = 'http://localhost:5190')
$ErrorActionPreference = 'Stop'
$catalogPage = Invoke-WebRequest -UseBasicParsing "$BaseUrl/"
$entries = @([regex]::Matches($catalogPage.Content, 'href="componentes/([a-z0-9-]+)"') | ForEach-Object { $_.Groups[1].Value } | Sort-Object -Unique)
if ($entries.Count -eq 0) { throw "Nenhum componente descoberto na página inicial." }
foreach ($slug in $entries) {
    $response = Invoke-WebRequest -UseBasicParsing "$BaseUrl/componentes/$slug"
    $html = [System.Net.WebUtility]::HtmlDecode($response.Content)
    if ($response.StatusCode -ne 200 -or $html -notmatch '<h1>[^<]+</h1>' -or $html -notmatch 'Referência de parâmetros' -or $html -notmatch '<table') {
        throw "Falha na página $slug."
    }
    if ($html -notmatch "componentes/$slug#parametros") { throw "Âncora incorreta em $slug." }
    Write-Output "PASS: $slug"
}
foreach ($asset in @('', 'manual.css', 'manual.js', 'ComponentDocs.styles.css', '_content/DesignSystem/css/bootstrap/css/bootstrap.min.css', '_content/DesignSystem/css/bootstrap-icons/font/bootstrap-icons.min.css')) {
    $response = Invoke-WebRequest -UseBasicParsing "$BaseUrl/$asset"
    if ($response.StatusCode -ne 200) { throw "Falha no recurso $asset." }
}
Write-Output "$($entries.Count) páginas e 6 recursos verificados."
