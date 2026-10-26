import { Datepicker } from '../css/bootstrap/js/bootstrap.bundle.min.js';

export function connect(element, nativeContainer, receiver, options) {
    const nativeInput = nativeContainer.querySelector('input');
    const datepicker = new Datepicker(element, options);

    const selected = event => {
        let dates = event.dates;
        if (!dates) {
            const isoDate = element.tagName === 'INPUT' && /^(\d{4}-\d{2}-\d{2})$/.exec(element.value.trim());
            if (!isoDate) return;
            dates = [isoDate[1]];
            datepicker.setSelectedDates(dates);
        }

        if (options.selectionMode === 'single') {
            if (!nativeInput) return;
            nativeInput.value = dates[0] ?? '';
            nativeInput.dispatchEvent(new Event('change', { bubbles: true }));
        } else {
            if (nativeInput) nativeInput.value = dates.join(',');
            receiver.invokeMethodAsync('NotifyDatepickerSelection', dates);
        }
    };

    element.addEventListener('change', selected);

    return {
        sync(dates) {
            datepicker.setSelectedDates(dates);
            if (nativeInput && options.selectionMode !== 'single') nativeInput.value = dates.join(',');
        },
        dispose() {
            element.removeEventListener('change', selected);
            datepicker.dispose();
        }
    };
}
