// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function SitePreLoadAnimation() {
   
    $('#blenderVideoContainer video')[0].play();
    setTimeout(() => {

        $('#blenderVideoContainer video')[0].pause();
        $('#login-cont').show();
        $('#blenderVideoContainer').show();

        setTimeout(() => {
            // far scomparire login form e poi play
            $('#login-cont').addClass("hide-fade-out");
            //slower video speed (the speed is half of default which is 1 )
            $('#blenderVideoContainer video')[0].playbackRate = 0.5;

            $('#blenderVideoContainer video')[0].play();

            setTimeout(() => {

                $('#blenderVideoContainer video')[0].pause();
                //showing RenderBody of layout cshtml login form
                $('#main').show();



            }, 3200);


        }, 2000);
        

    }, 1400);

    
}

function StartGlowing(_this) {

    console.log(_this);
    $(_this).addClass("glowing-button");

}