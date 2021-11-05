// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function SitePreLoadAnimation() {

    let preLoadPromise = new Promise(function (myResolve, myReject) {
        // "Producing Code" (May take some time)

        DoPreLoadAnimation();

        myResolve(); // when successful
        myReject();  // when error
    });

    preLoadPromise.then(
        function (value) { /* code if successful */

            

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
            y: 150,
            fromCurrentPosition: { y: false },
            duration: 3000
        }],
        {
            strokeWidth: 2,
            color: "#fff",
            fontSize: fontSize,
            textAlign: "center"
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
