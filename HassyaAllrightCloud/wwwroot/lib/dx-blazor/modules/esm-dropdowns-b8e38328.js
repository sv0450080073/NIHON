import"./esm-chunk-f9104efc.js";import{EnsureElement as e,AttachEventToElement as n,GetParentByClassName as t,changeDom as o,elementIsInDOM as r,DetachEventFromElement as c}from"./esm-dom-utils-d4fe413b.js";import{D as u,R as s}from"./esm-chunk-6ca2c4f2.js";import{T as i}from"./esm-chunk-2f760454.js";import{K as a}from"./esm-chunk-710198b6.js";const l=document.body,d={},f=[],m={subtree:!0,childList:!0},v=new MutationObserver(function(e,n){for(let n=0;n<e.length;n++)e[n].removedNodes.forEach(function(e){f.filter(n=>e===n.element).map(e=>e.callback).forEach(e=>e())})});function E(e){return d[e]||(d[e]=new Promise((n,t)=>{0===f.length&&v.observe(l,m),function(e,n){var t={element:e,callback:function(){f.splice(f.indexOf(t),1),delete d[e],0===f.length&&v.disconnect(),n()}};f.push(t)}(e,n)}))}const h={Popup:0,Modal:1};function p(e,n,t){for(var o=e.target;o;){if(o===n)return!1;o=o.parentElement}t&&t()}function D(e){return"none"!==e.style.display}const g="a[href], input:not([disabled]), button:not([disabled]), [tabindex]:not([tabindex='-1'])";const b={Init:function(t,o,a,l){if(t=e(t),o=e(o),a=e(a),t){if(u(t),a){var d=function(e){return p(e,t,function(){r(t)||u(t);var e=document.activeElement===o,n=a&&D(a);(e||n)&&l.invokeMethodAsync("OnDropDownLostFocus",o.value)})};n(document,i.touchMouseDownEventName,d),s(t,function(){c(document,i.touchMouseDownEventName,d)})}return Promise.resolve("ok")}},Dispose:function(n){return(n=e(n))&&u(n),Promise.resolve("ok")},ShowAdaptiveDropdown:function(n,r,c,u,s){n=e(n),t(n,c).querySelector("."+u);var l=document.documentElement,d=!1;function f(e){(!n.contains(e.srcElement)||r===h.Modal&&n===e.srcElement)&&v()}function m(e){l.removeEventListener("focusin",m),null===e.relatedTarget&&e.target&&n.contains(e.target)&&e.target.focus()}function v(){d||(d=!0,p(),s.invokeMethodAsync("CloseDialog"))}function p(){l.removeEventListener(i.touchMouseDownEventName,f)}function D(){var e=n.querySelector(g);e&&e.focus()}return l.addEventListener(i.touchMouseDownEventName,f),n.addEventListener("keydown",function(e){e.keyCode===a.Esc&&v()}),n.addEventListener("focusout",function(e){var t,o,r,c;d||(null===e.relatedTarget||n.contains(e.relatedTarget)?null===e.relatedTarget&&l.addEventListener("focusin",m):void((t=n.compareDocumentPosition(e.relatedTarget))&Node.DOCUMENT_POSITION_PRECEDING?(r=n,c=r.querySelectorAll(g),o=c[c.length-1],void(o&&o.focus())):t&Node.DOCUMENT_POSITION_FOLLOWING&&D()))}),r===h.Modal&&n.addEventListener("touchmove",e=>{e.srcElement===n&&e.preventDefault()}),E(n).then(()=>{p()}),o(D),Promise.resolve()}};export default b;export{D as IsDropDownVisible,p as OnOutsideClick};