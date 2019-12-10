var loadMoreCheck = function() {
  if (parseInt($("#item-count").val()) > $(".lazy-item:visible").length) {
    $("#btn-load-more").show();
  }
}

$(document).ready(function () {
  // Profile
  $("#validatedCustomFile").on("change",
    function () {
      const fileName = $("#validatedCustomFile").val().substring($("#validatedCustomFile").val().lastIndexOf("\\") + 1);
      $("#avatar-label").html(`<span>${fileName}</span>`);
    });

  // Success alerts
  if ($("#success-alert").length > 0) {
    setTimeout(function () {
      $("#success-alert").alert("close");
    }, 5000);
  }

  // Submit Delete Avatar button
  $(document).on("click",
    "#submit-delete-avatar",
    function(e) {
      e.preventDefault();

      $("#delete-avatar-form").submit();
    });

  // Submit Ajax form
  $(document).on("click", ".use-ajax", function (e) {
    e.preventDefault();
    const formId = $(this).data("form");
    const formIdHash = `#${formId}`;

    console.log(formIdHash);

    if (formId === "form-load-more") {
      const row = $("#row").val();
      const newRow = parseInt(row) + 1;
      $(`#row-${newRow}`).removeClass("d-none");
      $("#row").val(newRow);
      const skip = parseInt($("#skip").val());
      const skipBy = parseInt($("#skip-by").val());
      const newSkip = skip + skipBy;
      $("#skip").val(newSkip);

      loadMoreCheck();
    }
    $(formIdHash).submit();
  });

  loadMoreCheck();

  // Copy code
  $(".copy-code").on("click",
    function () {
      const $temp = $("<textarea>");
      $("body").append($temp);
      $temp.val($(this).siblings("code").text()).select();
      document.execCommand("copy");
      $temp.remove();
    });

  // Popovers
  $(function() {
    $('[data-toggle="popover"]').popover({
      trigger: "focus"
    });
  });

});

