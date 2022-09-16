
window.koboSlideDown = (target, duration = 500) => {
    target.style.removeProperty('display');
    let display = window.getComputedStyle(target).display;

    if (display === 'none')
        display = 'block';

    target.style.display = display;
    let height = target.offsetHeight;
    target.style.pointerEvents = 'none';
    target.style.overflow = 'hidden';
    target.style.height = 0;
    target.style.paddingTop = 0;
    target.style.paddingBottom = 0;
    target.style.marginTop = 0;
    target.style.marginBottom = 0;
    target.offsetHeight;
    target.style.boxSizing = 'border-box';
    target.style.transitionProperty = "height, margin, padding";
    target.style.transitionDuration = duration + 'ms';
    target.style.height = height + 'px';
    target.style.removeProperty('padding-top');
    target.style.removeProperty('padding-bottom');
    target.style.removeProperty('margin-top');
    target.style.removeProperty('margin-bottom');
    window.setTimeout(() => {
        target.style.removeProperty('pointer-events');
        target.style.removeProperty('height');
        target.style.removeProperty('overflow');
        target.style.removeProperty('transition-duration');
        target.style.removeProperty('transition-property');
    }, duration);
}

window.koboSlideUp = (target, duration = 500) => {
    target.style.pointerEvents = 'none';
    target.style.transitionProperty = 'height, margin, padding';
    target.style.transitionDuration = duration + 'ms';
    target.style.boxSizing = 'border-box';
    target.style.height = target.offsetHeight + 'px';
    target.offsetHeight;
    target.style.overflow = 'hidden';
    target.style.height = 0;
    target.style.paddingTop = 0;
    target.style.paddingBottom = 0;
    target.style.marginTop = 0;
    target.style.marginBottom = 0;
    window.setTimeout(() => {
        target.style.removeProperty('pointer-events');
        target.style.display = 'none';
        target.style.removeProperty('height');
        target.style.removeProperty('padding-top');
        target.style.removeProperty('padding-bottom');
        target.style.removeProperty('margin-top');
        target.style.removeProperty('margin-bottom');
        target.style.removeProperty('overflow');
        target.style.removeProperty('transition-duration');
        target.style.removeProperty('transition-property');
        //alert("!");
    }, duration);
}

window.toggle = function toggle(id, isExpand) {
    let el = document.getElementById(id);
    if (el) {
        return isExpand ? koboSlideDown(el, 200) : koboSlideUp(el, 200);
    }
}

window.initMenu = function initMenu() {
    $(document).on('click', '#kobo-menu-btn', function (event) {
        event.preventDefault();
        $('#kobo-vertical-menu').toggleClass('minimize');
        // Get grids in current page
        let grids = $('.dxbs-gridview');
        // get width of current container
        
        if (grids && grids.length) {
            for (let grid of grids) {
                let currentWidth = $(grid).parent().width();
                let child = $(grid).find('.card').children();
                if (child && child.length > 1) {
                    // get header and body of grid
                    let gridPaths = child.slice(0, 2);
                    for (let path of gridPaths) {
                        if ($(path).has('table')) {
                            $(path).width(currentWidth);
                        }
                    }
                }
            }
        }
    });
}