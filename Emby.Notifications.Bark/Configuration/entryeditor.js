define(['globalize', 'pluginManager', 'emby-input'], function (globalize, pluginManager) {
    'use strict';

    function EntryEditor() {
    }

    EntryEditor.setObjectValues = function (context, entry) {

        entry.FriendlyName = context.querySelector('.txtFriendlyName').value;
        entry.Options.ServerUrl = context.querySelector('.txtServerUrl').value;
        entry.Options.DeviceKey = context.querySelector('.txtDeviceKey').value;
        entry.Options.Params = context.querySelector('.txtParameter').value;
    };

    EntryEditor.setFormValues = function (context, entry) {

        context.querySelector('.txtFriendlyName').value = entry.FriendlyName || '';
        context.querySelector('.txtServerUrl').value = entry.Options.ServerUrl || '';
        context.querySelector('.txtDeviceKey').value = entry.Options.DeviceKey || '';
        context.querySelector('.txtParameter').value = entry.Options.Params || '';
    };

    EntryEditor.loadTemplate = function (context) {

        return require(['text!' + pluginManager.getConfigurationResourceUrl('barkeditortemplate')]).then(function (responses) {

            var template = responses[0];
            context.innerHTML = globalize.translateDocument(template);

            // setup any required event handlers here
        });
    };

    EntryEditor.destroy = function () {

    };

    return EntryEditor;
});