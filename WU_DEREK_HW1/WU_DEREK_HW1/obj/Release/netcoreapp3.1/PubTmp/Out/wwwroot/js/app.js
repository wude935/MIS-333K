$(document).ready(function () {
    // selects the correct nav-item to active
    var path = window.location.pathname;
    $('ul.navbar-nav li a').each(function () {
        if (this.getAttribute('path') == path) {
            $("ul.navbar-nav li").each(function () {
                if ($(this).hasClass("active")) {
                    $(this).removeClass("active");
                }
            });
            $(this).parent().addClass('active');
        }
    });
});

// creates the typewriter effect
var TxtType = function (el, toRotate, period) {
    this.toRotate = toRotate;
    this.el = el;
    this.loopNum = 0;
    this.period = parseInt(period, 10) || 2000;
    this.txt = '';
    this.tick();
    this.isDeleting = false;
};

TxtType.prototype.tick = function () {
    var i = this.loopNum % this.toRotate.length;
    var fullTxt = this.toRotate[i];

    if (this.isDeleting) {
        this.txt = fullTxt.substring(0, this.txt.length - 1);
    } else {
        this.txt = fullTxt.substring(0, this.txt.length + 1);
    }

    this.el.innerHTML = '<span class="wrap">' + this.txt + '</span>';

    var that = this;
    var delta = 200 - Math.random() * 100;

    if (this.isDeleting) { delta /= 2; }

    if (!this.isDeleting && this.txt === fullTxt) {
        delta = this.period;
        this.isDeleting = true;
    } else if (this.isDeleting && this.txt === '') {
        this.isDeleting = false;
        this.loopNum++;
        delta = 500;
    }

    setTimeout(function () {
        that.tick();
    }, delta);
};

window.onload = function () {
    var elements = document.getElementsByClassName('typewrite');
    for (var i = 0; i < elements.length; i++) {
        var toRotate = elements[i].getAttribute('data-type');
        var period = elements[i].getAttribute('data-period');
        if (toRotate) {
            new TxtType(elements[i], JSON.parse(toRotate), period);
        }
    }

    var css = document.createElement("style");
    css.type = "text/css";
    css.innerHTML = ".typewrite > .wrap { padding-right: .1em; border-right: 0.05em solid #2780e3; }";
    document.body.appendChild(css);
};

// creates scrollpoints
var controller = new ScrollMagic.Controller();
new ScrollMagic.Scene({ triggerElement: "#skillsTrigger", offset: -100 })
    .on("enter leave", () => {
        // animates the scollbar 
        jQuery('.skillbar').each(function () {
            jQuery(this).find('.skillbar-bar').animate({
                width: jQuery(this).attr('data-percent')
            }, 3000);
        });
    })
    .addTo(controller);

// creates popups
$('#inline-popups,#inline-popups-2,#inline-popups-3,#inline-popups-4,#inline-popups-5,#inline-popups-6, #inline-popups-7, #inline-popups-8, #inline-popups-9, #inline-popups-10').magnificPopup({
    delegate: 'a',
    removalDelay: 400, //delay removal by X to allow out-animation
    callbacks: {
        beforeOpen: function () {
            this.st.mainClass = this.st.el.attr('data-effect');
        }
    },
    midClick: true
});
