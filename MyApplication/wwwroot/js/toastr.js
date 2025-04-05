// Function to show a success message (green)
function SuccessToastr(message) {
    // Create a div for the toastr
    const toastr = document.createElement('div');

    // Create a close button
    const closeButton = document.createElement('button');
    closeButton.textContent = 'X';
    closeButton.style.background = 'none';
    closeButton.style.border = 'none';
    closeButton.style.color = 'white';
    closeButton.style.fontSize = '18px';
    closeButton.style.cursor = 'pointer';

    // Style the toastr for success (green)
    toastr.style.position = 'fixed';
    toastr.style.top = '20px';
    toastr.style.right = '20px';
    toastr.style.backgroundColor = '#28a745'; // Green color
    toastr.style.color = 'white';
    toastr.style.padding = '10px 20px';
    toastr.style.borderRadius = '5px';
    toastr.style.fontSize = '16px';
    toastr.style.boxShadow = '0 4px 6px rgba(0, 0, 0, 0.1)';
    toastr.style.zIndex = '9999';
    toastr.style.opacity = '0';
    toastr.style.transition = 'opacity 0.5s ease-in-out';
    toastr.style.maxWidth = '300px';

    // Use flexbox to ensure proper alignment
    toastr.style.display = 'flex';
    toastr.style.alignItems = 'center';
    toastr.style.justifyContent = 'space-between';

    // Create a container div for the message content
    const messageContainer = document.createElement('div');
    messageContainer.textContent = message;
    messageContainer.style.flexGrow = '1';  // Ensures the message takes up the remaining space

    // Append the message container and close button to the toastr
    toastr.appendChild(messageContainer);
    toastr.appendChild(closeButton);

    // Append the toastr to the body
    document.body.appendChild(toastr);

    // Show the toastr by changing its opacity
    setTimeout(() => {
        toastr.style.opacity = '1';
    }, 10);

    // Close the toastr when the close button is clicked
    closeButton.addEventListener('click', () => {
        toastr.style.opacity = '0';
        setTimeout(() => {
            toastr.remove(); // Remove the toastr from the DOM
        }, 500); // Match the fade-out duration
    });

    // Automatically hide the toastr after 3 seconds if not closed
    setTimeout(() => {
        toastr.style.opacity = '0';
        setTimeout(() => {
            toastr.remove(); // Remove the toastr from the DOM
        }, 500);
    }, 3000);
}

// Function to show an error message (red)
function ErrorToastr(message) {
    // Create a div for the toastr
    const toastr = document.createElement('div');

    // Create a close button
    const closeButton = document.createElement('button');
    closeButton.textContent = 'X';
    closeButton.style.background = 'none';
    closeButton.style.border = 'none';
    closeButton.style.color = 'white';
    closeButton.style.fontSize = '18px';
    closeButton.style.cursor = 'pointer';

    // Style the toastr for error (red)
    toastr.style.position = 'fixed';
    toastr.style.top = '20px';
    toastr.style.right = '20px';
    toastr.style.backgroundColor = '#dc3545'; // Red color
    toastr.style.color = 'white';
    toastr.style.padding = '10px 20px';
    toastr.style.borderRadius = '5px';
    toastr.style.fontSize = '16px';
    toastr.style.boxShadow = '0 4px 6px rgba(0, 0, 0, 0.1)';
    toastr.style.zIndex = '9999';
    toastr.style.opacity = '0';
    toastr.style.transition = 'opacity 0.5s ease-in-out';
    toastr.style.maxWidth = '300px';

    // Use flexbox to ensure proper alignment
    toastr.style.display = 'flex';
    toastr.style.alignItems = 'center';
    toastr.style.justifyContent = 'space-between';

    // Create a container div for the message content
    const messageContainer = document.createElement('div');
    messageContainer.textContent = message;
    messageContainer.style.flexGrow = '1';  // Ensures the message takes up the remaining space

    // Append the message container and close button to the toastr
    toastr.appendChild(messageContainer);
    toastr.appendChild(closeButton);

    // Append the toastr to the body
    document.body.appendChild(toastr);

    // Show the toastr by changing its opacity
    setTimeout(() => {
        toastr.style.opacity = '1';
    }, 10);

    // Close the toastr when the close button is clicked
    closeButton.addEventListener('click', () => {
        toastr.style.opacity = '0';
        setTimeout(() => {
            toastr.remove(); // Remove the toastr from the DOM
        }, 500); // Match the fade-out duration
    });

    // Automatically hide the toastr after 3 seconds if not closed
    setTimeout(() => {
        toastr.style.opacity = '0';
        setTimeout(() => {
            toastr.remove(); // Remove the toastr from the DOM
        }, 500);
    }, 3000);
}
