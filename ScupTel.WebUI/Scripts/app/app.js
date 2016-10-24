var ScupTel = ScupTel || {};
ScupTel.api = location.protocol + '//' + location.hostname + (location.port ? ':' + location.port : '') + "/";
ScupTel.namespace = function (ns_string) {
    var parts = ns_string.split('.'),
        parent = ScupTel,
        i;

    if (parts[0] === "ScuptTel") {
        parts = parts.slice(1);
    }
    for (i = 0; i < parts.length; i++) {
        if (typeof parent[parts[i]] === "undefined") {
            parent[parts[i]] = {};
        }
        return parent = parent[parts[i]];
    }
    return parent;
};

ScupTel.App = (function () {


   

    var $private = {}, $public = {};

    $(function () {
        $private.assignEvents();
        $public.initializeDataTables();
        $private.initializeToastr();
        $(document).ajaxError(function (event, xhr, options, thrownError) {
            var erro = $(xhr.responseText).filter("span").find("h2 > i").text();
            if (erro != "")
                toastr["error"](erro, "Erro")
        });
    });

    $public.initializeMasks = function () {
        $.each($('.numberOnlyMask'), function (i, item) {
            var length = ($(item).data("length") || 5) + 1;
            $(item).mask(Array(length).join("0"));
        });

        $(".moneyMask").maskMoney({
            showSymbol: true,
            symbol: "R$",
            decimal: ",",
            thousands: "."
        });
    };

    $private.initializeToastr = function () {
        $public.toastr = toastr;
        $public.toastr.options = {
            "closeButton": true,
            "debug": false,
            "progressBar": true,
            "positionClass": "toast-top-right",
            "onclick": null,
            "showDuration": "400",
            "hideDuration": "1000",
            "timeOut": "3000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }
    };

    $private.assignEvents = function () {
        $(document).on("click", ".datatables_novo", $private.newItem);
        $(document).on("click", ".datatables_editar", $private.editItem);
        $(document).on("click", ".datatables_excluir", $private.deleteItem);
        $(document).on("click", ".modalQuestionButton", $private.clickModalQuestionButton);
        $('.modal').on('show.bs.modal', $private.centralizeModal);
        $(window).on("resize", $private.centralizeVisibleModals);
    };


    $private.centralizeModal = function () {
        var $dialog = $(this).find(".modal-dialog"),
            offset = ($(window).height() - $dialog.height()) / 2;

        $(this).css('display', 'block');

        // Center modal vertically in window
        $dialog.css("margin-top", offset);
    };

    $private.centralizeVisibleModals = function () {
        $('.modal:visible').each($private.centralizeModal);
    };
 
    $private.serializeJsonToQuerystring = function (obj, prefix) {
        var str = [],
            k, p;

        for (p in obj) {
            if (obj.hasOwnProperty(p)) {
                k = prefix ? prefix + "[" + p + "]" : p, v = obj[p];
                str.push(typeof v == "object" ?
                $private.serializeJsonToQuerystring(v, k) :
                  encodeURIComponent(k) + "=" + encodeURIComponent(v));
            }
        }
        return str.join("&");
    }

    $private.newItem = function (e) {

        var table = $(this).closest("div").find("table[data-edit-action]"),
            customData = table.data("custom-data"),
            action = table.data("edit-action");

        if (table.data("edit-mode") == "NewWindow") {
            window.location.href = action + "?" + $private.serializeJsonToQuerystring(customData);
        } else {
            $("#loading").show();
            $("#form-modal-container").load(action + "?" + $private.serializeJsonToQuerystring(customData), null, function () {
                $public.initializeMasks();
            });

            $("#form-modal-container").off("validated.pf.form", $private.clickModalFormButton);
            $("#form-modal-container").one("validated.pf.form", $private.clickModalFormButton);
            $("#loading").hide();
        }

        e.preventDefault();
    };

    $private.editItem = function (e) {
        var ids = $private.getKeysFromDataTables($(this)),
            table = $(this).closest("div").find("table[data-edit-action]"),
            action = $private.getEditAction($(this));

        e.preventDefault();

        if (table.data("edit-mode") == "NewWindow") {
            window.location.href = action + "?" + $private.serializeJsonToQuerystring(ids);
        }
        else {
            $("#loading").show();
            $("#form-modal-container").load(action + "?" + $private.serializeJsonToQuerystring(ids), null, function () {
                $public.initializeMasks();                
            });

            $("#form-modal-container").off("validated.pf.form", $private.clickModalFormButton);
            $("#form-modal-container").one("validated.pf.form", $private.clickModalFormButton);
            $("#loading").hide();
        }
    };

    $private.deleteItem = function (e) {
        var ids = $private.getKeysFromDataTables($(this)),
            action = $private.getDeleteAction($(this));
        e.preventDefault();

        $("#loading").show();
        $("#form-modal-container").load(action + "?" + $private.serializeJsonToQuerystring(ids));
        $("#loading").hide();
    };

    $private.getKeysFromDataTables = function (editButton) {
        var keys = {},
            htmlTable = editButton.closest("table"),
            dataTablesObject = $(htmlTable).dataTable(),
            keyPropertyNames = decodeURIComponent(htmlTable.data("key-properties")).split("&"),
            rowIndex = editButton.closest("tr").index(),
            value = "";

        $.each(keyPropertyNames, function (i, item) {
            value = dataTablesObject.fnGetData(rowIndex)[item];
            keys[item] = value;
        });

        return keys;
    };

    $private.getEditAction = function (editButton) {
        return editButton.closest("table").data("edit-action");
    };

    $private.getDeleteAction = function (row) {
        return row.closest("table").data("delete-action");
    };

    $private.getFormData = function (form) {
        if ($private.getFormEncType(form) == "multipart/form-data") {
            return new FormData(form[0]);
        }
        else {
            return form.serialize();
        }
    };

    $private.getFormEncType = function (form) {
        return form.prop("enctype");
    };

 
    $private.clickModalFormButton = function (e, button) {

        var action = $(button).data("action-name"),
             notification = {
                 type: $(button).data("success-notification-type"),
                 message: $(button).data("success-notification-message"),
                 title: $(button).data("success-notification-title")
             },
             form = $(button).closest("form"),
             formData = $private.getFormData(form),
             formEncType = $private.getFormEncType(form);

        e.preventDefault();

        $private.postAction(action, formData, formEncType, notification).done(function (data) {
            $("#" + $(button).data("success-update-container-id")).html(data);
            $public.initializeDataTables();

            $(button).closest(".modal").modal('hide');
            if (notification.type)
                ScupTel.App.toastr[notification.type.toLowerCase()](notification.message, notification.title);
        });
    };

    $private.clickModalQuestionButton = function (e) {
        var that = this,
           ids = $private.getKeysFromParent($(that)),
           action = $(that).data("action-name"),
           notification = {
               type: $(that).data("notification-type"),
               message: $(that).data("notification-message"),
               title: $(that).data("notification-title")
           };


        e.preventDefault();
        $private.postAction(action + "?" + $private.serializeJsonToQuerystring(ids), null, null, notification).done(function (data) {
            $("#" + $(that).data("update-container-id")).html(data);
            $public.initializeDataTables();
            $(that).closest(".modal").modal('hide');
            if (notification.type)
                ScupTel.App.toastr[notification.type.toLowerCase()](notification.message, notification.title)
        });
    };

    $private.postAction = function (action, formData, formEncType, notification) {
        $("#loading").show();
        return $.ajax({
            url: action,
            method: "POST",
            data: formData,
            processData: formEncType != "multipart/form-data",
            contentType: (formEncType == "application/x-www-form-urlencoded" ? 'application/x-www-form-urlencoded; charset=UTF-8' : (formEncType == "multipart/form-data" ? false : formEncType)),
            complete: function (data) {
                $("#loading").hide();
            }
        });
    };

   

    $private.getKeysFromParent = function (element) {
        var keyValuePairs = decodeURIComponent(element.parent().data("key-properties")).split('&');
        dictionary = {};
        $.each(keyValuePairs, function (i, item) {
            var keyValuePair = item.split('=');
            dictionary[keyValuePair[0]] = keyValuePair[1];
        });
        return dictionary;
    }

    $public.initializeDataTables = function () {
        var tables = {};

        $.each($('.datatables'), function (i, item) {

            var columns = $.map($(item).find("th[data-property-name]"),
                    function (val, i) {
                        var columnConfig = {
                            data: $(val).data("property-name"),
                            visible: $(val).data("property-visible") == "True"
                        };

                        if ($(val).data("type") == "Date") {
                            columnConfig.type = "date";
                            columnConfig.render = function (data, type, full) {
                                if (data) {
                                    return moment(data, null, "pt-BR").format("DD/MM/YYYY");
                                }
                                else
                                    return "";
                            };
                        }
                        else if ($(val).data("type") == "DateTime") {
                            columnConfig.type = "dateTime";
                            columnConfig.render = function (data, type, full) {
                                if (data) {
                                    return moment(data, null, "pt-BR").format("DD/MM/YYYY HH:mm:SS");
                                }
                                else
                                    return "";
                            };
                        }
                        else if ($(val).data("type") == "Currency") {
                            columnConfig.type = "currency";
                            columnConfig.render = function (data, type, full) {
                                if (data) {
                                    return data.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
                                }
                                else
                                    return "";
                            };
                        }
                        return columnConfig;
                    });

            if ($(item).data("allow-edit") == "True" || $(item).data("allow-delete") == "True") {
                var buttons = "";

                if ($(item).data("allow-edit") == "True")
                    buttons += '<a href="" class="datatables_editar">Editar</a>';

                if ($(item).data("allow-delete") == "True") {
                    if ($(item).data("allow-edit") == "True") {
                        buttons += '|';
                    }

                    buttons += '<a href="" class="datatables_excluir">Deletar</a>';
                }

                columns.push({
                    data: null,
                    defaultContent: buttons,
                    orderable: false
                });
            }

            var customData = null;

            if ($(item).data("custom-data")) {
                customData = $(item).data("custom-data");
            }

            tables[$(item).attr("name")] = ($(item).DataTable({
                "columns": columns,
                "processing": true,
                "serverSide": true,
                "ajax": $.fn.dataTable.pipeline({
                    url: $(item).data("action"),
                    pages: 5,
                    method: "POST",
                    data: customData,
                }),
                "searching": false,
                "language": {
                    "sEmptyTable": "Nenhum registro encontrado",
                    "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
                    "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
                    "sInfoFiltered": "(Filtrados de _MAX_ registros)",
                    "sInfoPostFix": "",
                    "sInfoThousands": ".",
                    "sLengthMenu": "_MENU_ resultados por página",
                    "sLoadingRecords": "Carregando...",
                    "sProcessing": "Processando...",
                    "sZeroRecords": "Nenhum registro encontrado",
                    "sSearch": "Pesquisar",
                    "oPaginate": {
                        "sNext": "Próximo",
                        "sPrevious": "Anterior",
                        "sFirst": "Primeiro",
                        "sLast": "Último"
                    },
                    "oAria": {
                        "sSortAscending": ": Ordenar colunas de forma ascendente",
                        "sSortDescending": ": Ordenar colunas de forma descendente"
                    }
                },
                "retrieve": $.fn.DataTable.isDataTable(item)
            }));

        });

        return tables;
    };

    return $public;
}());