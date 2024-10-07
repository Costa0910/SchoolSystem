document.addEventListener('DOMContentLoaded', function () {
    var url = window.location.href;
    let home = document.getElementById('home');
    let privacy = document.getElementById('privacy');
    let about = document.getElementById('about');
    let courses = document.getElementById('courses');


    if (url.includes('Privacy')) {
        privacy.classList.add('active');
    } else if (url.includes('About')) {
        about.classList.add('active');
    } else if (url.includes('Courses') || url.includes('Course')) {
        courses.classList.add('active');
    } else {
        home.classList.add('active');
    }
});
