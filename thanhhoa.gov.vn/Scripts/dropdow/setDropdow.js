// JavaScript dropdow
$(function() {
    // Clickable Dropdown profile
    $('.click-nav > ul').toggleClass('no-js js');
    $('.click-nav .js ul').hide();
    $('.click-nav .js').click(function(e) {
        $('.click-nav .js ul').slideToggle(200);
        $('.clicker').toggleClass('active');
        e.stopPropagation();
    });
    $(document).click(function() {
        if ($('.click-nav .js ul').is(':visible')) {
            $('.click-nav .js ul', this).slideUp();
            $('.clicker').removeClass('active');
        }
    });
	
    // Clickable Dropdown flag
    $('.click-nav2 > ul').toggleClass('no-js js');
    $('.click-nav2 .js ul').hide();
    $('.click-nav2 .js').click(function(e) {
        $('.click-nav2 .js ul').slideToggle(200);
        $('.clicker2').toggleClass('active');
        e.stopPropagation();
    });
    $(document).click(function() {
        if ($('.click-nav2 .js ul').is(':visible')) {
            $('.click-nav2 .js ul', this).slideUp();
            $('.clicker2').removeClass('active');
        }
    });	
});