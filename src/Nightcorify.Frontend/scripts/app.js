
/**
 * On page load
 */
$( document ).ready( function() {

})

/**
 * Form submit
 */
$( "#form" ).submit( function( e ) {
  e.preventDefault();

  var formdata = new FormData( $( this )[0] );

  showResponseText( "Processing...", true );
  

  api.addJob( formdata, $( "#rate" ).val() )
    .then( function( r ) {
      showResponseText( "Processing song. Please hold on a bit...", true ); 
      var id = r.data.id;

      var count = 0;
      var timer = setInterval( function() {
        count++;
        api.getJob( id )
          .then( function( r2 ) {
             var job = r2.data;
             if ( job.status == 1 ) {
               showResponseText( "Processing song. Please hold on (" + count + ")", true );
             } else if ( job.status == 2 ) {
               showSuccessText( job );
             } else if ( job.status == 3 ) {
               showResponseText( "Converting the song failed for some reason.", false );
             }
             if ( job.status == 2 || job.status == 3 ) {
               clearInterval( timer );
             }
          })
          .done( function() {

          })
        if ( count >= 10 ) {
          clearInterval( timer );
        }  
      }, 1500 )

    })
    .catch( function( r ) {
      var data = r.responseJSON;
      if ( data && data.error.message ) {
        showResponseText( data.error.message + " Try again.", false );
      } else {
        showResponseText( "The service could not be reached. Try again (later).", false );
      }
    })
    .done( function() {
        
    })
})

showResponseText = function( text, loading ) {
  var $result = $( "#result" );
  $result.removeClass( "hide" );
  $result.empty();
  if ( loading == true ) {
    $result.append( "<img src='imgs/reload.svg'/> " );
  }
  $result.append( " <span>" + text + "</span>" );
}

showSuccessText = function( job ) {
  var $result = $( "#result" );
  $result.removeClass( "hide" );
  $result.empty();

  $result.append( "<p><a href='" + job.download_url + "'>"+ job.download_url +"</a></p>" );
  $result.append( "<audio class='player' src='" + job.download_url + "' controls>You need a HTML5 compatible browser to play the song</audio>"); 
}


/**
 * Configuration
 */
$cfg = {
  api: {
    baseUrl: 'http://nightcorify.deluvas.ro/api'
    // baseUrl: 'http://localhost:9010/'
  }
}

api = {
  addJob: function( formData, rate ) {
    return $.ajax({
      url: $cfg.api.baseUrl + '/v1/nightcorify?rate=' + rate,
      type: 'POST',
      data: formData,
      processData: false,
      contentType: false,
      dataType: "json"
    })
  },

  getJob: function( id ) {
    return $.ajax({
      url: $cfg.api.baseUrl + '/v1/nightcorify/' + id + '/status',
      type: 'GET',
      dataType: "json"
    })    
  }
}



