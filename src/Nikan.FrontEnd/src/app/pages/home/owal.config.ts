import { OwlOptions } from 'ngx-owl-carousel-o';

export const OWL_OPTIONS: OwlOptions = {
  rtl: true,
  loop: true,
  nav: true,
  autoplay: true,
  autoplayHoverPause: true,
  smartSpeed: 1000,
  autoplayTimeout: 8000,
  autoWidth: true,
  navText: [ '&#8250;','&#8249'],
  responsive: {
    0: {
      items: 1,
    },
    480: {
      items: 2,
    },
    768: {
      items: 3,
    },
    //600: {
    //    items: 5,
    //},
    //700: {
    //    items: 6,
    //},
    //847: {
    //    items: 7,
    //},
    //992: {
    //    items: 5,
    //},
    1100: {
      items: 5,
    },
    1200: {
      items: 5,
    },
    1367: {
      items: 5,
    },
  },
};
