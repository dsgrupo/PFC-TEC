$('#detalhesModalNotificacao').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal

    var nomeTrabalho = button.data('nome-trabalho')

    var codigo = button.data('codigo-orientador')
    var nomeOrientador = button.data('nome-orientador')
    var sobrenomeOrientador = button.data('sobrenome-orientador')
    var telefoneOrientador = button.data('telefone-orientador')
    var emailOrientador = button.data('email-orientador')

    var matricula = button.data('matricula-aluno')
    var nomeAluno = button.data('nome-aluno')
    var sobrenomeAluno = button.data('sobrenome-aluno')
    var telefoneAluno = button.data('telefone-aluno')
    var emailAluno = button.data('email-aluno')


    var modal = $(this)

    modal.find('#detalhesModalTitle').text('Visualizando "' + nomeTrabalho + '"')
    modal.find('#txtCodigo').val(codigo)
    modal.find('#txtNomeOrientador').val(nomeOrientador)
    modal.find('#txtSobrenomeOrientador').val(sobrenomeOrientador)
    modal.find('#txtTelefoneOrientador').val(telefoneOrientador)
    modal.find('#txtEmailOrientador').val(emailOrientador)

    modal.find('#txtMatricula').val(matricula)
    modal.find('#txtNomeAluno').val(nomeAluno)
    modal.find('#txtSobrenomeAluno').val(sobrenomeAluno)
    modal.find('#txtTelefoneAluno').val(telefoneAluno)
    modal.find('#txtEmailAluno').val(emailAluno)
});