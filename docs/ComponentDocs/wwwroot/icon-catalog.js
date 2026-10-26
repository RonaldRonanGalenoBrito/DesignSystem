export async function getIconNames() {
    const url = new URL('_content/DesignSystem/css/bootstrap-icons/font/bootstrap-icons.json?v=1.13.2', document.baseURI);
    const response = await fetch(url, { cache: 'no-cache' });
    if (!response.ok) {
        throw new Error('Não foi possível carregar o catálogo de ícones (' + response.status + ').');
    }

    const icons = await response.json();
    return Object.keys(icons).sort((first, second) => first.localeCompare(second));
}

export async function copyText(value) {
    await navigator.clipboard.writeText(value);
}
