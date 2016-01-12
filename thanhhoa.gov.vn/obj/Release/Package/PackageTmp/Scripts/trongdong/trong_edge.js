/**
 * Adobe Edge: symbol definitions
 */
(function($, Edge, compId){
//images folder
var im = '/Scripts/trongdong/images/';

var fonts = {};


var resources = [
];
var symbols = {
"stage": {
   version: "2.0.1",
   minimumCompatibleVersion: "2.0.0",
   build: "2.0.1.268",
   baseState: "Base State",
   initialState: "Base State",
   gpuAccelerate: false,
   resizeInstances: false,
   content: {
         dom: [
         {
            id:'trong',
            type:'image',
            rect:['-9px','-159px','817px','817px','auto','auto'],
            overflow:'hidden',
            fill:["rgba(0,0,0,0)",im+"trong.png",'0px','0px']
         }],
         symbolInstances: [

         ]
      },
   states: {
      "Base State": {
         "${_Stage}": [
            ["color", "background-color", 'rgba(255,255,255,0.00)'],
            ["style", "overflow", 'hidden'],
            ["style", "height", '130px'],
            ["style", "width", '800px']
         ],
         "${_trong}": [
            ["style", "top", '-83px'],
            ["transform", "rotateZ", '0deg'],
            ["style", "overflow", 'hidden'],
            ["style", "left", '-31px'],
            ["style", "-webkit-transform-origin", [50,50], {valueTemplate:'@@0@@% @@1@@%'} ],
            ["style", "-moz-transform-origin", [50,50],{valueTemplate:'@@0@@% @@1@@%'}],
            ["style", "-ms-transform-origin", [50,50],{valueTemplate:'@@0@@% @@1@@%'}],
            ["style", "msTransformOrigin", [50,50],{valueTemplate:'@@0@@% @@1@@%'}],
            ["style", "-o-transform-origin", [50,50],{valueTemplate:'@@0@@% @@1@@%'}]
         ]
      }
   },
   timelines: {
      "Default Timeline": {
         fromState: "Base State",
         toState: "",
         duration: 90000,
         autoPlay: true,
         timeline: [
            { id: "eid17", tween: [ "style", "${_trong}", "left", '-31px', { fromValue: '-31px'}], position: 0, duration: 0 },
            { id: "eid16", tween: [ "style", "${_trong}", "top", '-83px', { fromValue: '-83px'}], position: 0, duration: 0 },
            { id: "eid4", tween: [ "transform", "${_trong}", "rotateZ", '360deg', { fromValue: '0deg'}], position: 0, duration: 90000 }         ]
      }
   }
}
};


Edge.registerCompositionDefn(compId, symbols, fonts, resources);

/**
 * Adobe Edge DOM Ready Event Handler
 */
$(window).ready(function() {
     Edge.launchComposition(compId);
});
})(jQuery, AdobeEdge, "EDGE-16884271");
