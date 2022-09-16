_dxBlazorInternal.define("cjs-combobox-e39421aa.js", function (e, t, n) { e("./cjs-chunk-c43b3f7c.js"); var o = e("./cjs-dom-utils-6a09eac3.js"), s = e("./cjs-chunk-509db829.js"), a = e("./cjs-chunk-9109506d.js"), r = e("./cjs-chunk-7aa0d757.js"), c = e("./cjs-dropdowns-5546f970.js"); e("./cjs-chunk-e26655d2.js"); var u = e("./cjs-grid-b52f8083.js"); function i(e) { return e == r.Key.Tab || 16 <= e && e <= 20 } var l = 200, d = l / 2; function m(e) { var t = C(e); u.ScrollToSelectedItem(t) } function f(e) { e = o.EnsureElement(e), document.activeElement === e && (v(e), y(e)) } function v(e) { e && !e.readOnly && e.dataset.nullText && e.value === e.dataset.nullText && (e.value = "") } function E(e, t) { e.dataset.timerId && (clearTimeout(e.dataset.timerId), delete e.dataset.timerId), t || window.setTimeout(function () { E(e, !0) }, d) } function y(e) { e && e.dataset.focusedClass && (e.className = e.dataset.focusedClass) } function j(e, t) { return t && !function (e) { i(e.keyCode) || (e.target.dataset.previousValue = e.target.value) }(e), function (e) { return h(e) }(e) && (e.stopPropagation(), e.preventDefault()), !1 } function k(e, t, n, o) { var s = !1; if (o && (s = s || function (e) { if (i(e.keyCode)) return !1; var t = e.target; return !(t && t.dataset.previousValue === e.target.value) }(e)), s = s || function (e) { var t = e.altKey && (e.keyCode == r.Key.Down || e.keyCode == r.Key.Up), n = h(e), o = e.keyCode == r.Key.Esc || e.keyCode == r.Key.Enter; return t || n || o }(e)) { var a = e.target.value; if (n && u.HasParametersForVirtualScrollingRequest(n)) { var c = u.GetParametersForVirtualScrollingRequest(n, !1); t.invokeMethodAsync("OnComboBoxListBoxKeyUp", a, e.keyCode, e.altKey, e.ctrlKey, c.itemHeight, c.scrollTopForRequest, c.scrollHeightForRequest) } else t.invokeMethodAsync("OnComboBoxKeyUp", a, e.keyCode, e.altKey, e.ctrlKey) } } function h(e) { return e.keyCode === r.Key.Down || e.keyCode === r.Key.Up || e.keyCode === r.Key.PageUp || e.keyCode === r.Key.PageDown || e.ctrlKey && (e.keyCode === r.Key.Home || e.keyCode === r.Key.End) } function p(e, t, n) { if (t) { var o = (new Date).getTime(); t.dataset.lastLostFocusTime && o - t.dataset.lastLostFocusTime < l + 100 && !n || (t.dataset.lastLostFocusTime = (new Date).getTime(), e.invokeMethodAsync("OnComboBoxLostFocus", t.value)) } } function C(e) { return (e = o.EnsureElement(e)).querySelector("div.dxbs-dm.dropdown-menu") } function D(e, t, n, r, u) { if (e = o.EnsureElement(e)) { s.DisposeEvents(e), r = "true" === r.toLowerCase(), u = "true" === u.toLowerCase(), t = o.EnsureElement(t); var i = C(e), d = i; u && m(e); var f = function (e) { return j(e, r) }, h = function (e) { return k(e, n, d, r) }, D = function (e) { return function (e, t) { var n = e.target; v(n), y(n) }(e) }, T = function (e) { return function (e, t) { var n = e.target; E(n, !0), n.dataset.timerId = window.setTimeout(function () { if (delete n.dataset.timerId, function (e) { e && e.dataset.bluredClass && (e.className = e.dataset.bluredClass) }(n), document.activeElement !== n) try { p(t, n) } catch (e) { } }, l) }(e, n) }, w = function (e) { return E(t) }, I = function (a) { return c.OnOutsideClick(a, e, function () { o.elementIsInDOM(e) || s.DisposeEvents(e); var a = document.activeElement === t, r = t.dataset.timerId > 0, u = i && c.IsDropDownVisible(i); E(t), (a || r || u) && p(n, t, !0) }) }; return o.AttachEventToElement(t, "keydown", f), o.AttachEventToElement(t, "keyup", h), o.AttachEventToElement(t, "focus", D), o.AttachEventToElement(t, "blur", T), o.AttachEventToElement(e, "mousedown", w), o.AttachEventToElement(document, a.TouchUIHelper.touchMouseDownEventName, I), s.RegisterDisposableEvents(e, function () { o.DetachEventFromElement(t, "keydown", f), o.DetachEventFromElement(t, "keyup", h), o.DetachEventFromElement(t, "focus", D), o.DetachEventFromElement(t, "blur", T), o.DetachEventFromElement(e, "mousedown", w), o.DetachEventFromElement(document, a.TouchUIHelper.touchMouseDownEventName, I) }), Promise.resolve("ok") } } function T(e) { if (e = o.EnsureElement(e)) return s.DisposeEvents(e), Promise.resolve("ok") } var w = { Init: D, Dispose: T, PrepareInputIfFocused: f, ScrollToSelectedItem: m }; n.Dispose = T, n.Init = D, n.PrepareInputIfFocused = f, n.ScrollToSelectedItem = m, n.default = w }, ["cjs-chunk-c43b3f7c.js", "cjs-dom-utils-6a09eac3.js", "cjs-chunk-509db829.js", "cjs-chunk-9109506d.js", "cjs-chunk-7aa0d757.js", "cjs-dropdowns-5546f970.js", "cjs-chunk-e26655d2.js", "cjs-grid-b52f8083.js"]);