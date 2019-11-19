AbrirJanelaConfirmacao = function (titulo, mensagem) {
    return new Promise((resolve) => {
        $(".icon-msg-popup").removeClass("fa-exclamation-circle");
        $(".icon-msg-popup").addClass("fa-question-circle");
        $(".icon-msg-popup").css("color", "blue");
        $('.modal-btn-cancel').show();
        $(".modal-title").text(titulo);
        $(".modal-body").html("<b>" + mensagem + "</b>");
        $('body').on('click', '.modal-btn-ok', () => { resolve(true); });
        $('body').on('click', '.modal-btn-cancel', () => { resolve(false); });
        $('#LdsiModal').modal({ backdrop: 'static', keyboard: false });
    });
}

AbrirJanelaErro = function (titulo, mensagem) {
    $(".icon-msg-popup").removeClass("fa-question-circle");
    $(".icon-msg-popup").addClass("fa-exclamation-circle");
    $(".icon-msg-popup").css("color", "red");
    $(".modal-title").text(titulo);
    $(".modal-body").html("<b>" + mensagem + "</b>");
    $('.modal-btn-cancel').hide();
    $('#LdsiModal').modal({ backdrop: 'static', keyboard: false });
}