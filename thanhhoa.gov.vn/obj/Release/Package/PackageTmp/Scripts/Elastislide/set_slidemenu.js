var current = 3,
	$preview = $( '#preview' ),
	$carouselEl = $( '#carousel' ),
	$carouselItems = $carouselEl.children(),
	carousel = $carouselEl.elastislide( {
		current : current,
		minItems : 3,
		onClick : function( el, pos, evt ) {

			changeImage( el, pos );
			evt.preventDefault();

		},
		onReady : function() {

			changeImage( $carouselItems.eq( current ), current );
			
		}
	} );

function changeImage( el, pos ) {

	$preview.attr( 'src', el.data( 'preview' ) );
	$carouselItems.removeClass( 'current-img' );
	el.addClass( 'current-img' );
	carousel.setCurrent( pos );

}