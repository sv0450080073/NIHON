//Set Scrollbar chart
function customScrollbar() {
    $(".mouse-event").each(function () {
        var div1 = $(this).find('.busData--normal');
        var div2 = $(this).find('.busData--rentalBus');
        var div3 = $(this).find('.busData--spareBus');

        div2.scrollLeft(div1.scrollLeft());
        div3.scrollLeft(div1.scrollLeft());

        div1.scroll(function () {
            div2.scrollLeft($(this).scrollLeft());
            div3.scrollLeft($(this).scrollLeft());
        });

        div2.scroll(function () {
            div1.scrollLeft($(this).scrollLeft());
            div3.scrollLeft($(this).scrollLeft());
        });

        div3.scroll(function () {
            div1.scrollLeft($(this).scrollLeft());
            div2.scrollLeft($(this).scrollLeft());
        });
    })
}

//Mouse scroll
function Roll(mode) {
    if (mode == 1) {
        var sliderall = document.querySelectorAll('.busData--normal');
        var slidergreen = document.querySelector('.busData--rentalBus');
        var slidergray = document.querySelector('.busData--spareBus');
        let isDown = false;
        let startX;
        let scrollLeft;
        let classname;

        document.addEventListener('mouseover', function (e) {
            classname = e.target.className;
            classname = classname.trim();        
        }, false);

        sliderall.forEach(function (slider) {
            slider.addEventListener('mousedown', (e) => {
                if (classname == "listTimeline__item") {
                    isDown = true;
                    slider.classList.add('active');
                    startX = e.pageX - slider.offsetLeft;
                    scrollLeft = slider.scrollLeft;
                }
            });
            slider.addEventListener('mouseleave', () => {
                if (classname == "listTimeline__item") {
                    isDown = false;
                    slider.classList.remove('active');
                }
            });
            slider.addEventListener('mouseup', () => {
                if (classname == "listTimeline__item") {
                    isDown = false;
                    slider.classList.remove('active');
                }
            });
            slider.addEventListener('mousemove', (e) => {
                if (classname == "listTimeline__item") {
                    if (!isDown) return;
                    e.preventDefault();
                    const x = e.pageX - slider.offsetLeft;
                    const walk = (x - startX) * 1.5; //scroll-fast
                    slider.scrollLeft = scrollLeft - walk;
                }
            });

            if (slidergreen != null) {
                slidergreen.addEventListener('mousedown', (e) => {
                    if (classname == "listTimeline__item  listTimeline__item--rentalBus") {
                        isDown = true;
                        slidergreen.classList.add('active');
                        startX = e.pageX - slider.offsetLeft;
                        scrollLeft = slider.scrollLeft;
                    }
                });
                slidergreen.addEventListener('mouseleave', () => {
                    if (classname == "listTimeline__item  listTimeline__item--rentalBus") {
                        isDown = false;
                        slidergreen.classList.remove('active');
                    }
                });
                slidergreen.addEventListener('mouseup', () => {
                    if (classname == "listTimeline__item  listTimeline__item--rentalBus") {
                        isDown = false;
                        slidergreen.classList.remove('active');
                    }
                });
                slidergreen.addEventListener('mousemove', (e) => {
                    if (classname == "listTimeline__item  listTimeline__item--rentalBus") {
                        if (!isDown) return;
                        e.preventDefault();
                        const x = e.pageX - slider.offsetLeft;
                        const walk = (x - startX) * 1.5; //scroll-fast
                        slider.scrollLeft = scrollLeft - walk;
                    }
                });
            }

            if (slidergray != null) {
                slidergray.addEventListener('mousedown', (e) => {
                    if (classname == "listTimeline__item   listTimeline__item--spareBus") {
                        isDown = true;
                        slidergray.classList.add('active');
                        startX = e.pageX - slidergray.offsetLeft;
                        scrollLeft = slider.scrollLeft;
                    }
                });
                slidergray.addEventListener('mouseleave', () => {
                    if (classname == "listTimeline__item   listTimeline__item--spareBus") {
                        isDown = false;
                        slidergray.classList.remove('active');
                    }
                });
                slidergray.addEventListener('mouseup', () => {
                    if (classname == "listTimeline__item   listTimeline__item--spareBus") {
                        isDown = false;
                        slidergray.classList.remove('active');
                    }
                });
                slidergray.addEventListener('mousemove', (e) => {
                    if (classname == "listTimeline__item   listTimeline__item--spareBus") {
                        if (!isDown) return;
                        e.preventDefault();
                        const x = e.pageX - slider.offsetLeft;
                        const walk = (x - startX) * 1.5; //scroll-fast
                        slider.scrollLeft = scrollLeft - walk;
                    }
                });
            }
        });
    }    
}

// tooltip number driver
function TooltipNumberDriver() {
    $(document).ready(function () {
        $('.listColumn__item .description [data-toggle="tooltip"]').each(function () {
            $(this).tooltip({
                sanitize: false,
                title: $(this).attr("data-original-title"),
                trigger: "hover",
            });
        })
    })
}