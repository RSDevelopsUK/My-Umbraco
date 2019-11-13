angular.module("umbraco").controller("MyUmbraco.HeroImageGridEditor.Controller", function ($scope, mediaHelper, entityResource, editorService) {

  function initConfig() {

    // Make a "shortcut" for the grid editor configuration
    var cfg = $scope.control.editor.config;

    if (!cfg) cfg = {};

    if (!cfg.heroImage) cfg.heroImage = {};
    if (!cfg.heroImage.mode) cfg.heroImage.mode = 'optional';
    if (!cfg.heroImage.placeholder) cfg.heroImage.placeholder = '';

    if (!cfg.heroText) cfg.heroText = {};
    if (!cfg.heroText.mode) cfg.heroText.mode = 'optional';
    if (!cfg.heroText.placeholder) cfg.heroText.placeholder = '';

    cfg.heroImage.required = cfg.heroImage.mode == 'required';
    cfg.heroText.required = cfg.heroText.mode == 'required';

    $scope.cfg = cfg;

  }

  function initValue() {
    if ($scope.control.value === null) return;
    if (!$scope.control.value.imageId) return;

    // Use the entityResource to look up data about the media (as we only store the ID in our control value)
    entityResource.getById($scope.control.value.imageId, 'media').then(function (data) {
      setImage(data);
    });

  }

  function setImage(image) {

    // Make sure we have an object as value
    if (!$scope.control.value) $scope.control.value = {};

    // Reset the image properties if no image id specified
    if (!image) {
      $scope.control.value.imageId = 0;
      $scope.image = null;
      $scope.imageUrl = null;
      return;
    }

    // Set the image ID in the control value
    $scope.control.value.imageId = image.id;

    // Update the UI
    $scope.image = image;
    $scope.imageUrl = (image.image ? image.image : mediaHelper.resolveFileFromEntity(image)) + '?width=' + 500 + '&mode=crop';

  }

  $scope.selectImage = function () {
    var options = {
      heroText: 'Select image',
      multiPicker: false,
      onlyImages: true,
      disableFolderSelect: true,
      show: true,
      submit: function (model) {

        // Get the first image (there really only should be one)
        var data = model.selection[0];

        setImage(data);

        editorService.close();

      },
      close: function () {
        editorService.close();
      }
    }
    editorService.mediaPicker(options);
  };

  $scope.removeImage = function () {
    setImage(null);
  };

  $scope.rteconfig =
  {
    "toolbar": ["code", "removeformat", "styleselect", "bold", "alignleft", "aligncenter", "alignright", "bullist", "numlist", "outdent", "indent", "link", "unlink"],
    "stylesheets": ["rte"],
    "dimensions":
    {
      "height": 750
    },
    "maxImageSize": 1170
  };

  initConfig();

  initValue();

});