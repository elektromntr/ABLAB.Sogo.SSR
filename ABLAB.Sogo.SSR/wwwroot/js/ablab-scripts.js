function initializePopularApartmentsComponentOwlCarousel() {
    if ($('.property-carousel').length) {
        $('.property-carousel').owlCarousel({
            loop: true,
            margin: 30,
            nav: true,
            smartSpeed: 1000,
            autoplay: true,
            navText: ['<span class="la la-caret-square-o-left"></span>', '<span class="la la-caret-square-o-right"></span>'],
            responsive: {
                0: {
                    items: 1
                },
                600: {
                    items: 1
                },
                1024: {
                    items: 1
                }
            }
        });
    }
}