// This script is used to clear the toast message after 3 seconds
// And remove the message from the query string in the url
document.addEventListener("DOMContentLoaded", () => {
const toasts = document.querySelectorAll('.toast');

if (toasts.length > 0)
  {
      const urlParams = new URLSearchParams(window.location.search);
      urlParams.delete("message");

    // update the url query string
      if (urlParams.toString() === "") {
        window.history.replaceState({}, '', `${window.location.pathname}`);
      } else {
        window.history.replaceState({}, '', `${window.location.pathname}?${urlParams}`);
      }

  }

 toasts.forEach(toast => {
        const toastClose = toast.querySelector('.btn-close');
        setTimeout(() => {
          toastClose.click();
        }, 3000);
    });
});
