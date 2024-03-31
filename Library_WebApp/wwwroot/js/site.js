// Cache the loader element
var $loader = $("#loader");

function loader(flag) {
    console.log("Loader invoked");
    // Check if the loader is already in the desired state
    if (flag && $loader.is(":hidden")) {
        $loader.show()
    } else if (!flag && $loader.is(":visible")) {
        $loader.hide();
    }
}

function RedirectOn(path) {
    loader(true);
    window.location.href = path;
}

function displayModal(header, body, action) {
    $("#genericModal-header").html(header);
    $("#genericModal-body").html(body);
    if (action) {
        $("#genericModal-action").attr("onclick", action);
    }
    else {
        $("#genericModal-action").attr("onclick", "$('#genericModal').modal('hide');");
    }
    $("#genericModal").modal("show");
}