import"./esm-chunk-f9104efc.js";import{RequestAnimationFrame as e}from"./esm-dom-utils-d4fe413b.js";import{K as t}from"./esm-chunk-710198b6.js";function i(e){return e*(2-e)}class s{constructor(){this.subscribers=[]}subscribe(e){-1===this.subscribers.indexOf(e)&&this.subscribers.push(e)}unsubscribe(e){var t=this.subscribers.indexOf(e);-1!==t&&this.subscribers.splice(t,1)}}s.Empty=new s;class n extends s{emit(e){this.subscribers.forEach(t=>t(e))}}class r extends s{constructor(e){super(),this.isInitialized=1===arguments.length,this.value=e}update(e){this.isInitialized&&e===this.value||(this.isInitialized=!0,this.value=e,this.subscribers.forEach(t=>t(e)))}subscribe(e,t){this.isInitialized&&!t&&e(this.value),super.subscribe(e)}asTrigger(e){var t=new r;return this.subscribe("function"!=typeof e?i=>t.update(-1!==e.indexOf(i)):i=>t.update(e(i))),t}or(e){var t=new r;return this.subscribe(i=>t.update(i||e.value)),e.subscribe(e=>t.update(this.value||e)),t}and(e){var t=new r;return this.subscribe(i=>t.update(i&&e.value)),e.subscribe(e=>t.update(this.value&&e)),t}join(e){var t=new r;return this.subscribe(i=>t.update([i,e.value])),e.subscribe(e=>t.update([this.value,e])),t}}function a(e){return e/Math.abs(e)}function l(e,t,i,s){var n=null;function r(e){return n?n(e):0}function a(){n=null}function l(e,s,r){n=function(e,t,i,s,n){var r=t+i,a=0;return function(l,h){if(h)return h(e-a,l,s(l/r));l>=r&&n&&n();var o=e*s((Math.min(l,r)-t)/i)-a;return a+=o,o}}(s+e,r,t,i,a)}function h(e){null===n?(l(e.value,0,e.timeStamp),s(r)):n(e.timeStamp,(t,i)=>l(e.value,t,i))}return e.subscribe(h),function(){n=null,e.unsubscribe(h)}}function h(t,i,s,r,h){var o,u,m,c,d,v=function(e,t,i){var s=new n;return e.addEventListener(t,e=>{e.preventDefault();var t=i(e);0===t||isNaN(t)||s.emit({value:t,timeStamp:e.timeStamp})},!1),s}(t,"wheel",e=>a(e.deltaY)*i);l((o=v,u=30,m=new n,c=0,d=1,o.subscribe(e=>{(0===c||e.timeStamp>=c||a(e.value)!==d)&&(c=e.timeStamp+u,d=a(e.value),m.emit(e))}),m),300,s,function(t){h(new Promise((i,s)=>{!function t(i,s){e(e=>{var n=i(e);0!==n?(r(n),t(i,s)):s()})}(t,i)}))})}function o(e,t,i){var s=i(e);return s?t||s.visible.value?s:o(s,t,i):null}function u(e,t){var i,s,n=-1===t?"prevItem":"nextItem";return e[n]?e[n]:e[n]=(s=e.value+t*(i=e.collection).delta,-1===t&&-1!==i.min&&s<i.min?new c(i,i.max,!1,null,e):1===t&&-1!==i.max&&s>i.max?new c(i,i.min,!1,e,null):new c(i,s,!1))}class m{constructor(e,t,i,s,n,a){this.collection=e,this.prevItem=n,this.nextItem=a,this.value=t,this.visible=i||new r(!0),this.selected=new r(!!s),n&&(n.nextItem=this),a&&(a.prevItem=this),this.displayText=new r(t);var l=this.collection.selectedItem;this.selected.subscribe(e=>{e&&(l.value&&l.value.selected.update(!1),l.update(this))}),this.visible.subscribe(e=>{!e&&this.selected.value&&this.getPrevious().selected.update(!0),this.collection.visibleItemsChanged.emit()},!0)}getPrevious(e){return o(this,e,e=>e.prevItem)}getNext(e){return o(this,e,e=>e.nextItem)}getDisplayText(){return this.displayText.value||this.value}}class c extends m{constructor(e,t,i,s,n){super(e,t,null,i,s,n)}getPrevious(e){return u(this,-1)}getNext(e){return u(this,1)}}class d extends c{constructor(e,t){super(e,t,!0)}}class v{constructor(){this.items=[],this.selectedItem=new r,this.visibleItemsChanged=new n}static createMonthCollection(e,t){for(var i=new v,s=0;s<t.length;s++)i.add(s+1,null,s+1===e).displayText.value=t[s];return i.initialize(e),i}static createGenerator(e,t,i,s){var n=new p(t,i,s);return n.initialize(e),n}initialize(e){this.items[0].prevItem=this.items[this.items.length-1],this.items[this.items.length-1].nextItem=this.items[0],this.items.filter(t=>t.value===e)[0].selected.update(!0)}add(e,t,i){var s=new m(this,e,t,i,this.items[this.items.length-1]);return this.items.push(s),s}}class p extends v{constructor(e,t,i){super(),this.min=e,this.max=t,this.delta=i,this.originItem=null}initialize(e){this.originItem=new d(this,e)}}class b{constructor(e,t,i,s,n){this.displayTextSubscription=this.displayTextSubscription.bind(this),this.roller=i,this.prevItem=n,this.nextItem=null,this.index=t,this.elements=i.createRollerItemElements(),this.offset=t*s,this.height=s,this.position=0,this.visibleItemCount=i.visibleItemCount,n&&(n.nextItem=this),this.updateDataItem(e),this.selectItem=this.selectItem.bind(this)}initialize(t){var i=Math.floor(this.visibleItemCount/2);this.elements[1].style.top=(this.index-i)*this.height+"px",t.appendChild(this.elements[1]),this.move(this.offset),this.elements[0].addEventListener("click",t=>{e(this.selectItem)})}isSelected(){return Math.floor(this.visibleItemCount/2)*this.height===Math.round(this.position)}selectItem(t){var n=Math.floor(this.visibleItemCount/2);return this.roller.afterMove(C(s.Empty,e=>this.roller.move(e),{divisor:this.roller.itemSize.height,startTimestamp:t,endTimestamp:t+300,easing:i,frameCallback:e,value:(n-this.index)*this.height-(this.position-this.offset)}))}move(e){this.updatePosition(e);var t=this.position-this.offset;this.elements.forEach(e=>e.style.transform="matrix(1, 0, 0, 1, 0, "+t+")")}updatePosition(e){this.position+=e,this.position>this.visibleItemCount*this.height?(this.position-=(this.visibleItemCount+1)*this.height,this.updateDataItem(this.nextItem.dataItem.getPrevious())):this.position<-1*this.height&&(this.position+=(this.visibleItemCount+1)*this.height,this.updateDataItem(this.prevItem.dataItem.getNext()))}updateDataItem(e){this.dataItem!==e&&(this.dataItem&&this.dataItem.displayText.unsubscribe(this.displayTextSubscription),this.dataItem=e,this.dataItem.displayText.subscribe(this.displayTextSubscription))}displayTextSubscription(e){this.elements.forEach(t=>f(t,e))}raiseChanges(){this.isSelected()&&this.dataItem.selected.update(!0)}}function f(e,t){e.innerText=t}function g(e,t,i){var s=i(e);return Math.abs(Math.round(s.position-e.position))===t?[s].concat(g(s,t,i)):[]}function I(e,t,i){0!==e.length&&(e.shift().updateDataItem(t),I(e,i(t),i))}class x{constructor(e,t,i,s,n,r){this.itemCollection=e,this.visibleItemCount=t,this.itemContainers=[],this.itemSize=s,this.caption=n,this.longestVisibleDisplayText=r,this.container=i,this.rollerElement=null,this.rollerContainer=null,this.move=this.move.bind(this),this.afterMove=this.afterMove.bind(this)}initialize(){this.initializeRollerElements();for(var s=[this.itemCollection.selectedItem.value];s.length<this.visibleItemCount;)s.splice(0,0,s[0].getPrevious()),s.push(s[s.length-1].getNext());s.push(s[s.length-1].getNext());for(var r=0;r<s.length;r++)this.itemContainers.push(new b(s[r],r,this,this.itemSize.height,this.itemContainers[this.itemContainers.length-1]));this.longestVisibleDisplayText&&f(this.createRollerItemElement("roller-item expander"),this.longestVisibleDisplayText);var a=this.createRollerItemElement("roller-after");this.itemContainers[0].prevItem=this.itemContainers[this.itemContainers.length-1],this.itemContainers[this.itemContainers.length-1].nextItem=this.itemContainers[0];for(r=0;r<this.itemContainers.length;r++)this.itemContainers[r].initialize(a);this.itemCollection.selectedItem.subscribe(()=>this.updateVisibleDataItems(),!0),this.itemCollection.visibleItemsChanged.subscribe(()=>this.updateVisibleDataItems(),!0),h(this.rollerElement,this.itemSize.height,i,this.move,this.afterMove),function(t,i,s,r){var a=new n,l=new n,h=0;t.addEventListener("touchstart",function(e){h=e.touches[0].clientY},!1),t.addEventListener("touchend",function(t){h=0,e(e=>l.emit(e))},!1),t.addEventListener("touchmove",function(e){e.preventDefault();var t=e.changedTouches[0].clientY;a.emit(t-h),h=t},!1),y(a,l,{divisor:i},s,r,e)}(this.rollerElement,this.itemSize.height,this.move,this.afterMove),this.rollerContainer.addEventListener("keydown",e=>{var i=null;e.keyCode===t.Up?i=this.itemCollection.selectedItem.value.getNext():e.keyCode===t.Down&&(i=this.itemCollection.selectedItem.value.getPrevious()),i&&(e.preventDefault(),i.selected.update(!0))})}initializeRollerElements(){var e=this.rollerContainer=document.createElement("A");e.className="roller-container",e.href="javascript:;",e.style.minWidth=this.itemSize.width;var t=document.createElement("SPAN");t.innerText=this.caption,t.className="roller-title",e.appendChild(t);var i=this.rollerElement=document.createElement("SPAN");i.className="roller",e.appendChild(i),this.container&&this.container.appendChild(e)}updateVisibleDataItems(){var e=this.itemContainers.filter(e=>e.isSelected())[0];if(e){var t=g(e,this.itemSize.height,e=>e.prevItem),i=g(e,this.itemSize.height,e=>e.nextItem),s=this.itemCollection.selectedItem.value;e.updateDataItem(s),I(t.concat([]),s.getPrevious(),e=>e.getPrevious()),I(i.concat([]),s.getNext(),e=>e.getNext())}}createRollerItemElement(e){var t=document.createElement("SPAN");return t.className=e||"roller-item",this.rollerElement.appendChild(t),t}createRollerItemElements(){var e=this.createRollerItemElement();return[e,e.cloneNode()]}move(e){if(0!==e)for(var t=Math.sign(e),i=this.itemContainers.length-1,s=(-1===t?0:i)+t,n=-1===t?i:0;n!==s;n+=t)this.itemContainers[n].move(e)}afterMove(e){return e.then(()=>Promise.resolve(this.itemContainers.forEach(e=>e.raiseChanges())))}}function C(e,t,i){return new Promise((s,n)=>{function r(){e.unsubscribe(r),n()}e.subscribe(r,!0);var a=0,l=i.frameCallback;l(function n(h){if(h<i.endTimestamp){var o=i.easing((h-i.startTimestamp)/(i.endTimestamp-i.startTimestamp)),u=i.value*o-a;t(u),a+=u,l(n)}else e.unsubscribe(r),t(i.value-a),s()})})}function y(e,t,s,n,r,a){var l,h,o,u,m=1,c=s.accelerationTimeFrame||300;function d(e){h=0,o=e,m=1}e.subscribe(function(v){a(p=>{u||(u=!0,d(p),r(new Promise((r,l)=>{t.subscribe(function l(o){t.unsubscribe(l),C(e,n,function(e){var t=Math.abs(h%s.divisor);return{value:(s.divisor-t)*Math.sign(h),endTimestamp:e+300,startTimestamp:e,easing:i,frameCallback:a}}(o)).then(()=>{u=!1,r()}).catch(()=>{t.subscribe(l)}).finally(d)})}))),n(function(e,t){return(t*=m)>=l&&e-o<=c&&(m=Math.min(2,1.2*m)),h+=t,l=t,o=e,t}(p,v))})})}function w(e){return e.value%4==0}function N(e){return 2!==e.value}function S(e){return-1!==[1,3,5,7,8,10,12].indexOf(e.value)}const T={Day:0,DayWithShortName:1,DayWithFullName:2,MonthWithShortName:3,MonthWithFullName:4,Year:5};function E(e,t,i,s,n,r){var a=e.add(t,i,s);n.length>0&&r.subscribe(([e,i])=>{var s=n[D(i.value,e.value,t)];a.displayText.update(t+" "+s)})}function D(e,t,i){return new Date(e,t-1,i).getDay()}function M(e){return e&&e.length?" "+e.concat([]).sort((e,t)=>t.length-e.length)[0]:""}const z={InitializeDateRoller:function(e,t,i){for(var s=t.items.filter(e=>e.type===T.DayWithShortName||e.type===T.DayWithFullName||e.type===T.Day)[0].value,n=t.items.filter(e=>e.type===T.MonthWithShortName||e.type===T.MonthWithFullName)[0].value,r=t.items.filter(e=>e.type===T.Year)[0].value,a=t.monthNames||[],l=t.dayNames||[],h=v.createGenerator(r,-1,-1,1),o=v.createMonthCollection(n,a),u=o.selectedItem.asTrigger(N),m={29:h.selectedItem.asTrigger(w).or(u),30:u,31:o.selectedItem.asTrigger(S)},c=o.selectedItem.join(h.selectedItem),d=new v,p=1;p<=31;p++)E(d,p,m[p],p===s,l,c);d.initialize(s);var b=document.createDocumentFragment(),f=document.createElement("STYLE");function g(e){return function(t){e(t),i.invokeMethodAsync("UpdateDateTime",[s,n,r])}}return f.type="text/css",f.innerText=".roller { height: 180px; } .roller-item, .roller-after { height: 36px; } .roller-item, .roller-title { color: "+getComputedStyle(e).color+" !important; } .roller-after { background-color: "+getComputedStyle(e.parentNode.parentNode).backgroundColor+";}",b.appendChild(f),t.items.forEach(e=>{switch(e.type){case T.Day:case T.DayWithFullName:case T.DayWithShortName:new x(d,5,b,{width:"",height:36},"Day","25"+M(l)).initialize();break;case T.MonthWithFullName:case T.MonthWithShortName:new x(o,5,b,{width:"",height:36},"Month",M(a)).initialize();break;case T.Year:new x(h,5,b,{width:"",height:36},"Year","0000").initialize()}}),e.appendChild(b),d.selectedItem.subscribe(g(e=>{s=e.value})),o.selectedItem.subscribe(g(e=>{n=e.value})),h.selectedItem.subscribe(g(e=>{r=e.value})),Promise.resolve()}};export default z;export{x as Roller,m as RollerItem,v as RollerItemCollection,y as attachInputSource,D as getDayOfWeek};
