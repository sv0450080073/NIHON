_dxBlazorInternal.define("cjs-checkbox-3a64a69a.js", function (n, e, c) { n("./cjs-chunk-c43b3f7c.js"); var t = n("./cjs-dom-utils-6a09eac3.js"); function a(n, e, c, a, i) { if (n = t.EnsureElement(n)) { n.indeterminate = c; var s = function (n, e) { return e ? 0 : n ? 1 : 2 }(e, c), u = e; n.onchange = function (e) { a ? (function (n, e) { var c = !1, t = !1; 0 === e ? c = !0 : 1 === e && (t = !0); n.indeterminate = c, n.checked = t }(n, s = (s + 1) % 3), u = function (n) { return 0 === n ? null : 1 === n }(s)) : u = !u, i.invokeMethodAsync("RaiseCheckedChanged", u) } } } var i = { Init: a }; c.Init = a, c.default = i }, ["cjs-chunk-c43b3f7c.js", "cjs-dom-utils-6a09eac3.js"]);