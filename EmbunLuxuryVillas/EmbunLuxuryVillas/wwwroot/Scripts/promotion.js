$(document).ready(function () {

    // Animation for terms and conditions
    $(".terms-and-condition, .close-button").click(function () {
        $.each(this.classList, function (index, value) {
            if (value.search("_") != -1) {
                var promotionId = value.split("_").pop();
                if ($(".promotion-overlay_" + promotionId).hasClass("slideInUp")) {
                    if ($(".promotion-overlay_" + promotionId).hasClass("hidden")) {
                        $(".promotion-overlay-container_" + promotionId).removeClass("hidden");
                        $(".promotion-overlay_" + promotionId).removeClass("hidden");

                        $('.terms-and-condition_' + promotionId).html("Hide terms &amp; conditions");
                    } else {
                        $(".promotion-overlay_" + promotionId).removeClass("slideInUp");
                        $(".promotion-overlay_" + promotionId).addClass("slideOutDown");
                        $('.terms-and-condition_' + promotionId).html("Show terms &amp; conditions");

                        setTimeout(function () {
                            $(".promotion-overlay-container_" + promotionId).addClass("hidden");
                        }, 1000);
                    }
                } else {
                    if (!$(".promotion-overlay-container_" + promotionId).hasClass("hidden")) {
                        return;
                    }
                    $(".promotion-overlay-container_" + promotionId).removeClass("hidden");
                    $(".promotion-overlay_" + promotionId).removeClass("slideOutDown");
                    $(".promotion-overlay_" + promotionId).addClass("slideInUp");

                    $('.terms-and-condition_' + promotionId).html("Hide terms &amp; conditions");
                }
            }
        });
    });
});