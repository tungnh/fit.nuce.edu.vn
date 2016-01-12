//** jQuery Scroll to Top Control script- (c) Dynamic Drive DHTML code library: http://www.dynamicdrive.com.
//** Available/ usage terms at http://www.dynamicdrive.com (March 30th, 09')
//** v1.1 (April 7th, 09'):
//** 1) Adds ability to scroll to an absolute position (from top of page) or specific element on the page instead.
//** 2) Fixes scroll animation not working in Opera. 


var scrolltotop={
	//startline: Integer. Number of pixels from top of doc scrollbar is scrolled before showing control
	//scrollto: Keyword (Integer, or "Scroll_to_Element_ID"). How far to scroll document up when control is clicked on (0=top).
	setting: {startline:100, scrollto: 0, scrollduration:1000, fadeduration:[500, 100]},
	controlHTML: '<img src="/Images/top.png" style="width:58px; height:58px" />', //HTML for control, which is auto wrapped in DIV w/ ID="topcontrol"
	controlattrs: {offsetx:5, offsety:5}, //offset of control relative to right/ bottom of window corner
	anchorkeyword: '#top', //Enter href value of HTML anchors on the page that should also act as "Scroll Up" links

	state: {isvisible:false, shouldvisible:false},

	scrollup:function(){
		if (!this.cssfixedsupport) //if control is positioned using JavaScript
			this.$control.css({opacity:0}) //hide control immediately after clicking it
		var dest=isNaN(this.setting.scrollto)? this.setting.scrollto : parseInt(this.setting.scrollto)
		if (typeof dest=="string" && jQuery('#'+dest).length==1) //check element set by string exists
			dest=jQuery('#'+dest).offset().top
		else
			dest=0
		this.$body.animate({scrollTop: dest}, this.setting.scrollduration);
	},

	keepfixed:function(){
		var $window=jQuery(window)
		var controlx=$window.scrollLeft() + $window.width() - this.$control.width() - this.controlattrs.offsetx
		var controly=$window.scrollTop() + $window.height() - this.$control.height() - this.controlattrs.offsety
		this.$control.css({left:controlx+'px', top:controly+'px'})
	},

	togglecontrol:function(){
		var scrolltop=jQuery(window).scrollTop()
		if (!this.cssfixedsupport)
			this.keepfixed()
		this.state.shouldvisible=(scrolltop>=this.setting.startline)? true : false
		if (this.state.shouldvisible && !this.state.isvisible){
			this.$control.stop().animate({opacity:1}, this.setting.fadeduration[0])
			this.state.isvisible=true
		}
		else if (this.state.shouldvisible==false && this.state.isvisible){
			this.$control.stop().animate({opacity:0}, this.setting.fadeduration[1])
			this.state.isvisible=false
		}
	},
	
	init:function(){
		jQuery(document).ready(function($){
			var mainobj=scrolltotop
			var iebrws=document.all
			mainobj.cssfixedsupport=!iebrws || iebrws && document.compatMode=="CSS1Compat" && window.XMLHttpRequest //not IE or IE7+ browsers in standards mode
			mainobj.$body=(window.opera)? (document.compatMode=="CSS1Compat"? $('html') : $('body')) : $('html,body')
			mainobj.$control=$('<div id="topcontrol">'+mainobj.controlHTML+'</div>')
				.css({position:mainobj.cssfixedsupport? 'fixed' : 'absolute', bottom:mainobj.controlattrs.offsety, right:mainobj.controlattrs.offsetx, opacity:0, cursor:'pointer'})
				.attr({title:'Lên đầu trang'})
				.click(function(){mainobj.scrollup(); return false})
				.appendTo('body')
			if (document.all && !window.XMLHttpRequest && mainobj.$control.text()!='') //loose check for IE6 and below, plus whether control contains any text
				mainobj.$control.css({width:mainobj.$control.width()}) //IE6- seems to require an explicit width on a DIV containing text
			mainobj.togglecontrol()
			$('a[href="' + mainobj.anchorkeyword +'"]').click(function(){
				mainobj.scrollup()
				return false
			})
			$(window).bind('scroll resize', function(e){
				mainobj.togglecontrol()
			})
		})
	}
}

scrolltotop.init()



//** Scrolling HTML Bookmarks script- (c) Dynamic Drive DHTML code library: http://www.dynamicdrive.com.
//** Available/ usage terms at http://www.dynamicdrive.com/ (April 11th, 09')
//** Updated Nov 10th, 09'- Fixed anchor jumping issue in IE7

var bookmarkscroll={
	setting: {duration:1000, yoffset:0}, //{duration_of_scroll_milliseconds, offset_from_target_element_to_rest}
	topkeyword: '#top', //keyword used in your anchors and scrollTo() to cause script to scroll page to very top

	scrollTo:function(dest, options, hash){
		var $=jQuery, options=options || {}
		var $dest=(typeof dest=="string" && dest.length>0)? (dest==this.topkeyword? 0 : $('#'+dest)) : (dest)? $(dest) : [] //get element based on id, topkeyword, or dom ref
		if ($dest===0 || $dest.length==1 && (!options.autorun || options.autorun && Math.abs($dest.offset().top+(options.yoffset||this.setting.yoffset)-$(window).scrollTop())>5)){
			this.$body.animate({scrollTop: ($dest===0)? 0 : $dest.offset().top+(options.yoffset||this.setting.yoffset)}, (options.duration||this.setting.duration), function(){
				if ($dest!==0 && hash)
					location.hash=hash
			})
		}
	},

	urlparamselect:function(){
		var param=window.location.search.match(/scrollto=[\w\-_,]+/i) //search for scrollto=divid
		return (param)? param[0].split('=')[1] : null
	},
	
	init:function(){
		jQuery(document).ready(function($){
			var mainobj=bookmarkscroll
			mainobj.$body=(window.opera)? (document.compatMode=="CSS1Compat"? $('html') : $('body')) : $('html,body')
			var urlselectid=mainobj.urlparamselect() //get div of page.htm?scrollto=divid
			if (urlselectid) //if id defined
				setTimeout(function(){mainobj.scrollTo(document.getElementById(urlselectid) || $('a[name='+urlselectid+']:eq(0)').get(0), {autorun:true})}, 100)
			$('a[href^="#"]').each(function(){ //loop through links with "#" prefix
				var hashvalue=this.getAttribute('href').match(/#\w+$/i) //filter links at least 1 character following "#" prefix
				hashvalue=(hashvalue)? hashvalue[0].substring(1) : null //strip "#" from hashvalue
				if (this.hash.length>1){ //if hash value is more than just "#"
					var $bookmark=$('a[name='+this.hash.substr(1)+']:eq(0)')
					if ($bookmark.length==1 || this.hash==mainobj.topkeyword){ //if HTML anchor with given ID exists or href==topkeyword
						if ($bookmark.length==1 && !document.all) //non IE, or IE7+
							$bookmark.html('.').css({position:'absolute', fontSize:1, visibility:'hidden'})
						$(this).click(function(e){
							mainobj.scrollTo((this.hash==mainobj.topkeyword)? mainobj.topkeyword : $bookmark.get(0), {}, this.hash)
							e.preventDefault()
						})
					}
				}
			})
		})
	}
}

bookmarkscroll.init()