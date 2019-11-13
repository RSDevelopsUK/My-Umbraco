angular.module("umbraco").controller("MyUmbraco.BlockEditorGridEditor.Controller", function ($scope, mediaHelper, entityResource, editorService) {

  function initConfig() {

    // Make a "shortcut" for the grid editor configuration
    var cfg = $scope.control.editor.config;

    if (!cfg) cfg = {};

    if (!cfg.quote) cfg.quote = {};
    if (!cfg.quote.mode) cfg.quote.mode = "optional";
    if (!cfg.quote.placeholder) cfg.quote.placeholder = "";

    if (!cfg.quoteSourceText) cfg.quoteSourceText = {};
    if (!cfg.quoteSourceText.mode) cfg.quoteSourceText.mode = "optional";
    if (!cfg.quoteSourceText.placeholder) cfg.quoteSourceText.placeholder = "";

    if (!cfg.quoteSourceLink) cfg.quoteSourceLink = {};
    if (!cfg.quoteSourceLink.mode) cfg.quoteSourceLink.mode = "optional";
    if (!cfg.quoteSourceLink.placeholder) cfg.quoteSourceLink.placeholder = "";

    if (!cfg.quoteSourceLinkText) cfg.quoteSourceLinkText = {};
    if (!cfg.quoteSourceLinkText.mode) cfg.quoteSourceLinkText.mode = "optional";
    if (!cfg.quoteSourceLinkText.placeholder) cfg.quoteSourceLinkText.placeholder = "";

    cfg.quote.required = cfg.quote.mode === "required";
    cfg.quoteSourceText.required = cfg.quoteSourceText.mode === "required";
    cfg.quoteSourceLink.required = cfg.quoteSourceLink.mode === "required";
    cfg.quoteSourceLinkText.required = cfg.quoteSourceLinkText.mode === "required";

    $scope.cfg = cfg;

  }

  $scope.selectLink = function () {
    const options = {
      heroText: "Select link",
      multiPicker: false,
      submit: function (model) {
        $scope.control.value.quoteSourceLink = model.target.url;

        editorService.close();

      },
      close: function () {
        editorService.close();
      }
    }

    editorService.linkPicker(options);
  }

});