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
            //TO-DO: far scomparire login form e poi play
            $('#blenderVideoContainer video')[0].play();

            setTimeout(() => {

                $('#blenderVideoContainer video')[0].pause();
                //showing RenderBody of layout cshtml login form
                $('#main').show();



            }, 1600);


        }, 2000);
        

    }, 1400);

    

   

    
   

   

    




}