// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function SitePreLoadAnimation() {

    let preLoadPromise = new Promise((resolve, reject) => {

        DoPreLoadAnimation();
        setTimeout(() => {

            resolve('preLoadFinished');
        }, 3000);

    });

    preLoadPromise.then(
        function (value) { /* code if successful */

            console.log("preLoadFinished playing video");
            $('#preLoadContainer').hide();
            $('#blenderVideoContainer video source')[0].src = "../videos/BookOpen30001-0250.mp4";
            $('#blenderVideoContainer video')[0].play();
            

        },
        function (error) { /* code if some error */ }
    );

    preLoadPromise.catch((error) => {
        console.error(error);
    });


    

}

function DoPreLoadAnimation() {

    var fontSize = 72;

    if (window.screen.width > 700)
        fontSize = 32;
    else if (window.screen.width < 1200)
        fontSize = 57;

    var vara = new Vara(

        "#preLoadContainer",
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
