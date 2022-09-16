import{B as t}from"./esm-chunk-f9104efc.js";import{GetAbsoluteX as e,GetAbsoluteY as n,SetAbsoluteX as i,SetAbsoluteY as o,RemoveClassNameFromElement as r,AddClassNameToElement as a,GetDocumentScrollLeft as l,GetDocumentScrollTop as s,GetDocumentClientHeight as u,GetDocumentClientWidth as f,EnsureElement as c,AttachEventToElement as p,GetParentByClassName as h,ElementHasCssClass as m,subscribeElementContentWidth as d,PreventEventAndBubble as g,DetachEventFromElement as v}from"./esm-dom-utils-d4fe413b.js";import{D as y,R as A}from"./esm-chunk-6ca2c4f2.js";import{L as T,T as S,h as C,u as x,c as D}from"./esm-chunk-2f760454.js";import{T as E}from"./esm-chunk-38c61c5f.js";var I={showCallout:!0,showTitle:!1,position:"right",className:"",classNames:{sysClassName:"dx-hint",contentElementClassName:"dxh-content",calloutElementClassName:"arrow",titleElementClassName:"dxh-title"},allowFlip:!0,allowShift:!0,offset:4};function H(t,r,a){a||(a=I),r.style.visibility="hidden",r.style.display="block",function(t,r){var a=e(t),l=n(t);i(r,a),o(r,l)}(t,r),w.updatePosition(t,r,a),r.style.visibility=""}var w=function(){function t(t){return"bs-popover-"+t.toLowerCase()}function e(e,n,i){var o=b.set(e,n,i);!function(e,n,i){var o=t(n),l=t(i);r(e,o),a(e,l)}(n,i.position,o.flipPosition),function(t,e,n){var i=w.getCalloutElement(t,e.classNames);if(i){var o,r=!("left"===(o=e.position)||"right"===o),a=r?t.offsetWidth:t.offsetHeight,l=a/2-(r?n.x:n.y);l=function(t,e){var n={min:15,max:e-15},i=t<n.min,o=t>n.max;return o&&(t=n.max),i&&(t=n.min),t+="px",i&&o&&(t="50%"),t}(l,a),i.style[r?"left":"top"]=l}}(n,i,o.shift)}return{getCalloutElement:function(t,e){return t?t.getElementsByClassName(e.calloutElementClassName)[0]:null},updatePosition:function(t,n,i){var o=10,r=!1;do{var a={w:n.offsetWidth,h:n.offsetHeight};e(t,n,i),r=a.w!==n.offsetWidth||a.h!==n.offsetHeight,o--}while(r&&o>0)}}}();function z(i,o,r){this.targetElement=i,this.hintElement=o,this.options=r,this.position=r.position,this.calloutSize={width:0,height:0};var a=w.getCalloutElement(o,r.classNames);function c(i,o,r,a,c){this.location=0,this.screen={min:0,max:0},this.screen.min=c?l():s();var p=t.WebKitTouchUI?window.innerWidth:f();this.screen.max=this.screen.min+(c?p:u()),this.target={min:0,max:0},this.target.min=c?e(i):n(i),this.target.max=this.target.min+(c?i.offsetWidth:i.offsetHeight),this.hintSize=c?o.offsetWidth:o.offsetHeight}a&&(this.calloutSize.width=a.offsetWidth,this.calloutSize.height=a.offsetHeight),this.horizontal=new c(i,o,r,this.calloutSize,!0),this.vertical=new c(i,o,r,this.calloutSize,!1)}var b=function(){function t(t,e,n){var i=new z(t,e,n),o=r(i.position);n.allowFlip&&(o.horizontal?i.position=O.flipPositionIfRequired(i.horizontal,i.position):o.vertical&&(i.position=O.flipPositionIfRequired(i.vertical,i.position)));var l=function(t){return a(t.position,{width:t.horizontal.target.max-t.horizontal.target.min,height:t.vertical.target.max-t.vertical.target.min},{width:t.horizontal.hintSize,height:t.vertical.hintSize},t.calloutSize)}(i),s={x:0,y:0};return n.allowShift&&(s.x=M.getShift(i.horizontal,o,l.x,0,!0),s.y=M.getShift(i.vertical,o,l.y,0,!1)),{location:{x:l.x+s.x,y:l.y+s.y},shift:s,flipPosition:i.position}}function r(t){return{vertical:"bottom"===t||"top"===t,horizontal:"right"===t||"left"===t}}function a(t,e,n,i){var o={x:0,y:0};return"top"===t?o.y-=n.height+i.height:"bottom"===t?o.y+=e.height+i.height:"left"===t?o.x-=n.width+i.width:"right"===t&&(o.x+=e.width+i.width),"top"!==t&&"bottom"!==t||(o.x+=e.width/2-n.width/2),"left"!==t&&"right"!==t||(o.y+=e.height/2-n.height/2),o}return{set:function(a,l,s){var u=t(a,l,s),f={x:0,y:0},c=r(u.flipPosition);c.horizontal&&(f.x=s.offset*u.location.x/Math.abs(u.location.x)),c.vertical&&(f.y=s.offset*u.location.y/Math.abs(u.location.y));var p=void 0!==s.x?s.x:e(a)+u.location.x+f.x,h=void 0!==s.y?s.y:n(a)+u.location.y+f.y;return i(l,p),o(l,h),{flipPosition:u.flipPosition,shift:{x:u.shift.x,y:u.shift.y}}},getNotShiftedOffsetCore:a}}(),O={flipPositionIfRequired:function(t,e){return this.ensureFlipCore(t.screen,t.target,t.hintSize,e)},getFlipPosition:function(t){return"bottom"===t?"top":"top"===t?"bottom":"right"===t?"left":"left"===t?"right":t},ensureFlipCore:function(t,e,n,i){var o="bottom"===i||"right"===i,r="top"===i||"left"===i,a=e.min-n-t.min;e.min-n>t.max&&(a=-1);var l=t.max-(e.max+n);e.max+n<t.min&&(l=-1);var s=a>=0,u=l>=0;if(!s&&!u)return i;if(r&&s)return i;if(o&&u)return i;var f=o&&!u&&l<a;return r&&!s&&a<l?this.getFlipPosition(i):f?this.getFlipPosition(i):i}},M={getShift:function(t,e,n,i,o){var r=0;return!(e.horizontal&&o||e.vertical&&!o)&&(r=this.getShiftCore(t.screen,t.target,t.hintSize,n,i)),r},getShiftCore:function(t,e,n,i,o){if(e.max<t.min+o||e.min>t.max-o)return 0;var r=e.min+i,a=e.min+i+n,l=r<t.min&&a>t.min,s=a>t.max&&r<t.max;return l&&!s?t.min-r:s&&!l?t.max-n-r:0}},N={None:"none",Drag:"drag",ResizeTop:"resizeTop",ResizeBottom:"resizeBottom",ResizeSelection:"resizeSelection"};function L(t,e,n,i,o){if(t&&(t.appointmentToolTipElement&&y(t.appointmentToolTipElement),t.dropDownDateNavigatorElement&&y(t.dropDownDateNavigatorElement)),t=c(t)){if(t.dropDownDateNavigatorElement=c(e),t.appointmentToolTipElement=c(n),t.appointmentEditForm=c(i),t.mouseMoveHandlerState||(t.mouseMoveHandlerState=N.None),t.appointmentToolTipElement){var r=k.getSelectedAppointment(t);if(r){var a=function(t,e,n){var i=t.getBoundingClientRect(),o=n.getBoundingClientRect();I.position=m(e,"dxsc-tooltip")?i.right-o.right>370?"right":o.left-i.left>370?"left":o.top-i.top>370?"top":"bottom":i.right-o.right>450?"right":o.left-i.left>450?"left":"auto";return I}(t,t.appointmentToolTipElement,r);H(r,t.appointmentToolTipElement,a)}}var l=new B(t,"dxbs-sc-all-day-area"),s=new B(t,"dxbs-sc-time-cell"),u=k.getHorizontalAppointments(t),f=k.getVerticalAppointments(t);if(t.appointmentInfos=j.createItems(u,l,s),t.appointmentInfos=t.appointmentInfos.concat(j.createItems(f,l,s)),t.skipCalculateAllAppointments){var h=function(t){for(var e,n=[],i=0;e=t[i];i++)m(e,"dxbs-apt-edited")&&n.push(e);return n};return u=h(u),f=h(f),z(u,f,!1),Promise.resolve("ok")}return w(),function(t,e,n,i){T.attachEventToElement(t,"mousedown",M),T.attachEventToElement(t,"mouseup",q),T.attachEventToElement(t,"mousemove",L),T.attachLongTabEventToElement(t,R),T.attachEventToElement(t,"touchstart",W),T.attachEventToElement(t,"touchmove",K),T.attachEventToElement(t,"touchend",$),e&&(e.dateNavigatorLostFocusHandler=function(t){return J(t,e,"OnDropDownDateNavigatorLostFocus",i)},p(document,S.touchMouseDownEventName,e.dateNavigatorLostFocusHandler),A(e,function(){v(document,S.touchMouseDownEventName,e.dateNavigatorLostFocusHandler)}));n&&(n.toolTipLostFocusHandler=function(t){var e=k.getAppointmentContainer(t.srcElement);if(!e||!F(e))return J(t,n,"OnAppointmentToolTipLostFocus",i)},p(document,S.touchMouseDownEventName,n.toolTipLostFocusHandler),A(n,function(){v(document,S.touchMouseDownEventName,n.toolTipLostFocusHandler)}));d(t,w)}(t,t.dropDownDateNavigatorElement,t.appointmentToolTipElement,o),Promise.resolve("ok")}function w(){z(u,f,!0),function(t,e){var n=(l=(new Date).getTime(),new Date(l)),i=(a=n,new Date(a.getFullYear(),a.getMonth(),a.getDate())),o=k.getTimeMarkerContainer(t),r=k.getTimeIndicatorContainer(t);var a;var l;if(!r)return;var s=function(t,e){for(var n,i,o=0;i=t[o];o++)e-k.Attr.getStart(i)>=0&&k.Attr.getEnd(i)-e>0&&(n=i);return n}(e,n);if(!s)return o.style.display="none",r.style.display="none",void 0;o.style.display="",r.style.display="";var u=Math.floor(function(t,e,n){var i=function(t,e){var n=t,i=Z(e,n),o=Math.abs(i)%864e5;i<0&&(o=864e5-o);return function(t,e){var n=Q(t,e),i=6e4*(n.getTimezoneOffset()-t.getTimezoneOffset());return Q(n,i)}(n,o)}(n,t),o=k.Attr.getStart(e),r=k.Attr.getEnd(e),a=Z(i,o);return e.offsetTop+e.offsetHeight*a/(r-o)}(n,s,i));o.style.top=u-o.offsetHeight/2+"px",o.style.width=s.offsetLeft+o.offsetHeight/2+1+"px",f=r,f&&"none"!==f.style.display&&(r.style.top=u-1+"px",r.style.width=s.offsetWidth+"px",r.style.left=s.offsetLeft+"px");var f}(t,s.getTimeCells())}function z(t,e,n){l.clearObjects(),U(l.getTimeCells(),t),U(s.getTimeCells(),e),function(t){for(var e,n=0;e=t[n];n++)X(e)}(t),n&&function(t){for(var e,n=0,i=0;e=t[i];i++){var o=0;e.intersects.forEach(function(t){o+=t.offsetHeight}),o>n&&(n=o),e.lastAppointmentTop=void 0}t[0].style.height=n+15+"px"}(l.getTimeCells()),function(t){for(var e,n=0;e=t[n];n++)G(e)}(e)}function b(t){t&&o.invokeMethodAsync("OnAppointmentSelect",k.Attr.getAppointmentKey(t))}function O(e){return t.appointmentInfos.filter(function(t){return t.id===e})[0]}function M(e){if(C("TouchStart")||2===e.button)return 2===e.button&&g(e),void 0;var n=k.getAppointmentContainer(e.srcElement);n?(F(n)||b(n),m(e.srcElement,"dxsc-top-handle")||function(t){return m(t,"dxsc-left-handle")}(e.srcElement)?t.mouseMoveHandlerState=N.ResizeTop:function(t){return m(t,"dxsc-bottom-handle")}(e.srcElement)||function(t){return m(t,"dxsc-right-handle")}(e.srcElement)?t.mouseMoveHandlerState=N.ResizeBottom:x(function(){t.mouseMoveHandlerState=N.Drag},"drag",50,!0)):t.appointmentToolTipElement||P(e.srcElement)&&(t.cellSelectionHelper=new V(t,o),t.cellSelectionHelper.start(e.srcElement),t.mouseMoveHandlerState=N.ResizeSelection)}function L(e){t.mouseMoveHandlerState!==N.None&&(t.throttledDrag||(t.throttledDrag=E.Throttle(function(e){var n=s.findCellByPos(e.clientX,e.clientY)||l.findCellByPos(e.clientX,e.clientY);if(n)if(t.mouseMoveHandlerState===N.ResizeSelection&&t.cellSelectionHelper)t.cellSelectionHelper.resizeTo(n);else{var i=function(){var e=k.getSelectedAppointment(t);return e?O(k.Attr.getAppointmentKey(e)):null}();i&&(t.mouseMoveHandlerState!==N.Drag&&t.mouseMoveHandlerState!==N.ResizeTop&&t.mouseMoveHandlerState!==N.ResizeBottom||(t.dragHelper||(t.dragHelper=new Y(t,o),t.dragHelper.dragStart(i,n)),t.mouseMoveHandlerState===N.Drag?t.dragHelper.drag(n):t.dragHelper.resize(n,t.mouseMoveHandlerState===N.ResizeTop)))}},20)),t.throttledDrag(e))}function q(e){if(C("TouchEnd")||2===e.button)return 2===e.button&&g(e),void 0;t.dragHelper||t.cellSelectionHelper?t.dragHelper?(t.dragHelper.dragEnd(),t.dragHelper=null):t.cellSelectionHelper&&(t.cellSelectionHelper.end(),t.cellSelectionHelper=null):k.getAppointmentContainer(e.srcElement)&&!C("skipToolTip")&&(o.invokeMethodAsync("ShowAppointmentToolTip"),x(function(){},"skipToolTip",300)),D("drag"),t.mouseMoveHandlerState=N.None}function W(t){x(function(){},"TouchStart",300,!0),b(k.getAppointmentContainer(t.srcElement))}function R(e){var n=k.getAppointmentContainer(e.srcElement);if(n){var i=e.touches[0].clientX,r=e.touches[0].clientY,a=s.findCellByPos(i,r)||l.findCellByPos(i,r),u=O(k.Attr.getAppointmentKey(n));t.dragHelper=new Y(t,o),t.dragHelper.dragStart(u,a)}else P(e.srcElement)&&!t.appointmentToolTipElement&&(t.cellSelectionHelper=new V(t,o),t.cellSelectionHelper.start(e.srcElement))}function K(e){if(t.dragHelper||t.cellSelectionHelper){var n=e.touches[0].clientX,i=e.touches[0].clientY,o=s.findCellByPos(n,i)||l.findCellByPos(n,i);o&&(t.dragHelper?t.dragHelper.drag(o):t.cellSelectionHelper&&t.cellSelectionHelper.resizeTo(o)),g(e)}}function $(t){q(t),x(function(){},"TouchEnd",300,!0)}}var k={getVerticalAppointmentsContainer:function(t){return t.querySelectorAll(".dxbs-sc-vertical-apts")[0]},getHorizontalAppointmentsContainer:function(t){return t.querySelectorAll(".dxbs-sc-horizontal-apts")[0]},getHorizontalAppointments:function(t){return t.querySelectorAll(".dxbs-sc-horizontal-apt")},getVerticalAppointments:function(t){return t.querySelectorAll(".dxbs-sc-vertical-apt")},getTimeMarkerContainer:function(t){return t.querySelectorAll(".dxbs-sc-time-marker")[0]},getTimeIndicatorContainer:function(t){return t.querySelectorAll(".dxbs-sc-time-indicator")[0]},getAppointmentContainer:function(t){return h(t,"dxbs-sc-apt")},getSelectedAppointment:function(t){return t.querySelectorAll(".dxbs-apt-selected")[0]},getEditedAppointment:function(t){return t.querySelectorAll(".dxbs-apt-edited")[0]},getCellSelectionContainer:function(t){return t.querySelectorAll(".dxsc-cell-selection")[0]},setElementDisplay:function(t,e){t.style.display=e?"":"none"},Attr:{getContainerIndex:function(t){return t.getAttribute("data-container-index")},getAppointmentFirstCellIndex:function(t){return parseInt(t.getAttribute("data-first-cell-index"))},getAppointmentLastCellIndex:function(t){return parseInt(t.getAttribute("data-last-cell-index"))},getAppointmentColumnsCount:function(t){return parseInt(t.getAttribute("data-columns-count"))},setAppointmentColumnsCount:function(t,e){t.setAttribute("data-columns-count",e)},getAppointmentColumn:function(t){return parseInt(t.getAttribute("data-column"))},setAppointmentColumn:function(t,e){t.setAttribute("data-column",e)},getAppointmentKey:function(t){return t.getAttribute("data-key")},getStart:function(t){var e=new Date(parseInt(t.getAttribute("data-start"))),n=e.getTime()+6e4*e.getTimezoneOffset()+0;return new Date(n)},getEnd:function(t){var e=new Date(parseInt(t.getAttribute("data-end"))),n=e.getTime()+6e4*e.getTimezoneOffset()+0;return new Date(n)},getDuration:function(t){return parseInt(t.getAttribute("data-duration"))},getAllDay:function(t){return""===t.getAttribute("data-allday")}}};function P(t){return m(t,"dxbs-sc-all-day-area")||m(t,"dxbs-sc-time-cell")}function q(t){return m(t,"dxbs-sc-all-day-area")}function F(t){return m(t,"dxbs-apt-selected")}function W(t){var e=k.Attr.getStart(t);return R(e,k.Attr.getEnd(t)-e)}function R(t,e){return{start:t,duration:e,isLongerOrEqualDay:e>=K.DaySpan}}var B=function(t,e){this.element=t,this.cellClassName=e};B.prototype={getContainers:function(){if(!this.containers){var t=this.element.querySelectorAll("."+this.cellClassName);this.containers={};for(var e,n=0;e=t[n];n++){var i=k.Attr.getContainerIndex(e);this.containers[i]||(this.containers[i]={cells:[]}),this.containers[i].cells.push(e)}}return this.containers},clearObjects:function(){for(var t,e=this.getTimeCells(),n=0;t=e[n];n++)this.clearObject(t)},clearObject:function(t){t.lastAppointmentTop=void 0},getTimeCells:function(){return this.timeCells||(this.timeCells=this.element.querySelectorAll("."+this.cellClassName)),this.timeCells},findCell:function(t){var e=this.getContainers();for(var n in e)if(e.hasOwnProperty(n))for(var i,o=0;i=e[n].cells[o];o++){var r=k.Attr.getStart(i),a=k.Attr.getEnd(i);if(r-t<=0&&t-a<0)return i}return null},findStartCell:function(t){var e=this.getContainers();for(var n in e)if(e.hasOwnProperty(n))for(var i,o=0;i=e[n].cells[o];o++){if(t-k.Attr.getStart(i)<=0)return i}return null},findEndCell:function(t){var e=this.getContainers();for(var n in e)if(e.hasOwnProperty(n))for(var i,o=0;i=e[n].cells[o];o++){if(t-k.Attr.getEnd(i)<=0)return i}return null},findCellByPos:function(t,e){for(var n,i=this.getTimeCells(),o=0;n=i[o];o++){var r=n.getBoundingClientRect();if(r.top<=e&&e<r.bottom&&r.left<=t&&t<r.right)return n}}};var j=function(t,e,n,i){this.id=t,this.views=e,this.interval=n,this.allDay=k.Attr.getAllDay(e[0]),this.sourceView=null,this.aptCont=null,this.dateTimeViewLayout=i,this.init()};j.prototype={init:function(){this.sourceView=this.views[0].cloneNode(!0),this.aptCont=this.views[0].parentElement},getStart:function(){return this.interval.start},getDuration:function(){return this.interval.duration},getEnd:function(){return K.DateIncreaseWithUtcOffset(this.getStart(),this.getDuration())},clearViews:function(){this.views.forEach(function(t){t.parentElement.removeChild(t)}),this.views=[]}},j.createItem=function(t,e,n,i){var o=function(t){return R(k.Attr.getStart(t[0]),k.Attr.getDuration(t[0]))}(e);return new j(t,e,o,o.duration>=K.DaySpan?n:i)},j.createItems=function(t,e,n){for(var i,o={},r=0;i=t[r];r++){o[l=k.Attr.getAppointmentKey(i)]||(o[l]=[]),o[l].push(i)}var a=[];for(var l in o)o.hasOwnProperty(l)&&a.push(j.createItem(l,o[l],e,n));return a};var K={HalfHourSpan:18e5,DaySpan:864e5,DateSubsWithTimezone:function(t,e){return t-e+6e4*(e.getTimezoneOffset()-t.getTimezoneOffset())},TruncToDate:function(t){return new Date(t.getFullYear(),t.getMonth(),t.getDate())},CalculateDaysDifference:function(t,e){var n=K.TruncToDate(t),i=K.TruncToDate(e);return K.DateSubsWithTimezone(i,n)/K.DaySpan},DateIncreaseWithUtcOffset:function(t,e){var n=K.DateIncrease(t,e),i=6e4*(n.getTimezoneOffset()-t.getTimezoneOffset());return K.DateIncrease(n,i)},DateIncrease:function(t,e){return new Date(t.valueOf()+e)},AddTimeSpan:function(t,e){return new Date(t.valueOf()+e)},ToDayTime:function(t){return t.valueOf()-K.TruncToDate(t).valueOf()}};function V(t,e){this.scheduler=t,this.dotnetHelper=e,this.interval=null,this.start=function(t){this.interval=W(t);var e=this.interval.start,n=K.DateIncreaseWithUtcOffset(e,this.interval.duration),i=6e4*e.getTimezoneOffset()*-1;this.dotnetHelper.invokeMethodAsync("OnCellSelectionStart",K.AddTimeSpan(e,i),K.AddTimeSpan(n,i),q(t))},this.resizeTo=function(t){var e=W(t),n=e.start-this.interval.start;if(0!==n||this.interval.duration!==e.duration){n<0&&this.interval.duration===e.duration?this.direction="top":n>0&&this.interval.duration===e.duration&&(this.direction="bottom"),"bottom"===this.direction?this.interval.duration=n+e.duration:"top"===this.direction&&(this.interval.start=e.start,this.interval.duration+=-1*n);var i=this.interval.start,o=K.DateIncreaseWithUtcOffset(i,this.interval.duration),r=6e4*i.getTimezoneOffset()*-1;this.dotnetHelper.invokeMethodAsync("OnCellSelectionResize",K.AddTimeSpan(i,r),K.AddTimeSpan(o,r),q(t))}},this.end=function(){this.dotnetHelper.invokeMethodAsync("OnCellSelectionEnd",this.scheduler.offsetWidth<500)}}function Y(t,e){this.scheduler=t,this.dotnetHelper=e,this.appointmentInfo=null,this.interval=null,this.dragStart=function(t,e){this.appointmentInfo=t,this.sourceAppointmentInterval=R(t.getStart(),t.getDuration()),this.cellInterval=W(e),this.dateDiff=t.getStart()-this.cellInterval.start,this.dotnetHelper.invokeMethodAsync("OnAppointmentDragStart"),this.scheduler.skipCalculateAllAppointments=!0},this.drag=function(t){if(this.cellInterval){var e=W(t);if(this.cellInterval.start-e.start!=0||this.cellInterval.duration!==e.duration){e.isLongerOrEqualDay?this.sourceAppointmentInterval.isLongerOrEqualDay&&this.cellInterval.isLongerOrEqualDay===e.isLongerOrEqualDay?this.appointmentInfo.interval=R(K.AddTimeSpan(e.start,this.dateDiff),this.appointmentInfo.interval.duration):(this.appointmentInfo.interval=e,this.appointmentInfo.allDay=!0):(this.appointmentInfo.interval.isLongerOrEqualDay&&(this.appointmentInfo.interval.duration=this.sourceAppointmentInterval.isLongerOrEqualDay?e.duration:this.sourceAppointmentInterval.duration),this.appointmentInfo.interval=R(K.AddTimeSpan(e.start,this.dateDiff),this.appointmentInfo.interval.duration),this.appointmentInfo.allDay=!1);var n=6e4*e.start.getTimezoneOffset()*-1;this.dotnetHelper.invokeMethodAsync("OnAppointmentDrag",K.AddTimeSpan(this.appointmentInfo.getStart(),n),K.AddTimeSpan(this.appointmentInfo.getEnd(),n),this.appointmentInfo.allDay),this.cellInterval=e}}},this.resize=function(t,e,n){if(this.cellInterval){var i=W(t);if((this.cellInterval.start-i.start!=0||this.cellInterval.duration!==i.duration)&&this.cellInterval.isLongerOrEqualDay===i.isLongerOrEqualDay){var o=this.cellInterval.start-i.start;e?this.appointmentInfo.interval=R(K.AddTimeSpan(i.start,this.dateDiff),this.appointmentInfo.interval.duration+o):this.appointmentInfo.interval.duration-=o;var r=6e4*i.start.getTimezoneOffset()*-1;this.dotnetHelper.invokeMethodAsync("OnAppointmentDrag",K.AddTimeSpan(this.appointmentInfo.getStart(),r),K.AddTimeSpan(this.appointmentInfo.getEnd(),r),this.appointmentInfo.allDay),this.cellInterval=i}}},this.dragEnd=function(){this.dotnetHelper.invokeMethodAsync("OnAppointmentDragEnd"),this.scheduler.skipCalculateAllAppointments=!1}}function U(t,e){for(var n,i={},o=0;n=t[o];o++){n.intersects=[];var r=k.Attr.getContainerIndex(n);i[r]||(i[r]=[]),i[r].push(n)}var a;for(o=0;a=e[o];o++){var l=k.Attr.getContainerIndex(a),s=k.Attr.getAppointmentFirstCellIndex(a),u=k.Attr.getAppointmentLastCellIndex(a);a.firstCell=i[l][s],a.lastCell=i[l][u]}}function X(t){t.style.height="",function(t,e){t.intersects.findIndex(function(t){return k.Attr.getAppointmentKey(t)===k.Attr.getAppointmentKey(e)})<0&&t.intersects.push(e)}(t.firstCell,t),t.style.left=t.firstCell.offsetLeft+"px",t.style.width=t.lastCell===t.firstCell?t.firstCell.offsetWidth+"px":t.lastCell.offsetLeft-t.firstCell.offsetLeft+t.firstCell.offsetWidth+"px",t.firstCell.lastAppointmentTop||(t.firstCell.lastAppointmentTop=t.firstCell.offsetTop),t.style.display="",t.style.top=t.firstCell.lastAppointmentTop+"px",t.firstCell.lastAppointmentTop+=t.offsetHeight}function G(t){var e=k.Attr.getAppointmentColumn(t),n=t.firstCell.offsetWidth/k.Attr.getAppointmentColumnsCount(t);t.style.top=t.firstCell.offsetTop+"px",t.style.left=t.firstCell.offsetLeft+n*e+"px",t.style.width=n-10+"px",t.style.height=t.lastCell.offsetTop+t.lastCell.offsetHeight-t.firstCell.offsetTop+"px",t.style.display=""}function J(t,e,n,i){return function(t,e,n){var i=t.target;for(;i;){if(i===e)return!1;i=i.parentElement}n&&n()}(t,e,function(){e&&"string"!=typeof e&&"none"!==e.style.display&&i.invokeMethodAsync(n)})}function Q(t,e){return new Date(t.valueOf()+e)}function Z(t,e){return t-e+6e4*(e.getTimezoneOffset()-t.getTimezoneOffset())}function $(t){return(t=c(t))&&y(t),Promise.resolve("ok")}const _={Init:L,Dispose:$};export default _;export{$ as Dispose,L as Init};
