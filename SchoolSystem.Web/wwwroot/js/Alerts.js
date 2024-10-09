
function ShowAlert(data) {
  const alertsContainer = document.getElementById('userAlerts');

  const alert = document.createElement('div');
  alert.className = 'alert alert-primary alert-dismissible';
  alert.role = 'alert';
  alert.textContent = data.message;

  const time = document.createElement('br');
  alert.appendChild(time);

  const small = document.createElement('small');
  small.className = 'text-muted ms-auto';
  small.textContent = data.dateCreated;
  alert.appendChild(small);

  const button = document.createElement('button');
  button.type = 'button';
  button.className = 'btn-close';
  button.setAttribute('data-bs-dismiss', 'alert');
  button.setAttribute('aria-label', 'Close');

  alert.appendChild(button);

  alertsContainer.insertBefore(alert, alertsContainer.firstChild);
}
async function SendAlert(message, sendTo) {
  const response = await fetch('Alert/Create', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ Message: message, SendTo: sendTo })
  });


  if (response.ok) {
    const data = await response.json();
    ShowAlert(data);
  } else {
    console.log('Failed to send alert');
    alert('Failed to send alert');
  }
}
function AddErrorMessageOrClear(message, sendTo) {
    const messageError = document.getElementById("message-error");
    const sendToError = document.getElementById("sendTo-error");

    if (message === "" || message === undefined || message.trim() === "") {
        messageError.textContent = "Message cannot be empty";
    } else {
        messageError.textContent = "";
    }
    if (sendTo === "" || sendTo === undefined || sendTo.trim() === "") {
        sendToError.textContent = "Choose a recipient to send the message to";
    } else {
        sendToError.textContent = "";
    }
}

function ValidateInput(message, sendTo) {
    if (message === "" || message === undefined || message.trim() === "") {
        return false;
    }
    else if (sendTo === "" || sendTo === undefined || sendTo.trim() === "")
        return false;

    return true;
}

const handleSubmit = async (event) => {
    event.preventDefault();
    const form = event.target;

    const data = new FormData(form);
    const message = data.get("message");
    const sendTo = data.get("sendTo");

    if (ValidateInput(message, sendTo)) {
        const btn = document.getElementById("send");
        btn.disabled = true;
        btn.classList.remove("btn-primary");
        btn.classList.add("btn-secondary");
        btn.textContent = "Sending...";

        AddErrorMessageOrClear(message, sendTo);
        await SendAlert(message, sendTo);

        // Clear the form
        form.reset();
        btn.disabled = false;
        btn.textContent = "Send";
        btn.classList.remove("btn-secondary");
        btn.classList.add("btn-primary");
    } else {
        console.log("Invalid input");
        AddErrorMessageOrClear(message, sendTo);
    }
};

const form = document.getElementById("submit");
form.addEventListener("submit", handleSubmit);
