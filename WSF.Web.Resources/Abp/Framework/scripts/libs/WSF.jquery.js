var WSF = WSF || {};
(function ($) {

    /* JQUERY ENHANCEMENTS ***************************************************/

    // WSF.ajax -> uses $.ajax ------------------------------------------------

    //TODO: Think to implement success, error and complete callbacks
    WSF.ajax = function (userOptions) {
        userOptions = userOptions || {};
        var options = $.extend({}, WSF.ajax.defaultOpts, userOptions);
        return $.Deferred(function ($dfd) {
            $.ajax(options)
                .done(function (data) {
                    WSFAjaxHelper.handleData(data, userOptions, $dfd);
                }).fail(function () {
                    $dfd.reject.apply(this, arguments);
                });
        });
    };

    WSF.ajax.defaultOpts = {
        dataType: 'json',
        type: 'POST',
        contentType: 'application/json'
    };

    /* JQUERY PLUGIN ENHANCEMENTS ********************************************/

    /* jQuery Form Plugin 
     * http://www.malsup.com/jquery/form/
     */

    // WSFAjaxForm -> uses ajaxForm ------------------------------------------

    $.fn.WSFAjaxForm = function (userOptions) {
        userOptions = userOptions || {};

        var options = $.extend({}, $.fn.WSFAjaxForm.defaults, userOptions);

        options.beforeSubmit = function () {
            WSFAjaxHelper.blockUI(options);
            userOptions.beforeSubmit && userOptions.beforeSubmit.apply(this, arguments);
        };

        options.success = function (data) {
            WSFAjaxHelper.handleData(data, userOptions);
        };

        //TODO: Error?

        options.complete = function () {
            WSFAjaxHelper.unblockUI(options);
            userOptions.complete && userOptions.complete.apply(this, arguments);
        };

        return this.ajaxForm(options);
    };

    $.fn.WSFAjaxForm.defaults = {
        method: 'POST'
    };

    /* PRIVATE METHODS *******************************************************/

    //TODO: Extract block/spin options

    //Used on ajax request
    var WSFAjaxHelper = {

        blockUI: function (options) {
            if (options.blockUI) {
                if (options.blockUI === true) { //block whole page
                    WSF.ui.setBusy();
                } else { //block an element
                    WSF.ui.setBusy(options.blockUI);
                }
            }
        },

        unblockUI: function (options) {
            if (options.blockUI) {
                if (options.blockUI === true) { //unblock whole page
                    WSF.ui.clearBusy();
                } else { //unblock an element
                    WSF.ui.clearBusy(options.blockUI);
                }
            }
        },

        handleData: function (data, userOptions, $dfd) {
            if (data) {
                if (data.success === true) {
                    $dfd && $dfd.resolve(data.result, data);
                    userOptions.success && userOptions.success(data.result, data);
                } else { //data.success === false
                    if (data.error) {
                        WSF.message.error(data.error.message);
                        $dfd && $dfd.reject(data.error);
                        userOptions.error && userOptions.error(data.error);
                    }

                    if (data.unAuthorizedRequest && !data.targetUrl) {
                        location.reload();
                    }
                }

                if (data.targetUrl) {
                    location.href = data.targetUrl;
                }
            } else { //no data sent to back
                $dfd && $dfd.resolve();
                userOptions.success && userOptions.success();
            }
        }
    };

})(jQuery);