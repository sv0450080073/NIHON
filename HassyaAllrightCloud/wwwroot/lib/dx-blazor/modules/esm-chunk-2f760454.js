import{B as o}from"./esm-chunk-f9104efc.js";import{DetachEventFromElement as t,AttachEventToElement as e}from"./esm-dom-utils-d4fe413b.js";var n={};function u(o,t,e,u){u&&i(t)&&c(t),void 0===n[t]&&(n[t]=setTimeout(function(){o(),n[t]=void 0},e))}function i(o){return!!n[o]}function c(o){clearTimeout(n[o]),n[o]=void 0}var s={touchMouseDownEventName:o.WebKitTouchUI?"touchstart":o.Edge&&o.MSTouchUI&&window.PointerEvent?"pointerdown":"mousedown",touchMouseUpEventName:o.WebKitTouchUI?"touchend":o.Edge&&o.MSTouchUI&&window.PointerEvent?"pointerup":"mouseup",touchMouseMoveEventName:o.WebKitTouchUI?"touchmove":o.Edge&&o.MSTouchUI&&window.PointerEvent?"pointermove":"mousemove"},m=function(){var o,n,s=500,m=0;function r(o,n,u,i){o[i=i||u.name]||(o[i]=u),t(o,n,u=o[i]),e(o,n,u)}function a(t){f(t)&&(m++,u(function(){!function(t){1===m&&(m=0,o.call(this,t))}(t)},"longPress",s,!0))}function h(o){f(o)&&(m=0,c("longPress"))}function v(o){i("longPress")&&(m=0,c("longPress"))}function f(o){return o.timeStamp!==n&&(n=o.timeStamp,!0)}return{attachEventToElement:r,attachLongTabEventToElement:function(t,e){o=e,r(t,"touchstart",a,"ts"),r(t,"touchend",h,"te"),r(t,"touchmove",v,"tm")},longTouchTimeout:s}}();export{m as L,s as T,c,i as h,u};
