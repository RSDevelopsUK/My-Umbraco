function addNewSlide(data) {
  $('.slider').slick('slickAdd', data);
}

$(document).ready(function () {
  // Media Downloads

  $(".media-preview-button").on("click",
    function() {
      $("#media-preview").css("background-image", "url('" + $(this).data("url") + "')");
      $("#media-preview-download").attr("href", $(this).data("url"));
    });

  // Mobile Menu 

  $("#mainmenu").on("show.bs.collapse",
    function() {
      $("#mainmenu").append($("#patram-nav-wrapper").html());
    });

  $("#mainmenu").on("hidden.bs.collapse",
    function() {
      $("#mainmenu #patram-nav").remove();
    });

  $(window).on("resize",
    function() {
      if (window.innerWidth > 991) {
        $("#mainmenu").collapse("hide");
      }
    });

  // Profile

  $("#validatedCustomFile").on("change",
    function () {
      const fileName = $("#validatedCustomFile").val().substring($("#validatedCustomFile").val().lastIndexOf("\\") + 1);
      $("#avatar-label").text(fileName);
    });

  // Success alerts

  if ($("#success-alert").length > 0) {
    setTimeout(function(){
      $("#success-alert").alert("close");
    }, 5000);
  }

  // Forms

  $(document).on("click", ".use-ajax", function (e) {
    e.preventDefault();
    const formId = $(this).data("form");
    const formIdHash = `#${formId}`;

    if (formId === "form-load-more") {
      //const row = $("#row").val();
      //const newRow = parseInt(row) + 1;
      //$(`#row-${newRow}`).removeClass("d-none");
      //$("#row").val(newRow);
      const skip = parseInt($("#skip").val());
      const skipBy = parseInt($("#skip-by").val());
      const newSkip = skip + skipBy;
      $("#skip").val(newSkip);

      //if (parseInt($("#announcement-count").val()) === $(".announcement:visible").length) {
      //  $(this).hide();
      //}
    }

    $(formIdHash).submit();
  });

  // Slider

  $(".slider").slick({
    slidesToShow: 2,
    slidesToScroll: 1,
    autoplay: false,
    autoplaySpeed: 2000,
    infinite: false,
    arrows: false,
    dots: false
  });
  $('button.next').click(function () {
    $(".slider").slick("slickNext");
  });
  $('button.prev').click(function () {
    $(".slider").slick("slickPrev");
  });

});