import { Dialog } from '../css/bootstrap/js/bootstrap.bundle.min.js';

export function connect(element, receiver, backdrop) {
    const dialog = new Dialog(element, { backdrop: backdrop ? true : 'static' });
    const closed = () => receiver.invokeMethodAsync('NotifyClosed');
    element.addEventListener('hidden.bs.dialog', closed);
    return {
        show() { return dialog.show(); },
        hide() { return dialog.hide(); },
        dispose() {
            element.removeEventListener('hidden.bs.dialog', closed);
            dialog.dispose();
        }
    };
}
