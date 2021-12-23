// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function SitePreLoadAnimation() {

    let writeAnimationPromise = new Promise((resolve, reject) => {

        WritingSiteNameAnimation();
        setTimeout(() => {

            resolve('WriteAnimationFinished');
        }, 4000);

    });

    writeAnimationPromise.then(
        function (value) { /* code if successful */

            console.log("writeAnimation finished, start playing video");
            $('#writeAnimationContainer').hide();
            $('#blenderVideoContainer').show();
            
            $('#blenderVideoContainer video')[0].play();
            setTimeout(() => {

                $('#blenderVideoContainer video')[0].pause();

                //showing RenderBody of layout cshtml login form
                $('#main').show();


            }, 1600);

            
            

        },
        function (error) { /* code if some error */ }
    );

    writeAnimationPromise.catch((error) => {
        console.error(error);
    });


    

}

function WritingSiteNameAnimation() {

    var fontSize = 72;

    if (window.screen.width > 700)
        fontSize = 32;
    else if (window.screen.width < 1200)
        fontSize = 57;

    var vara = new Vara(

        "#writeAnimationContainer",
        "../lib/vara/fonts/Satisfy/SatisfySL.json",
        [{
            text: "Welcome to Books' Bay!",
            y: 0,
            fromCurrentPosition: { y: true },
            duration: 3000
        }],
        {
            strokeWidth: 2,
            color: "#fff",
            fontSize: fontSize,
            textAlign: "center",
            verticalAlign: "top"
        }

    );

    vara.ready(function () {

        var erase = true;

        vara.animationEnd(function (i, o) {

            if (i == "no_erase")
                erase = false;
            if (erase) {
                o.container.style.transition = "opacity 1s 1s";
                o.container.style.opacity = 0;
            }
            


        });

    });

}
