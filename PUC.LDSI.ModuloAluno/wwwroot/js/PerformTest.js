var provaCompleta;
var questaoAtual = -1;

var obterProvaCompleta = (function () {
    $.ajax({
        type: 'GET',
        url: '/Prova/ObterProvaCompleta?id=' + $("#PublicacaoId").val(),
        success: function (result) {
            provaCompleta = result;
            exibirProximaQuestao();
        }
    });
}());

var eventosPagina = (function () {
    $("body").on('click', '.icon-navigation-left', function (e) {
        aceitarRespostaQuestao();
        exibirQuestaoAnterior();
    });

    $("body").on('click', '.icon-navigation-right', function (e) {
        aceitarRespostaQuestao();
        exibirProximaQuestao();
    });

    $("body").on('click', '.questao-summary', function (e) {
        var idx = parseInt(this.attributes["idx"].value);
        questaoAtual = idx;
        exibirQuestao();
    });

    $("body").on('click', '#btnConcluirProva', function (e) {
        AbrirJanelaConfirmacao("Concluir a Prova", "Confirma a conclusão desta prova?").then((yes) => {
            if (yes) concluirProva();
        });
    });
}());

var concluirProva = function () {
    var dataObject = { AvaliacaoId: provaCompleta.avaliacaoId, PublicacaoId: provaCompleta.publicacaoId, Questoes: [] };

    provaCompleta.questoes.forEach((questao, i) => {
        dataObject.Questoes.push({ QuestaoId: questao.questaoId, Opcoes: [] });
        questao.opcoes.forEach((opcao) => {
            dataObject.Questoes[i].Opcoes.push({ OpcaoAvaliacaoId: opcao.opcaoAvaliacaoId, Resposta: opcao.resposta });
        });
    });

    $.ajax({
        url: '/Prova/ConcluirProva',
        data: JSON.stringify(dataObject),
        contentType: 'application/json',
        type: 'POST',
        success: function (data) {
            if (data.success)
                window.location = '/Prova/Index';
            else
                AbrirJanelaErro('Conclusão da Prova', data.error);
        }
    });
}

var exibirProximaQuestao = function () {
    if (provaCompleta) {
        if (provaCompleta.questoes.length > (questaoAtual + 1)) {
            questaoAtual++;
            exibirQuestao();
        }
        else exibirSumario();
    }
}

var exibirQuestaoAnterior = function () {
    if (provaCompleta) {
        if (questaoAtual === -1)
            questaoAtual = provaCompleta.questoes.length - 1;
        else
            questaoAtual--;

        if (questaoAtual >= 0) 
            exibirQuestao();
        else
            exibirSumario();
    }
}

var exibirSumario = function () {
    questaoAtual = -1;
    $(".opcoes-prova-enunciado").empty().text("Resumo da Prova");
    $(".opcoes-prova-opcoes").empty();

    provaCompleta.questoes.forEach((item, i) => {
        var infoQuestao = $("<div idx='"+ i +"'/>");
        infoQuestao.append("<spam style='margin-right:10px'>Questão " + (i + 1) + "</spam>");
        if (item.opcoes.find(x => x.resposta == true))
            infoQuestao.append("<i class='fas fa-smile-beam questao-ok'></i>");
        else if (item.tipo === 2) {
            infoQuestao.append("<i class='fas fa-meh-rolling-eyes questao-ok'></i>");
            infoQuestao.append("<spam>(*) nenhuma opção foi marcada!</spam><br/>")
        }
        else {
            infoQuestao.append("<i class='fas fa-grimace questao-bad'></i>");
            infoQuestao.append("<spam>(*) questão não respondida!</spam><br/>")
        }
        infoQuestao.addClass("questao-summary");
        $(".opcoes-prova-opcoes").append(infoQuestao);
    });

    if ($(".questao-bad").length === 0)
        $(".opcoes-prova-submit").show();
    else
        $(".opcoes-prova-submit").hide();
}

var aceitarRespostaQuestao = function () {
    if (provaCompleta && questaoAtual >= 0) {
        provaCompleta.questoes[questaoAtual].opcoes.forEach((item, index) => {
            item.resposta = false
        });

        var tipoInput = provaCompleta.questoes[questaoAtual].tipo === 1 ? "radio" : "checkbox";

        $(".opcoes-prova-opcoes input:" + tipoInput + ":checked").each(function () {
            provaCompleta.questoes[questaoAtual].opcoes.find(x => x.opcaoAvaliacaoId == $(this).val()).resposta = true;
        });
    }
}

var exibirQuestao = function () {
    if (provaCompleta) {
        var type = provaCompleta.questoes[questaoAtual].tipo === 1 ? "radio" : "checkbox";
        $(".opcoes-prova-enunciado").empty().text((questaoAtual + 1) + " - " + provaCompleta.questoes[questaoAtual].enunciado);
        $(".opcoes-prova-opcoes").empty();
        $(".opcoes-prova-submit").hide();
        provaCompleta.questoes[questaoAtual].opcoes.forEach((item, index) => {
            var input = $("<input/>");
            input.prop("type", type).prop("checked", item.resposta).prop("id", "opc" + index).prop("name", "opc" + (type === "radio" ? "" : index));
            input.val(item.opcaoAvaliacaoId);

            var label = $("<label/>");
            label.prop("for", "opc" + index).addClass("check-resposta").text(item.descricao);

            $(".opcoes-prova-opcoes").append(input).append(label).append("<br/>");
        });
    }
}
