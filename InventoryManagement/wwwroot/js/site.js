$(function () {
    if ($('div.alert.notification').length) {
        setTimeout(() => {
            $('div.alert.notification').fadeOut();
        }, 2000);
    }
});
$(function () {
    $('#btnSubmitExport').click(function () {
        $("input[name='htmlTable']").val($("#exportData").html())
    })
}

)