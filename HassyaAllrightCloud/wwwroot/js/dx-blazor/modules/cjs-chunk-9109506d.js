_dxBlazorInternal.define("cjs-chunk-9109506d.js", function (o, e, n) { var t = o("./cjs-chunk-c43b3f7c.js"), c = o("./cjs-dom-utils-6a09eac3.js"), u = {}; function r(o, e, n, t) { t && i(e) && s(e), void 0 === u[e] && (u[e] = setTimeout(function () { o(), u[e] = void 0 }, n)) } function i(o) { return !!u[o] } function s(o) { clearTimeout(u[o]), u[o] = void 0 } var a = { touchMouseDownEventName: t.Browser.WebKitTouchUI ? "touchstart" : t.Browser.Edge && t.Browser.MSTouchUI && window.PointerEvent ? "pointerdown" : "mousedown", touchMouseUpEventName: t.Browser.WebKitTouchUI ? "touchend" : t.Browser.Edge && t.Browser.MSTouchUI && window.PointerEvent ? "pointerup" : "mouseup", touchMouseMoveEventName: t.Browser.WebKitTouchUI ? "touchmove" : t.Browser.Edge && t.Browser.MSTouchUI && window.PointerEvent ? "pointermove" : "mousemove" }, h = function () { var o, e, n = 500, t = 0; function u(o, e, n, t) { o[t = t || n.name] || (o[t] = n), c.DetachEventFromElement(o, e, n = o[t]), c.AttachEventToElement(o, e, n) } function a(e) { v(e) && (t++ , r(function () { !function (e) { 1 === t && (t = 0, o.call(this, e)) }(e) }, "longPress", n, !0)) } function h(o) { v(o) && (t = 0, s("longPress")) } function m(o) { i("longPress") && (t = 0, s("longPress")) } function v(o) { return o.timeStamp !== e && (e = o.timeStamp, !0) } return { attachEventToElement: u, attachLongTabEventToElement: function (e, n) { o = n, u(e, "touchstart", a, "ts"), u(e, "touchend", h, "te"), u(e, "touchmove", m, "tm") }, longTouchTimeout: n } }(); n.LongTabEventHelper = h, n.TouchUIHelper = a, n.clearUnforcedFunctionByKey = s, n.hasUnforcedFunction = i, n.unforcedFunctionCall = r }, ["cjs-chunk-c43b3f7c.js", "cjs-dom-utils-6a09eac3.js"]);