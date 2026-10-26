import { Combobox } from '../css/bootstrap/js/bootstrap.bundle.min.js';

export function connect(toggle, receiver, options) {
    const onChange = event => {
        const values = Array.isArray(event.value)
            ? event.value
            : event.value == null || event.value === '' ? [] : [event.value];
        receiver.invokeMethodAsync('NotifyComboboxSelection', values);
    };

    toggle.addEventListener('change.bs.combobox', onChange);
    const combobox = Combobox.getOrCreateInstance(toggle, options);

    return {
        sync(values, displayText, placeholder) {
            const menu = toggle.nextElementSibling;
            if (!menu) return;
            const selected = new Set(values);
            for (const item of menu.querySelectorAll('.menu-item[data-bs-value]')) {
                const isSelected = selected.has(item.dataset.bsValue);
                item.classList.toggle('selected', isSelected);
                item.setAttribute('aria-selected', String(isSelected));
            }
            const valueDisplay = toggle.querySelector('.combobox-value');
            if (valueDisplay) {
                const hasValue = values.length > 0;
                valueDisplay.textContent = hasValue ? displayText : placeholder;
                valueDisplay.classList.toggle('combobox-placeholder', !hasValue);
            }
        },
        dispose() {
            toggle.removeEventListener('change.bs.combobox', onChange);
            combobox.dispose();
        }
    };
}
