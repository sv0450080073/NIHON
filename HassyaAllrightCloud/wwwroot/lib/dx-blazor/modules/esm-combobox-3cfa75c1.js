import"./esm-chunk-f9104efc.js";import{EnsureElement as e,AttachEventToElement as t,elementIsInDOM as o,DetachEventFromElement as n}from"./esm-dom-utils-d4fe413b.js";import{D as r,R as a}from"./esm-chunk-6ca2c4f2.js";import{T as u}from"./esm-chunk-2f760454.js";import{K as s}from"./esm-chunk-710198b6.js";import{OnOutsideClick as i,IsDropDownVisible as c}from"./esm-dropdowns-b8e38328.js";import"./esm-chunk-95f069f9.js";import{ScrollToSelectedItem as d,HasParametersForVirtualScrollingRequest as m,GetParametersForVirtualScrollingRequest as f}from"./esm-grid-91fdba30.js";function l(e){return e==s.Tab||16<=e&&e<=20}var v=200,y=v/2;function k(e){var t=D(e);d(t)}function p(t){t=e(t),document.activeElement===t&&(C(t),g(t))}function C(e){e&&!e.readOnly&&e.dataset.nullText&&e.value===e.dataset.nullText&&(e.value="")}function w(e,t){e.dataset.timerId&&(clearTimeout(e.dataset.timerId),delete e.dataset.timerId),t||window.setTimeout(function(){w(e,!0)},y)}function g(e){e&&e.dataset.focusedClass&&(e.className=e.dataset.focusedClass)}function T(e,t){return t&&!function(e){l(e.keyCode)||(e.target.dataset.previousValue=e.target.value)}(e),function(e){return h(e)}(e)&&(e.stopPropagation(),e.preventDefault()),!1}function b(e,t,o,n){var r=!1;if(n&&(r=r||function(e){if(l(e.keyCode))return!1;var t=e.target;return!(t&&t.dataset.previousValue===e.target.value)}(e)),r=r||function(e){var t=e.altKey&&(e.keyCode==s.Down||e.keyCode==s.Up),o=h(e),n=e.keyCode==s.Esc||e.keyCode==s.Enter;return t||o||n}(e)){var a=e.target.value;if(o&&m(o)){var u=f(o,!1);t.invokeMethodAsync("OnComboBoxListBoxKeyUp",a,e.keyCode,e.altKey,e.ctrlKey,u.itemHeight,u.scrollTopForRequest,u.scrollHeightForRequest)}else t.invokeMethodAsync("OnComboBoxKeyUp",a,e.keyCode,e.altKey,e.ctrlKey)}}function h(e){return e.keyCode===s.Down||e.keyCode===s.Up||e.keyCode===s.PageUp||e.keyCode===s.PageDown||e.ctrlKey&&(e.keyCode===s.Home||e.keyCode===s.End)}function I(e,t,o){if(t){var n=(new Date).getTime();t.dataset.lastLostFocusTime&&n-t.dataset.lastLostFocusTime<v+100&&!o||(t.dataset.lastLostFocusTime=(new Date).getTime(),e.invokeMethodAsync("OnComboBoxLostFocus",t.value))}}function D(t){return(t=e(t)).querySelector("div.dxbs-dm.dropdown-menu")}function x(s,d,m,f,l){if(s=e(s)){r(s),f="true"===f.toLowerCase(),l="true"===l.toLowerCase(),d=e(d);var y=D(s),p=y;l&&k(s);var h=function(e){return T(e,f)},x=function(e){return b(e,m,p,f)},K=function(e){return function(e,t){var o=e.target;C(o),g(o)}(e)},j=function(e){return function(e,t){var o=e.target;w(o,!0),o.dataset.timerId=window.setTimeout(function(){if(delete o.dataset.timerId,function(e){e&&e.dataset.bluredClass&&(e.className=e.dataset.bluredClass)}(o),document.activeElement!==o)try{I(t,o)}catch(e){}},v)}(e,m)},E=function(e){return w(d)},F=function(e){return i(e,s,function(){o(s)||r(s);var e=document.activeElement===d,t=d.dataset.timerId>0,n=y&&c(y);w(d),(e||t||n)&&I(m,d,!0)})};return t(d,"keydown",h),t(d,"keyup",x),t(d,"focus",K),t(d,"blur",j),t(s,"mousedown",E),t(document,u.touchMouseDownEventName,F),a(s,function(){n(d,"keydown",h),n(d,"keyup",x),n(d,"focus",K),n(d,"blur",j),n(s,"mousedown",E),n(document,u.touchMouseDownEventName,F)}),Promise.resolve("ok")}}function K(t){if(t=e(t))return r(t),Promise.resolve("ok")}const j={Init:x,Dispose:K,PrepareInputIfFocused:p,ScrollToSelectedItem:k};export default j;export{K as Dispose,x as Init,p as PrepareInputIfFocused,k as ScrollToSelectedItem};