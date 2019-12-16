angular.module("umbraco").controller("WNTI.HeroVideoGridEditor.Controller", function ($scope, mediaHelper, entityResource, editorService, notificationsService) {

  function initConfig() {

    // Make a "shortcut" for the grid editor configuration
    var cfg = $scope.control.editor.config;

    if (!cfg) cfg = {};

    if (!cfg.heroVideo) cfg.heroVideo = {};
    if (!cfg.heroVideo.mode) cfg.heroVideo.mode = 'optional';
    if (!cfg.heroVideo.placeholder) cfg.heroVideo.placeholder = '';

    if (!cfg.heroText) cfg.heroText = {};
    if (!cfg.heroText.mode) cfg.heroText.mode = 'optional';
    if (!cfg.heroText.placeholder) cfg.heroText.placeholder = '';

    cfg.heroVideo.required = cfg.heroVideo.mode == 'required';
    cfg.heroText.required = cfg.heroText.mode == 'required';

    $scope.cfg = cfg;

  }

  function initValue() {
    if ($scope.control.value === null) return;
    if (!$scope.control.value.videoId) return;

    // Use the entityResource to look up data about the media (as we only store the ID in our control value)
    entityResource.getById($scope.control.value.videoId, 'media').then(function (data) {
      setVideo(data);
    });

  }

  function setVideo(video) {

    // Make sure we have an object as value
    if (!$scope.control.value) $scope.control.value = {};

    // Reset the video properties if no video id specified
    if (!video) {
      $scope.control.value.videoId = 0;
      $scope.video = null;
      $scope.videoUrl = null;
      return;
    }

    // Set the video ID in the control value
    $scope.control.value.videoId = video.id;

    // Update the UI
    $scope.video = video;
    $scope.videoUrl = (video.video ? video.video : mediaHelper.resolveFileFromEntity(video)) + '?width=' + 500 + '&mode=crop';

  }

  $scope.selectVideo = function () {
    var options = {
      heroText: 'Select Video',
      multiPicker: false,
      onlyImages: false,
      disableFolderSelect: true,
      show: true,
      submit: function (model) {

        // Get the first video (there really only should be one)
        var data = model.selection[0];

        if (data.extension !== "mp4") {
          notificationsService.error("Wrong Document Type!", "Please select an .mp4 video");
        } else {
          setVideo(data);

          editorService.close();
        }
      },
      close: function () {
        editorService.close();
      }
    }
    editorService.mediaPicker(options);
  };

  $scope.selectLink = function () {
    var options = {
      heroText: 'Select link',
      multiPicker: false,
      submit: function (model) {
        $scope.control.value.link = model.target.url;

        editorService.close();

      },
      close: function () {
        editorService.close();
      }
    }

    editorService.linkPicker(options);
  }

  $scope.removeVideo = function () {
    setVideo(null);
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