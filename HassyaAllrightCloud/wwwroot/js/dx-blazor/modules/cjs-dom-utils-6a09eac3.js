_dxBlazorInternal.define("cjs-dom-utils-6a09eac3.js",function(e,t,n){var r=e("./cjs-chunk-c43b3f7c.js");function o(e){return function(e,t,n){var r=e.length;if(!r)return e;if(r<764833){var o=e;return t&&(o=o.replace(/^\s+/,"")),n&&(o=o.replace(/\s+$/,"")),o}var i=0;if(n)for(;r>0&&l[e.charCodeAt(r-1)];)r--;if(t&&r>0)for(;i<r&&l[e.charCodeAt(i)];)i++;return e.substring(i,r)}(e,!0,!0)}var i,l={9:1,10:1,11:1,12:1,13:1,32:1,133:1,160:1,5760:1,6158:1,8192:1,8193:1,8194:1,8195:1,8196:1,8197:1,8198:1,8199:1,8200:1,8201:1,8202:1,8203:1,8232:1,8233:1,8239:1,8287:1,12288:1};function u(e){if("object"!=typeof e||null==e)return e;var t={};for(var n in e)t[n]=e[n];return t}function s(){var e=r.Browser.IE&&"hidden"==N(document.body).overflow&&document.body.scrollLeft>0;return r.Browser.Edge||e?document.body?document.body.scrollLeft:document.documentElement.scrollLeft:r.Browser.WebKitFamily?document.documentElement.scrollLeft||document.body.scrollLeft:document.documentElement.scrollLeft}function a(){var e=r.Browser.IE&&"hidden"==N(document.body).overflow&&document.body.scrollTop>0;return r.Browser.WebKitFamily||r.Browser.Edge||e?r.Browser.MacOSMobilePlatform?window.pageYOffset:r.Browser.WebKitFamily&&document.documentElement.scrollTop||document.body.scrollTop:document.documentElement.scrollTop}function c(){if(void 0===i){var e=document.createElement("DIV");e.style.cssText="position: absolute; top: 0px; left: 0px; visibility: hidden; width: 200px; height: 150px; overflow: hidden; box-sizing: content-box",document.body.appendChild(e);var t=document.createElement("P");e.appendChild(t),t.style.cssText="width: 100%; height: 200px;";var n=t.offsetWidth;e.style.overflow="scroll";var r=t.offsetWidth;n==r&&(r=e.clientWidth),i=n-r,document.body.removeChild(e)}return i}function d(e){return m(e)}function f(e){return p(e)}function m(e){return r.Browser.IE?function(e){if(null==e||r.Browser.IE&&null==e.parentNode)return 0;return e.getBoundingClientRect().left+s()}(e):r.Browser.Firefox&&r.Browser.Version>=3?h(e):r.Browser.WebKitFamily||r.Browser.Edge?h(e):getAbsolutePositionX_Other(e)}function h(e){return null==e?0:e.getBoundingClientRect().left+s()}function p(e){return r.Browser.IE?function(e){if(null==e||r.Browser.IE&&null==e.parentNode)return 0;return e.getBoundingClientRect().top+a()}(e):r.Browser.Firefox&&r.Browser.Version>=3?g(e):r.Browser.WebKitFamily||r.Browser.Edge?g(e):getAbsolutePositionY_Other(e)}function g(e){return null==e?0:e.getBoundingClientRect().top+a()}function y(e,t,n){return e-=w(t,n)}function v(e,t){var n=function(e){var t=document.createElement("DIV");return t.style.top="0px",t.style.left="0px",t.visibility="hidden",t.style.position=N(e).position,t}(e);"static"==n.style.position&&(n.style.position="absolute"),e.parentNode.appendChild(n);var r=t?d(n):f(n);return e.parentNode.removeChild(n),r}function w(e,t){return v(e,t)}function b(e,t){try{var n,r=E(e);if(!r){var o=B(e);if(!o)return!1;n=o.split(" ")}for(var i=t.split(" "),l=i.length-1;l>=0;l--)if(r){if(-1===r.indexOf(i[l]))return!1}else if(Data.ArrayIndexOf(n,i[l])<0)return!1;return!0}catch(e){return!1}}function E(e){return e.classList?[].slice.call(e.classList):B(e).replace(/^\s+|\s+$/g,"").split(/\s+/)}function B(e){return"svg"===e.tagName?e.className.baseVal:e.className}function S(e,t){"svg"===e.tagName?e.className.baseVal=o(t):e.className=o(t)}function C(e,t){var n=t.toUpperCase(),o=null;return e&&(e.getElementsByTagName?0===(o=e.getElementsByTagName(n)).length&&(o=e.getElementsByTagName(t)):e.all&&void 0!==e.all.tags&&(o=r.Browser.Netscape?e.all.tags[n]:e.all.tags(n))),o}function I(e,t,n){return null!=e?function(e,t){return t||(t=0),null!=e&&e.length>t?e[t]:null}(C(e,t),n):null}function N(e){if(document.defaultView&&document.defaultView.getComputedStyle){var t=document.defaultView.getComputedStyle(e,null);if(!t&&r.Browser.Firefox&&window.frameElement){for(var n=[],o=window.frameElement;!(t=document.defaultView.getComputedStyle(e,null));)n.push([o,o.style.display]),x(o,"display","block",!0),o="BODY"==o.tagName?o.ownerDocument.defaultView.frameElement:o.parentNode;t=u(t);for(var i,l=0;i=n[l];l++)x(i[0],"display",i[1])}return r.Browser.Firefox&&r.Browser.MajorVersion>=62&&window.frameElement&&0===t.length&&((t=u(t)).display=e.style.display),t}return window.getComputedStyle(e,null)}function T(e){if(!e.createStyleSheet){var t=e.createElement("STYLE");return I(e,"HEAD",0).appendChild(t),t.sheet}try{return e.createStyleSheet()}catch(e){throw new Error("The CSS link limit (31) has been exceeded. Please enable CSS merging or reduce the number of CSS files on the page. For details, see http://www.devexpress.com/Support/Center/p/K18487.aspx.")}}var F=null;function x(e,t,n,r){if(r){var o=t.search("[A-Z]");-1!=o&&(t=t.replace(t.charAt(o),"-"+t.charAt(o).toLowerCase())),e.style.setProperty?e.style.setProperty(t,n,"important"):e.style.cssText+=";"+t+":"+n+"!important"}else e.style[t]=n}function P(e){"string"==typeof e&&(e=document.querySelector(e)),e&&e.focus()}function W(e,t,n){e&&(e[t]=n)}function D(e,t){e&&(e.indeterminate=t)}function O(e){return e.preventDefault?e.preventDefault():e.returnValue=!1,!1}function A(e){if(!e)return null;var t=null;if("string"==typeof e){var n=JSON.parse(e);n&&n.__internalId&&(t=n.__internalId)}(!t&&e.__internalId&&(t=e.__internalId),t)&&(e=document.querySelector("["+("_bl_"+t)+"]"));return e.tagName||(e=null),e}function L(e,t,n){(e=A(e))&&W(e,t,n)}function _(e,t){e.removeAttribute?e.removeAttribute(t):e.removeProperty&&e.removeProperty(t)}function G(e,t,n){n?W(e,t,n):_(e,t)}function V(e,t){return j(e,t)+H(e,t)}function M(e,t){return R(e,t)+q(e,t)}function j(e,t){var n=t||N(e);return parseInt(n.paddingLeft)+parseInt(n.paddingRight)}function R(e,t){var n=t||N(e);return parseInt(n.paddingTop)+parseInt(n.paddingBottom)}function q(e,t){t||(t=r.Browser.IE&&9!==r.Browser.MajorVersion&&window.getComputedStyle?window.getComputedStyle(e):N(e));var n=0;return"none"!==t.borderTopStyle&&(n+=parseFloat(t.borderTopWidth),r.Browser.IE&&r.Browser.MajorVersion<9&&(n+=getIe8BorderWidthFromText(t.borderTopWidth))),"none"!==t.borderBottomStyle&&(n+=parseFloat(t.borderBottomWidth),r.Browser.IE&&r.Browser.MajorVersion<9&&(n+=getIe8BorderWidthFromText(t.borderBottomWidth))),n}function H(e,t){t||(t=r.Browser.IE&&window.getComputedStyle?window.getComputedStyle(e):N(e));var n=0;return"none"!==t.borderLeftStyle&&(n+=parseFloat(t.borderLeftWidth),r.Browser.IE&&r.Browser.MajorVersion<9&&(n+=getIe8BorderWidthFromText(t.borderLeftWidth))),"none"!==t.borderRightStyle&&(n+=parseFloat(t.borderRightWidth),r.Browser.IE&&r.Browser.MajorVersion<9&&(n+=getIe8BorderWidthFromText(t.borderRightWidth))),n}var Y=window.requestAnimationFrame||function(e){e()},K=window.cancelAnimationFrame||function(e){};function U(e){return Y(e)}var k=function(e){this.requestFrame=e,this.cache=[[]],this.isInFrame=!1,this.frameTimestamp=null,this.isWaiting=!1,this.getBuffer=function(e){return e||(e=0),this.cache.length<=e&&(this.cache[e]=[]),this.cache[e]},this.execute=function(e,t){if(!this.isInFrame)return!1;var n=this.cache[t||0];return null===n?e(z,this.frameTimestamp):(n=this.getBuffer(t)).push(e),!0},this.runAll=function(e){this.isWaiting=!1,this.isInFrame=!0,this.frameTimestamp=e;for(var t=0;t<this.cache.length;t++){var n=this.cache[t];if(n)for(this.cache[t]=null;n.length;)n.shift()(z,this.frameTimestamp)}this.waitNextFrame()},this.waitNextFrame=function(){this.cache=[[]],this.isInFrame=!1,this.isWaiting=!1},this.requestExecution=function(e,t){var n=this;return new Promise(function(r){function o(t,n){r(e(t,n))}n.execute(o,t)||(n.getBuffer(t).push(o),!1===n.isWaiting&&(n.isWaiting=!0,n.requestFrame(n.runAll.bind(n))))})}},z=null;function X(e){var t=new k(e);return t.requestExecution.bind(t)}var $=X(function(e){return z=U(e)}),J=X(function(e){return $(function(){setTimeout(e)})});function Q(e){return $(e)}function Z(e){return J(e)}var ee=[],te=50;function ne(e){0===ee.length?(ee.push(e),Z(re)):ee.push(e)}function re(){(ee=ee.filter(function(e){return e()})).length>0&&setTimeout(function(){Z(re)},te)}function oe(e,t){var n=[];for(var r in t)t.hasOwnProperty(r)&&n.push({attr:r,value:t[r]});if(1===n.length)e.style[n[0].attr]=n[0].value;else{var o="";if(e.style.cssText)for(var i=e.style.cssText.split(";").map(function(e){return e.trim().split(":")}),l=0;l<i.length;l++){var u=i[l];2===u.length&&void 0===t[u[0]]&&(o+=u[0]+":"+u[1].trim()+";")}for(l=0;l<n.length;l++){var s=n[l];""!==s.value&&(o+=s.attr+":"+s.value+";")}G(e,"style",o)}}function ie(e,t){for(var n in null===t.inlineStyles?_(e,"style"):oe(e,t.inlineStyles),t.attributes)t.attributes.hasOwnProperty(n)&&G(e,n,t.attributes[n]);var r=E(t);if(r){var o=t.cssClassToggleInfo,i=r.filter(function(e){return!1!==o[e]});for(var l in o)o.hasOwnProperty(l)&&o[l]&&-1===i.indexOf(l)&&i.push(l);var u=i.join(" ");u?S(e,u):_(e,"class")}}function le(e){return{inlineStyles:{},cssClassToggleInfo:{},className:B(e),attributes:{}}}function ue(e,t){if(void 0===e.length){var n=e;n._dxCurrentFrameElementStateInfo?t(n._dxCurrentFrameElementStateInfo):(t(n._dxCurrentFrameElementStateInfo=le(n)),Q(function(){ie(n,n._dxCurrentFrameElementStateInfo),n._dxCurrentFrameElementStateInfo=null}))}else for(var r=0;r<e.length;r++)ue(e[r],t)}var se={FocusElement:P,SetInputAttribute:L,SetCheckInputIndeterminate:D};n.AddClassNameToElement=function(e,t){if(e&&"string"==typeof t&&!b(e,t=t.trim())&&""!==t){var n=B(e);S(e,""===n?t:n+" "+t)}},n.AttachEventToElement=function(e,t,n,r,o){e.addEventListener?e.addEventListener(t,n,{capture:!r,passive:!!o}):e.attachEvent("on"+t,n)},n.CancelAnimationFrame=function(e){K(e)},n.CloneObject=u,n.CreateStyleSheetInDocument=T,n.DetachEventFromElement=function(e,t,n){e.removeEventListener?e.removeEventListener(t,n,!0):e.detachEvent("on"+t,n)},n.ElementHasCssClass=b,n.EnsureElement=A,n.FocusElement=P,n.GetAbsolutePositionX=m,n.GetAbsolutePositionY=p,n.GetAbsoluteX=d,n.GetAbsoluteY=f,n.GetClassName=B,n.GetClassNameList=E,n.GetCurrentStyle=N,n.GetCurrentStyleSheet=function(){return F||(F=T(document)),F},n.GetDocumentClientHeight=function(){return r.Browser.Firefox&&window.innerHeight-document.documentElement.clientHeight>c()?window.innerHeight:r.Browser.Opera&&r.Browser.Version<9.6||0==document.documentElement.clientHeight?document.body.clientHeight:document.documentElement.clientHeight},n.GetDocumentClientWidth=function(){return 0==document.documentElement.clientWidth?document.body.clientWidth:document.documentElement.clientWidth},n.GetDocumentScrollLeft=s,n.GetDocumentScrollTop=a,n.GetHorizontalBordersWidth=H,n.GetLeftRightBordersAndPaddingsSummaryValue=V,n.GetLeftRightPaddings=j,n.GetNodeByTagName=I,n.GetNodesByTagName=C,n.GetParentByClassName=function(e,t){return function(e,t,n){for(;null!=e;){if("BODY"==e.tagName||"#document"==e.nodeName)return null;if(n(e,t))return e;e=e.parentNode}return null}(e,t,b)},n.GetPositionElementOffset=w,n.GetTopBottomBordersAndPaddingsSummaryValue=M,n.GetTopBottomPaddings=R,n.GetVerticalBordersWidth=q,n.GetVerticalScrollBarWidth=c,n.Log=function(e){},n.PrepareClientPosForElement=y,n.PreventEvent=O,n.PreventEventAndBubble=function(e){return O(e),e.stopPropagation&&e.stopPropagation(),e.cancelBubble=!0,!1},n.QuerySelectorFromRoot=function(e,t){e.dataset.tempUniqueId="tempUniqueId";try{t("[data-temp-unique-id]")}catch(e){}finally{delete e.dataset.tempUniqueId}},n.RemoveClassNameFromElement=function(e,t){if(e){var n=" "+B(e)+" ",r=n.replace(" "+t+" "," ");n.length!=r.length&&S(e,o(r))}},n.RequestAnimationFrame=U,n.RetrieveByPredicate=function(e,t){for(var n=[],r=0;r<e.length;r++){var o=e[r];t&&!t(o)||n.push(o)}return n},n.SetAbsoluteX=function(e,t){e.style.left=y(t,e,!0)+"px"},n.SetAbsoluteY=function(e,t){e.style.top=y(t,e,!1)+"px"},n.SetAttribute=W,n.SetCheckInputIndeterminate=D,n.SetClassName=S,n.SetInputAttribute=L,n.SetStylesCore=x,n.applyStateToElement=ie,n.applyStyles=oe,n.calculateStyles=Z,n.changeDom=Q,n.clearStyles=function(e){ue(e,function(e){e.inlineStyles=null})},n.createElementState=le,n.default=se,n.elementIsInDOM=function(e){return document.body.contains(e)},n.findParentBySelector=function(e,t){if(!e)return null;if(e.closest)return e.closest(t);do{if((e.matches||e.msMatchesSelector).call(e,t))return e;e=e.parentElement||e.parentNode}while(e&&"BODY"!==e.tagName);return null},n.getDocumentScrollLeft=function(){return r.Browser.Edge?document.body?document.body.scrollLeft:document.documentElement.scrollLeft:r.Browser.WebKitFamily?document.documentElement.scrollLeft||document.body.scrollLeft:document.documentElement.scrollLeft},n.getDocumentScrollTop=function(){return r.Browser.WebKitFamily||r.Browser.Edge?r.Browser.MacOSMobilePlatform?window.pageYOffset:r.Browser.WebKitFamily&&document.documentElement.scrollTop||document.body.scrollTop:document.documentElement.scrollTop},n.nextChangeDOM=function(e){Q(e)},n.setCssClassName=function(e,t){ue(e,function(e){e.cssClassToggleInfo={},e.className=t})},n.setStyles=function(e,t){ue(e,function(e){if(null===e.inlineStyles)e.inlineStyles=t;else for(var n in t)t.hasOwnProperty(n)&&(e.inlineStyles[n]=t[n])})},n.subscribeElementContentSize=function(e,t){ne(function(e,t,n){return function(){if(e.compareDocumentPosition(document.body)&Node.DOCUMENT_POSITION_DISCONNECTED)return!1;var r=N(e);if("auto"===r.width)return!0;var o=parseInt(r.width)-V(e),i=parseInt(r.height)-M(e);return n.width===o&&n.height===i||(n.width=o,n.height=i,t(n)),!0}}(e,t,{width:-1,height:-1}))},n.subscribeElementContentWidth=function(e,t){ne(function(e,t,n){return function(){if(e.compareDocumentPosition(document.body)&Node.DOCUMENT_POSITION_DISCONNECTED)return!1;var r=N(e);if("auto"===r.width)return!0;var o=parseInt(r.width)-V(e);return n!==o&&t(n=o),!0}}(e,t,-1))},n.subscribeElementDisconnected=function(e,t){ne(function(e,t){return function(){return!(e.compareDocumentPosition(document.body)&Node.DOCUMENT_POSITION_DISCONNECTED&&(t(),1))}}(e,t))},n.toggleCssClass=function(e,t,n){ue(e,function(e){e.cssClassToggleInfo[t]=n})},n.updateAttribute=function(e,t,n){ue(e,function(e){e.attributes[t]=n})},n.updateElementState=ue},["cjs-chunk-c43b3f7c.js"]);
