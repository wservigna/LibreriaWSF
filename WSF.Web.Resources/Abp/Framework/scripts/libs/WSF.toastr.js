var WSF = WSF || {};
(function () {

    if (!toastr) {
        return;
    }

    /* DEFAULTS *************************************************/

    toastr.options.positionClass = 'toast-bottom-right';

    /* NOTIFICATION *********************************************/

    var showNotification = function (type, message, title) {
        toastr[type](message, title);
    };

    WSF.notify.success = function (message, title) {
        showNotification('success', message, title);
    };

    WSF.notify.info = function (message, title) {
        showNotification('info', message, title);
    };

    WSF.notify.warn = function (message, title) {
        showNotification('warning', message, title);
    };

    WSF.notify.error = function (message, title) {
        showNotification('error', message, title);
    };

})();