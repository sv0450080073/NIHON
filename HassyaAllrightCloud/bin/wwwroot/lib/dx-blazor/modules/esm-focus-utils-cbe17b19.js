import"./esm-chunk-eaca7b99.js";import{x as n,D as o,A as s,R as t}from"./esm-chunk-64a12c37.js";import{D as e,R as c}from"./esm-chunk-0778bac7.js";import{T as u}from"./esm-chunk-c94f8125.js";function a(e){function c(n){e.contains(n.srcElement)&&s(e,"dxbs-focus-hidden")}function a(n){!e.contains(n.relatedTarget)&&document.hasFocus()&&t(e,"dxbs-focus-hidden")}function i(n){9===n.keyCode&&t(e,"dxbs-focus-hidden")}var d=document.documentElement;return n(d,u.touchMouseDownEventName,c),n(e,"keydown",i),n(e,"focusout",a),function(){o(d,u.touchMouseDownEventName,c),o(e,"keydown",i),o(e,"focusout",a)}}function i(n){if(n){e(n);var o=a(n);c(n,o)}}const d={AttachEventsForFocusHiding:a,InitFocusHidingEvents:i};export default d;export{a as AttachEventsForFocusHiding,i as InitFocusHidingEvents};