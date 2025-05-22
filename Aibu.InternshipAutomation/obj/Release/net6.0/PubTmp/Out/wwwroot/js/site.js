// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function showToast(status, message) {
    switch (status.toLowerCase()) {
        case 'success':
            toastr.success(message);
            break;
        case 'info':
            toastr.info(message);
            break;
        case 'warning':
            toastr.warning(message);
            break;
        case 'error':
            toastr.error(message);
            break;
        default:
            console.error("Geçersiz statu değeri. Lütfen 'Success', 'Info', 'Warning' veya 'Error' kullanın.");
    }
}